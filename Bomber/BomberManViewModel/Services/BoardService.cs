using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Zaktualizuj rozmieszczenie elementów na planszy.
        /// </summary>
        /// <param name="elements">elementy</param>
        /// <param name="message">wiadomośc, przekazywana w razie porażki</param>
        /// <returns></returns>
        static public bool UpdateBoardElementLocations(List<BoardElementLocation> elements, out String message)
        {

            message = null;
            var query = from element in DataManager.DataBaseContext.BoardElementLocations
                        where element.Game.Id == elements[0].Game.Id
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
        static public List<BoardElementDao> GetAllBonusesForGame(GameDao gameDao, out String message)
        {
            message = null;
            List<BoardElementDao> bonuses = new List<BoardElementDao>();
            var query = from element in DataManager.DataBaseContext.BoardElements
                        where element.ElementType == BoardElementType.BombAmountBonus ||
                        element.ElementType == BoardElementType.FastBonus ||
                        element.ElementType == BoardElementType.InmortalBonus ||
                        element.ElementType == BoardElementType.LifeBonus ||
                        element.ElementType == BoardElementType.SlowBonus ||
                        element.ElementType == BoardElementType.StrenghtBonus
                        select element;
            BoardElement[] bElements = query.ToArray();
            for (int i = 0; i < bElements.Length; i++)
            {
                BoardElementDao block = Mapper.Map<BoardElement, BoardElementDao>(bElements[i]);
                bonuses.Add(block);
            }
            return bonuses;
        }
    }
}
