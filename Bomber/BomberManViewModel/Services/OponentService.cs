using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.Services
{
    public class OponentService
    {
        static public bool CreateNewOponent(OpponentDao oponentDAO, out String message)
        {
            if (CheckIfOponentExists(oponentDAO, out message) == false)
                return false;

            return false;
        }

        static public bool CheckIfOponentExists(OpponentDao oponentDAO, out String message)
        {
            message = null;
            return false;
        }

        static public bool CheckIfOponentLocationExists(OpponentLocationDao oponentDAO, out String message)
        {
            message = null;
            return false;
        }

        static public bool CreateNewOponentLocation(OpponentLocationDao oponentDAO, out String message)
        {
            message = null;
            return false;
        }

        static public List<OpponentDao> GetAllOponentsByGame(GameDao gameDAO, out String message)
        {
            message = null;
            return null;
        }

        static public List<OpponentDao> GetAllOponentsByGameAndLocation(GameDao gameDAO, uint x, uint y, out String message)
        {
            message = null;
            return null;
        }

        static public List<OpponentLocationDao> GetAllOponentsWithLocationsByGame(GameDao gameDAO, out String message)
        {
            message = null;
            return null;
        }

        static public List<OpponentLocationDao> GetAllOponentsWithLocationsByGameAndLocation(GameDao gameDAO,  uint x, uint y, out String message)
        {
            message = null;
            return null;
        }

        static public bool UpdateOponentLocation(OpponentLocationDao gameDAO,  uint x, uint y, out String message)
        {
            message = null;
            return false;
        }
    }
}
