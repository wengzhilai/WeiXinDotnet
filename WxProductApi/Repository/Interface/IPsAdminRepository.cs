
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;
using Models.Entity;

namespace IRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPsAdminRepository
    {

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<ResultObj<int>> Save(DtoSave<PsAdminEntity> inEnt);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="openid">主键 ID</param>
        /// <returns></returns>
        Task<ResultObj<int>> Delete(string openid);

    }
}
