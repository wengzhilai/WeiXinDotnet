
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Entity;

namespace Models.EntityView
{
    /// <summary>
    /// 用户扩展
    /// </summary>
    [Table("fa_user_info a left join fa_user b on a.ID=b.ID LEFT JOIN fa_user father on father.ID=a.FATHER_ID LEFT JOIN fa_user Couple on Couple.ID=a.COUPLE_ID LEFT JOIN fa_elder elder on elder.ID=a.ELDER_ID")]
    public class FaUserInfoEntityView
    {



        /// <summary>
        /// 父亲姓名
        /// </summary>
        [Column("father.`NAME`")]
        public string fatherName { get; set; }

        /// <summary>
        /// 辈分
        /// </summary>
        [Column("elder.`NAME`")]
        public string elderName { get; set; }

        /// <summary>
        /// 辈分排号
        /// </summary>
        [Column("elder.SORT")]
        public string elderSort { get; set; }

        /// <summary>
        /// 配偶姓名
        /// </summary>
        [Column("Couple.`NAME`")]
        public string coupleName { get; set; }


        /// <summary>
        /// 子女数
        /// </summary>
        [Column("(SELECT count(1) from fa_user_info x where x.FATHER_ID=a.ID)")]
        public int childNum { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        [Column("a.ID")]
        public int id { get; set; }
        /// <summary>
        /// LEVEL_ID
        /// </summary>
        [Display(Name = "LEVEL_ID")]
        [Column("a.LEVEL_ID")]
        public int levelId { get; set; }

        /// <summary>
        /// ELDER_ID
        /// </summary>
        [Display(Name = "ELDER_ID")]
        [Column("a.ELDER_ID")]
        public int elderId { get; set; }

        /// <summary>
        /// 父亲ID
        /// </summary>
        [Display(Name = "父亲ID")]
        [Column("a.FATHER_ID")]
        public int fatherId { get; set; }

        /// <summary>
        /// 配偶ID
        /// </summary>
        [Display(Name = "配偶ID")]
        [Column("a.COUPLE_ID")]
        public int coupleId { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Display(Name = "出生日期")]
        [Column("a.BIRTHDAY_TIME")]
        public Nullable<DateTime> birthdayTime { get; set; }
        /// <summary>
        /// 出生地点
        /// </summary>
        [Display(Name = "出生地点")]
        [Column("a.BIRTHDAY_PLACE")]
        public string birthdayPlace { get; set; }
        /// <summary>
        /// 是否健在
        /// </summary>
        [Display(Name = "是否健在")]
        [Column("a.IS_LIVE")]
        public decimal isLive { get; set; }
        /// <summary>
        /// 过逝日期
        /// </summary>
        [Display(Name = "过逝日期")]
        [Column("a.DIED_TIME")]
        public Nullable<DateTime> diedTime { get; set; }
        /// <summary>
        /// 过逝地点
        /// </summary>
        [Display(Name = "过逝地点")]
        [Column("a.DIED_PLACE")]
        public string diedPlace { get; set; }
        /// <summary>
        /// 日期类型
        /// </summary>
        [Display(Name = "日期类型")]
        [Column("a.YEARS_TYPE")]
        public string yearsType { get; set; }


        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        [Column("a.SEX")]
        public string sex { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        [Column("a.STATUS")]
        public string status { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        /// <value></value>
        [Display(Name = "别名")]
        [Column("a.ALIAS")]
        public string alias { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        [Display(Name = "权限")]
        [Column("a.AUTHORITY")]
        public int authority { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [Column("a.REMARK")]
        public string remark { get; set; }

        /// <summary>
        /// 教育背影
        /// </summary>
        /// <value></value>
        [Display(Name = "教育背影")]
        [Column("a.EDUCATION")]
        public string education { get; set; }

        /// <summary>
        /// 行业
        /// </summary>
        /// <value></value>
        [Display(Name = "行业")]
        [Column("a.INDUSTRY")]
        public string industry { get; set; }


        /// <summary>
        /// 出生国号
        /// </summary>
        /// <value></value>
        [Display(Name = "出生国号")]
        [Column("a.BIRTHDAY_CHINA_YEAR")]
        public string birthdayChinaYear { get; set; }

        /// <summary>
        /// 过逝国号
        /// </summary>
        /// <value></value>
        [Display(Name = "过逝国号")]
        [Column("a.DIED_CHINA_YEAR")]
        public string diedChinaYear { get; set; }

        /// <summary>
        /// 创建用户ID
        /// </summary>
        [Display(Name = "创建用户ID")]
        [Column("a.CREATE_USER_ID")]
        public int createUserId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Column("b.NAME")]
        public string name { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        [Display(Name = "登录名")]
        [Column("b.`LOGIN_NAME`")]
        public string loginName { get; set; }
        /// <summary>
        /// 头像图片
        /// </summary>
        [Display(Name = "头像图片")]
        [Column("b.`ICON_FILES`")]
        public string iconFiles { get; set; }
        /// <summary>
        /// 归属地
        /// </summary>
        [Display(Name = "归属地")]
        [Column("b.`DISTRICT_ID`")]
        public int districtId { get; set; }
        /// <summary>
        /// 锁定
        /// </summary>
        [Display(Name = "锁定")]
        [Column("b.`IS_LOCKED`")]
        public decimal isLocked { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column("b.`CREATE_TIME`")]
        public Nullable<DateTime> createTime { get; set; }

        /// <summary>
        /// 出生阴历
        /// </summary>
        /// <value></value>
        public string birthdaylunlarDate { get; set; }

        /// <summary>
        /// 出生阳历
        /// </summary>
        /// <value></value>
        public string birthdaysolarDate { get; set; }


        /// <summary>
        /// 逝世阴历
        /// </summary>
        /// <value></value>
        public string diedlunlarDate { get; set; }

        /// <summary>
        /// 逝世阳历
        /// </summary>
        /// <value></value>
        public string diedsolarDate { get; set; }

        /// <summary>
        /// 头像信息，只用于添加头像时有效
        /// </summary>
        /// <value></value>
        public FilesEntity IconFilesEnt { get; set; }
    }
}
