using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerApplication.DB
{
    class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory = null;

        private static void InitalizeSessionFactory()
        {
            _sessionFactory = Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(db => db.Server("192.168.1.196").Database("fightserver").Username("root").Password("baide123456")))
                .Mappings(a => a.FluentMappings.AddFromAssemblyOf<NHibernateHelper>()).BuildSessionFactory();
        }

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null) InitalizeSessionFactory();
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }


    }
}
