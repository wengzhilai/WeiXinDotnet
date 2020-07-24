namespace Models
{
    /// <summary>
    /// 关系数据
    /// </summary>
    public class RelativeItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 辈字排号
        /// </summary>
        public int ElderId { get; set; }
        /// <summary>
        /// 辈字
        /// </summary>
        public string ElderName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父亲ID
        /// </summary>
        public int? FatherId { get; set; }
        /// <summary>
        /// 头像图片
        /// </summary>
        public string IcoUrl { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 坐标X
        /// </summary>
        public int x { get; set; }
        /// <summary>
        /// 坐标Y
        /// </summary>
        public int y { get; set; }

        public string Authority { get; set; }
        public string CreateUserId { get; set; }

        /// <summary>
        /// 完成比例
        /// </summary>
        public int CompletionRatio{get;set;}

    }
}