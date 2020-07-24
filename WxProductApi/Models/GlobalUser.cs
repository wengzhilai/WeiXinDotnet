using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Models
{
    public class GlobalUser
    {
        public int loginHistoryId { get; set; }

        /// <summary>
        /// 登录凭证
        /// </summary>
        [Required]
        public string Ticket { get; set; }
        /// <summary>
        /// 登录凭证
        /// </summary>
        [Required, MaxLength(36)]
        public string Guid { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 当前节点ID
        /// </summary>
        public int DistrictId { get; set; }
        /// <summary>
        /// 当前节点代码
        /// </summary>
        public string DistrictCode { get; set; }
        /// <summary>
        /// 用户层级
        /// </summary>
        public int LevelId { get; set; }
        /// <summary>
        /// 当前区域
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 用户权限ID
        /// </summary>
        public IList<int> RoleID { get; set; }

        /// <summary>
        /// 区域层级
        /// </summary>
        public IList<string> RegionList { get; set; }

        /// <summary>
        /// 用户管辖区域
        /// </summary>
        public string RuleRegionStr { get; set; }

        /// <summary>
        /// 最后操作时间
        /// </summary>
        public DateTime LastOpTime { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIP { get; set; }

        /// <summary>
        /// 区域层级
        /// </summary>
        public string GetRegionLeveStr()
        {
            return string.Join(",", RegionList.Select(x => "'" + x + "'"));
        }

        /// <summary>
        /// 所有角色字符串
        /// </summary>
        public string GetRoleAllStr()
        {
            return string.Join(",", RoleID);
        }
    }
}