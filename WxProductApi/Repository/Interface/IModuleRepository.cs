

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;
using Models.Entity;

namespace IRepository
{
    public interface IModuleRepository
    {
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<FaModuleEntity> SingleByKey(int key);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="scriptId">主键 ID</param>
        /// <returns></returns>
        Task<ResultObj<int>> Delete(int scriptId);

        /// <summary>
        /// 保存基本信息
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<ResultObj<int>> Save(DtoSave<FaModuleEntity> inEnt);


        /// <summary>
        /// 查找所有模块菜单
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        Task<ResultObj<FaModuleEntity>> GetMenu(Expression<Func<FaModuleEntity, bool>> inParm = null);

        Task<ResultObj<FaModuleEntity>> GetMenuByRoleId(List<int> roleIdList);
        
        Task<ResultObj<FaModuleEntity>> GetMGetMenuByUserId(int userId);


    }
}
