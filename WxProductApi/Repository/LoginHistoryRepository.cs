
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
    public class LoginHistoryRepository
    {
        DapperHelper<SysLoginHistoryEntity> dbHelper = new DapperHelper<SysLoginHistoryEntity>();
        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<SysLoginHistoryEntity> SingleByKey(int key)
        {
            return dbHelper.SingleByKey(key);
        }

        /// <summary>
        /// 查找所有
        /// </summary>
        /// <param name="inParm"></param>
        /// <returns></returns>
        public Task<IEnumerable<SysLoginHistoryEntity>> FindAll(Expression<Func<SysLoginHistoryEntity, bool>> inParm = null)
        {
            return dbHelper.FindAll(inParm);
        }
        /// <summary>
        /// 保存操作记录
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns></returns>
        public Task<int> Save(DtoSave<SysLoginHistoryEntity> inObj)
        {
            return dbHelper.Save(inObj);
        }
    }
}
