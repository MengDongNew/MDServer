using Common.Modal;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerApplication.DB.Mapping
{
    class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("user");
            Id(m => m.Accesstoken).Column("accesstoken");
            Map(m => m.ID).Column("userId");
            Map(m => m.Username).Column("username");
            Map(m => m.Password).Column("password");
        }
    }
}
