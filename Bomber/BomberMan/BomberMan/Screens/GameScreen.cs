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
using BomberMan.Common;
using Microsoft.Xna.Framework.Input;

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
        private const int PercentageOfOpponents = 5;

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
        private readonly Dictionary<int, List<CharacterType>> _characterLocations;
        private readonly List<int> _bombLocations;
        private List<ProgressBar> _bonuses;
        private List<ProgressBar> _hearts;

        /// <summary>
        /// Przechowywane textury ładowane podczas włączania gry
        /// Jedna textura na jeden obrazek
        /// </summary>
        private readonly List<Texture2D> _blockTextures;

        private readonly Texture2D _bombTexture;
        private readonly List<Texture2D> _characterTextures;
        private readonly List<Texture2D> _bonusesTextures;

        float countDuration = 0.1f; //every  0.5s.
        float currentTime;

        /// <summary>
        /// Utwórz widok gry ze wszystkimi polami jednostkowymi
        /// Jeżeli nie ma utworzonego GameDAO w Utils to wygeneruj nową grę z poziomem 0
        /// W przeciwnym przypadku załąduj grę z Utils i utwórz widok całej planszy
        /// </summary>
        public GameScreen(List<Texture2D> blockTextures, List<Texture2D> bonusesTextures,
            Texture2D bombTexture, List<Texture2D> characterTextures)
        {
            _bonusesTextures = bonusesTextures;
            _blockTextures = blockTextures;
            _bombTexture = bombTexture;
            _characterTextures = characterTextures;
            _random = new Random();
            _boradBlocksTypes = new List<BlockType>();
            _bonusLocations = new Dictionary<int, BonusType>();
            _bombLocations = new List<int>();
            _characterLocations = new Dictionary<int, List<CharacterType>>();
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
                //    characterType blockKind = characterType.White;
                //    switch (blocks[i].BoardElement.ElementType)
                //    {
                //        case BoardElementType.WhiteBlock:
                //            blockKind = characterType.White;
                //            break;
                //        case BoardElementType.RedBlock:
                //            blockKind = characterType.Red;
                //            break;
                //        case BoardElementType.GrayBlock:
                //            blockKind = characterType.Grey;
                //            break;
                //        case BoardElementType.BlackBlock:
                //            blockKind = characterType.Black;
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
            _boardEngine.Draw(spriteBatch);
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
            _boardEngine.Update(_boradBlocksTypes, _bonusLocations,
                _characterLocations, _bombLocations, windowWidth, windowHeight);
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (currentTime >= countDuration)
            {
                currentTime -= countDuration;
                HandleKeyboard();
            }
        }

        /// <summary>
        /// Obsłuż wciskane klawisze na klawiaturze w zależności od wybranej opcji.
        /// </summary>
        public override void HandleKeyboard()
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            Keys[] keymap = KeyboardState.GetPressedKeys();
            int gamer = _boardEngine.PlayerLocation;
            int columns = _boardEngine.Columns;
            int rows = _boardEngine.Rows;
            int x = gamer/columns;
            int y = gamer - x*columns;
            foreach (Keys k in keymap)
            {
                if (Configuration.KeyboardOption.Equals(KeyboardOption.Arrows))
                {
                    switch (k)
                    {
                        case Keys.Down:
                            if (x < rows - 1)
                            {
                                int tmp = (x + 1)*columns + y;
                                if (_boradBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boradBlocksTypes[tmp].Equals(BlockType.Red))
                                {
                                    _characterLocations[gamer].Remove(CharacterType.Player);
                                    x++;
                                    gamer = x*columns + y;
                                    if (_characterLocations.ContainsKey(gamer))
                                        _characterLocations[gamer].Add(CharacterType.Player);
                                    else
                                    {
                                        _characterLocations.Add(gamer, new List<CharacterType>()
                                        {
                                            CharacterType.Player
                                        });
                                    }
                                }
                            }
                            break;
                        case Keys.Up:
                            if (x > 0)
                            {
                                int tmp = (x - 1)*columns + y;
                                if (_boradBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boradBlocksTypes[tmp].Equals(BlockType.Red))
                                {
                                    _characterLocations[gamer].Remove(CharacterType.Player);
                                    x--;
                                    gamer = x*columns + y;
                                    if (_characterLocations.ContainsKey(gamer))
                                        _characterLocations[gamer].Add(CharacterType.Player);
                                    else
                                    {
                                        _characterLocations.Add(gamer, new List<CharacterType>()
                                        {
                                            CharacterType.Player
                                        });
                                    }
                                }
                            }
                            break;
                        case Keys.Left:
                            if (y > 0)
                            {
                                int tmp = x*columns + y - 1;
                                if (_boradBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boradBlocksTypes[tmp].Equals(BlockType.Red))
                                {
                                    _characterLocations[gamer].Remove(CharacterType.Player);
                                    y--;
                                    gamer = x*columns + y;
                                    if (_characterLocations.ContainsKey(gamer))
                                        _characterLocations[gamer].Add(CharacterType.Player);
                                    else
                                    {
                                        _characterLocations.Add(gamer, new List<CharacterType>()
                                        {
                                            CharacterType.Player
                                        });
                                    }
                                }
                            }
                            break;
                        case Keys.Right:
                            if (y < columns - 1)
                            {
                                int tmp = x*columns + y + 1;
                                if (_boradBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boradBlocksTypes[tmp].Equals(BlockType.Red))
                                {
                                    _characterLocations[gamer].Remove(CharacterType.Player);
                                    y++;
                                    gamer = x*columns + y;
                                    if (_characterLocations.ContainsKey(gamer))
                                        _characterLocations[gamer].Add(CharacterType.Player);
                                    else
                                    {
                                        _characterLocations.Add(gamer, new List<CharacterType>()
                                        {
                                            CharacterType.Player
                                        });
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    switch (k)
                    {
                        case Keys.W:
                            break;
                        case Keys.S:
                            break;
                        case Keys.A:
                            break;
                        case Keys.D:
                            break;
                    }
                }

                // sprawdź czy nie trzeba postawić nowej bomby
                switch (k)
                {
                    case Keys.Space:
                        if (Configuration.BombKeyboardOption.Equals(BombKeyboardOption.Spcace))
                        {
                            _bombLocations.Add(gamer);
                        }
                        break;
                    case Keys.P:
                        if (Configuration.BombKeyboardOption.Equals(BombKeyboardOption.P))
                        {
                            _bombLocations.Add(gamer);
                        }
                        break;
                }
            }
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
                PlayerXLocation = (uint) 1,
                PlayerYLocation = (uint) 1,
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
            _boardEngine = new BoardEngine(_blockTextures, _bonusesTextures, _characterTextures,
                _bombTexture, rows, columns);
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
                RandomCharacters(SimpleLevelColumns);
            }
            else if (level < 10)
            {
                RandomBlocks(MediumLevelRows, MediumLevelComulns);
                RandomCharacters(MediumLevelComulns);
            }
            else if (level < 15)
            {
                RandomBlocks(HighLevelRows, HighLevelColumns);
                RandomCharacters(HighLevelColumns);
            }
            else
            {
                RandomBlocks(SuperLevelRows, SuperLevelColumns);
                RandomCharacters(SuperLevelColumns);
            }
            RandomBonuses();
        }

        #region RandomBoardValues

        /// <summary>
        /// Wylosuj pola, które powinny być niezniszczalne lub zniszczalne, pozostałe ustaw na białe, zwykłe
        /// Pola ustawiane jedynie na wartości <value>GREY</value>, <value>BLACK</value>, <value>WHITE</value>
        /// Do wszystkich pól white/grey da się dojść
        /// </summary>
        /// <param name="rows">ilość wierszy pól jednostkowch na planszy</param>
        /// <param name="columns">ilość kolumn pól jednostkowych na planszy</param>
        private void RandomBlocks(int rows, int columns)
        {
            CreateBoardEngine(rows, columns);
            int randomBlocks = PercentageOfSolidBlocks*rows*columns/100;
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
                } while (_boradBlocksTypes[x*columns + y].Equals(BlockType.Black));
                _boradBlocksTypes[x*columns + y] = BlockType.Black;
            }
            randomBlocks = PercentageOfGreyBlocks*rows*columns/100;
            // wylosuj pozycje na których ma znajdować się szary block
            for (int i = 0; i < randomBlocks; i++)
            {
                int x, y;
                do
                {
                    x = _random.Next(rows);
                    y = _random.Next(columns);
                } while (_boradBlocksTypes[x*columns + y].Equals(BlockType.Grey) ||
                         _boradBlocksTypes[x*columns + y].Equals(BlockType.Black));
                _boradBlocksTypes[x*columns + y] = BlockType.Grey;
            }
        }

        /// <summary>
        /// Wylosuj pola, na których powinny znaleźć się bonusy
        /// Każdy typ bonusa losuj z częstotliwością w zależności od poziomu
        /// Bonusy tworzone są tylko na polach szarych, na każdym polu szarym wsytępuje maksymalnie jeden bonus.
        /// Ilość wszystkich bonusów na planszy po rozpoczęsciu poziomu to <value>PercentageOfBonuses</value> * ilość pól
        /// Wylosuj z prawdopodobieństwem 1/10 Bonus Inmortal, 3/10 Life, 2/10 Fast, 1/10 Slow, 2/10 Strength, 1/10 Extra Bomb
        /// </summary>
        private void RandomBonuses()
        {
            int maxBonusesAmount = PercentageOfBonuses*_boradBlocksTypes.Count/100 - 1;
            int counter = 0;
            while (counter < maxBonusesAmount)
            {
                int index = _random.Next(_boradBlocksTypes.Count);
                if (_boradBlocksTypes[index].Equals(BlockType.Grey) && !_bonusLocations.ContainsKey(index))
                {
                    int number = _random.Next()%10;
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
                    _bonusLocations.Add(index, bonusType);
                    counter++;
                }
            }
        }

        /// <summary>
        /// Wylosuj pola, na których powinny znaleźć się postacie przeciwników i gracz.
        /// Ilość przeciwników zależy od poziomu i jest równa <value>PercentageOfOpponents</value>
        /// Prawdopodobieństwo wylosowania ośmiornicy wynosi 65% a ducha 35%
        /// </summary>
        /// <param name="columns">ilość kolumn pól jednostkowych na planszy</param>
        private void RandomCharacters(int columns)
        {
            // wylosuj przeciwników
            int maxOpponentAmount = PercentageOfOpponents*_boradBlocksTypes.Count/100 - 1;
            int counter = 0;
            while (counter < maxOpponentAmount)
            {
                int index = _random.Next(_boradBlocksTypes.Count);
                if (_boradBlocksTypes[index].Equals(BlockType.White) && !_characterLocations.ContainsKey(index))
                {
                    int number = _random.Next()%100;
                    var characterType = number < 65 ? CharacterType.Octopus : CharacterType.Ghost;
                    _characterLocations.Add(index, new List<CharacterType>()
                    {
                        characterType
                    });
                    counter++;
                }
            }
            while (true)
            {
                int index = _random.Next(_boradBlocksTypes.Count);
                if (_boradBlocksTypes[index].Equals(BlockType.White)
                    && !_characterLocations.ContainsKey(index)
                    && !_characterLocations.ContainsKey(index - 1)
                    && !_characterLocations.ContainsKey(index + 1)
                    && !_characterLocations.ContainsKey(index + columns)
                    && ((index >= columns && !_characterLocations.ContainsKey(index - columns)) || index < columns)
                    && _boradBlocksTypes[index].Equals(BlockType.White))
                {
                    _characterLocations.Add(index, new List<CharacterType>()
                    {
                        CharacterType.Player
                    });
                    break;
                }
            }
        }

        #endregion
    }
}
