
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
using System.Globalization;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Repository
{
    public class QueryRepository:IQueryRepository
    {
        public QueryRepository()
        {
            //ExceptionlessClient.Default.Startup("ZevZKuGg0jZOohtFYKma1kcDiCWvUhBTROnzR1pN");
        }
        private DapperHelper<SysQueryEntity> dal = new DapperHelper<SysQueryEntity>();

        /// <summary>
        /// 获取所有数据
        /// msg为SQL语句
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        public async Task<ResultObj<Dictionary<string, object>>> QueryExecute(QuerySearchDto inEnt)
        {
            ResultObj<Dictionary<string, object>> reObj = new ResultObj<Dictionary<string, object>>();
            Dictionary<string, object> reEnt = new Dictionary<string, object>();

            SysQueryEntity query = await dal.Single(i => i.code == inEnt.code);
            if (query == null)
            {
                return reObj;
            }
            IList<QueryCfg> cfg = TypeChange.ToJsonObject<List<QueryCfg>>(query.queryCfgJson);

            string whereStr = "";
            string AllSql = MakeSql(inEnt, query.queryConf, ref whereStr);
            reObj.msg = AllSql;
            if (string.IsNullOrEmpty(inEnt.orderStr)) inEnt.orderStr = "(SELECT 0)";
            try
            {
                reObj.dataList = dal.Query(AllSql);
                reObj.total = reObj.dataList.Count();

            }
            catch
            {
                return reObj;
            }
            return reObj;
        }
        /// <summary>
        /// 获取Csv数据,支持大数据下载
        /// </summary>
        /// <param name="inEnt"></param>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public async Task<ResultObj<List<byte>>> QueryExecuteCsv(QuerySearchDto inEnt)
        {
            ResultObj<List<byte>> reObj = new ResultObj<List<byte>>();
            List<byte> reEnt = new List<byte>();

            SysQueryEntity query = await dal.Single(i => i.code == inEnt.code);
            if (query == null)
            {

                return reObj;
            }

            JObject cfg = TypeChange.JsonToObject(query.queryCfgJson);
            Dictionary<string,string> dict=new Dictionary<string, string>();
            foreach (var item in cfg)
            {
                try
                {
                    dict.Add(item.Key,item.Value["title"].ToString());
                }
                catch 
                {
                }
            }
            string whereStr = "";
            string AllSql = MakeSql(inEnt, query.queryConf, ref whereStr);
            //如果条件为空
            if (!string.IsNullOrEmpty(whereStr))
            {
                if (!whereStr.Trim().ToLower().StartsWith("where")) whereStr = "where " + whereStr;

                AllSql = string.Format(@"
                    SELECT T.* FROM 
                    ( 
	                    {0}
                    ) T {1}
                    ", AllSql, whereStr);

            }
            try
            {
                reObj.msg = AllSql;
                reObj.data = dal.ExecuteBytesAsync(AllSql,null,dict);
            }
            catch(Exception e)
            {
                reObj.msg=e.Message;
                reObj.success=false;
                return reObj;
            }
            return reObj;
        }


        /// <summary>
        /// 执行分页数据
        /// </summary>
        /// <param name="inEnt"></param>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public async Task<ResultObj<Dictionary<string, object>>> QueryPageExecute(QuerySearchDto inEnt)
        {
            ResultObj<Dictionary<string, object>> reObj = new ResultObj<Dictionary<string, object>>();
            Dictionary<string, object> reEnt = new Dictionary<string, object>();
            SysQueryEntity query = await dal.Single(i => i.code == inEnt.code);
            if (query == null)
            {
                return reObj;
            }

            string whereStr = "";
            string AllSql = MakeSql(inEnt, query.queryConf, ref whereStr);
            if (string.IsNullOrWhiteSpace(inEnt.orderStr)) inEnt.orderStr = "(SELECT 0)";
            reObj.msg = MakePageSql(AllSql, inEnt.page, inEnt.rows, inEnt.orderStr, whereStr);
            try
            {
                var sqlList = reObj.msg.Split(';');
                if (sqlList.Count() > 0)
                {
                    reObj.dataList = dal.Query(sqlList[0]);
                }

                if (sqlList.Count() > 1)
                {
                    int allNum = 0;
                    int.TryParse(await dal.ExecuteScalarAsync(sqlList[1]), out allNum);
                    reObj.total = allNum;
                }
            }
            catch(Exception e)
            {
                LogHelper.WriteErrorLog(this.GetType(),"执行分页数据失败",e);
                return reObj;
            }
            return reObj;
        }

        /// <summary>
        /// 生成配置数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public async Task<ResultObj<string>> MakeQueryCfg(string code)
        {
            ResultObj<String> reObj = new ResultObj<String>();
            QuerySearchDto inEnt = new QuerySearchDto() { code = code };
            JObject reEnt = new JObject();
            SysQueryEntity query = await dal.Single(i => i.code == inEnt.code);

            if (query == null)
            {
                return reObj;
            }
            IList<QueryCfg> cfg = new List<QueryCfg>();
            if (!string.IsNullOrEmpty(query.queryCfgJson))
            {
                cfg = TypeChange.ToJsonObject<List<QueryCfg>>(query.queryCfgJson);
            }

            string whereStr = "";
            string AllSql = MakeSql(inEnt, query.queryConf, ref whereStr);
            reObj.msg = MakePageSql(AllSql);
            try
            {

                DataTable dt = dal.GetDataTable(reObj.msg);                
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var t = dt.Columns[i];
                    var tmp = t.DataType.FullName.ToLower().Substring(t.DataType.FullName.IndexOf(".") + 1);
                    IList<string> numberList = new[] { "int", "decimal", "double", "int64", "int16" };
                    if (numberList.Contains(tmp))
                    {
                        tmp = "int";
                    }
                    string searchType = "";
                    switch (tmp)
                    {
                        case "int":
                            searchType = "numberbox";
                            break;
                        case "datetime":
                            searchType = "datetimebox";
                            break;
                        default:
                            searchType = "text";
                            break;
                    }
                    JObject itemObj=new JObject();
                    itemObj["title"]=t.ColumnName;
                    itemObj["type"]=searchType;
                    itemObj["editable"]=true;
                    reEnt[t.ColumnName]=itemObj;
                }
                
                reObj.data = reEnt.ToString();
                return reObj;
            }
            catch
            {
                return reObj;
            }
        }

        /// <summary>
        /// 生成分页的SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderStr"></param>
        /// <param name="whereStr"></param>
        /// <param name="fieldList"></param>
        /// <returns></returns>
        public string MakePageSql(string sql, int pageIndex = 1, int pageSize = 1, string orderStr = "1", string whereStr = null, IList<string> fieldList = null)
        {
            if (pageIndex < 0) pageIndex = 1;
            if (pageSize < 0) pageSize = 10;
            if (!string.IsNullOrWhiteSpace(whereStr) && !whereStr.Trim().ToLower().StartsWith("where")) whereStr = "where " + whereStr;
            if (string.IsNullOrWhiteSpace(orderStr)) orderStr = "1";
            orderStr = string.Format("ORDER BY {0}", orderStr);


            string withStr = "";
            string WithStartStr = "-----WithStart-----";
            string WithEndStr = "-----WithEnd-----";
            int WithStartP = sql.IndexOf(WithStartStr);
            int WithEndP = sql.IndexOf(WithEndStr);
            if (WithStartP > -1 && WithEndP > -1 && WithEndP > WithStartP)
            {
                withStr = sql.Substring(WithStartStr.Length, WithEndP - WithStartP - WithStartStr.Length);
                sql = sql.Substring(WithEndP + WithEndStr.Length);
            }


            string t1File = "*";
            string tFile = "*";
            if (fieldList != null)
            {
                t1File = string.Join(",T1.", fieldList);
                tFile = string.Join(",T.", fieldList);
            }
            string sqlStr = @"
{5}
SELECT T.{6} FROM 
( 
    {0}
) T {4} {1} LIMIT {2},{3};
SELECT COUNT(1) ALL_NUM FROM ({0}) T {4}
";
            if (pageIndex == 0) pageIndex = 1;
            if (pageSize == 0) pageSize = 10;
            int startNum = (pageIndex - 1) * pageSize;
            sqlStr = string.Format(sqlStr, string.Format(sql, whereStr), orderStr, startNum, startNum + pageSize, whereStr, withStr, tFile, t1File);
            return sqlStr;
        }

        /// <summary>
        /// 根据Query的SQL 生成需要的SQL
        /// </summary>
        /// <param name="inEnt"></param>
        /// <param name="querySql"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public string MakeSql(QuerySearchDto inEnt, string querySql, ref string whereStr)
        {

            if (inEnt.paraList == null) inEnt.paraList = new List<QueryPara>();
            if (inEnt.whereList == null)
            {
                if (string.IsNullOrEmpty(inEnt.whereListStr))
                {
                    inEnt.whereList = new List<QueryRowBtnShowCondition>();
                }
                else
                {
                    inEnt.whereList = TypeChange.ToJsonObject<List<QueryRowBtnShowCondition>>(inEnt.whereListStr);
                }
            }

            if (inEnt.paraList == null)
            {
                if (string.IsNullOrEmpty(inEnt.paraListStr))
                {
                    inEnt.paraList = new List<QueryPara>();
                }
                else
                {
                    inEnt.paraList = TypeChange.ToJsonObject<List<QueryPara>>(inEnt.paraListStr);
                }
            }


            //替换地址参数
            foreach (var tmp in inEnt.paraList)
            {
                if (tmp.value == "@(NOWDATA)")
                {
                    tmp.value = DateTime.Today.ToString("yyyy-MM-dd");
                }
                querySql = querySql.Replace("@(" + tmp.paraName + ")", tmp.value);
            }

            //替换搜索的参数
            foreach (var tmp in inEnt.whereList)
            {
                if (string.IsNullOrEmpty(tmp.objFiled)) tmp.objFiled = tmp.fieldName;
                if (string.IsNullOrEmpty(tmp.fieldName)) tmp.fieldName = tmp.objFiled;
                querySql = querySql.Replace("@(" + tmp.objFiled + ")", tmp.value);
            }

            StringBuilder whereSb = new StringBuilder();
            foreach (var tmp in inEnt.whereList.Where(x => x.opType != null && !string.IsNullOrEmpty(x.opType) && !string.IsNullOrEmpty(x.value)))
            {
                if (tmp.fieldType == null) tmp.fieldType = "string";
                var nowType = tmp.fieldType.ToLower();
                int subIndex = tmp.fieldType.IndexOf(".");
                if (subIndex > -1)
                {
                    nowType = nowType.Substring(subIndex + 1);
                }
                switch (nowType)
                {
                    case "text":
                    case "string":
                        switch (tmp.opType)
                        {
                            case "in":
                                whereSb.Append(string.Format(" {0} {1} ('{2}') and ", tmp.objFiled, tmp.opType, tmp.value.Replace(",", "','")));
                                break;
                            default:
                                if (tmp.opType == "like") tmp.value = "%" + tmp.value + "%";
                                whereSb.Append(string.Format(" {0} {1} '{2}' and ", tmp.objFiled, tmp.opType, tmp.value));
                                break;
                        }
                        break;
                    case "date":
                    case "datetime":
                        switch (tmp.opType)
                        {
                            case "not between":
                            case "between":
                                whereSb.Append(string.Format(" {0} {1} '{2}' and ", tmp.objFiled, tmp.opType, string.Join("' and '", tmp.value.Split('~').ToList().Select(x => x.Trim()))));
                                break;
                            default:
                                whereSb.Append(string.Format(" {0} {1} '{2}' and ", tmp.objFiled, tmp.opType, tmp.value));
                                break;
                        }
                        break;
                    default:
                        if (tmp.opType == "in")
                        {
                            whereSb.Append(string.Format(" {0} {1} ('{2}') and ", tmp.objFiled, tmp.opType, tmp.value.Replace(",", "','")));
                        }
                        else
                        {
                            whereSb.Append(string.Format(" {0} {1} {2} and ", tmp.objFiled, tmp.opType, tmp.value));
                        }
                        break;
                }
            }
            if (whereSb.Length > 4)
                whereSb = whereSb.Remove(whereSb.Length - 4, 4);
            whereStr = whereSb.ToString();
            return querySql;
        }



        public async Task<int> Save(DtoSave<SysQueryEntity> inEnt)
        {
            if(inEnt.data.id==0){
                inEnt.data.id=await SequenceRepository.GetNextID<SysQueryEntity>();
            }
            return await dal.Save(inEnt);
        }

        public async Task<List<SysQueryEntity>> FindAll(DtoSearch inSearch)
        {
            var reList = await dal.FindAll(inSearch);
            return reList.ToList();
        }

        public async Task<ResultObj<SysQueryEntity>> FindAllPage(DtoSearch inSearch)
        {
            var reObj=new ResultObj<SysQueryEntity>();
            var reList = await dal.FindAllS<SysQueryEntity,KV>(inSearch);
            reObj.dataList=reList.Item1.ToList();
            if(reList.Item2!=null && reList.Item2.Count()>0){
                reObj.msg=reList.Item2.ToList()[0].v;
            }
            return reObj;
        }

        public async Task<int> Update(DtoSave<SysQueryEntity> inEnt)
        {
            return await dal.Update(inEnt);
        }

        public async Task<SysQueryEntity> Single(Expression<Func<SysQueryEntity, bool>> where)
        {
            var reEnt = await dal.Single(where);
            reEnt._DictStr = TypeChange.ObjectToStr(new ModelHelper<SysQueryEntity>(reEnt).GetDisplayDirct());
            reEnt._dictQueryCfgStr = TypeChange.ObjectToStr(new ModelHelper<QueryCfg>(new QueryCfg()).GetDisplayDirct());
            return reEnt;

        }

        public async Task<int> Delete(Expression<Func<SysQueryEntity, bool>> where)
        {
            return await dal.Delete(where);

        }

    }

}