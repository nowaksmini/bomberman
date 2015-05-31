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
    /// Klasa odpowiedzialna za integrację widoku z bazą. Wykonuje operacje dotyczące elementów planszy. 
    /// </summary>
    public class BoardService
    {
        /// <summary>
        /// Zwróć wszytskie elementy z planszy dla zadanej gry.
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        public static List<BoardElementLocationDao> GetAllBoardElementsForGame(GameDao gameDao, out String message)
        {
            try
            {
                message = null;
                List<BoardElementLocationDao> elements = new List<BoardElementLocationDao>();
                var query = from element in DataManager.DataBaseContext.BoardElementLocations
                    select element;
                if (query.Any())
                {
                    List<BoardElementLocation> list = query.ToList();
                    list.All(x =>
                    {
                        elements.Add(Mapper.Map<BoardElementLocationDao>(x));
                        return true;
                    });
                    return elements;
                }
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
        /// Znajdź obiekt planszy na podstawie typu.
        /// </summary>
        /// <param name="boardElementType">typ szukanego elementu</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        public static BoardElementDao FindBoardElementByType(BoardElementType boardElementType, out String message)
        {
            try
            {
                var query = from b in DataManager.DataBaseContext.BoardElements
                    where b.ElementType == boardElementType
                    select b;
                if (query.Any())
                {
                    message = null;
                    return Mapper.Map<BoardElementDao>(query.First());
                }
                message = "Nie znaleziono szukanego elementu planszy";
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
        /// Zaktualizuj rozmieszczenie elementów na planszy.
        /// </summary>
        /// <param name="gameId">id uaktualnianej gry</param>
        /// <param name="elements">elementy</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        public static bool UpdateBoardElementLocations(int gameId, List<BoardElementLocationDao> elements,
            out String message)
        {
            try
            {
                message = null;
                var query = from element in DataManager.DataBaseContext.BoardElementLocations
                    where element.Game.Id == gameId
                    select element;

                DataManager.DataBaseContext.BoardElementLocations.RemoveRange(query);
                DataManager.DataBaseContext.SaveChanges();
                var gameQuery = from b in DataManager.DataBaseContext.Games
                    where b.Id == gameId
                    select b;
                if (!gameQuery.Any())
                {
                    message = "Nie istnieje taka gra";
                    return false;
                }
                for (int i = 0; i < elements.Count; i++)
                {
                    BoardElementLocation boardElementLocation = Mapper.Map<BoardElementLocation>(elements[i]);
                    boardElementLocation.Game = gameQuery.First();
                    DataManager.DataBaseContext.BoardElementLocations.Add(boardElementLocation);
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

        /// <summary>
        /// Zwróć wszystkie jednostkowe pola dla danej gry.
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        public static List<BoardElementLocationDao> GetAllBlocksForGame(GameDao gameDao, out String message)
        {
            try
            {
                message = null;
                List<BoardElementLocationDao> blocks = new List<BoardElementLocationDao>();
                var query = from element in DataManager.DataBaseContext.BoardElementLocations
                    where element.Game.Id == gameDao.Id &&
                          (element.BoardElement.ElementType == BoardElementType.BlackBlock ||
                          element.BoardElement.ElementType == BoardElementType.GrayBlock ||
                          element.BoardElement.ElementType == BoardElementType.WhiteBlock ||
                          element.BoardElement.ElementType == BoardElementType.RedBlock)
                    orderby element.XLocation, element.YLocation
                    select element;
                BoardElementLocation[] bElements = query.ToArray();
                for (int i = 0; i < bElements.Length; i++)
                {
                    BoardElementLocationDao block =
                        Mapper.Map<BoardElementLocation, BoardElementLocationDao>(bElements[i]);
                    blocks.Add(block);
                }
                return blocks;
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
        /// Zwróć rozmieszczenie wszystkich bomb dla zadanej gry.
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        public static List<BoardElementLocationDao> GetAllBombsForGame(GameDao gameDao, out String message)
        {
            try
            {
                message = null;
                List<BoardElementLocationDao> bombs = new List<BoardElementLocationDao>();
                var query = from element in DataManager.DataBaseContext.BoardElementLocations
                    where element.BoardElement.ElementType == BoardElementType.Bomb
                    && element.Game.Id == gameDao.Id
                    select element;
                BoardElementLocation[] bElements = query.ToArray();
                for (int i = 0; i < bElements.Length; i++)
                {
                    BoardElementLocationDao block = Mapper.Map<BoardElementLocationDao>(bElements[i]);
                    bombs.Add(block);
                }
                return bombs;
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
        /// Zwróć rozmieszczenie wszystkich bonusów dla zadanej gry.
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        public static List<BoardElementLocationDao> GetAllBonusesForGame(GameDao gameDao, out String message)
        {
            try
            {
                message = null;
                List<BoardElementLocationDao> bonuses = new List<BoardElementLocationDao>();
                var query = from element in DataManager.DataBaseContext.BoardElementLocations
                    where (element.BoardElement.ElementType == BoardElementType.BombAmountBonus ||
                           element.BoardElement.ElementType == BoardElementType.FastBonus ||
                           element.BoardElement.ElementType == BoardElementType.InmortalBonus ||
                           element.BoardElement.ElementType == BoardElementType.PointBonus ||
                           element.BoardElement.ElementType == BoardElementType.SlowBonus ||
                           element.BoardElement.ElementType == BoardElementType.StrenghtBonus)
                          && element.Game.Id == gameDao.Id
                    select element;
                BoardElementLocation[] bElements = query.ToArray();
                for (int i = 0; i < bElements.Length; i++)
                {
                    BoardElementLocationDao block = Mapper.Map<BoardElementLocationDao>(bElements[i]);
                    bonuses.Add(block);
                }
                return bonuses;
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
    }
}
