using BomberMan.Common.Components.StateComponents;
using BomberMan.Common.Engines;
using BomberManModel;
using BomberManViewModel.DataAccessObjects;
using BomberManViewModel.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;

namespace BomberMan.Screens
{
    public class GameScreen : Screen
    {
        private const int SimpleLevelRows = 12;
        private const int SimpleLevelColumns = 18;
        private const int MediumLevelRows = 14;
        private const int MediumLevelComulns = 20;
        private const int HighLevelRows = 18;
        private const int HighLevelColumns = 22;
        private const int SuperLevelRows = 22;
        private const int SuperLevelColumns = 32;
        private const int MaxNumberOfLevel = 19;
        private const int PercentageOfSolidBlocks = 10;
        private const int PercentageOfGreyBlocks = 40;
        private const int PercentageOfBonuses = 5;

        private readonly Random _random;
        private List<OponentLocationDAO> _opponents;
        private List<BoardElementDAO> _boardElements;
        private BoardEngine _boardEngine;
        /// <summary>
        /// Lista zawierająca wszystkie jednostkowe pola planszy ze wzgędu na typ pola
        /// Przekazywana do BoardEngine w celu narysowania jednostkowych pól
        /// </summary>
        private List<BlockType> _boradBlocksTypes;
        private readonly Dictionary<int, BonusType> _bonusLocations;
        private List<ProgressBar> _bonuses;
        private List<ProgressBar> _hearts;

        /// <summary>
        /// Przechowywane textury ładowane podczas włączania gry
        /// Jedna textura na jeden obrazek
        /// </summary>
        private readonly List<Texture2D> _blockTextures;
        private readonly Texture2D _playerTexture;
        private readonly Texture2D _bombTexture;
        private readonly List<Texture2D> _opponentsTxtures;
        private readonly List<Texture2D> _bonusesTextures;


        /// <summary>
        /// Utwórz widok gry ze wszystkimi polami jednostkowymi
        /// Jeżeli nie ma utworzonego GameDAO w Utils to wygeneruj nową grę z poziomem 0
        /// W przeciwnym przypadku załąduj grę z Utils i utwórz widok całej planszy
        /// </summary>
        public GameScreen(List<Texture2D> blockTextures, List<Texture2D> bonusesTextures,
            Texture2D bombTexture, Texture2D playerTexture, List<Texture2D> opponentsTxtures)
        {
            _bonusesTextures = bonusesTextures;
            _blockTextures = blockTextures;
            _bombTexture = bombTexture;
            _playerTexture = playerTexture;
            _opponentsTxtures = opponentsTxtures;
            _random = new Random();
            _boradBlocksTypes = new List<BlockType>();
            _bonusLocations = new Dictionary<int, BonusType>();
            if (Utils.Game == null)
            {
                Utils.User = new UserDAO()
                {
                    ID = 1,
                    Name = "ala",
                    Password = "ala"
                };
                Utils.Game = CreateNewGame();
            }
            else
            {
                //String message = "";
                //List<BoardElementLocationDAO> blocks = BoardService.GetAllBlocksForGame(Utils.Game, out message);
                //for(int i = 0; i< blocks.Count; i++)
                //{
                //    BlockType blockKind = BlockType.White;
                //    switch (blocks[i].BoardElement.ElementType)
                //    {
                //        case BoardElementType.WhiteBlock:
                //            blockKind = BlockType.White;
                //            break;
                //        case BoardElementType.RedBlock:
                //            blockKind = BlockType.Red;
                //            break;
                //        case BoardElementType.GrayBlock:
                //            blockKind = BlockType.Grey;
                //            break;
                //        case BoardElementType.BlackBlock:
                //            blockKind = BlockType.Black;
                //            break;
                //    }
                //    _boradBlocksTypes.Add(blockKind);
                //}
                //List<BoardElementDAO> bonuses = BoardService.GetAllBonusesForGame(Utils.Game, out message);
                //List<BoardElementDAO> bombs = BoardService.GetAllBombsForGame(Utils.Game, out message);
            }
            // najpierw generujemy blocki
            // potem bomby + inne lementy znaczące
            // potem generujemy opponentów
            // na koniec gracza
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_boardEngine != null)
            {
                _boardEngine.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Uaktualnij widok planszy w zależności od rozmiaru okna gry
        /// </summary>
        /// <param name="gameTime">Czas gry</param>
        /// <param name="windowWidth">Szerokość okna</param>
        /// <param name="windowHeight">Wysokość okna</param>
        public override void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            // dla każdej bomby która nie ma ustawione <0  w czaie do wybuchu zmniejszyć czas o interval jak zmniejszymy i pojawi się mniej niż zero
            // "usuwamy obiekt" z listy do rysowania

            //gameTime.ElapsedGameTime.Milliseconds;
            if (_boardEngine != null)
            {
                int player = 1;
                List<int> bomList = new List<int>();
                _boardEngine.Update(_boradBlocksTypes, _bonusLocations,
                    new Dictionary<int, OpponentType>(), bomList, player, windowWidth, windowHeight);
            }
        }

