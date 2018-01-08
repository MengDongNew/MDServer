using Common.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerApplication.DB.Manager
{
    public class UserManager
    {
        public User GetUserByAccesstoken(string accesstoken)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var lst = session.QueryOver<User>().Where(x => x.Accesstoken == accesstoken).List();
                    transaction.Commit();
                    if (lst != null && lst.Count > 0)
                    {
                        return lst[0];
                    }
                    return null;
                }
            }
        }

        public void AddUser(User user)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }
    }
}
