using System;
using System.Linq;
using System.Net.Sockets;
using BomberManModel;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Services
{
    /// <summary>
    /// Klasa odpowiedzialna za integrację widoku z bazą. Wykonuje operacje dotyczące użytkownika.
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// Sól wykorzystywana do solenia hasła.
        /// </summary>
        private const String Salt = "21ywhihAkjOIJI898uHJBJH8hakfsaasf527635*(@)!JKJDsldjjdsoiaOIASDAKSHDao8asdashADO";

        /// <summary>
        /// Sprawdź czy użytkownik przeszedł weryfikację logiunu i hasła.
        /// </summary>
        /// <param name="userDao">użytkownik, którego weryfikujemy</param>
        /// <param name="message">wiadomości otrzymane po zakończeniu weryfikacji</param>
        /// <returns>zwróć <value>true</value> w razie poprawnej weryfikacji loginu i hasła, w przeciwnym przypadku zwróć <value>false</value></returns>
        public static bool VerificateUser(ref UserDao userDao, out String message)
        {
            if (CheckIfUserExists(userDao, out message) == false)
                return false;
            String name = userDao.Name;
            var query = from b in DataManager.DataBaseContext.Users
                where b.Name == name
                select b;
            User user = query.First();
            if (user != null)
            {
                if (VerificatePassword(userDao.Password, user.Password, out message))
                {
                    userDao = AutoMapper.Mapper.Map<UserDao>(user);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sprawdź czy użytkownik o podanym loginie istniej.
        /// </summary>
        /// <param name="userDao">użytkownik, dla którego sprawdzamy login w bazie danych</param>
        /// <param name="message">wiadomości otrzymane po wykonaniu sprawdzenia loginu użytkownika</param>
        /// <returns>zwróć <value>true</value> w przypdaku znalezienia loginu, w przeciwnym przypadku zwróć <value>false</value></returns>
        private static bool CheckIfUserExists(UserDao userDao, out String message)
        {
            var query = from b in DataManager.DataBaseContext.Users
                where b.Name == userDao.Name
                select b;
            if (!query.Any())
            {
                message = "Nie istnieje użytkownik o podanej nazwie " + userDao.Name;
                return false;
            }
            message = null;
            return true;
        }

        /// <summary>
        /// Utwórz nowego użytkownika w bazie danych.
        /// </summary>
        /// <param name="userDao">nowy użytkownik z podanym loginem i hasłem</param>
        /// <param name="message">wiadomości otrzymane po próbie utworzenia użytkownika</param>
        /// <returns></returns>
        public static bool CreateUser(UserDao userDao, out String message)
        {
            if (CheckIfUserExists(userDao, out message))
            {
                message = "Błąd, użytkownik o podanej nazwie już istnieje";
                return false;
            }
            message = null;
            userDao.Password += Salt;
            userDao.Password = userDao.Password.GetHashCode().ToString();
            DataManager.DataBaseContext.Users.Add(AutoMapper.Mapper.Map<User>(userDao));
            DataManager.DataBaseContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Sprawdź czy hasło użytkownika zgadza się z istniejącym w bazie danych.
        /// </summary>
        /// <param name="passwordIn">hasło do sprawdzenia</param>
        /// <param name="passwordDataBase">hasło w bazie danych</param>
        /// <param name="message">wiadomości związane ze sprawdzaniem hasła</param>
        /// <returns></returns>
        private static bool VerificatePassword(String passwordIn, String passwordDataBase, out String message)
        {
            String tmp = passwordIn;
            tmp += Salt;
            tmp = tmp.GetHashCode().ToString();
            if (tmp.Equals(passwordDataBase))
            {
                message = null;
                return true;
            }

            message = "Niepoprawne hasło";
            return false;
        }
    }
}
