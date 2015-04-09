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
    }
}