        public override void HandleKeyboard()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Utwórz nową instancję gry z poziomem 0, rozpoczynającym grę
        /// </summary>
        /// <returns>Zwróć instancję GameDAO i operuj na niej do zakończenia jednej gry</returns>
        private GameDAO CreateNewGame()
        {
            GenerateGameForSpecifiedLevel(0);
            GameDAO gameDao = new GameDAO()
            {
                Level = 0,
                Life = 100,
                Finished = false,
                PlayerXLocation = (uint)1,
                PlayerYLocation = (uint)1,
                Points = 0,
                SaveTime = DateTime.Now,
                Time = 100,
                User = Utils.User
            };
            return gameDao;
        }

        /// <summary>
        /// Utwórz wartości wag pól potrzebnych do zapuszczenia algorytmu A*
        /// Wykorzystywane do wyznaczenia ścieżko gonienia gracza przez przeciwnika
        /// </summary>
        /// <returns>Odpowiednik dwuwyniarowej tablicy zawierającej pary<stary_index_pola, waga_pola></returns>
        private List<List<Tuple<int, int>>> GenereteFieldValues()
        {
            /* for (int i = 0; i < _boardEngine.rows; i++)
                 for (int j = 0; j < _boardEngine.columns; j++ )
                 {

                 }*/
            return null;
        }

        /// <summary>
        /// Utwórz nowy BoadEngine potrzebny do zarządzania polami planszy
        /// </summary>
        /// <param name="rows">ilość jednoskowych pól w jednej kolumnie</param>
        /// <param name="columns">ilość jednostkowych pól w jednym wierszu</param>
        private void CreateBoardEngine(int rows, int columns)
        {
            _boardEngine = new BoardEngine(_blockTextures, _bonusesTextures, _opponentsTxtures,
                _bombTexture, _playerTexture, rows, columns);
        }

        /// <summary>
        /// Zapisz dotychczasowy stan gry do bazie danych
        /// </summary>
        private void SaveGame()
        {

        }

        /// <summary>
        /// Utwórz wszytskie potrzebne informacje wymagane do wyświetlenia planszy, bonusów, przeciników oraz gracza
        /// poziomy [0,4] plansza ma wyniary <value>SIMPLE</value>
        /// poziomy [5,9] plansza ma wymiary <value>MEDIUM</value>
        /// poziomy [10,14] plansza ma wymiary <value>HIGH</value>
        /// poziomy [15,19] plansza ma wymiary <value>SUPER</value>
        /// </summary>
        /// <param name="level">poziom, dla którego generowana jest plansza</param>
        private void GenerateGameForSpecifiedLevel(int level)
        {
            if (level < 0 || level > MaxNumberOfLevel)
                throw new NotImplementedException("Level Should be between indexes 0 and " + MaxNumberOfLevel);
            if (level < 5)
            {
                RandomBlocks(SimpleLevelRows, SimpleLevelColumns);
                RandomBonuses(SimpleLevelRows, SimpleLevelColumns, level);
            }
            else if (level < 10)
            {
                RandomBlocks(MediumLevelRows, MediumLevelComulns);
            }
            else if (level < 15)
            {
                RandomBlocks(HighLevelRows, HighLevelColumns);
            }
            else
            {
                RandomBlocks(SuperLevelRows, SuperLevelColumns);
            }
        }

