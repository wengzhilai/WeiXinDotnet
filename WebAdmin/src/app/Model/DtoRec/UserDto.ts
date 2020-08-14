export class UserDto {
           /// <summary>
        /// ID
        /// </summary>
        
        ID:number;
        /// <summary>
        /// 姓名
        /// </summary>
        NAME:string;

        /// <summary>
        /// 登录名
        /// </summary>
        LOGIN_NAME:string;
 
        /// <summary>
        /// 头像图片
        /// </summary>
        ICON_FILES_ID:any;

        /// <summary>
        /// 归属地
        /// </summary>
        DISTRICT_ID:number;

        /// <summary>
        /// 锁定
        /// </summary>
        IS_LOCKED:any;

        /// <summary>
        /// 创建时间
        /// </summary>
        CREATE_TIME:any;

        /// <summary>
        /// 登录次数
        /// </summary>
        LOGIN_COUNT:any;

        /// <summary>
        /// 最后登录时间
        /// </summary>
        LAST_LOGIN_TIME:any;
  
        /// <summary>
        /// 最后离开时间
        /// </summary>
        LAST_LOGOUT_TIME:any;

        /// <summary>
        /// 最后活动时间
        /// </summary>
        LAST_ACTIVE_TIME:any;

        /// <summary>
        /// 备注
        /// </summary>
        REMARK:string;

        /** 超级管理员 */
        IsAdmin:boolean
        /** 普通管理员 */
        IsLeader:Boolean

}