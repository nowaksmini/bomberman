using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.DataBaseServices
{
    public class OponentService
    {
        static public bool CreateNewOponent(OponentDAO oponentDAO, out String message)
        {
            if (CheckIfOponentExists(oponentDAO, out message) == false)
                return false;

            return false;
        }

        static public bool CheckIfOponentExists(OponentDAO oponentDAO, out String message)
        {
            message = null;
            return false;
        }

        static public bool CheckIfOponentLocationExists(OponentLocationDAO oponentDAO, out String message)
        {
            message = null;
            return false;
        }

        static public bool CreateNewOponentLocation(OponentLocationDAO oponentDAO, out String message)
        {
            message = null;
            return false;
        }

        static public List<OponentDAO> GetAllOponentsByGame(GameDAO gameDAO, out String message)
        {
            message = null;
            return null;
        }

        static public List<OponentDAO> GetAllOponentsByGameAndLocation(GameDAO gameDAO, uint x, uint y, out String message)
        {
            message = null;
            return null;
        }

        static public List<OponentLocationDAO> GetAllOponentsWithLocationsByGame(GameDAO gameDAO, out String message)
        {
            message = null;
            return null;
        }

        static public List<OponentLocationDAO> GetAllOponentsWithLocationsByGameAndLocation(GameDAO gameDAO,  uint x, uint y, out String message)
        {
            message = null;
            return null;
        }

        static public bool UpdateOponentLocation(OponentLocationDAO gameDAO,  uint x, uint y, out String message)
        {
            message = null;
            return false;
        }
    }
}
