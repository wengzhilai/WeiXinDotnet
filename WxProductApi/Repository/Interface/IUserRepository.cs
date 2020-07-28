
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;
using Models.Entity;

namespace IRepository
{
    public interface IUserRepository 
    {
        /// <summary>
        /// 单个查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<SysUserEntity> SingleByKey(int key);

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        Task<IEnumerable<SysUserEntity>> FindAll(Expression<Func<SysUserEntity, bool>> inParm = null);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<ResultObj<SysUserEntity>> UserLogin(string username,string password);

        /// <summary>
        /// 保存基本信息
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<ResultObj<int>> Save(DtoSave<SysUserEntity> inEnt);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="scriptId">主键 ID</param>
        /// <returns></returns>
        Task<ResultObj<int>> Delete(int keyId);

    }
}
