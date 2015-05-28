using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel.Services
{
    public class GameService
    {
        static public bool CreateNewGame(GameDao gameDAO, out String message)
        {
            if (CheckIfGameExists(gameDAO, out message) == false)
                return false;
            
            return false;
        }

        static public bool CheckIfGameExists(GameDao gameDAO, out String message)
        {
            message = null;
            return false;
        }

        static public List<GameDao> GetAllGamesForUser(UserDao userDAO, out String message)
        {
            message = null;
            return null;
        }

        static public GameDao GetGameForUserByID(UserDao userDAO, int gameID, out String message)
        {
            message = null;
            return null;
        }

        static public bool UpdateGame(GameDao gameDAO, out String message)
        {
            message = null;
            return false;
        }

        static public bool UpdatePlayerLocationByGame(GameDao gameDAO,  uint x, uint y, out String message)
        {
            message = null;
            return false;
        }

        static public int GetScoreByGame(GameDao gameDAO, out String message)
        {
            message = null;
            return 0;
        }

        static public List<GameDao> GetBestHighSocredGames(int n, out String message)
        {
            message = null;
            return null;
        }

        static public List<GameDao> GetLastGamesForUser(UserDao userDAO, int n, out String message)
        {
            message = null;
            return null;
        }

        static public GameDao GetLastGameForUser(UserDao userDAO, out String message)
        {
            message = null;
            return null;
        }
    }
}
