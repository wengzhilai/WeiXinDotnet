
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
    /// <summary>
    /// 图片处理服务
    /// </summary>
    public class FileRepository : IFileRepository
    {
        DapperHelper<SysFilesEntity> dbHelper = new DapperHelper<SysFilesEntity>();
        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<SysFilesEntity> SingleByKey(int key)
        {
            return dbHelper.SingleByKey(key);
        }

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        public Task<IEnumerable<SysFilesEntity>> FindAll(Expression<Func<SysFilesEntity, bool>> inParm = null)
        {
            return dbHelper.FindAll(inParm);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        public async Task<int> Save(DtoSave<SysFilesEntity> inEnt)
        {
            if(inEnt.data.id==0){
                inEnt.data.id=await SequenceRepository.GetNextID<SysFilesEntity>();
            }
            return await dbHelper.Save(inEnt);
        }
    }
}
