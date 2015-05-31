using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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
        /// Zwróć jedną grę w zależności od id gry.
        /// </summary>
        /// <param name="userDao">gracz</param>
        /// <param name="gameId">id gry</param>
        /// <param name="message">wiadomości otrzymane po zakończeniu weryfikacji</param>
        /// <returns>zwróć znalezioną grę</returns>
        static public GameDao GetGameForUserById(UserDao userDao, int gameId, out String message)
        {
            try
            {
                var query = DataManager.DataBaseContext.Games.Where(b => b.User.Name == userDao.Name && b.Id == gameId);
                if (query.Any())
                {
                    Game game = query.First();
                    message = null;
                    return Mapper.Map<GameDao>(game);
                }
                message = "Nie znaleziono gry.";
                return null;
            }
            catch (Exception e)
            {
                var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
                if (declaringType != null)
                    Logger.LogMessage(declaringType.Name, MethodBase.GetCurrentMethod().Name,
                        e.StackTrace);
                message = e.Message;
                return null;
            }
        }

        /// <summary>
        /// Utwórz nową grę.
        /// </summary>
        /// <param name="gameDao">gra przekazywana z widoku do bazy</param>
        /// <param name="message">wiadomości otrzymane po zakończeniu weryfikacji</param>
        /// <returns>status powodzenia</returns>
        static public bool CreateGame(ref GameDao gameDao, out String message)
        {
            try
            {
                message = null;
                GameDao dao = gameDao;
                var query = from b in DataManager.DataBaseContext.Games
                    where b.Id == dao.Id
                    select b;
                if (query.Any())
                {
                    message = "Już istnieje taka gra.";
                    return false;
                }
                Game game = Mapper.Map<Game>(gameDao);
                game.SaveTime = DateTime.Now;
                GameDao dao1 = gameDao;
                var userQuery = from b in DataManager.DataBaseContext.Users
                    where b.Id == dao1.User.Id
                    select b;
                if (!userQuery.Any())
                {
                    message = "Nie istnieje użytkownik o nazwie " + dao1.User.Name;
                    return false;
                }
                game.User = userQuery.First();
                DataManager.DataBaseContext.Games.Add(game);
                DataManager.DataBaseContext.SaveChanges();
                gameDao = Mapper.Map<GameDao>(game);
                return true;
            }
            catch (Exception e)
            {
                var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
                if (declaringType != null)
                    Logger.LogMessage(declaringType.Name, MethodBase.GetCurrentMethod().Name,
                        e.StackTrace);
                message = e.Message;
            }
            return false;
        }

        /// <summary>
        /// Utwórz lub uaktualnij istniejącą grę
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomości otrzymane po zakończeniu weryfikacji</param>
        /// <returns>status powodzenia</returns>
        static public bool UpdateGame(ref GameDao gameDao, out String message)
        {
            try
            {
                message = null;
                GameDao dao = gameDao;
                var query = from b in DataManager.DataBaseContext.Games
                    where b.Id == dao.Id
                    select b;
                if (!query.Any())
                {
                    message = "Nie istnieje taka gra.";
                    return false;
                }
                Game game = query.First();
                game.SaveTime = DateTime.Now;
                game.BombsAmount = gameDao.BombsAmount;
                game.Finished = gameDao.Finished;
                game.Level = gameDao.Level;
                game.PlayerXLocation = gameDao.PlayerXLocation;
                game.PlayerYLocation = gameDao.PlayerYLocation;
                game.Points = gameDao.Points;
                var userQuery = from b in DataManager.DataBaseContext.Users
                                where b.Id == dao.User.Id
                                select b;
                if (!userQuery.Any())
                {
                    message = "Gra nie ma przypisanego gracza";
                    return false;
                }
                game.User = userQuery.First();
                DataManager.DataBaseContext.Entry(game).State = EntityState.Modified;
                DataManager.DataBaseContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
                if (declaringType != null)
                    Logger.LogMessage(declaringType.Name, MethodBase.GetCurrentMethod().Name,
                        e.StackTrace);
                message = e.Message;
            }
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
            try
            {
                message = null;
                var query = from b in DataManager.DataBaseContext.Games
                            orderby b.Points
                            descending
                            select b;
                var top = query.Take(n);
                List<Game> games = top.ToList();
                List<GameDao> gamesDaos = games.Select(game => Mapper.Map<GameDao>(game)).ToList();
                return gamesDaos;
            }
            catch (Exception e)
            {
                var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
                if (declaringType != null)
                    Logger.LogMessage(declaringType.Name, MethodBase.GetCurrentMethod().Name,
                        e.StackTrace);
                message = e.Message;
                return null;
            }
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
            try
            {
                var query = from b in DataManager.DataBaseContext.Games
                    where b.User.Id == userDao.Id && b.Finished == false
                            orderby b.SaveTime
                            descending 
                            select b;
                var top = query.Take(n);
                List<Game> games = top.ToList();
                List<GameDao> gamesDaos = games.Select(game => Mapper.Map<GameDao>(game)).ToList();
                message = null;
                return gamesDaos;
            }
            catch (Exception e)
            {
                var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
                if (declaringType != null)
                    Logger.LogMessage(declaringType.Name, MethodBase.GetCurrentMethod().Name,
                        e.StackTrace);
                message = e.Message;
                return null;
            }
           
        }

    }
}
