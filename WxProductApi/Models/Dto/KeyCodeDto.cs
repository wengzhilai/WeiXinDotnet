
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// key和code
    /// </summary>
    public class KeyCodeDto
    {
        /// <summary>
        /// key
        /// </summary>
        /// <value></value>
        public string key { get; set; }

        public string code { get; set; }
    }
}