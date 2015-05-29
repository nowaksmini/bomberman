using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Services
{
    /// <summary>
    /// Klasa odpowiedzialna za integrację widoku z bazą. Wykonuje operacje dotyczące logiki gry.
    /// </summary>
    public class GameService
    {
        /// <summary>
        /// Zwróć wszystkie zapisane gry użytownika.
        /// </summary>
        /// <param name="userDao">użytkownik</param>
        /// <param name="message">wiadomości otrzymane po zakończeniu weryfikacji</param>
        /// <returns>lista zapisanych gier użytkownika</returns>
        static public List<GameDao> GetAllGamesForUser(UserDao userDao, out String message)
        {
            var query = from b in DataManager.DataBaseContext.Games
                        where b.User.Name == userDao.Name
                        select b;
            List<GameDao> gamesDaos = new List<GameDao>();
            List<Game> games = query.ToList();
            foreach (var game in games)
            {
                gamesDaos.Add(Mapper.Map<GameDao>(game));
            }
            message = null;
            return gamesDaos;
        }

        /// <summary>
        /// Zwróć jedną grę w zależności od id gry.
        /// </summary>
        /// <param name="userDao">gracz</param>
        /// <param name="gameId">id gry</param>
        /// <param name="message">wiadomości otrzymane po zakończeniu weryfikacji</param>
        /// <returns>zwróć znalezioną grę</returns>
        static public GameDao GetGameForUserById(UserDao userDao, int gameId, out String message)
        {
            var query =
                DataManager.DataBaseContext.Games.Where(b => b.User.Name == userDao.Name && b.Id == gameId);
            if (query.Any())
            {
                Game game = query.First();
                message = null;
                return Mapper.Map<GameDao>(game);
            }
            message = "Nie znaleziono gry.";
            return null;
        }

        /// <summary>
        /// Utwórz lub uaktualnij istniejącą grę
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomości otrzymane po zakończeniu weryfikacji</param>
        /// <returns>status powodzenia</returns>
        static public bool UpdateGame(GameDao gameDao, out String message)
        {
            var query = from b in DataManager.DataBaseContext.Games
                        where b.Id == gameDao.Id
                        select b;
            if (!query.Any())
            {
                var userquery = from b in DataManager.DataBaseContext.Users
                                where b.Name == gameDao.User.Name
                                select b;
                if (!userquery.Any())
                {
                    message = "Nie istnieje taki użytkownik : " + gameDao.User.Name;
                    return false;
                }
                User u = userquery.First();
                Game game = Mapper.Map<Game>(gameDao);
                game.User = u;
                DataManager.DataBaseContext.Users.Add(u);
                DataManager.DataBaseContext.SaveChanges();
                message = null;
                return true;
            }
            Game game1 = query.First();
            gameDao.Id = game1.Id;
            gameDao.User.Id = game1.User.Id;
            game1 = Mapper.Map<Game>(gameDao);
            DataManager.DataBaseContext.Entry(game1).State = EntityState.Modified;
            DataManager.DataBaseContext.SaveChanges();
            message = null;
            return false;
        }

        /// <summary>
        /// Zwróć wybraną ilość najlepszych wyników wszytskich gier.
        /// </summary>
        /// <param name="n">ilość najlepszych wyników</param>
        /// <param name="message">wiadomości otrzymane po zakończeniu weryfikacji</param>
        /// <returns>lista najlepszych wyników</returns>
        static public List<GameDao> GetBestHighSocredGames(int n, out String message)
        {
            var query = from b in DataManager.DataBaseContext.Games
                        orderby b.Points
                        select  b;
            var top = query.Take(n);
            List<GameDao> gamesDaos = new List<GameDao>();
            List<Game> games = top.ToList();
            foreach (var game in games)
            {
                gamesDaos.Add(Mapper.Map<GameDao>(game));
            }
            message = null;
            return gamesDaos;
        }

        /// <summary>
        /// Zwróć wybraną ilość ostatnio zapisanych gier użytkownika.
        /// </summary>
        /// <param name="userDao"></param>
        /// <param name="n"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        static public List<GameDao> GetLastGamesForUser(UserDao userDao, int n, out String message)
        {
            var query = from b in DataManager.DataBaseContext.Games
                        where b.User.Name == userDao.Name
                        orderby b.SaveTime
                        select b;
            var top = query.Take(n);
            List<GameDao> gamesDaos = new List<GameDao>();
            List<Game> games = top.ToList();
            foreach (var game in games)
            {
                gamesDaos.Add(Mapper.Map<GameDao>(game));
            }
            message = null;
            return null;
        }

    }
}
