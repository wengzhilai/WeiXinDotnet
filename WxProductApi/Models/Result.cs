using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Result
    {

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <value></value>
        public bool success { get; set; } = true;
        /// <summary>
        /// 消息
        /// </summary>
        /// <value></value>
        public string msg { get; set; } = "";
        /// <summary>
        /// 代码
        /// </summary>
        /// <value></value>
        public string code { get; set; } = "";
    }
    /// <summary>
    /// 返回的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultObj<T> : Result
    {
        public ResultObj()
        {

        }

        public ResultObj(bool success,string msg)
        {
            this.success = success;
            this.msg = msg;
        }


        /// <summary>
        /// 返回数据
        /// </summary>
        /// <value></value>
        public T data { get; set; }
        /// <summary>
        /// 返回列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> dataList { get; set; } = new List<T>();

        /// <summary>
        /// 备注字段
        /// </summary>
        /// <value></value>
        public object tmp { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int total { get; set; }
    }

    /// <summary>
    /// 分页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultPaging<T> : ResultObj<T>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int currentPage { get; set; }
        /// <summary>
        /// 页面条数
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPage { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int totalCount { get; set; }
    }
}