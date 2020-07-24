
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Models
{
    /// <summary>
    /// 用于查询
    /// </summary>
    public class DtoSearch
    {
        public DtoSearch()
        {
            PageIndex = 1;
            PageSize = 10;
            FilterList = new Dictionary<string, object>();
        }
        /// <summary>
        /// 筛选条件
        /// </summary>
        public Dictionary<string, object> FilterList { get; set; }
        /// <summary>
        /// 排序字符串，包括字段和类型，如 ："ID DESC"
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 当前页面
        /// </summary>
        [Range(1, 10000, ErrorMessage = "{0}不能小于1")]
        public int PageIndex { get; set; }
        /// <summary>
        /// 页码大小
        /// </summary>
        [Range(1, 100, ErrorMessage = "{0}值不能小于1")]
        public int PageSize { get; set; }

        /// <summary>
        /// 查询时需要忽略的字段
        /// </summary>
        public List<string> IgnoreFieldList { get; set; }
    }


    public class DtoSearch<T>
    {
        public DtoSearch()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        /// <summary>
        /// 筛选条件
        /// </summary>
        public Expression<Func<T, bool>> FilterList { get; set; }
        /// <summary>
        /// 排序字符串，包括字段和类型，如 ："ID DESC"
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 当前页面
        /// </summary>
        [Range(1, 10000, ErrorMessage = "{0}不能小于1")]
        public int PageIndex { get; set; }
        /// <summary>
        /// 页码大小
        /// </summary>
        [Range(1, 10000, ErrorMessage = "{0}值不能小于1")]
        public int PageSize { get; set; }

        /// <summary>
        /// 查询时需要忽略的字段
        /// </summary>
        public List<string> IgnoreFieldList { get; set; }
    }


}
