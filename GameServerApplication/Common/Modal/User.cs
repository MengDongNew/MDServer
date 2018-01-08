using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Modal
{
    public class User
    {
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Accesstoken { get; set; }
        /// <summary>
        /// 游戏ID
        /// </summary>
        public virtual int ID { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }
    }
}
