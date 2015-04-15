using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.DataBaseServices
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

        static public List<BoardElementDAO> GetAllBlocksForGame(GameDAO gameDAO)
        {
            List<BoardElementDAO> blocks = new List<BoardElementDAO>();
            var query = from element in DataManager.DataBaseContext.BoardElements
                        where element.ElementType == BomberManModel.BoardElementType.BlackBlock || 
                        element.ElementType == BomberManModel.BoardElementType.GrayBlock ||
                        element.ElementType == BomberManModel.BoardElementType.WhiteBlock ||
                        element.ElementType == BomberManModel.BoardElementType.RedBlock
                        select element;
            if (query == null) return blocks;
            BoardElement[] bElements = query.ToArray<BoardElement>();
            for(int i = 0; i < bElements.Length; i++)
            {
                BoardElementDAO block = Mapper.Map<BoardElement, BoardElementDAO>(bElements[i]);
                blocks.Add(block);
            }
            return blocks;
        }

        static public List<BoardElementDAO> GetAllBombsForGame(GameDAO gameDAO)
        {
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

        static public List<BoardElementDAO> GetAllBonusesForGame(GameDAO gameDAO)
        {
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
