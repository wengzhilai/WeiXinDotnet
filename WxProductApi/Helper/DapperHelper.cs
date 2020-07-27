
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using MySql.Data.MySqlClient;
using Dapper;
using System.Linq.Expressions;
using Models.Entity;
using WxProductApi.Config;

namespace Helper
{

    public class DapperHelper : IDisposable
    {

        protected IDbConnection connection;
        protected IDbTransaction transaction = null;

        public DapperHelper()
        {
            var tt = TypeChange.DynamicToKeyValueList(AppConfig.MysqlSettings);
            var alldict = string.Join(";", tt.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
            connection = new MySqlConnection(alldict + ";CharSet=utf8");
        }
        public DapperHelper(IDbConnection _connection, IDbTransaction _transaction)
        {
            transaction = _transaction;
            connection = _connection;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void TranscationBegin()
        {
            connection.Open();
            transaction = connection.BeginTransaction();
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            return connection;
        }

        public IDbTransaction GetTransaction()
        {
            return transaction;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void TranscationRollback()
        {
            transaction.Rollback();
            connection.Close();
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void TranscationCommit()
        {
            transaction.Commit();
            connection.Close();
        }

        public void Dispose()
        {
            if (transaction != null)
            {
                transaction.Dispose();
            }
            if (connection != null)
            {
                connection.Dispose();
            }
        }

        async public Task<int> Exec(string sql, object param = null)
        {
            var result = await connection.ExecuteAsync(sql, param, transaction);
            return result;
        }

        public List<byte> ExecuteBytesAsync(string sql, object param = null)
        {
            List<byte> reEnt = new List<byte>();

            connection.Open();
            try
            {
                var reader = connection.ExecuteReader(sql, param, transaction);
                if (reader != null)
                {

                    #region 创建列

                    List<string> allColumns = new List<string>();
                    for (int a = 0; a < reader.FieldCount; a++)
                    {
                        allColumns.Add(reader.GetName(a).Trim());
                    }
                    reEnt.AddRange(Encoding.UTF8.GetBytes(string.Join(",", allColumns) + "\r\n"));

                    #endregion
                    object[] objValues = new object[reader.FieldCount];
                    while (reader.Read())
                    {
                        reader.GetValues(objValues);
                        reEnt.AddRange(Encoding.UTF8.GetBytes(string.Join(",", objValues) + "\r\n"));

                    }
                    reader.Dispose();
                }
                return reEnt;
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog(typeof(DapperHelper), e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return null;
        }

        async public Task<string> ExecuteScalarAsync(string sql, object param = null)
        {
            var result = await connection.ExecuteScalarAsync(sql, param, transaction);
            if (result != null)
                return result.ToString();
            else
                return "";
        }

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            return connection.Query<T>(sql, param, transaction);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            return connection.QueryAsync<T>(sql, param, transaction);
        }

        public List<Dictionary<string, object>> Query(string sql, object param = null)
        {
            var rows = new List<Dictionary<string, object>>();
            using (var reader = connection.ExecuteReader(sql, param, transaction))
            {
                while (reader.Read())
                {
                    var dict = new Dictionary<string, object>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        dict[reader.GetName(i)] = reader.GetValue(i);
                    }
                    rows.Add(dict);
                }
            }
            return rows;
        }


        /// <summary>
        /// 获取Table
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, object param = null)
        {
            DataTable table = new DataTable("MyTable");
            connection.Open();
            try
            {
                var reader = connection.ExecuteReader(sql, param, transaction);
                if (reader != null)
                {
                    table.Load(reader);
                    reader.Dispose();
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog(typeof(DapperHelper), e.ToString());
            }
            finally
            {
                connection.Close();
            }

            return table;
        }

        public void Init(IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
             connection=dbConnection;
             transaction=dbTransaction;
        }
    }
    public class DapperHelper<T> : DapperHelper where T : class, new()
    {
        public ModelHelper<T> modelHelper;


        public DapperHelper()
            : base()
        {
            modelHelper = new ModelHelper<T>();
        }
        public DapperHelper(IDbConnection _connection, IDbTransaction _transaction)
            : base(_connection, _transaction)
        {
            modelHelper = new ModelHelper<T>();
        }

        public Task<int> Save(DtoSave<T> inEnt)
        {
            var mh = new ModelHelper<T>(inEnt.data);
            string sql = mh.GetSaveSql(null, inEnt.ignoreFieldList);
            var result = connection.ExecuteAsync(sql, mh.GetDynamicParameters(), transaction);
            return result;
        }

        public Task<int> Saves(DtoSave<List<T>> inEnt)
        {
            var mh = new ModelHelper<T>();
            string sql = mh.GetSaveSql(inEnt.saveFieldList, inEnt.ignoreFieldList);
            var result = connection.ExecuteAsync(sql, inEnt.data, transaction);
            return result;
        }


        public async Task<IEnumerable<T>> FindAll(DtoSearch inSearch)
        {
            string sql = this.modelHelper.GetFindAllSql(inSearch);
            return await connection.QueryAsync<T>(sql, this.modelHelper.GetDynamicParameters(), transaction);
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> FindAllS<T1, T2>(DtoSearch inSearch)
        {
            var reOjb = new Tuple<IEnumerable<T1>, IEnumerable<T2>>(null, null);
            string[] sqlArr = this.modelHelper.GetFindAllAndCountSql(inSearch);
            IEnumerable<T1> item1 = null;
            IEnumerable<T2> item2 = null;
            if (sqlArr.Count() > 0)
            {
                item1 = await connection.QueryAsync<T1>(sqlArr[0], this.modelHelper.GetDynamicParameters(), transaction);
            }
            if (sqlArr.Count() > 1)
            {
                item2 = await connection.QueryAsync<T2>(sqlArr[1], this.modelHelper.GetDynamicParameters(), transaction);
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
        }

        public async Task<IEnumerable<T>> FindAll(DtoSearch<T> inSearch)
        {

            List<KeyValuePair<string, object>> listSqlParaModel = new List<KeyValuePair<string, object>>();
            var whereStr = Helper.LambdaToSqlHelper.GetWhereSql<T>(inSearch.FilterList, listSqlParaModel);
            var dbField = this.modelHelper.GetTableFieldDirct();
            foreach (var item in dbField)
            {
                whereStr = whereStr.Replace(string.Format("({0})", item.Key), item.Value.ToString());
            }
            string sql = this.modelHelper.GetFindAllSql(inSearch, whereStr);
            return await connection.QueryAsync<T>(sql, listSqlParaModel, transaction);
        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> where)
        {
            string sql = "";
            IEnumerable<T> reList = new List<T>();
            if (where == null)
            {
                sql = modelHelper.GetFindAllSql();
                reList = (await connection.QueryAsync<T>(sql, null, transaction)).ToList();
            }
            else
            {
                List<KeyValuePair<string, object>> listSqlParaModel = new List<KeyValuePair<string, object>>();
                var whereStr = Helper.LambdaToSqlHelper.GetWhereSql<T>(where, listSqlParaModel);
                var dbField = this.modelHelper.GetTableFieldDirct();
                foreach (var item in dbField)
                {
                    whereStr = whereStr.Replace(string.Format("({0})", item.Key), item.Value.ToString());
                }
                sql = modelHelper.GetFindAllSql(whereStr);
                reList = await connection.QueryAsync<T>(sql, listSqlParaModel, transaction);
            }
            return reList;
        }

        /// <summary>
        /// 获取指定,字段的类
        /// </summary>
        /// <param name="whereStr">不带where的字符串</param>
        /// <returns></returns>
        public Task<IEnumerable<T>> FindAll(string whereStr, List<string> allItems = null)
        {
            string sql = modelHelper.GetFindAllSql(whereStr, allItems);
            return connection.QueryAsync<T>(sql, null, transaction);
        }

        /// <summary>
        /// 获取满足条件的个数
        /// </summary>
        /// <param name="inEnt"></param>
        /// <param name="inSearch"></param>
        /// <returns></returns>
        public async Task<int> FindNum(T inEnt, DtoSearch inSearch)
        {

            var mh = new ModelHelper<T>(inEnt);
            string sql = mh.GetFindNumSql(inSearch);
            var ds = await connection.ExecuteScalarAsync(sql, mh.GetDynamicParameters(), transaction);
            return Convert.ToInt32(ds);
        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<int> Count(Expression<Func<T, bool>> where)
        {
            var mh = new ModelHelper<T>();
            List<KeyValuePair<string, object>> listSqlParaModel = new List<KeyValuePair<string, object>>();
            var whereStr = Helper.LambdaToSqlHelper.GetWhereSql<T>(where, listSqlParaModel);
            var dbField = this.modelHelper.GetTableFieldDirct();
            foreach (var item in dbField)
            {
                whereStr = whereStr.Replace(string.Format("({0})", item.Key), item.Value.ToString());
            }

            string sql = mh.GetFindNumSql(whereStr);
            var ds = await connection.ExecuteScalarAsync(sql, listSqlParaModel, transaction);
            return Convert.ToInt32(ds);

        }

        /// <summary>
        /// 总数
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public async Task<int> Count(string whereStr)
        {
            var mh = new ModelHelper<T>();
            string sql = mh.GetFindNumSql(whereStr);
            var ds = await connection.ExecuteScalarAsync(sql, null, transaction);
            return Convert.ToInt32(ds);
        }

        /// <summary>
        /// 返回影响的条数
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns></returns>
        public Task<int> Update(DtoSave<T> inObj)
        {
            var mh = new ModelHelper<T>(inObj.data);
            string sql = mh.GetUpdateSql(inObj.saveFieldList, inObj.ignoreFieldList, inObj.whereList);
            var result = connection.ExecuteAsync(sql, mh.GetDynamicParameters(inObj.saveFieldList, inObj.whereList), transaction);
            return result;
        }

        public Task<int> Update(string upStr, string whereStr)
        {
            string sql = modelHelper.GetUpdateSql(upStr, whereStr);
            var result = connection.ExecuteAsync(sql, null, transaction);
            return result;
        }

        public Task<T> Single(Expression<Func<T, bool>> where, string order = "")
        {
            List<KeyValuePair<string, object>> listSqlParaModel = new List<KeyValuePair<string, object>>();
            var whereStr = Helper.LambdaToSqlHelper.GetWhereSql<T>(where, listSqlParaModel);
            var dbField = this.modelHelper.GetTableFieldDirct();
            foreach (var item in dbField)
            {
                whereStr = whereStr.Replace(string.Format("({0})", item.Key), item.Value.ToString());
            }

            string sql = modelHelper.GetSingleSql(whereStr, order);
            var query = connection.QueryFirstOrDefaultAsync<T>(sql, listSqlParaModel, transaction);
            return query;
        }

        public async Task<T> SingleByKey<t>(t key)
        {
            string sql = modelHelper.GetSingleSql();
            DynamicParameters dynamicP = new DynamicParameters();
            dynamicP.Add(modelHelper.GetKeyField().Key, key);

            var query = await connection.QueryFirstOrDefaultAsync<T>(sql, dynamicP, transaction);
            return query;
        }


        public Task<int> Delete(Expression<Func<T, bool>> where)
        {
            try
            {
                List<KeyValuePair<string, object>> listSqlParaModel = new List<KeyValuePair<string, object>>();
                var whereStr = Helper.LambdaToSqlHelper.GetWhereSql<T>(where, listSqlParaModel);
                var dbField = this.modelHelper.GetTableFieldDirct();
                foreach (var item in dbField)
                {
                    whereStr = whereStr.Replace(string.Format("({0})", item.Key), item.Value.ToString());
                }

                string sql = modelHelper.GetDeleteSql(whereStr);
                var query = connection.ExecuteAsync(sql, listSqlParaModel, transaction);
                return query;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog<DapperHelper<T>>(ex.ToString());
                throw new Exception("请先删除子项");
            }
        }

        public Task<int> Delete(string whereStr)
        {
            try
            {
                string sql = modelHelper.GetDeleteSql(whereStr);
                var query = connection.ExecuteAsync(sql, null, transaction);
                return query;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog<DapperHelper<T>>(ex.ToString());
                throw new Exception("请先删除子项");
            }
        }
    }
}
