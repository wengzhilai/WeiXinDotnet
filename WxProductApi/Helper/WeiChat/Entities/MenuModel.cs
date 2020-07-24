using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.WeiChat.Entities
{


    /// <summary>
    /// 微信菜单
    /// </summary>
    public class MenuModel
    {
        /// <summary>
        /// 按钮
        /// </summary>
        public LinkedList<MenuNodeModel> button { get; set; }
    }

    public class MenuNodeModel { 

        /// <summary>
        /// 类型 click,view
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>

        public LinkedList<MenuNodeModel> sub_button { get; set; }
    }
}
