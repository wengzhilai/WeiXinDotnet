namespace Models
{
    /// <summary>
    /// 
    /// </summary>
    public class QuartzTaskModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string keyName { get; set; }
        /// <summary>
        /// 组名称
        /// </summary>
        /// <value></value>
        public string keyGroup { get; set; }
        /// <summary>
        /// 传入的参数
        /// </summary>
        /// <value></value>
        public string jobDataListStr { get; set; }
        /// <summary>
        /// 日历名称
        /// </summary>
        /// <value></value>
        public string calendarName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string description { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <value></value>
        public string endTime { get; set; }

        /// <summary>
        /// 最后一次执行时间
        /// </summary>
        /// <value></value>
        public string finalFireTimeUtc { get; set; }
        /// <summary>
        /// 下次执行时间
        /// </summary>
        /// <value></value>
        public string nextFireTime { get; set; }
        /// <summary>
        /// 执行级别
        /// </summary>
        /// <value></value>
        public int priority { get; set; }
        /// <summary>
        /// 开始执行时间
        /// </summary>
        /// <value></value>
        public string startTimeUtc { get; set; }
    }
}