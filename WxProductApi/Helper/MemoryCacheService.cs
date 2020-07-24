using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;

public class MemoryCacheService : ICacheService
{
    public static IMemoryCache cache=new MemoryCache(new MemoryCacheOptions());
    protected IMemoryCache _cache;

    public MemoryCacheService()
    {
        _cache = MemoryCacheService.cache;
    }

    // private static IDatabase _cache;

    // public MemoryCacheService(IDatabase cache)
    // {
    //     _cache = cache;
    // }


    #region 检测缓存
    /// <summary>
    /// 验证缓存项是否存在
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    public bool Exists(string key)
    {
        if (key == null)
        {
            return false;
        }
        object cached;
        return _cache.TryGetValue(key, out cached);
    }
    #endregion



    #region 添加缓存

    /// <summary>
    /// 添加缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">缓存Value</param>
    /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
    /// <param name="expiressAbsoulte">绝对过期时长</param>
    /// <returns></returns>
    public bool Add<T>(string key, T value, TimeSpan? expiresSliding = null, TimeSpan? expiressAbsoulte = null)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (expiresSliding == null && expiressAbsoulte == null)
        {
            _cache.Set(key, value);
        }
        else
        {
            var opt = new MemoryCacheEntryOptions();
            if (expiresSliding != null) opt.SetSlidingExpiration(expiresSliding.Value);
            if (expiressAbsoulte != null) opt.SetSlidingExpiration(expiresSliding.Value);
            _cache.Set(key, value, opt);
        }
        return Exists(key);
    }

    #endregion

    #region 删除缓存

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    public bool Remove(string key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        _cache.Remove(key);

        return !Exists(key);
    }
    /// <summary>
    /// 批量删除缓存
    /// </summary>
    /// <param name="key">缓存Key集合</param>
    /// <returns></returns>
    public void RemoveAll(IEnumerable<string> keys)
    {
        if (keys == null)
        {
            throw new ArgumentNullException(nameof(keys));
        }

        keys.ToList().ForEach(item => _cache.Remove(item));
    }
    #endregion

    #region 获取缓存
    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    public T Get<T>(string key) where T : class, new()
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        return _cache.Get<T>(key);
    }


    public string Get(string key)
    {
        string reObj = "";
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        _cache.TryGetValue(key,out reObj);
        return reObj;
    }

    /// <summary>
    /// 获取缓存集合
    /// </summary>
    /// <param name="keys">缓存Key集合</param>
    /// <returns></returns>
    public List<T> GetAll<T>(List<string> keys) where T : class, new()
    {
        if (keys == null)
        {
            throw new ArgumentNullException(nameof(keys));
        }

        var dict = new List<T>();

        keys.ToList().ForEach(item => dict.Add(_cache.Get<T>(item)));

        return dict;
    }
    #endregion

    #region 修改缓存

    /// <summary>
    /// 修改缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">新的缓存Value</param>
    /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
    /// <param name="expiressAbsoulte">绝对过期时长</param>
    /// <returns></returns>
    public bool Replace<T>(string key, T value, TimeSpan? expiresSliding = null, TimeSpan? expiressAbsoulte = null)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (Exists(key))
            if (!Remove(key)) return false;

        return Add(key, value, expiresSliding, expiressAbsoulte);
    }

    public bool ReplaceHashSetKey<T>(string name, string key, object value, TimeSpan? expiresSliding = null, TimeSpan? expiressAbsoulte = null) where T : class, new()
    {
        T ent = Get<T>(name);
        if(ent==null) ent=new T();
        PropertyInfo[] proInfoArr = ent.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);//得到该类的所有公共属性
        PropertyInfo outproInfo = ent.GetType().GetProperty(key);
        if (outproInfo != null)
        {
            outproInfo.SetValue(ent, value, null);
        }
        return this.Add(name, ent, expiresSliding, expiressAbsoulte);
    }

    #endregion

    public void Dispose()
    {
        if (_cache != null)
            _cache.Dispose();
        GC.SuppressFinalize(this);
    }




}