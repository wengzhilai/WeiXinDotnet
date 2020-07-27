
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;
using Models.Entity;

namespace IRepository
{
    public interface ILoginRepository 
    {
        Task<FaLoginEntity> SingleByKey(int key);

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        Task<IEnumerable<FaLoginEntity>> FindAll(Expression<Func<FaLoginEntity, bool>> inParm = null);


        /// <summary>
        /// 注册账号
        /// <para>1、添加登录工号 </para>
        /// <para>2、添加用户</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<ResultObj<int>> LoginReg(LogingDto inEnt);
        /// <summary>
        /// 注销用户登录状态
        /// <para>清除用户的缓存状态</para>
        /// <para>记录退出日志</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<Result> LoginOut(DtoSave<SysLoginHistoryEntity> inEnt);
        /// <summary>
        /// 用户登录
        /// <para>只验证用户账号</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        
        Task<ResultObj<FaUserEntity>>  UserLogin(LogingDto inEnt);
        /// <summary>
        /// 重置用户密码
        /// <para>VerifyCode:短信验证码</para>
        /// <para>LoginName:登录名</para>
        /// <para>NewPwd:新密码</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<Result> ResetPassword(ResetPasswordDto inEnt);
        /// <summary>
        /// 修改用户密码
        /// <para>entity:旧密码</para>
        /// <para>NewPwd:新密码</para>
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<ResultObj<bool>> UserEditPwd(EditPwdDto inEnt);

        /// <summary>
        /// 改变登录名
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<Result> ChangeLoginName(ChangeLoginNameDto inEnt);

    }

    public class ChangeLoginNameDto
    {
        /// <summary>
        /// 原登录名
        /// </summary>
        public string oldLoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 新的登录名
        /// </summary>
        public string newLoginName { get; set; }
    }
}
