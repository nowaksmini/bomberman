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
        static public bool CreateNewBoardElement(BoardElementDAO gameDAO, out String message)
        {
            if (CheckIfElementExists(gameDAO, out message) == false)
                return false;

            return false;
        }

        static public bool CheckIfElementExists(BoardElementDAO gameDAO, out String message)
        {
            message = null;
            return false;
        }

        static public List<BoardElementDAO> GetAllBoardElementsForGame(GameDAO gameDAO, out String message)
        {
            message = null;
            return null;
        }

        static public bool UpdateBoardElementForGame(GameDAO gameDAO, BoardElementDAO boardElementDAO, out String message)
        {
            message = null;
            return false;
        }

        static public List<BoardElementLocationDAO> GetAllBlocksForGame(GameDAO gameDAO, out String message)
        {
            message = null;
            List<BoardElementLocationDAO> blocks = new List<BoardElementLocationDAO>();
            var query = from element in DataManager.DataBaseContext.BoardElementLocations
                        where element.Game.ID == gameDAO.ID &&
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
                BoardElementLocationDAO block = Mapper.Map<BoardElementLocation, BoardElementLocationDAO>(bElements[i]);
                blocks.Add(block);
            }
            return blocks;
        }

        static public List<BoardElementDAO> GetAllBombsForGame(GameDAO gameDAO, out String message)
        {
            message = null;
            List<BoardElementDAO> bombs = new List<BoardElementDAO>();
            var query = from element in DataManager.DataBaseContext.BoardElements
                        where element.ElementType == BomberManModel.BoardElementType.Bomb
                        select element;
            if (query == null) return bombs;
            BoardElement[] bElements = query.ToArray<BoardElement>();
            for (int i = 0; i < bElements.Length; i++)
            {
                BoardElementDAO block = Mapper.Map<BoardElement, BoardElementDAO>(bElements[i]);
                bombs.Add(block);
            }
            return bombs;
        }

        static public List<BoardElementDAO> GetAllBonusesForGame(GameDAO gameDAO, out String message)
        {
            message = null;
            List<BoardElementDAO> bonuses = new List<BoardElementDAO>();
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
                BoardElementDAO block = Mapper.Map<BoardElement, BoardElementDAO>(bElements[i]);
                bonuses.Add(block);
            }
            return bonuses;
        }
    }
}
