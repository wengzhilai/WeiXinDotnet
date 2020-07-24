
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 用于查询和删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DtoDo<T>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public DtoDo()
        {

        }
        /// <summary>
        /// 传入的值
        /// </summary>
        public T Key { get; set; }
    }

    public class DtoKey
    {
        /// <summary>
        /// 传入的值
        /// </summary>
        public string Key { get; set; }
    }
}