using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICacheService
{
    /// <summary>
    /// 验证缓存项是否存在
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    bool Exists(string key);





    /// <summary>
    /// 添加缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">缓存Value</param>
    /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
    /// <param name="expiressAbsoulte">绝对过期时长</param>
    /// <returns></returns>
    bool Add<T>(string key, T value, TimeSpan? expiresSliding = null, TimeSpan? expiressAbsoulte = null);





    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    bool Remove(string key);



    /// <summary>
    /// 批量删除缓存
    /// </summary>
    /// <param name="key">缓存Key集合</param>
    /// <returns></returns>
    void RemoveAll(IEnumerable<string> keys);



    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <returns></returns>
    T Get<T>(string key) where T : class, new();

    string Get(string key);



    /// <summary>
    /// 获取缓存集合
    /// </summary>
    /// <param name="keys">缓存Key集合</param>
    /// <returns></returns>
    List<T> GetAll<T>(List<string> keys)where T : class, new();





    /// <summary>
    /// 修改缓存
    /// </summary>
    /// <param name="key">缓存Key</param>
    /// <param name="value">新的缓存Value</param>
    /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
    /// <param name="expiressAbsoulte">绝对过期时长</param>
    /// <returns></returns>
    bool Replace<T>(string key, T value, TimeSpan? expiresSliding = null, TimeSpan? expiressAbsoulte = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name">缓存Key</param>
    /// <param name="key">字段名称</param>
    /// <param name="value">新的缓存Value</param>
    /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
    /// <param name="expiressAbsoulte">绝对过期时长</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    bool ReplaceHashSetKey<T>(string name, string key, object value, TimeSpan? expiresSliding = null, TimeSpan? expiressAbsoulte = null) where T : class, new();



}