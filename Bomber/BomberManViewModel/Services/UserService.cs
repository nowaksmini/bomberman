using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.Services
{
    public class UserService
    {
        private const String SALT = "21ywhihAkjOIJI898uHJBJH8haksldjjdsoiaOIASDAKSHDao8asdashADO";

        static public bool CheckPassword(UserDAO userDAO, out String message)
        {
            if (CheckIfUserExists(userDAO, out message) == false)
                return false;
            var query = from b in DataManager.DataBaseContext.Users
                        where b.Name == userDAO.Name
                        select b;
            if(query == null) return false;
            User user = query.First<User>();
            if(user != null)
            {
                return user.Password.Equals(userDAO.Password);
            }
            return false;
        }

        static public bool CheckIfUserExists(UserDAO userDAO, out String message)
        {
            message = null;
            return false;
        }

        static public bool CreateUser(UserDAO userDAO, out String message)
        {
            message = null;
            if (CheckIfUserExists(userDAO, out message) == true)
                return false;
            return false;
        }

        static public bool GenerateHashedPassword(UserDAO userDAO, out String message)
        {
            message = null;
            return false;
        }
    }
}
