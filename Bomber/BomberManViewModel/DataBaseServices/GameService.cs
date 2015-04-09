using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.DataBaseServices
{
    public class GameService
    {
        static public bool CreateNewGame(GameDAO gameDAO, out String message)
        {
            if (CheckIfGameExists(gameDAO, out message) == false)
                return false;
            
            return false;
        }

        static public bool CheckIfGameExists(GameDAO gameDAO, out String message)
        {
            message = null;
            return false;
        }

        static public List<GameDAO> GetAllGamesForUser(UserDAO userDAO, out String message)
        {
            message = null;
            return null;
        }

        static public GameDAO GetGameForUserByID(UserDAO userDAO, int gameID, out String message)
        {
            message = null;
            return null;
        }

        static public bool UpdateGame(GameDAO gameDAO, out String message)
        {
            message = null;
            return false;
        }

        static public bool UpdatePlayerLocationByGame(GameDAO gameDAO, LocationDAO location, out String message)
        {
            message = null;
            return false;
        }

        static public int GetScoreByGame(GameDAO gameDAO, out String message)
        {
            message = null;
            return 0;
        }

        static public List<GameDAO> GetBestHighSocredGames(int n, out String message)
        {
            message = null;
            return null;
        }

        static public List<GameDAO> GetLastGamesForUser(UserDAO userDAO,int n, out String message)
        {
            message = null;
            return null;
        }

        static public GameDAO GetLastGameForUser(UserDAO userDAO, out String message)
        {
            message = null;
            return null;
        }
    }
}
