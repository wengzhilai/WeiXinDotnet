
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Models.Entity
{
    /// <summary>
    /// 查询
    /// </summary>
    [Table("fa_query")]
    public class FaQueryEntity
    {
        public string _DictStr;

        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Display(Name = "ID")]
        [Column("ID")]
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        [Display(Name = "名称")]
        [Column("NAME")]
        public string name { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        [StringLength(20)]
        [Display(Name = "代码")]
        [Column("CODE")]
        public string code { get; set; }
        /// <summary>
        /// 自动加载
        /// </summary>
        [Display(Name = "自动加载")]
        [Column("AUTO_LOAD")]
        public bool autoLoad { get; set; }
        /// <summary>
        /// 页面大小
        /// </summary>
        [Display(Name = "页面大小")]
        [Column("PAGE_SIZE")]
        public int pageSize { get; set; }
        /// <summary>
        /// 复选框
        /// </summary>
        [Display(Name = "复选框")]
        [Column("SHOW_CHECKBOX")]
        public bool showCheckbox { get; set; }

        /// <summary>
        /// 调试
        /// </summary>
        [Display(Name = "调试")]
        [Column("IS_DEBUG")]
        public bool isDebug { get; set; }

        /// <summary>
        /// 查询语句
        /// </summary>
        [Display(Name = "查询语句")]
        [Column("QUERY_CONF")]
       
        public string queryConf { get; set; }
        /// <summary>
        /// 配置信息
        /// </summary>
        [Display(Name = "配置信息")]
        [Column("QUERY_CFG_JSON")]
        public string queryCfgJson { get; set; }
        /// <summary>
        /// 传入参数
        /// </summary>
        [Display(Name = "传入参数")]
        [Column("IN_PARA_JSON")]
        public string inParaJson { get; set; }
        /// <summary>
        /// JS脚本
        /// </summary>
        [Display(Name = "JS脚本")]
        [Column("JS_STR")]
        public string jsStr { get; set; }
        /// <summary>
        /// 行按钮
        /// </summary>
        [Display(Name = "行按钮")]
        [Column("ROWS_BTN")]
        public string rowsBtn { get; set; }
        /// <summary>
        /// 表头按钮
        /// </summary>
        [Display(Name = "表头按钮")]
        [Column("HEARD_BTN")]
        public string heardBtn { get; set; }
        /// <summary>
        /// 报表脚本
        /// </summary>
        [Display(Name = "报表脚本")]
        [Column("REPORT_SCRIPT")]
        public string reportScript { get; set; }

        /// <summary>
        /// 图表配置
        /// </summary>
        [Display(Name = "图表配置")]
        [Column("CHARTS_CFG")]
        public string chartsCfg { get; set; }


        /// <summary>
        /// 图表类型
        /// </summary>
        [StringLength(50)]
        [Display(Name = "图表类型")]
        [Column("CHARTS_TYPE")]
        public string chartsType { get; set; }


        /// <summary>
        /// 统计范围
        /// </summary>
        [Display(Name = "统计范围")]
        [Column("FILTR_LEVEL")]
        public short filtrLevel { get; set; }

        /// <summary>
        /// 过滤配置
        /// </summary>
        [Display(Name = "过滤配置")]
        [Column("FILTR_STR")]
        public string filtrStr { get; set; }

        /// <summary>
        /// 查询库
        /// </summary>
        [Display(Name = "查询库")]
        [Column("DB_SERVER_ID")]
        public int dbServerId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明")]
        [Column("REMARK")]
        public string remark { get; set; }

        /// <summary>
        /// 最新时间
        /// </summary>
        [StringLength(50)]
        [Display(Name = "最新时间")]
        [Column("NEW_DATA")]
        public string new_data { get; set; }

        public string _dictQueryCfgStr { get; set; }
    }

    public class QuerySearchDto
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// 排序类型 asc|desc
        /// </summary>
        public string order { get; set; }
        public string code { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 每页显示数
        /// </summary>
        public int rows { get; set; }
        private string _OrderStr;
        public string orderStr
        {
            get
            {
                if (string.IsNullOrEmpty(_OrderStr))
                {
                    _OrderStr = string.Format("{0} {1}", sort, order);
                }
                return _OrderStr;
            }
            set {
                _OrderStr = value;
            }
        }
        /// <summary>
        /// 简单筛选条件
        /// </summary>
        public List<QueryPara> paraList { get; set; }
        /// <summary>
        /// 复杂筛选条件
        /// </summary>
        public IList<QueryRowBtnShowCondition> whereList { get; set; }
        public string whereListStr { get; set; }
        public string paraListStr { get; set; }

    }

    /// <summary>
    /// 参数
    /// </summary>
    public class QueryPara
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string paraName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string value { get; set; }
    }
    /// <summary>
    /// 配置
    /// </summary>
    public class QueryCfg
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        [Display(Name = "字段名称")]
        public string fieldName { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        [Display(Name = "别名")]
        public string alias { get; set; }
        /// <summary>
        /// 是否可搜索
        /// </summary>
        [Display(Name = "是否可搜索")]
        public bool canSearch { get; set; }

        /// <summary>
        /// 过滤控件类型
        /// </summary>
        [Display(Name = "过滤控件类型")]
        public string searchType { get; set; }

        /// <summary>
        /// 过滤控件脚本
        /// </summary>
        [Display(Name = "过滤控件脚本")]
        public string searchScript { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        [Display(Name = "是否显示")]
        public bool show { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        [Display(Name = "宽度")]
        public string width { get; set; }
        /// <summary>
        /// 可排序
        /// </summary>
        [Display(Name = "可排序")]
        public bool sortable { get; set; }

        /// <summary>
        /// 变量
        /// </summary>
        [Display(Name = "变量")]
        public string isVariable { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        [Display(Name = "字段类型")]
        public string fieldType { get; set; }
        /// <summary>
        /// 格式化
        /// </summary>
        [Display(Name = "格式化")]
        public string format { get; set; }
    }


    /// <summary>
    /// 行按键显示条件
    /// </summary>
    public class QueryRowBtnShowCondition
    {
        /// <summary>
        /// 对象字段
        /// </summary>
        [Display(Name = "对象字段")]
        public string objFiled { get; set; }
        /// <summary>
        /// 操作符 如：between、in、《、》、=、like
        /// </summary>
        [Display(Name = "操作符")]
        public string opType { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [Display(Name = "值")]
        public string value { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [Display(Name = "字段名称")]
        public string fieldName { get; set; }

        public string fieldType { get; set; }
    }


}
