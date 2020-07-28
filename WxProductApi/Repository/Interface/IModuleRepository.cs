

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
        Task<SysModuleEntity> SingleByKey(int key);

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
        Task<ResultObj<int>> Save(DtoSave<SysModuleEntity> inEnt);


        /// <summary>
        /// 查找所有模块菜单
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        Task<ResultObj<SysModuleEntity>> GetMenu(Expression<Func<SysModuleEntity, bool>> inParm = null);

        Task<ResultObj<SysModuleEntity>> GetMenuByRoleId(List<int> roleIdList);
        
        Task<ResultObj<SysModuleEntity>> GetMGetMenuByUserId(int userId);


    }
}
