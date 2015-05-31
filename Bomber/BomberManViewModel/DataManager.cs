using System.Collections.Generic;
using System.Data.Entity;
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
        public static BomberManContext DataBaseContext;

        /// <summary>
        /// Zainicjalizuj kontext bazy danych.
        /// </summary>
        /// <param name="context"></param>
        public static void InitContext(BomberManContext context = null)
        {
            CreateMappers();
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BomberManContext>());
            DataBaseContext = context ?? new BomberManContext();
            CreateElements();
            CreateOponents();
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
            Mapper.CreateMap<BoardElementLocation, BoardElementLocationDao>()
                .ConvertUsing<BoardElementLocationEntityToBoardElementLocationDao>();
            Mapper.CreateMap<BoardElementLocationDao, BoardElementLocation>()
                .ConvertUsing<BoardElementLocationDaoToBoardElementLocationEntity>();
            Mapper.CreateMap<OpponentLocation, OpponentLocationDao>()
                .ConvertUsing<OpponentLocationEntityToOpponentLocationDao>();
            Mapper.CreateMap<OpponentLocationDao, OpponentLocation>()
                .ConvertUsing<OpponentLocationDaoToOpponentLocationEntity>();
        }

        #region TEST

        /// <summary>
        /// Wyczyść bazę danych, metoda stworzona na potzreby testów.
        /// </summary>
        private static void Clear()
        {
            var query = from b in DataBaseContext.OponentLocations select b;
            while (query.Any())
            {
                DataBaseContext.OponentLocations.Remove(query.First());
                DataBaseContext.SaveChanges();
            }
            var query1 = from b in DataBaseContext.BoardElementLocations select b;
            while (query1.Any())
            {
                DataBaseContext.BoardElementLocations.Remove(query1.First());
                DataBaseContext.SaveChanges();
            }
            var query2 = from b in DataBaseContext.Games select b;
            while (query2.Any())
            {
                DataBaseContext.Games.Remove(query2.First());
                DataBaseContext.SaveChanges();
            }
            var query3 = from b in DataBaseContext.Users select b;
            while (query3.Any())
            {
                DataBaseContext.Users.Remove(query3.First());
                DataBaseContext.SaveChanges();
            }
            var query4 = from b in DataBaseContext.Opponents select b;
            while (query4.Any())
            {
                DataBaseContext.Opponents.Remove(query4.First());
                DataBaseContext.SaveChanges();
            }
            var query5 = from b in DataBaseContext.BoardElements select b;
            while (query5.Any())
            {
                DataBaseContext.BoardElements.Remove(query5.First());
                DataBaseContext.SaveChanges();
            }
        }

        #endregion

        /// <summary>
        /// Utwórz opisy przeciwników oraz nowe rekordy w bazie danych o ile już nie istnieją.
        /// </summary>
        /// <returns></returns>
        private static void CreateOponents()
        {
            if (!DataBaseContext.Opponents.Any())
            {
                var oponents = new List<Opponent>
                {
                    new Opponent
                    {
                        Description =
                            "Zły duch. Potrafi poruszać się po polach białych i szarych (zniszczalnych przez bomby). " +
                            "Goni gracza," +
                            " po usłyszeniu wybuchu bomby w okolicy biegnie w jej kierunku sądząc, że zasta tam gracza. " +
                            "Spotkanie Ducha z graczem oznacza koniec gry dla gracza. Duch po wejści na pole oznaczone " +
                            "kolorem czerwonym ginie.",
                        OpponentType = OpponentType.Ghost,
                        Id = 1
                    },
                    new Opponent
                    {
                        Description = "Zła ośmiornica. Potrafi poruszać się jedynie po polach białych. " +
                                      "Porusza się w losowym kierunku chyba, że w lini prostej zauważy gracza wówczas " +
                                      "goni gracza do miejsca gdzie widział go ostatnim razem." +
                                      " Po spotkaniu gracza z panem ośmiornicą gracz traci 40% życia. " +
                                      "Pan Ośmiornica ginie od eksplozji bomby.",
                        OpponentType = OpponentType.Octopus,
                        Id = 2
                    },
                };
                oponents.ForEach(s => DataBaseContext.Opponents.Add(s));
                DataBaseContext.SaveChanges();
            }
        }

        /// <summary>
        /// Utwóz elementy planszy wraz z opisami o ile już nie istnieją.
        /// </summary>
        /// <returns></returns>
        private static void CreateElements()
        {
            if (!DataBaseContext.BoardElements.Any())
            {
                var elements = new List<BoardElement>
                {
                    new BoardElement
                    {
                        Id = 1,
                        Description = "Bomba. Zakres bomby to po 3 pola w każdym z czterech kierunków. " +
                                      "Wykorzystywany element przez gracza do zabijania przeciwników. Gracz na początku poziomu ma 3 " +
                                      "bomby do wykorzystania na raz. Ilość bomb może się zwiększyć" +
                                      "w zależności od ilości zyskanych bonusów związanych ze zwiększeniem ilości bomb.",
                        ElementType = BoardElementType.Bomb
                    },
                    new BoardElement
                    {
                        Id = 2,
                        Description =
                            "Niezniszczlny Blok. Żadna postać znajdująca się na planszy nie może stanąc na tym polu. " +
                            "Bomby po wybuchnięciu nie zniszczają tego pola.",
                        ElementType = BoardElementType.BlackBlock
                    },
                    new BoardElement
                    {
                        Id = 3,
                        Description = "Zwiększona ilość bomb. Po natrafieniu na ten bonus gracz otzrymuje dodatkową " +
                                      "bombę do wykorzystywania przez cały poziom.",
                        ElementType = BoardElementType.BombAmountBonus
                    },
                    new BoardElement
                    {
                        Id = 4,
                        Description = "Przyspieszenie. Prędkość gracza wzrasta dwukrotnie na czas 4 sekund.",
                        ElementType = BoardElementType.FastBonus
                    },
                    new BoardElement
                    {
                        Id = 5,
                        Description =
                            "Zniszczalny blok. Jeżeli w pobliżu tego bloku znajdzie się bomba to blok zostanie zamieniony na biały. " +
                            "Jedynie duchy mogą przechodzić przez ten block jednakże dwa razy wolniej " +
                            "niż jakby pokonywałay białe bloki",
                        ElementType = BoardElementType.GrayBlock
                    },
                    new BoardElement
                    {
                        Id = 6,
                        Description = "Nieśmiertelność. Dzięki temu bonusowi gracz nie może umrzeć przez 4 sekundy gry",
                        ElementType = BoardElementType.InmortalBonus
                    },
                    new BoardElement
                    {
                        Id = 7,
                        Description =
                            "Dodtakowe punkty. Dzięki temu bonusowi gracz otrzymuje ododatkowe punkty do całkowitej puli.",
                        ElementType = BoardElementType.PointBonus
                    },
                    new BoardElement
                    {
                        Id = 8,
                        Description =
                            "Blok gotowy do wybuchu. Jest to blok, który pojawia się na chwilę po wybuchu bomby w jej pobliżu. Wejście na taki blok kończy się" +
                            "śmiercią dla każdej postaci na planszy.",
                        ElementType = BoardElementType.RedBlock
                    },
                    new BoardElement
                    {
                        Id = 9,
                        Description =
                            "Spowolnienie. Gracz po uzyskaniu tego bonusa niestety spowalnia ruch przeciwników dwukrotnie na kilka sekund.",
                        ElementType = BoardElementType.SlowBonus
                    },
                    new BoardElement
                    {
                        Id = 10,
                        Description =
                            "Zwiększenie mocy bomb. Po uzyskaniu tego bonusa gracz otrzymuje zwiększenie mocy bomb z zakresu 3 do 5 jedynie na kilka sekund.",
                        ElementType = BoardElementType.StrenghtBonus
                    },
                    new BoardElement
                    {
                        Id = 11,
                        Description =
                            "Zwykłe pole. Jest to pole oznaczone kolorem białym. Każda postać może poruszać się po takich polach.",
                        ElementType = BoardElementType.WhiteBlock
                    }
                };

                elements.ForEach(s => DataBaseContext.BoardElements.Add(s));
                DataBaseContext.SaveChanges();
            }
        }
    }
}
