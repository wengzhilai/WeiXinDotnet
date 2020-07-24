using System.Collections.Generic;

namespace Models
{
        /// <summary>
    /// 用户登录实体
    /// </summary>
    public class SmartTableSetting
    {
        
        public string HEARD_BTN{ get; set; }
        public string ROWS_BTN{ get; set; }
        public bool SHOW_CHECKBOX{ get; set; }
        public List<SmartTableColumnSetting> ColumnsList{ get; set; }
    }

    public class SmartTableColumnSetting{
        public string ColumnName { get; set; }
        
        public string title { get; set; }
        public string width { get; set; }
        
        public bool filter { get; set; }
        public bool editable { get; set; }
        public bool sort { get; set; }
        
        /// <summary>
        /// 'text'|'html'|'custom'
        /// </summary>
        /// <value></value>
        public string type { get; set; }

        public ColumnEditorSetting editor { get; set; }

        public bool Show { get; set; }
    }

    public class ColumnEditorSetting{
        /// <summary>
        /// 'text' | 'textarea' | 'completer' | 'list' | 'checkbox'
        /// </summary>
        /// <value></value>
        public string type { get; set; }
        /// <summary>
        ///  completer, list
        /// </summary>
        /// <value></value>
        public ColumnEditorConfigSetting config { get; set; }
    }
    public class ColumnEditorConfigSetting{
        public List<ColumnEditorConfigSettingList> list  { get; set; }
    }

    public class ColumnEditorConfigSettingList{
        public string value { get; set; }
        public string title { get; set; }
    }
}