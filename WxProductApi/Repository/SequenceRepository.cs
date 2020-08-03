
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
// CREATE TABLE `sequence` (
//   `seq_name` varchar(50) NOT NULL,
//   `current_val` int(11) NOT NULL,
//   `increment_val` int(11) NOT NULL DEFAULT '1',
//   PRIMARY KEY (`seq_name`)
//   )
    public class SequenceRepository
    {
        static DapperHelper<SysSequenceEntity> dbHelper = new DapperHelper<SysSequenceEntity>();
        /// <summary>
        /// 获取单条
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<int> GetNextID<T>()where T : new()
        {
            string tableName=new ModelHelper<T> ().GetTableName();
            var single=await dbHelper.SingleByKey(tableName);
            if(single==null){
                single= new SysSequenceEntity();
                single.seq_name=tableName;
                single.current_val=1;
                single.increment_val=1;
                await dbHelper.Save(new DtoSave<SysSequenceEntity>{
                    data=single,
                    ignoreFieldList=new List<string>()
                });
            }else{
                single.current_val+=single.increment_val;
                await dbHelper.Update(new DtoSave<SysSequenceEntity>
                {
                    data = single,
                    saveFieldListExp = x => new object[] { x.current_val },
                });
            }
            return single.current_val;
        }
    }
}
