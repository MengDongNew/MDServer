using Common.Modal;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerApplication.DB.Mapping
{
    class ServerPropertyMap : ClassMap<ServerProperty>
    {
        public ServerPropertyMap()
        {
            Table("serverlist");
            Id(a => a.ID).Column("ID");
            Map(a => a.Name).Column("name");
            Map(a => a.IP).Column("IP");
            Map(a => a.Count).Column("count");
        }
    }
}
