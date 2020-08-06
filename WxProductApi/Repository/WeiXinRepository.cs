
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
    public class WeiXinRepository : IWeiXinRepository
    {
        DapperHelper<WxQuestLogEntity> dbHelper = new DapperHelper<WxQuestLogEntity>();
        DapperHelper<WxUserEntity> dbHelper_user = new DapperHelper<WxUserEntity>();
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns></returns>
        public async Task<ResultObj<int>> saveLog(WxQuestLogEntity inObj)
        {
            ResultObj<int> resultObj = new ResultObj<int>();
            var saveEnt = new DtoSave<WxQuestLogEntity>() { data = inObj };
            saveEnt.data.id = await SequenceRepository.GetNextID<WxQuestLogEntity>();
            resultObj.data = await dbHelper.Save(saveEnt);
            resultObj.success = resultObj.data > 0;
            return resultObj;
        }
        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns></returns>
        public async Task<ResultObj<int>> saveUser(WxUserEntity inObj,List<string> saveFieldList)
        {
            ResultObj<int> resultObj = new ResultObj<int>();
            var saveEnt = new DtoSave<WxUserEntity>() { data = inObj };
            string openid = inObj.openid;
            if (await dbHelper_user.Count(x => x.openid == openid) > 0)
            {
                saveEnt.ignoreFieldList=new List<string>{"openid"};
                saveEnt.whereList=new List<string>{"openid"};
                saveEnt.data.lastTime=DataTimeHelper.getDateLong(DateTime.Now);
                saveEnt.saveFieldList=saveFieldList;
                resultObj.data = await dbHelper_user.Update(saveEnt);
            }
            else
            {
                saveEnt.data.createTime=DataTimeHelper.getDateLong(DateTime.Now);
                saveEnt.data.lastTime=saveEnt.data.createTime;
                resultObj.data = await dbHelper_user.Save(saveEnt);
            }

            resultObj.success = resultObj.data > 0;
            return resultObj;
        }
    }
}
