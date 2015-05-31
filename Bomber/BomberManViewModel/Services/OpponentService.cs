using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using BomberManModel;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Services
{
    /// <summary>
    /// Klasa odpowiedzialna za integrację widoku z bazą. Wykonuje operacje dotyczące przeciwników gracza.
    /// </summary>
    public class OpponentService
    {
        /// <summary>
        /// Zwróć Lokalizację wszystkich przeciwników w zależności od wybranej gry.
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomość przesyłana w razie niepowodzenia</param>
        /// <returns></returns>
        public static List<OpponentLocationDao> GetAllOponentsWithLocationsByGame(GameDao gameDao, out String message)
        {
            try
            {
                message = null;
                List<OpponentLocationDao> opponentLocation = new List<OpponentLocationDao>();
                var query = from element in DataManager.DataBaseContext.OponentLocations
                    where element.Game.Id == gameDao.Id
                    select element;
                OpponentLocation[] bElements = query.ToArray();
                for (int i = 0; i < bElements.Length; i++)
                {
                    OpponentLocationDao opponentL = Mapper.Map<OpponentLocationDao>(bElements[i]);
                    opponentLocation.Add(opponentL);
                }
                return opponentLocation;
            }
            catch (Exception e)
            {
                var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
                if (declaringType != null)
                    Logger.LogMessage(declaringType.Name, MethodBase.GetCurrentMethod().Name,
                        e.StackTrace);
                message = e.Message;
            }
            return null;
        }

        /// <summary>
        /// Znajdź przeciwnika planszy na podstawie typu.
        /// </summary>
        /// <param name="opponentType">typ szukanego przeciwnika</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        public static OpponentDao FindBoardElementByType(OpponentType opponentType, out String message)
        {
            try
            {
                var query = from b in DataManager.DataBaseContext.Opponents
                    where b.OpponentType == opponentType
                    select b;
                if (query.Any())
                {
                    message = null;
                    return Mapper.Map<OpponentDao>(query.First());
                }
                message = "Nie znaleziono szukanego przeciwnika";
                return null;
            }
            catch (Exception e)
            {
                var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
                if (declaringType != null)
                    Logger.LogMessage(declaringType.Name, MethodBase.GetCurrentMethod().Name,
                        e.StackTrace);
                message = e.Message;
            }
            return null;
        }

        /// <summary>
        /// Zaktualizuj rozmieszczenie przeciwników.
        /// </summary>
        /// <param name="gameId">id aktalnie zapisywanej gry</param>
        /// <param name="opponents">przeciwnicy</param>
        /// <param name="message">wiadomość przesyłana w razie niepowodzenia</param>
        /// <returns></returns>
        public static bool UpdateOpponentLocations(int gameId, List<OpponentLocationDao> opponents, out String message)
        {
            try
            {
                message = null;
                var query = from element in DataManager.DataBaseContext.OponentLocations
                    where element.Game.Id == gameId
                    select element;
                DataManager.DataBaseContext.OponentLocations.RemoveRange(query);
                DataManager.DataBaseContext.SaveChanges();
                var gameQuery = from element in DataManager.DataBaseContext.Games
                    where element.Id == gameId
                    select element;
                if (!gameQuery.Any())
                {
                    message = "Nie istnieje taka gra";
                    return false;
                }
                Game game = gameQuery.First();
                for (int i = 0; i < opponents.Count; i++)
                {
                    OpponentLocation opponentLocation = Mapper.Map<OpponentLocation>(opponents[i]);
                    opponentLocation.Game = game;
                    DataManager.DataBaseContext.OponentLocations.Add(opponentLocation);
                }
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
    }
}
