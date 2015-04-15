using AutoMapper;
using BomberManModel;
using BomberManModel.Entities;
using BomberManViewModel.Converters;
using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomberManViewModel
{
    public class DataManager
    {
        static bool init = true;
        
        static public BomberManContext DataBaseContext;

        static public void InitContext(BomberManContext Context = null)
        {
            CreateMappers();
            if (Context == null) 
                DataBaseContext = new BomberManContext();
            else 
                DataBaseContext = Context;
            if (init) 
                return; 
            init = true;
            CreateGame();
        }

        private static void CreateMappers() 
        {
            Mapper.CreateMap<User, UserDAO>().ConvertUsing<UserEntityToUserDAO>();
            Mapper.CreateMap<UserDAO, User>().ConvertUsing<UserDAOToUserEntity>();
            Mapper.CreateMap<Game, GameDAO>().ConvertUsing<GameEntityToGameDAO>();
            Mapper.CreateMap<GameDAO, Game>().ConvertUsing<GameDAOToGameEntity>();
            Mapper.CreateMap<Oponent, OponentDAO>().ConvertUsing<OponentEntityToOponentDAO>();
            Mapper.CreateMap<OponentDAO, Oponent>().ConvertUsing<OponentDAOToOponentEntity>();
            Mapper.CreateMap<BoardElement, BoardElementDAO>().ConvertUsing<BoardElementEntityToBoardElementDAO>();
            Mapper.CreateMap<BoardElementDAO, BoardElement>().ConvertUsing<BoardElementDAOToBoardElementEntity>();
            Mapper.CreateMap<BoardElementLocation, BoardElementLocationDAO>().ConvertUsing<BoardElementLocationEntityToBoardElementLocationDAO>();
            Mapper.CreateMap<BoardElementLocationDAO, BoardElementLocation>().ConvertUsing<BoardElementLocationDAOToBoardElementLocationEntity>();
            Mapper.CreateMap<OponentLocation, OponentLocationDAO>().ConvertUsing<OponentLocationEntityToOponentLocationDAO>();
            Mapper.CreateMap<OponentLocationDAO, OponentLocation>().ConvertUsing<OponentLocationDAOToOponentLocationEntity>();
        }

        #region TEST

        private static User CreateUser()
        {
            var elements = new List<User>{
                               new User{Name = "SYLWIA", Password = "a", ID = 1 }
                           };
            elements.ForEach(s => DataBaseContext.Users.Add(s));
            DataBaseContext.SaveChanges();
            return elements.First<User>();
        }

        private static List<Oponent> CreateOponents() 
        {
            var oponents = new List<Oponent>{
                               new Oponent{ Description="Zły duch", OpponentType = BomberManModel.OpponentType.Ghost,
                                   ID = 1  },
                               new Oponent{ Description="Zła ośmiornica", OpponentType = BomberManModel.OpponentType.Octopus,
                                   ID = 2  },
                           };
            oponents.ForEach(s => DataBaseContext.Oponents.Add(s));
            DataBaseContext.SaveChanges();
            return oponents;
        }

        private static void CreateGame() 
        {
            var game = new List<Game>{
                               new Game{ID = 1, User = CreateUser(), Points = 0, SaveTime = DateTime.Now,
                                Finished = false, Life = 100, Level = 1, PlayerXLocation = 0, PlayerYLocation = 0}
                           };
            game.ForEach(s => DataBaseContext.Games.Add(s));
            var oponents = CreateOponents();
            var elements = CreateElements();
            var oponentsLocations = new List<OponentLocation>();
            for (int i = 0; i < oponents.Count; i++ )
            {
                oponentsLocations.Add(new OponentLocation
                {
                    ID = 1,
                    IsAlive = true,
                    Game = game.First<Game>(),
                    XLocation = i+1,
                    YLocation = i+1,
                    Oponent = oponents.ElementAt<Oponent>(i)
                });

            }
            oponentsLocations.ForEach(s => DataBaseContext.OponentLocations.Add(s));
            DataBaseContext.SaveChanges();
            var elementsLocations = new List<BoardElementLocation>();
            for (int i = 0; i < elements.Count; i++)
            {
                elementsLocations.Add(new BoardElementLocation
                {
                    ID = 1,
                    Game = game.First<Game>(),
                    XLocation = i + 1,
                    YLocation = i + 1,
                    BoardElement = elements.ElementAt<BoardElement>(i),
                    Timeout = 10000
                });

            }
            elementsLocations.ForEach(s => DataBaseContext.BoardElementLocations.Add(s));
            DataBaseContext.SaveChanges();
        }

        private static List<BoardElement> CreateElements()
        {
            var elements = new List<BoardElement>{
            new BoardElement{ID = 1, Description = "Bomba",  ElementType = BoardElementType.Bomb},
            new BoardElement{ID = 2, Description = "Niezniszczlny Blok",  ElementType = BoardElementType.BlackBlock},
            new BoardElement{ID = 3, Description = "Zwiększona ilość bomb", ElementType = BoardElementType.BombAmountBonus},
            new BoardElement{ID = 4, Description = "Przyspieszenie", ElementType = BoardElementType.FastBonus},
            new BoardElement{ID = 5, Description = "Zniszczalny blok", ElementType = BoardElementType.GrayBlock},
            new BoardElement{ID = 6, Description = "Nieśmiertelność", ElementType = BoardElementType.InmortalBonus},
            new BoardElement{ID = 7, Description = "Dodtakowe życie", ElementType = BoardElementType.LifeBonus},
            new BoardElement{ID = 8, Description = "Blok gotowy do wybuchu", ElementType = BoardElementType.RedBlock},
            new BoardElement{ID = 9, Description = "Spowolnienie", ElementType = BoardElementType.SlowBonus},
            new BoardElement{ID = 10, Description = "Zwiększenie mocy bomb", ElementType = BoardElementType.StrenghtBonus},
            new BoardElement{ID = 11, Description = "Zwykłe pole", ElementType = BoardElementType.WhiteBlock}
            };
            elements.ForEach(s => DataBaseContext.BoardElements.Add(s));
            DataBaseContext.SaveChanges();
            return elements;
        }

        #endregion
    }
}
