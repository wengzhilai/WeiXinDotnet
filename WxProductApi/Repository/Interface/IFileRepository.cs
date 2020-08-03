
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;
using Models.Entity;

namespace IRepository
{
    public interface IFileRepository 
    {
        Task<SysFilesEntity> SingleByKey(int key);

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        Task<IEnumerable<SysFilesEntity>> FindAll(Expression<Func<SysFilesEntity, bool>> inParm = null);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<int> Save(DtoSave<SysFilesEntity> inEnt);

    }
}
