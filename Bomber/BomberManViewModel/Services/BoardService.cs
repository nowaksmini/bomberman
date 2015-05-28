using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.Services
{
    public class BoardService
    {
        static public bool CreateNewBoardElement(BoardElementDao gameDAO, out String message)
        {
            if (CheckIfElementExists(gameDAO, out message) == false)
                return false;

            return false;
        }

        static public bool CheckIfElementExists(BoardElementDao gameDAO, out String message)
        {
            message = null;
            return false;
        }

        static public List<BoardElementDao> GetAllBoardElementsForGame(GameDao gameDAO, out String message)
        {
            message = null;
            return null;
        }

        static public bool UpdateBoardElementForGame(GameDao gameDAO, BoardElementDao boardElementDAO, out String message)
        {
            message = null;
            return false;
        }

        static public List<BoardElementLocationDao> GetAllBlocksForGame(GameDao gameDAO, out String message)
        {
            message = null;
            List<BoardElementLocationDao> blocks = new List<BoardElementLocationDao>();
            var query = from element in DataManager.DataBaseContext.BoardElementLocations
                        where element.Game.Id == gameDAO.Id &&
                        element.BoardElement.ElementType == BomberManModel.BoardElementType.BlackBlock || 
                        element.BoardElement.ElementType == BomberManModel.BoardElementType.GrayBlock ||
                        element.BoardElement.ElementType == BomberManModel.BoardElementType.WhiteBlock ||
                        element.BoardElement.ElementType == BomberManModel.BoardElementType.RedBlock
                        orderby element.XLocation, element.YLocation
                        select element;
            if (query == null) return blocks;
            BoardElementLocation[] bElements = query.ToArray<BoardElementLocation>();
            for(int i = 0; i < bElements.Length; i++)
            {
                BoardElementLocationDao block = Mapper.Map<BoardElementLocation, BoardElementLocationDao>(bElements[i]);
                blocks.Add(block);
            }
            return blocks;
        }

        static public List<BoardElementDao> GetAllBombsForGame(GameDao gameDAO, out String message)
        {
            message = null;
            List<BoardElementDao> bombs = new List<BoardElementDao>();
            var query = from element in DataManager.DataBaseContext.BoardElements
                        where element.ElementType == BomberManModel.BoardElementType.Bomb
                        select element;
            if (query == null) return bombs;
            BoardElement[] bElements = query.ToArray<BoardElement>();
            for (int i = 0; i < bElements.Length; i++)
            {
                BoardElementDao block = Mapper.Map<BoardElement, BoardElementDao>(bElements[i]);
                bombs.Add(block);
            }
            return bombs;
        }

        static public List<BoardElementDao> GetAllBonusesForGame(GameDao gameDAO, out String message)
        {
            message = null;
            List<BoardElementDao> bonuses = new List<BoardElementDao>();
            var query = from element in DataManager.DataBaseContext.BoardElements
                        where element.ElementType == BomberManModel.BoardElementType.BombAmountBonus ||
                        element.ElementType == BomberManModel.BoardElementType.FastBonus ||
                        element.ElementType == BomberManModel.BoardElementType.InmortalBonus ||
                        element.ElementType == BomberManModel.BoardElementType.LifeBonus ||
                        element.ElementType == BomberManModel.BoardElementType.SlowBonus ||
                        element.ElementType == BomberManModel.BoardElementType.StrenghtBonus
                        select element;
            if (query == null) return bonuses;
            BoardElement[] bElements = query.ToArray<BoardElement>();
            for (int i = 0; i < bElements.Length; i++)
            {
                BoardElementDao block = Mapper.Map<BoardElement, BoardElementDao>(bElements[i]);
                bonuses.Add(block);
            }
            return bonuses;
        }
    }
}
