using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// 主键 类型 值
    /// </summary>
    public class KTV:KV
    {
        public KTV()
        {
            TClass =new KV();
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string t { get; set; }
        public KV TClass { get; set; }
        public IList<KTV> child;

    }
}