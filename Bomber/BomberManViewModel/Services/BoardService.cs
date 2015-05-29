using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        static public List<BoardElementLocationDao> GetAllBoardElementsForGame(GameDao gameDao, out String message)
        {
            List<BoardElementLocationDao> elements = new List<BoardElementLocationDao>();
            var query = from element in DataManager.DataBaseContext.BoardElementLocations
                select element;
            if (query.Any())
            {
                message = null;
                List<BoardElementLocation> list = query.ToList();
                list.All(x =>
                {
                    elements.Add(Mapper.Map<BoardElementLocationDao>(x));
                    return true;
                });
                return elements;
            }
            message = null;
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
            var query = from b in DataManager.DataBaseContext.BoardElements
                where b.ElementType == boardElementType
                select b;
            if (query.Any())
            {
                message = null;
                return AutoMapper.Mapper.Map<BoardElementDao>(query.First());
            }
            message = "Nie znaleziono szukanego elementu planszy";
            return null;
        }

        /// <summary>
        /// Zaktualizuj rozmieszczenie elementów na planszy.
        /// </summary>
        /// <param name="elements">elementy</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        static public bool UpdateBoardElementLocations(List<BoardElementLocationDao> elements, out String message)
        {
            uint gameid = (uint) elements[0].Game.Id;
            message = null;
            var query = from element in DataManager.DataBaseContext.BoardElementLocations
                        where element.Game.Id == gameid
                        select element;

            BoardElementLocation [] bElements = query.ToArray();
            for (int i = 0; i < bElements.Length; i++)
            {
                DataManager.DataBaseContext.BoardElementLocations.Remove(bElements[i]);
            }
            DataManager.DataBaseContext.SaveChanges();
            for (int i = 0; i < elements.Count; i++)
            {
                DataManager.DataBaseContext.BoardElementLocations.Add(Mapper.Map<BoardElementLocation>(elements[i]));
            }
            DataManager.DataBaseContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Zwróć wszystkie jednostkowe pola dla danej gry.
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        static public List<BoardElementLocationDao> GetAllBlocksForGame(GameDao gameDao, out String message)
        {
            message = null;
            List<BoardElementLocationDao> blocks = new List<BoardElementLocationDao>();
            var query = from element in DataManager.DataBaseContext.BoardElementLocations
                        where element.Game.Id == gameDao.Id &&
                        element.BoardElement.ElementType == BoardElementType.BlackBlock || 
                        element.BoardElement.ElementType == BoardElementType.GrayBlock ||
                        element.BoardElement.ElementType == BoardElementType.WhiteBlock ||
                        element.BoardElement.ElementType == BoardElementType.RedBlock
                        orderby element.XLocation, element.YLocation
                        select element;
            BoardElementLocation[] bElements = query.ToArray();
            for(int i = 0; i < bElements.Length; i++)
            {
                BoardElementLocationDao block = Mapper.Map<BoardElementLocation, BoardElementLocationDao>(bElements[i]);
                blocks.Add(block);
            }
            return blocks;
        }

        /// <summary>
        /// Zwróć rozmieszczenie wszystkich bomb dla zadanej gry.
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        static public List<BoardElementDao> GetAllBombsForGame(GameDao gameDao, out String message)
        {
            message = null;
            List<BoardElementDao> bombs = new List<BoardElementDao>();
            var query = from element in DataManager.DataBaseContext.BoardElements
                        where element.ElementType == BoardElementType.Bomb
                        select element;
            BoardElement[] bElements = query.ToArray();
            for (int i = 0; i < bElements.Length; i++)
            {
                BoardElementDao block = Mapper.Map<BoardElement, BoardElementDao>(bElements[i]);
                bombs.Add(block);
            }
            return bombs;
        }

        /// <summary>
        /// Zwróć rozmieszczenie wszystkich bonusów dla zadanej gry.
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        static public List<BoardElementLocationDao> GetAllBonusesForGame(GameDao gameDao, out String message)
        {
            message = null;
            List<BoardElementLocationDao> bonuses = new List<BoardElementLocationDao>();
            var query = from element in DataManager.DataBaseContext.BoardElementLocations
                where (element.BoardElement.ElementType == BoardElementType.BombAmountBonus ||
                       element.BoardElement.ElementType == BoardElementType.FastBonus ||
                       element.BoardElement.ElementType == BoardElementType.InmortalBonus ||
                       element.BoardElement.ElementType == BoardElementType.LifeBonus ||
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
    }
}
