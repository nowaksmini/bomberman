using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel.Services
{
    /// <summary>
    /// Klasa odpowiedzialna za integrację widoku z bazą. Wykonuje operacje dotyczące przeciwników gracza.
    /// </summary>
    public class OpponentService
    {
        /// <summary>
        /// Zwróć Lokalizację wszystkich przeciwników w zależności od wybranej gry.
        /// </summary>
        /// <param name="gameDao">gra</param>
        /// <param name="message">wiadomość przesyłana w razie niepowodzenia</param>
        /// <returns></returns>
        static public List<OpponentLocationDao> GetAllOponentsWithLocationsByGame(GameDao gameDao, out String message)
        {
            message = null;
            List<OpponentLocationDao> opponentLocation = new List<OpponentLocationDao>();
            var query = from element in DataManager.DataBaseContext.OponentLocations
                        where element.Game.Id == gameDao.Id
                        select element;
            OpponentLocation[] bElements = query.ToArray();
            for (int i = 0; i < bElements.Length; i++)
            {
                OpponentLocationDao opponentL = Mapper.Map<OpponentLocationDao>(bElements[i]);
                opponentLocation.Add(opponentL);
            }
            return opponentLocation;
        }

        /// <summary>
        /// Zaktualizuj rozmieszczenie przeciwników.
        /// </summary>
        /// <param name="opponents">przeciwnicy</param>
        /// <param name="message">wiadomość przesyłana w razie niepowodzenia</param>
        /// <returns></returns>
        static public bool UpdateOpponentLocations(List<OpponentLocationDao> opponents, out String message)
        {

            message = null;
            var query = from element in DataManager.DataBaseContext.OponentLocations
                        where element.Game.Id == opponents[0].Game.Id
                        select element;

            OpponentLocation[] bElements = query.ToArray();
            for (int i = 0; i < bElements.Length; i++)
            {
                DataManager.DataBaseContext.OponentLocations.Remove(bElements[i]);
            }
            DataManager.DataBaseContext.SaveChanges();
            for (int i = 0; i < opponents.Count; i++)
            {
                DataManager.DataBaseContext.OponentLocations.Add(Mapper.Map<OpponentLocation>(opponents[i]));
            }
            DataManager.DataBaseContext.SaveChanges();
            return true;
        }
    }
}