        /// <summary>
        /// Wylosuj pola, które powinny być niezniszczalne lub zniszczalne, pozostałe ustaw na białe, zwykłe
        /// Pola ustawiane jedynie na wartości <value>GREY</value>, <value>BLACK</value>, <value>WHITE</value>
        /// </summary>
        /// <param name="rows">ilość wierszy pól jednostkowch na planszy</param>
        /// <param name="columns">ilość kolumn pól jednostkowych na planszy</param>
        private void RandomBlocks(int rows, int columns)
        {
            CreateBoardEngine(rows, columns);
            int randomBlocks = PercentageOfSolidBlocks * rows * columns / 100;
            _boradBlocksTypes = new List<BlockType>();
            // zapełnij całą listę szarymi zniszczalnymi blokami
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _boradBlocksTypes.Add(BlockType.White);
                }
            }
            // wylosuj pozycje na których ma znajdować się czarny block
            for (int i = 0; i < randomBlocks; i++)
            {
                int x, y;
                do
                {
                    x = _random.Next(rows);
                    y = _random.Next(columns);
                } while (_boradBlocksTypes[x * columns + y].Equals(BlockType.Black));
                _boradBlocksTypes[x * columns + y] = BlockType.Black;
            }
            randomBlocks = PercentageOfGreyBlocks * rows * columns / 100;
            // wylosuj pozycje na których ma znajdować się szary block
            for (int i = 0; i < randomBlocks; i++)
            {
                int x, y;
                do
                {
                    x = _random.Next(rows);
                    y = _random.Next(columns);
                } while (_boradBlocksTypes[x * columns + y].Equals(BlockType.Grey) ||
                    _boradBlocksTypes[x * columns + y].Equals(BlockType.Black));
                _boradBlocksTypes[x * columns + y] = BlockType.Grey;
            }
        }

        /// <summary>
        /// Wylosuj pola, na których powinny znaleźć się bonusy
        /// Każdy typ bonusa losuj z częstotliwością w zależności od poziomu
        /// Bonusy tworzone są tylko na polach szarych, na każdym polu szarym wsytępuje maksymalnie jeden bonus.
        /// Ilość wszystkich bonusów na planszy po rozpoczęsciu poziomu to <value>PercentageOfBonuses</value> * ilość pól
        /// Wylosuj z prawdopodobieństwem 1/10 Bonus Inmortal, 3/10 Life, 2/10 Fast, 1/10 Slow, 2/10 Strength, 1/10 Extra Bomb
        /// </summary>
        /// <param name="rows">ilość wierszy pól jednostkowch na planszy</param>
        /// <param name="columns">ilość kolumn pól jednostkowych na planszy</param>
        /// <param name="level">poziom, dla którego generowana jest plansza</param>
        private void RandomBonuses(int rows, int columns, int level)
        {
            int maxBonusesAmount = PercentageOfBonuses * _boradBlocksTypes.Count - 1;
            int counter = 0;
            for (int i = 0; i < _boradBlocksTypes.Count; i++)
            {
                if (counter == maxBonusesAmount) break;
                if (_boradBlocksTypes[i].Equals(BlockType.Grey) && !_bonusLocations.ContainsKey(i))
                {
                    int number = _random.Next() % 10;
                    BonusType bonusType;
                    if (number == 0)
                    {
                        bonusType = BonusType.Inmortal;
                    }
                    else if (number > 0 && number < 4)
                    {
                        bonusType = BonusType.Life;
                    }
                    else if (number == 4 || number == 5)
                    {
                        bonusType = BonusType.Fast;
                    }
                    else if (number == 6)
                    {
                        bonusType = BonusType.Slow;
                    }
                    else if (number == 7 || number == 8)
                    {
                        bonusType = BonusType.Strenght;
                    }
                    else
                    {
                        bonusType = BonusType.BombAmount;
                    }
                    _bonusLocations.Add(i, bonusType);
                    counter++;

                }
            }
        }
    }
}
