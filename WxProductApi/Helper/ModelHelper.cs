using Dapper;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary>
    /// 用于，根据实体类，生成相应的SQL，以及数据验证
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelHelper<T> where T : new()
    {
        /// <summary>
        /// 传入的参数
        /// </summary>
        T inEnt { get; set; }
        public ModelHelper(T model)
        {
            inEnt = model;
        }

        public ModelHelper()
        {
            inEnt = new T();
        }
        public List<ValidationResult> Validate()
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(inEnt, new ValidationContext(inEnt, null, null), results, true);
            return results;
        }

        /// <summary>
        /// 获取所有字段列表,Key表示属性名，Value表示字段名
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetTableFields(List<string> saveFieldList = null, List<string> ignoreFieldList = null)
        {
            Dictionary<string, object> reFieldStr = new Dictionary<string, object>();   //所有数据字段名
            Type type = typeof(T);
            PropertyInfo[] PropertyList = type.GetProperties();//得到该类的所有公共属性
            foreach (PropertyInfo proInfo in PropertyList)
            {
                if (
                    (saveFieldList != null && saveFieldList.Count != 0 && !saveFieldList.Contains(proInfo.Name)) ||
                    (ignoreFieldList != null && ignoreFieldList.Count != 0 && ignoreFieldList.Contains(proInfo.Name))
                    )
                {
                    continue;
                }
                object[] attrsPi = proInfo.GetCustomAttributes(true);
                foreach (object obj in attrsPi)
                {
                    if (obj is ColumnAttribute)//定义了Display属性的字段为字数库字段
                    {
                        var column = (ColumnAttribute)obj;
                        reFieldStr.Add(proInfo.Name, string.IsNullOrEmpty(column.Name) ? proInfo.Name : column.Name);
                        continue;
                    }
                }
            }
            return reFieldStr;
        }


        Dictionary<string, object> _Dirct;
        /// <summary>
        /// 获取类的键值数据
        /// </summary>
        /// <param name="saveFieldList"></param>
        /// <param name="ignoreFieldList"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetDirct(List<string> saveFieldList = null, List<string> ignoreFieldList = null)
        {
            if (_Dirct == null)
            {

                Dictionary<string, object> reField = new Dictionary<string, object>();   //所有数据字段名

                Type type = typeof(T);
                PropertyInfo[] PropertyList = type.GetProperties();//得到该类的所有公共属性
                foreach (PropertyInfo proInfo in PropertyList)
                {
                    object[] attrsPi = proInfo.GetCustomAttributes(true);
                    foreach (object obj in attrsPi)
                    {
                        if (obj is DisplayAttribute)//定义了Display属性的字段为字数库字段
                        {
                            reField.Add(proInfo.Name, proInfo.GetValue(inEnt, null));
                            continue;
                        }
                    }
                }
                _Dirct = reField;
            }
            return _Dirct;
        }



        /// <summary>
        /// 获取Dapper的动态参数
        /// </summary>
        /// <param name="saveFieldList"></param>
        /// <param name="ignoreFieldList"></param>
        /// <returns></returns>
        public DynamicParameters GetDynamicParameters(List<string> saveFieldList = null, List<string> whereFieldList = null)
        {
            DynamicParameters reField = new DynamicParameters();
            Type type = typeof(T);

            var allFiled = new List<string>();

            #region 合并数据
            if (saveFieldList != null)
            {
                foreach (var item in saveFieldList)
                {
                    allFiled.Add(item);
                }
            }
            if (whereFieldList != null)
            {
                foreach (var item in whereFieldList)
                {
                    allFiled.Add(item);
                }
            }

            if (allFiled != null && allFiled.Count != 0)
            {
                allFiled.Add(GetKeyField().Key);
            }
            #endregion

            PropertyInfo[] PropertyList = type.GetProperties();//得到该类的所有公共属性
            foreach (PropertyInfo proInfo in PropertyList)
            {
                if (allFiled != null && allFiled.Count != 0 && !allFiled.Contains(proInfo.Name))
                {
                    continue;
                }
                object[] attrsPi = proInfo.GetCustomAttributes(true);
                foreach (object obj in attrsPi)
                {
                    if (obj is ColumnAttribute)//定义了Display属性的字段为字数库字段
                    {
                        object objV = proInfo.GetValue(inEnt, null);
                        //如果是主键并且，不是自增加，并且，值不空或为0
                        if (GetKeyField().Equals(proInfo.Name) && !GetKeyIsAuto() && (objV == null || objV.ToString() == "0"))
                        {
                            throw new Exception("主键没设置值");
                        }

                        if (objV == null)
                        {
                            objV = "";
                        }

                        reField.Add(proInfo.Name, objV);
                        continue;
                    }
                }
            }
            return reField;
        }
        public string GetGetStr(List<string> saveFieldList = null, List<string> ignoreFieldList = null)
        {
            var dictList = GetDirct(saveFieldList, ignoreFieldList);
            string reStr = string.Join("&", dictList.Select(x => x.Key + "=" + x.Value.ToString()).ToArray());
            return reStr;
        }


        Dictionary<string, object> _DisplayDirct;
        /// <summary>
        /// 获取类的每个字段的中文说明
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetDisplayDirct()
        {
            if (_DisplayDirct == null)
            {

                Dictionary<string, object> reField = new Dictionary<string, object>();   //所有数据字段名
                Type type = typeof(T);
                PropertyInfo[] PropertyList = type.GetProperties();//得到该类的所有公共属性
                foreach (PropertyInfo proInfo in PropertyList)
                {
                    object[] attrsPi = proInfo.GetCustomAttributes(true);
                    foreach (object obj in attrsPi)
                    {
                        if (obj is DisplayAttribute)//定义了Display属性的字段为字数库字段
                        {
                            var display = (DisplayAttribute)obj;
                            reField.Add(proInfo.Name, display.Name);
                            continue;
                        }
                    }
                }
                _DisplayDirct = reField;
            }
            return _DisplayDirct;
        }

        /// <summary>
        /// 获取属性对表字段
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetTableFieldDirct()
        {
            if (_DisplayDirct == null)
            {

                Dictionary<string, object> reField = new Dictionary<string, object>();   //所有数据字段名
                Type type = typeof(T);
                PropertyInfo[] PropertyList = type.GetProperties();//得到该类的所有公共属性
                foreach (PropertyInfo proInfo in PropertyList)
                {
                    object[] attrsPi = proInfo.GetCustomAttributes(true);
                    foreach (object obj in attrsPi)
                    {
                        if (obj is ColumnAttribute)//定义了Display属性的字段为字数库字段
                        {
                            var display = (ColumnAttribute)obj;
                            if (string.IsNullOrEmpty(display.Name))
                            {
                                reField.Add(proInfo.Name, proInfo.Name);
                            }
                            else
                            {
                                reField.Add(proInfo.Name, display.Name.Split(' ')[0]);
                            }
                            continue;
                        }
                    }
                }
                _DisplayDirct = reField;
            }
            return _DisplayDirct;
        }



        private string _TableName = null;
        /// <summary>
        /// 得到表名
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            if (_TableName == null)
            {

                Type type = typeof(T);
                Attribute[] attrs = Attribute.GetCustomAttributes(type);  // 得到对类的自定义属性数组
                foreach (System.Attribute attr in attrs)
                {
                    if (attr is TableAttribute)
                    {
                        TableAttribute a = (TableAttribute)attr;
                        _TableName = a.Name;
                    }
                }
            }
            return _TableName;
        }

        /// <summary>
        /// 获取主键,Key为属性名，Value为数据名
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, string> GetKeyField()
        {
            var reObj = new KeyValuePair<string, string>();
            Type type = typeof(T);
            PropertyInfo[] proInfoArr = type.GetProperties();//得到该类的所有公共属性
            for (int a = 0; a < proInfoArr.Length; a++)
            {
                PropertyInfo proInfo = proInfoArr[a];
                object[] attrsPro = proInfo.GetCustomAttributes(typeof(KeyAttribute), true);
                if (attrsPro.Length > 0)
                {

                    object[] attrsColumn = proInfo.GetCustomAttributes(typeof(ColumnAttribute), true);
                    if (attrsColumn.Length > 0 && !string.IsNullOrEmpty(((ColumnAttribute)attrsColumn[0]).Name))
                    {
                        reObj = new KeyValuePair<string, string>(proInfo.Name, ((ColumnAttribute)attrsColumn[0]).Name);
                    }
                    else
                    {
                        reObj = new KeyValuePair<string, string> (proInfo.Name, proInfo.Name );
                    }
                    
                    break;
                }
            }
            return reObj;
        }



        /// <summary>
        /// 获取Key是否是自动生成的
        /// </summary>
        /// <returns></returns>
        public bool GetKeyIsAuto()
        {

            //默认是自动生成
            bool _IsAuto = true;
            Type type = typeof(T);
            PropertyInfo[] proInfoArr = type.GetProperties();//得到该类的所有公共属性
            for (int a = 0; a < proInfoArr.Length; a++)
            {
                PropertyInfo proInfo = proInfoArr[a];
                object[] attrsPro = proInfo.GetCustomAttributes(typeof(KeyAttribute), true);
                if (attrsPro.Length > 0)
                {
                    object[] attrsGenerated = proInfo.GetCustomAttributes(typeof(DatabaseGeneratedAttribute), true);
                    if (attrsGenerated.Length > 0)
                    {
                        _IsAuto = ((DatabaseGeneratedAttribute)attrsGenerated[0]).DatabaseGeneratedOption != DatabaseGeneratedOption.Computed;
                    }
                    else
                    {
                        _IsAuto = true;
                    }
                    break;
                }
            }
            return _IsAuto;
        }

        /// <summary>
        /// 新增加数据，并返回增加的ID
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetSaveSql(List<string> saveFieldList = null, List<string> ignoreFieldList = null)
        {
            string sql = null;
            //如果没设置并且主键是自动生成
            if (GetKeyIsAuto())
            {
                if (ignoreFieldList == null)
                {
                    ignoreFieldList = new List<string> { GetKeyField().Key };
                }
                else
                {
                    ignoreFieldList.Add(GetKeyField().Key);
                }
            }
            var saveDict = GetTableFields(saveFieldList, ignoreFieldList);
            sql = "INSERT INTO  " + GetTableName() + "(" + string.Join(",", saveDict.Select(x=>x.Value)) + ") VALUES(" + string.Join(",", saveDict.Select(x => "@" + x.Key)) + ");";
            if (GetKeyIsAuto())
            {
                //sql += "\r\n select @@IDENTITY ";
                sql += "\r\n SELECT LAST_INSERT_ID(); ";
                
            }
            return sql;
        }

        public string GetSaveSqlNoIdentity(List<string> saveFieldList = null)
        {
            string sql = null;
            var ignoreFieldList = new List<string> { GetKeyField().Key };
            sql = "INSERT INTO  " + GetTableName() + "(" + string.Join(",", GetTableFields(saveFieldList, ignoreFieldList).Select(x => x.Value)) + ") VALUES(" + string.Join(",", GetTableFields(saveFieldList, ignoreFieldList).Select(x => "@" + x.Value)) + ")";
            return sql;
        }

        /// <summary>
        /// 生成查询的SQL语句
        /// 如果没有传条件值，则默认为主键
        /// </summary>
        /// <param name="saveFieldList">保存的字段</param>
        /// <param name="ignoreFieldList">忽略的字段</param>
        /// <param name="whereList">条件字段</param>
        /// <returns></returns>
        public string GetUpdateSql(List<string> saveFieldList = null, List<string> ignoreFieldList = null, List<string> whereList = null)
        {
            string key = GetKeyField().Key;
            if (ignoreFieldList == null) ignoreFieldList = new List<string> { key };
            if (whereList == null) whereList = new List<string> { key };
            string sql = null;
            sql = "UPDATE " + GetTableName() + " SET ";
            sql += string.Join(",", GetTableFields(saveFieldList, ignoreFieldList).Select(x => string.Format("{0}=@{1}", x.Value,x.Key)));
            sql += " WHERE ";
            sql += string.Join(" AND ", whereList.Select(x => string.Format("{0}=@{0}", x)));
            return sql;
        }

        public string GetUpdateSql(string upStr, string whereStr)
        {
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}", GetTableName(), upStr, whereStr);
            return sql;
        }

        /// <summary>
        /// 获取删除SQL
        /// </summary>
        /// <param name="filterList"></param>
        /// <returns></returns>
        public string GetDeleteSql(List<string> filterList = null)
        {
            if (filterList == null || filterList.Count() == 0)
            {
                throw new Exception("删除参数，不能为空");
            }

            string sql = "DELETE FROM {0} WHERE {1}";
            sql = string.Format(sql, GetTableName(), string.Join(" AND ", filterList.Select(x => string.Format("{0}=@{0}", x))));
            return sql;
        }

        public string GetDeleteSql(string whereStr)
        {
            if (string.IsNullOrEmpty(whereStr))
            {
                throw new Exception("删除参数，不能为空");
            }

            string sql = "DELETE FROM {0} WHERE {1}";
            sql = string.Format(sql, GetTableName(), whereStr);
            return sql;
        }

        /// <summary>
        /// 获取查看所有SQL
        /// </summary>
        /// <param name="inSearch"></param>
        /// <returns></returns>
        public string GetFindAllSql(DtoSearch inSearch)
        {
            string key = GetKeyField().Key;
            if (string.IsNullOrEmpty(inSearch.OrderType))
            {
                inSearch.OrderType = string.Format("{0} DESC", key);
            }
            string whereSql = "";
            if (inSearch.FilterList != null && inSearch.FilterList.Count() > 0)
            {
                var whereDict = GetTableFields(inSearch.FilterList.Select(x=>x.Key).ToList());
                whereSql = " where " + string.Join(" AND ", whereDict.Select(x => string.Format("{0}=@{1}", x.Value,x.Key)));
            }
            if (inSearch.PageIndex < 1) inSearch.PageIndex = 1;
            if (inSearch.PageSize < 1) inSearch.PageSize = 10;
            string sql = string.Format(@"
                select {0} from {1} {2} ORDER  BY  {5} limit {3},{4};
                ",
                string.Join(",", GetTableFields(null, inSearch.IgnoreFieldList).Select(x => x.Value + " " + x.Key)),
                GetTableName(),
                whereSql,
                (inSearch.PageIndex - 1) * inSearch.PageSize,
                inSearch.PageSize,
                inSearch.OrderType
                );
            return sql;
        }


        public string[] GetFindAllAndCountSql(DtoSearch inSearch)
        {
            string key = GetKeyField().Value;
            if (string.IsNullOrEmpty(inSearch.OrderType))
            {
                inSearch.OrderType = string.Format("{0} DESC", key);
            }
            string whereSql = "";
            if (inSearch.FilterList != null && inSearch.FilterList.Count() > 0)
            {
                whereSql = " where " + string.Join(" AND ", inSearch.FilterList.Select(x => string.Format("{0}=@{0}", x)));
            }
            if (inSearch.PageIndex < 1) inSearch.PageIndex = 1;
            if (inSearch.PageSize < 1) inSearch.PageSize = 10;
            string sql = string.Format(@"
                select {0} from {1} {2} ORDER  BY  {5} limit {3},{4};
                select count(1) V from {1} {2}
                ",
                string.Join(",", GetTableFields(null, inSearch.IgnoreFieldList).Select(x => x.Value + " " + x.Key)),
                GetTableName(),
                whereSql,
                (inSearch.PageIndex - 1) * inSearch.PageSize,
                inSearch.PageSize,
                inSearch.OrderType
                );
            return sql.Split(';');
        }

        public string GetFindAllSql(DtoSearch<T> inSearch, string whereSql = "")
        {
            if (inSearch.OrderType == null)
            {
                string key = GetKeyField().Value;
                inSearch.OrderType = string.Format("{0} DESC", key);
            }

            if (!string.IsNullOrEmpty(whereSql))
            {
                whereSql = " where " + whereSql;
            }

            if (inSearch.PageIndex < 1) inSearch.PageIndex = 1;
            if (inSearch.PageSize < 1) inSearch.PageSize = 10;
            string sql = string.Format(@"
                select {0} from {1} {2} ORDER  BY  {5} limit {3},{4};
                ",
                string.Join(",", GetTableFields(null, inSearch.IgnoreFieldList).Select(x => x.Value + " " + x.Key)),
                GetTableName(),
                whereSql,
                (inSearch.PageIndex - 1) * inSearch.PageSize,
                inSearch.PageSize,
                inSearch.OrderType
                );
            return sql;
        }

        public string GetFindAllSql(List<string> filterList)
        {
            if (filterList == null || filterList.Count() == 0)
            {
                return GetFindAllSql();
            }
            else
            {
                return GetFindAllSql(string.Join(" AND ", filterList.Select(x => string.Format("{0}=@{0}", x))));
            }
        }

        public string GetFindAllSql(string whereStr = null, List<string> allItems = null)
        {
            string sql = "SELECT {0} FROM {1} WHERE {2}";
            if (string.IsNullOrEmpty(whereStr))
            {
                sql = "SELECT {0} FROM {1}";
                sql = string.Format(sql, string.Join(",", GetTableFields(allItems).Select(x=>x.Value +" "+x.Key)), GetTableName());
            }
            else
            {
                sql = string.Format(sql, string.Join(",", GetTableFields(allItems).Select(x => x.Value + " " + x.Key)), GetTableName(), whereStr);
            }
            return sql;
        }

        /// <summary>
        /// 查看满足条件的数量
        /// </summary>
        /// <param name="inSearch"></param>
        /// <returns></returns>
        public string GetFindNumSql(DtoSearch inSearch)
        {
            string sql = "SELECT Count(1) num FROM {0} WHERE {1}";
            if (inSearch.FilterList == null)
            {
                sql = "SELECT  Count(1) num FROM {0}";
                sql = string.Format(sql, GetTableName());
            }
            else
            {
                sql = string.Format(sql,
                                GetTableName(),
                                string.Join(" AND ", inSearch.FilterList.Select(x => string.Format("convert(nvarchar(max),{0})=@{0}", x.Key)))
                                );
            }
            return sql;
        }

        public string GetFindNumSql(List<string> filterList)
        {
            if (filterList == null || filterList.Count() == 0)
            {
                return GetFindNumSql();
            }
            else
            {
                return GetFindNumSql(string.Join(" AND ", filterList.Select(x => string.Format("convert(nvarchar(max),{0})=@{0}", x))));

            }
        }

        public string GetFindNumSql(string whereStr = null)
        {

            string sql = "SELECT Count(1) num FROM {0} WHERE {1}";
            if (string.IsNullOrEmpty(whereStr))
            {
                sql = "SELECT  Count(1) num FROM {0}";
                sql = string.Format(sql, GetTableName());
            }
            else
            {
                sql = string.Format(sql, GetTableName(), whereStr);
            }
            return sql;
        }


        /// <summary>
        /// 获取单条SQL
        /// </summary>
        /// <param name="filterList"></param>
        /// <returns></returns>
        public string GetSingleSql(List<string> filterList, string orderByStr = "")
        {
            string sql = "SELECT {0} FROM {1} WHERE {2} {3}";
            if (filterList == null || filterList.Count() == 0)
            {
                sql = "SELECT  {0} FROM {1} {2}";
                sql = string.Format(sql, string.Join(",", GetTableFields().Select(x => x.Value + " " + x.Key)), GetTableName(), orderByStr);
            }
            else
            {
                sql = string.Format(sql, string.Join(",", GetTableFields().Select(x => x.Value + " " + x.Key)), GetTableName(), string.Join(" AND ", filterList.Select(x => string.Format("{0}=@{0}", x))), orderByStr);
            }
            return sql;
        }

        /// <summary>
        /// 获取单条SQL 根据主键获取单条
        /// </summary>
        /// <returns></returns>
        public string GetSingleSql()
        {
            var key = GetKeyField();
            string sql = "SELECT  {0} FROM {1} WHERE {2}=@{3}";
            sql = string.Format(sql, string.Join(",", GetTableFields().Select(x => x.Value + " " + x.Key)), GetTableName(), key.Value, key.Key);
            return sql;
        }

        public string GetSingleSql(string whereStr = "", string orderByStr = "")
        {
            var key = GetKeyField();
            string sql = "SELECT  {0} FROM {1} WHERE {2}=@{3} {4}";
            if (string.IsNullOrEmpty(whereStr))
            {
                sql = string.Format(sql, string.Join(",", GetTableFields().Select(x => x.Value + " " + x.Key)), GetTableName(), key.Value,key.Key, orderByStr);
            }
            else
            {
                sql = "SELECT  {0} FROM {1} WHERE {2} {3}";
                sql = string.Format(sql, string.Join(",", GetTableFields().Select(x => x.Value + " " + x.Key)), GetTableName(), whereStr, orderByStr);

            }
            return sql;
        }


    }
}
