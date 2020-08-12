
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

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        DapperHelper<SysUserEntity> dbHelper = new DapperHelper<SysUserEntity>();
        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<SysUserEntity> SingleByKey(int key)
        {
            var ent=await dbHelper.SingleByKey(key);
            ent.roleIdList = (await new DapperHelper<FaUserRoleEntityView>().FindAll(i => i.userId == key)).Select(x => x.roleId).ToList();
            DapperHelper<FaUserRoleEntityView> dapperUserRole = new DapperHelper<FaUserRoleEntityView>();
            var role = await dapperUserRole.FindAll(i => i.userId == ent.id);
            ent.isAdmin = role.Count(i => i.roleId == 1) > 0;
            ent.isLeader = role.Count(i => i.roleId == 2) > 0;
            return ent;
        }
        

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        public Task<IEnumerable<SysUserEntity>> FindAll(Expression<Func<SysUserEntity, bool>> inParm = null)
        {
            return dbHelper.FindAll(inParm);
        }

        public Task<int> Update(DtoSave<SysUserEntity> inObj)
        {
            return dbHelper.Update(inObj);
        }

        public async Task<ResultObj<SysUserEntity>> UserLogin(string username, string password)
        {
            ResultObj<SysUserEntity> reObj = new ResultObj<SysUserEntity>();
            DapperHelper<SysLoginEntity> dapper=new DapperHelper<SysLoginEntity>();
            var login=await dapper.Single(x=>x.loginName==username);
            if (login != null)
            {
                if (login.password.ToLower().Equals(Fun.Md5Hash(password).ToLower()))
                {
                    reObj.data =await new DapperHelper<SysUserEntity>().Single(x=>x.loginName == username);
                }
                else
                {
                    reObj.msg = "密码错误";
                }
            }
            else
            {
                reObj.msg = "用户名不存在";
            }
            return reObj;
        }

        public async Task<ResultObj<int>> Save(DtoSave<SysUserEntity> inEnt)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            try
            {
                dbHelper.TranscationBegin();
                if (inEnt.data.id == 0)
                {
                    inEnt.data.id = await SequenceRepository.GetNextID<SysUserEntity>();
                    reObj.data = await dbHelper.Save(inEnt);
                }
                else
                {
                    reObj.data = await dbHelper.Update(inEnt);
                }

                reObj.success = reObj.data > 0;
                if (!reObj.success)
                {
                    reObj.msg = "保存角色失败";
                    dbHelper.TranscationRollback();
                }
                else
                {

                    int v = await dbHelper.Exec("delete from sys_user_role where user_id = " + inEnt.data.id);
                
                    foreach (var item in inEnt.data.roleIdList)
                    {
                        var opNum = await dbHelper.Exec(string.Format("insert into sys_user_role(role_id,user_id) values({0},{1}) ", item, inEnt.data.id));
                        if (opNum != 1)
                        {
                            reObj.success = false;
                            reObj.msg = "保存用户模块失败";
                            dbHelper.TranscationRollback();
                            return reObj;
                        }
                    }

                    reObj.success = true;
                    dbHelper.TranscationCommit();
                }

            }
            catch (Exception e)
            {
                reObj.success = false;
                reObj.msg = "保存用户失败";
                LogHelper.WriteErrorLog(this.GetType(), reObj.msg, e);
                dbHelper.TranscationRollback();
            }
            return reObj;
        }

        public async Task<ResultObj<int>> Delete(int keyId)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            reObj.data = await dbHelper.Delete(i => i.id == keyId);
            reObj.success = reObj.data > 0;
            return reObj;
        }
    }
}
