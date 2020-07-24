using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;


namespace Helper
{

    public class RedisReadHelper
    {

        //private static MemoryCacheService cache = new MemoryCacheService();
        private static ICacheService cache = new MemoryCacheService();
        
        /// <summary>
        /// 根据名称获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Tuple<T, Result> GetObject<T>(string name) where T : class, new()
        {

            Result result = new Result();
            T reEnt = cache.Get<T>(name);
            result.success = true;
            return Tuple.Create<T, Result>(reEnt, result);
        }

        /// <summary>
        /// 根据名称获取，对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="names"></param>
        /// <returns></returns>
        public static List<T> GetObjects<T>(List<string> names) where T : class, new()
        {
            List<T> reEnts = new List<T>();
            foreach (var name in names)
            {
                Result result = new Result();
                var tmp = GetObject<T>(name);
                result = tmp.Item2;
                reEnts.Add(tmp.Item1);
            }
            return reEnts;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T StringGet<T>(string key) where T : class, new()
        {
            return cache.Get<T>(key);
        }
        public static string StringGet(string key)
        {
            return cache.Get(key);
        }



        /// <summary>
        /// 判断Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyExists(string key)
        {
            return cache.Exists(key);
        }
    }
    public class RedisWriteHelper
    {
        private static ICacheService cache = new MemoryCacheService();

        #region 操作hash数据类型


        /// <summary>
        /// 实体类，保存到redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="inObj"></param>
        /// <param name="saveItems"></param>
        /// <returns></returns>
        public static bool SetObject<T>(string name, T inObj) where T : new()
        {
            return cache.Add(name, inObj);
        }

        public static bool SetString(string name, string inObj, TimeSpan? expiressAbsoulte = null)
        {
            return cache.Add(name, inObj, expiressAbsoulte);
        }

        /// <summary>
        /// 将key中存储的哈希中的字段设置为value。 如果key不存在，则创建一个包含哈希的新密钥。 如果哈希中已存在字段，则会覆盖该字段。
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static bool HashSetKey<T>(string hash, string key, string value) where T : class, new()
        {  
            return cache.ReplaceHashSetKey<T>(hash, key, value);
        }



        #endregion

        #region 删除
        /// <summary>
        /// 删除根据Key删除
        /// </summary>
        /// <param name="hash">keyName</param>
        public static bool KeyDelete(string keyName)
        {
            return cache.Remove(keyName);
        }

        /// <summary>
        /// 删除hask的Key
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HashDelete<T>(string hash, string key)where T : class, new()
        {
            return cache.ReplaceHashSetKey<T>(hash, key,"");
        }

        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool StringHashSetKey<T>(string hash, string key, string value)where T : class, new()
        {
            return cache.ReplaceHashSetKey<T>(hash, key, value);
        }
        #endregion

    }

}