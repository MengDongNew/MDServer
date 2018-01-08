using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Modal
{
    public class ServerProperty
    {
        public virtual int ID { get; set; }
        public virtual string IP { get; set; }
        public virtual string Name { get; set; }
        /// <summary>
        /// 连接数量
        /// </summary>
        public virtual int Count { get; set; }
    }
}
