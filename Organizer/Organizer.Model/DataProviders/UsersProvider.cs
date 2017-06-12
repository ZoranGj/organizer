using Model.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.DataProviders
{
    public class UsersProvider : DataProvider<User>
    {
        public UsersProvider(DataContext db) : base(db) { }

        public User Get(string email, string password)
        {
            return _dbSet.FirstOrDefault(u => u.Email.Equals(email) && u.Password.Equals(password));
        }

        public User GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(u => u.Email.Equals(email));
        }
    }
}
