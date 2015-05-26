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
        private const int SIMPLE_LEVEL_ROWS = 12;
        private const int SIMPLE_LEVEL_COLUMNS = 18;
        private const int MEDIUM_LEVEL_ROWS = 14;
        private const int MEDIUM_LEVEL_COMULNS = 20;
        private const int HIGH_LEVEL_ROWS = 18;
        private const int HIGH_LEVEL_COLUMNS = 22;
        private const int SUPER_LEVEL_ROWS = 22;
        private const int SUPER_LEVEL_COLUMNS = 32;
        private const int MAX_NUMBER_OF_LEVEL = 19;
        private const int PERCENTAGE_OF_SOLID_BLOCKS = 10;
        private const int PERCENTAGE_OF_GREY_BLOCKS = 40;

        private Random _random;
        private List<OponentLocationDAO> opponents;
        private List<BoardElementDAO> boardElements;
        private BoardEngine boardEngine;
        private SpecialElementsEngine specialEngine;
        /// <summary>
        /// Lista zawierająca wszystkie jednostkowe pola planszy ze wzgędu na typ pola
        /// Przekazywana do BoardEngine w celu narysowania jednostkowych pól
        /// </summary>
        private List<BlockKind> _boradBlocksKinds;
        private List<ProgressBar> _bonuses;
        private List<ProgressBar> _hearts;

        /// <summary>
        /// Przechowywane textury ładowane podczas włączania gry
        /// Jedna textura na jeden obrazek
        /// </summary>
        private List<Texture2D> _blockTextures;
        private Texture2D _playerTexture;
        private Texture2D _bombTexture;
        private List<Texture2D> _opponentsTxtures;


        /// <summary>
        /// Utwórz widok gry ze wszystkimi polami jednostkowymi
        /// Jeżeli nie ma utworzonego GameDAO w Utils to wygeneruj nową grę z poziomem 0
        /// W przeciwnym przypadku załąduj grę z Utils i utwórz widok całej planszy
        /// </summary>
        public GameScreen(List<Texture2D> blockTextures, Texture2D bombTexture)
        {
            _blockTextures = blockTextures;
            _bombTexture = bombTexture;
            _random = new Random();
            _boradBlocksKinds = new List<BlockKind>();
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
                //    BlockKind blockKind = BlockKind.White;
                //    switch (blocks[i].BoardElement.ElementType)
                //    {
                //        case BoardElementType.WhiteBlock:
                //            blockKind = BlockKind.White;
                //            break;
                //        case BoardElementType.RedBlock:
                //            blockKind = BlockKind.Red;
                //            break;
                //        case BoardElementType.GrayBlock:
                //            blockKind = BlockKind.Grey;
                //            break;
                //        case BoardElementType.BlackBlock:
                //            blockKind = BlockKind.Black;
                //            break;
                //    }
                //    _boradBlocksKinds.Add(blockKind);
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
            if (boardEngine != null)
            {
                boardEngine.Draw(spriteBatch);

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
            if (boardEngine != null)
            {
                boardEngine.Update(_boradBlocksKinds, windowWidth, windowHeight);
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
            /* for (int i = 0; i < boardEngine.rows; i++)
                 for (int j = 0; j < boardEngine.columns; j++ )
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
            boardEngine = new BoardEngine(_blockTextures, rows, columns);
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
            if (level < 0 || level > MAX_NUMBER_OF_LEVEL)
                throw new NotImplementedException("Level Should be between indexes 0 and " + MAX_NUMBER_OF_LEVEL);
            if (level < 5)
            {
                RandomBlocks(SIMPLE_LEVEL_ROWS, SIMPLE_LEVEL_COLUMNS);
            }
            else if (level < 10)
            {
                RandomBlocks(MEDIUM_LEVEL_ROWS, MEDIUM_LEVEL_COMULNS);
            }
            else if (level < 15)
            {
                RandomBlocks(HIGH_LEVEL_ROWS, HIGH_LEVEL_COLUMNS);
            }
            else
            {
                RandomBlocks(SUPER_LEVEL_ROWS, SUPER_LEVEL_COLUMNS);
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
            int randomBlocks = PERCENTAGE_OF_SOLID_BLOCKS * rows * columns / 100;
            _boradBlocksKinds = new List<BlockKind>();
            // zapełnij całą listę szarymi zniszczalnymi blokami
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _boradBlocksKinds.Add(BlockKind.White);
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
                } while (_boradBlocksKinds[x * columns + y].Equals(BlockKind.Black));
                _boradBlocksKinds[x * columns + y] = BlockKind.Black;
            }
            randomBlocks = PERCENTAGE_OF_GREY_BLOCKS * rows * columns / 100;
            // wylosuj pozycje na których ma znajdować się szary block
            for (int i = 0; i < randomBlocks; i++)
            {
                int x, y;
                do
                {
                    x = _random.Next(rows);
                    y = _random.Next(columns);
                } while (_boradBlocksKinds[x * columns + y].Equals(BlockKind.Grey) ||
                    _boradBlocksKinds[x * columns + y].Equals(BlockKind.Black));
                _boradBlocksKinds[x * columns + y] = BlockKind.Grey;
            }
        }

        /// <summary>
        /// Wylosuj pola, na których powinny znaleźć się bonusy
        /// Każdy typ bonusa losuj z częstotliwością w zależności od poziomu
        /// Bonusy nie mogą być tworzone na polach, na których znajduje się blok czarny, gracz nie może na niego wejść
        /// </summary>
        /// <param name="rows">ilość wierszy pól jednostkowch na planszy</param>
        /// <param name="columns">ilość kolumn pól jednostkowych na planszy</param>
        /// <param name="level">poziom, dla którego generowana jest plansza</param>
        private void RandomBonuses(int rows, int columns, int level)
        {
            
        }
    }
}
