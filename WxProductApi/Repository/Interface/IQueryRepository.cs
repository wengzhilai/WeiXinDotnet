using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;
using Models.Entity;

namespace IRepository
{
    public interface IQueryRepository
    {
        
        /// <summary>
        /// 获取所有数据
        /// msg为SQL语句
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        Task<ResultObj<Dictionary<string, object>>> QueryExecute(QuerySearchDto inEnt);
        /// <summary>
        /// 获取Csv数据,支持大数据下载
        /// </summary>
        /// <param name="inEnt"></param>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        Task<ResultObj<List<byte>>> QueryExecuteCsv(QuerySearchDto inEnt);


        /// <summary>
        /// 执行分页数据
        /// </summary>
        /// <param name="inEnt"></param>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        Task<ResultObj<Dictionary<string, object>>> QueryPageExecute(QuerySearchDto inEnt);
        /// <summary>
        /// 生成配置数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        Task<ResultObj<string>> MakeQueryCfg(string code);

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
        string MakePageSql(string sql, int pageIndex = 1, int pageSize = 1, string orderStr = "getdate()", string whereStr = null, IList<string> fieldList = null);

        /// <summary>
        /// 根据Query的SQL 生成需要的SQL
        /// </summary>
        /// <param name="inEnt"></param>
        /// <param name="query"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        string MakeSql(QuerySearchDto inEnt, string query, ref string whereStr);



        Task<int> Save(DtoSave<SysQueryEntity> inEnt);

        Task<List<SysQueryEntity>> FindAll(DtoSearch inSearch);
        Task<ResultObj<SysQueryEntity>> FindAllPage(DtoSearch inSearch);
        Task<int> Update(DtoSave<SysQueryEntity> inEnt);
        Task<SysQueryEntity> Single(Expression<Func<SysQueryEntity, bool>> where);

        Task<int> Delete(Expression<Func<SysQueryEntity, bool>> where);
    }
}