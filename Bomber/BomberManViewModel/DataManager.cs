using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BomberManModel;
using BomberManModel.Entities;
using BomberManViewModel.Converters;
using BomberManViewModel.DataAccessObjects;

namespace BomberManViewModel
{
    /// <summary>
    /// Klasa zarządzająca kontekstem bazy danych.
    /// </summary>
    public class DataManager
    {
        static public BomberManContext DataBaseContext;

        static public void InitContext(BomberManContext context = null)
        {
            CreateMappers();
            DataBaseContext = context ?? new BomberManContext();
            //CreateGame();
        }

        /// <summary>
        /// Ustaw wszytskie automatyczne mapowania encji na elementy z widoku i odwrotnie.
        /// </summary>
        private static void CreateMappers() 
        {
            Mapper.CreateMap<User, UserDao>().ConvertUsing<UserEntityToUserDao>();
            Mapper.CreateMap<UserDao, User>().ConvertUsing<UserDaoToUserEntity>();
            Mapper.CreateMap<Game, GameDao>().ConvertUsing<GameEntityToGameDao>();
            Mapper.CreateMap<GameDao, Game>().ConvertUsing<GameDaoToGameEntity>();
            Mapper.CreateMap<Opponent, OpponentDao>().ConvertUsing<OpponentEntityToOpponentDao>();
            Mapper.CreateMap<OpponentDao, Opponent>().ConvertUsing<OpponentDaoToOpponentEntity>();
            Mapper.CreateMap<BoardElement, BoardElementDao>().ConvertUsing<BoardElementEntityToBoardElementDao>();
            Mapper.CreateMap<BoardElementDao, BoardElement>().ConvertUsing<BoardElementDaoToBoardElementEntity>();
            Mapper.CreateMap<BoardElementLocation, BoardElementLocationDao>().ConvertUsing<BoardElementLocationEntityToBoardElementLocationDao>();
            Mapper.CreateMap<BoardElementLocationDao, BoardElementLocation>().ConvertUsing<BoardElementLocationDaoToBoardElementLocationEntity>();
            Mapper.CreateMap<OpponentLocation, OpponentLocationDao>().ConvertUsing<OpponentLocationEntityToOpponentLocationDao>();
            Mapper.CreateMap<OpponentLocationDao, OpponentLocation>().ConvertUsing<OpponentLocationDaoToOpponentLocationEntity>();
        }

        #region TEST

        private static User CreateUser()
        {
            var elements = new List<User>{
                               new User{Name = "SYLWIA", Password = "a", Id = 1 }
                           };
            elements.ForEach(s => DataBaseContext.Users.Add(s));
            DataBaseContext.SaveChanges();
            return elements.First<User>();
        }

        private static List<Opponent> CreateOponents() 
        {
            var oponents = new List<Opponent>{
                               new Opponent{ Description="Zły duch", OpponentType = OpponentType.Ghost,
                                   Id = 1  },
                               new Opponent{ Description="Zła ośmiornica", OpponentType = OpponentType.Octopus,
                                   Id = 2  },
                           };
            oponents.ForEach(s => DataBaseContext.Oponents.Add(s));
            DataBaseContext.SaveChanges();
            return oponents;
        }

        private static void CreateGame() 
        {
            var game = new List<Game>{
                               new Game{Id = 1, User = CreateUser(), Points = 0, SaveTime = DateTime.Now,
                                Finished = false, Life = 100, Level = 1, PlayerXLocation = 0, PlayerYLocation = 0}
                           };
            game.ForEach(s => DataBaseContext.Games.Add(s));
            var oponents = CreateOponents();
            var elements = CreateElements();
            var oponentsLocations = new List<OpponentLocation>();
            for (int i = 0; i < oponents.Count; i++ )
            {
                oponentsLocations.Add(new OpponentLocation
                {
                    Id = 1,
                    IsAlive = true,
                    Game = game.First<Game>(),
                    XLocation = i+1,
                    YLocation = i+1,
                    Oponent = oponents.ElementAt<Opponent>(i)
                });

            }
            oponentsLocations.ForEach(s => DataBaseContext.OponentLocations.Add(s));
            DataBaseContext.SaveChanges();
            var elementsLocations = new List<BoardElementLocation>();
            int w = 0;
            for (int j = 0; j < 12; j++ )
                for (int i = 0; i < 16; i++)
                {
                    elementsLocations.Add(new BoardElementLocation
                    {
                        Id = w+1,
                        Game = game.First<Game>(),
                        XLocation = i,
                        YLocation = j,
                        BoardElement = elements.ElementAt<BoardElement>(w%4 == 0? 1 : 10),
                        Timeout = 10000
                    });
                    w++;

                }
            elementsLocations.ForEach(s => DataBaseContext.BoardElementLocations.Add(s));
            DataBaseContext.SaveChanges();
        }

        private static List<BoardElement> CreateElements()
        {
            var elements = new List<BoardElement>{
            new BoardElement{Id = 1, Description = "Bomba",  ElementType = BoardElementType.Bomb},
            new BoardElement{Id = 2, Description = "Niezniszczlny Blok",  ElementType = BoardElementType.BlackBlock},
            new BoardElement{Id = 3, Description = "Zwiększona ilość bomb", ElementType = BoardElementType.BombAmountBonus},
            new BoardElement{Id = 4, Description = "Przyspieszenie", ElementType = BoardElementType.FastBonus},
            new BoardElement{Id = 5, Description = "Zniszczalny blok", ElementType = BoardElementType.GrayBlock},
            new BoardElement{Id = 6, Description = "Nieśmiertelność", ElementType = BoardElementType.InmortalBonus},
            new BoardElement{Id = 7, Description = "Dodtakowe życie", ElementType = BoardElementType.LifeBonus},
            new BoardElement{Id = 8, Description = "Blok gotowy do wybuchu", ElementType = BoardElementType.RedBlock},
            new BoardElement{Id = 9, Description = "Spowolnienie", ElementType = BoardElementType.SlowBonus},
            new BoardElement{Id = 10, Description = "Zwiększenie mocy bomb", ElementType = BoardElementType.StrenghtBonus},
            new BoardElement{Id = 11, Description = "Zwykłe pole", ElementType = BoardElementType.WhiteBlock}
            };
            elements.ForEach(s => DataBaseContext.BoardElements.Add(s));
            DataBaseContext.SaveChanges();
            return elements;
        }

        #endregion
    }
}
