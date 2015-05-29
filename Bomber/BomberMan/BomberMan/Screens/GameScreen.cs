using System;
using System.Collections.Generic;
using BomberMan.Common;
using BomberMan.Common.Components.StateComponents;
using BomberMan.Common.Engines;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private const string Level = "Poziom";
        private const string Points = "Punkty";
        private int _rows;
        private int _columns;

        private readonly Random _random;
        private List<OpponentLocationDao> _opponents;
        private List<BoardElementDao> _boardElements;
        private BoardEngine _boardEngine;

        /// <summary>
        /// Lista zawierająca wszystkie jednostkowe pola planszy ze wzgędu na typ pola
        /// Przekazywana do BoardEngine w celu narysowania jednostkowych pól
        /// </summary>
        private List<BlockType> _boradBlocksTypes;

        private readonly Dictionary<int, BonusType> _bonusLocations;
        private Dictionary<int, List<CharacterType>> _characterLocations;
        private readonly List<int> _bombLocations;
        private List<ProgressBar> _bonuses;
        private List<ProgressBar> _hearts;
        private readonly Label _levelLabel;

        /// <summary>
        /// Przechowywane textury ładowane podczas włączania gry
        /// Jedna textura na jeden obrazek
        /// </summary>
        private readonly List<Texture2D> _blockTextures;

        private readonly Texture2D _bombTexture;
        private readonly List<Texture2D> _characterTextures;
        private readonly List<Texture2D> _bonusesTextures;
        private Button _backButton;

        private float _countDuration = 0.1f; //every  0.1s.
        private float _currentTime;
        private float _opponentMoveCycyle = 0.3f; // every 0.5s
        private float _currentOpponentTime;


        /// <summary>
        /// Utwórz widok gry ze wszystkimi polami jednostkowymi
        /// Jeżeli nie ma utworzonego GameDAO w Utils to wygeneruj nową grę z poziomem 0
        /// W przeciwnym przypadku załąduj grę z Utils i utwórz widok całej planszy
        /// </summary>
        public GameScreen(List<Texture2D> blockTextures, List<Texture2D> bonusesTextures,
            Texture2D bombTexture, List<Texture2D> characterTextures, Texture2D backButtonTexture,
            SpriteFont titleFont)
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
            CreateBackButton(backButtonTexture);
            if (Utils.Game == null)
            {
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
            _levelLabel = new Label(titleFont, Level + " " + (Utils.Game.Level + 1) + " " + Points + Utils.Game.Points
                , Color.White);
        }

        /// <summary>
        /// Utwórz przycisk powrotu do głównego menu.
        /// </summary>
        /// <param name="backButtonTexture">tło przycisku powrotu</param>
        private void CreateBackButton(Texture2D backButtonTexture)
        {
            _backButton = new Button(backButtonTexture, Color.White, null, "", Color.White)
            {
                Click = delegate()
                {
                    GameManager.ScreenType = ScreenType.MainMenu;
                    return Color.Transparent;
                }
            };
            _backButton.Scale = new Vector2(GameManager.BackButtonSize/_backButton.Texture.Width,
                GameManager.BackButtonSize/_backButton.Texture.Height);
            _backButton.Position = new Vector2(GameManager.BackButtonSize/2, GameManager.BackButtonSize/2);
        }

        /// <summary>
        /// Narysuj wsyztskie komponenty w oknie gry.
        /// </summary>
        /// <param name="spriteBatch">Obiekt, w którym rysowane są komponenty.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            _boardEngine.Draw(spriteBatch);
            spriteBatch.Begin();
            _backButton.Draw(spriteBatch);
            _levelLabel.Draw(spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// Uaktualnij widok planszy w zależności od rozmiaru okna gry
        /// </summary>
        /// <param name="gameTime">Czas gry</param>
        /// <param name="windowWidth">Szerokość okna</param>
        /// <param name="windowHeight">Wysokość okna</param>
        public override void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            double frameTime = gameTime.ElapsedGameTime.Milliseconds/1000.0;
            MouseState mouseState = Mouse.GetState();
            PrevMousePressed = MousePressed;
            MousePressed = mouseState.LeftButton == ButtonState.Pressed;
            _backButton.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            _currentTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
            _currentOpponentTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (_currentOpponentTime >= _opponentMoveCycyle)
            {
                _currentOpponentTime -= _opponentMoveCycyle;
                Dictionary<int, List<CharacterType>> tmpDictionary = new Dictionary<int, List<CharacterType>>();
                foreach (var record in _characterLocations)
                {
                    foreach (var character in record.Value)
                    {
                        if (character == CharacterType.Octopus)
                        {
                            GenerateOneMoveForOctpus(ref tmpDictionary, record.Key, character);
                        }
                        else if (character == CharacterType.Ghost)
                        {
                            GenerateOneMoveForGhoast(ref tmpDictionary, record.Key, character);
                        }
                        else
                        {
                            if (!tmpDictionary.ContainsKey(record.Key))
                            {
                                tmpDictionary.Add(record.Key, new List<CharacterType>());
                            }
                            tmpDictionary[record.Key].Add(character);
                        }
                    }
                }
                _characterLocations = tmpDictionary;
            }
            _boardEngine.Update(_boradBlocksTypes, _bonusLocations,
                _characterLocations, _bombLocations, windowWidth, windowHeight);
            if (_currentTime >= _countDuration)
            {
                _currentTime -= _countDuration;
                HandleKeyboard();
            }
            _levelLabel.Position = new Vector2(GameManager.BackButtonSize*2, 0);
            _levelLabel.Text = Level + " " + (Utils.Game.Level + 1) + " " + Points + " " + Utils.Game.Points;
        }

        /// <summary>
        /// Szczegółowa analiza ruchu, który powinna wykonać ośmiornica.
        /// </summary>
        /// <param name="tmpDictionary">słownik, do którego dodawana jest ośmiornica zpozycją, na której stoi</param>
        /// <param name="actualPosition">aktualna pozycja ośmiornicy</param>
        /// <param name="character">ośmiornica</param>
        private void GenerateOneMoveForOctpus(ref Dictionary<int, List<CharacterType>> tmpDictionary, int actualPosition,
            CharacterType character)
        {
            //sprwadź czy ośmiornica widzi gracza
            if (CheckIfOctopusSeesPlayer(actualPosition, _boardEngine.PlayerLocation))
            {
                int octopusPosition = actualPosition;
                int playerPosition = _boardEngine.PlayerLocation;
                if (octopusPosition > playerPosition)
                {
                    if ((octopusPosition - playerPosition)%_columns == 0)
                    {
                        //ośmiornica i gracz w tej samej kolumnie
                        if (!tmpDictionary.ContainsKey(octopusPosition - _columns))
                        {
                            tmpDictionary.Add(octopusPosition - _columns, new List<CharacterType>());
                        }
                        tmpDictionary[octopusPosition - _columns].Add(character);
                    }
                    else
                    {
                        // ośmiornica i gracz w tym samym wierszu
                        if (!tmpDictionary.ContainsKey(actualPosition - 1))
                        {
                            tmpDictionary.Add(actualPosition - 1, new List<CharacterType>());
                        }
                        tmpDictionary[actualPosition - 1].Add(character);
                    }
                }
                else
                {
                    if ((playerPosition - octopusPosition)%_columns == 0)
                    {
                        //ośmiornica i gracz w tej samej kolumnie
                        if (!tmpDictionary.ContainsKey(actualPosition + _columns))
                        {
                            tmpDictionary.Add(actualPosition + _columns, new List<CharacterType>());
                        }
                        tmpDictionary[actualPosition + _columns].Add(character);
                    }
                    else
                    {
                        // ośmiornica i gracz w tym samym wierszu
                        if (!tmpDictionary.ContainsKey(actualPosition + 1))
                        {
                            tmpDictionary.Add(actualPosition + 1, new List<CharacterType>());
                        }
                        tmpDictionary[actualPosition + 1].Add(character);
                    }
                }
            }
            else
            {
                //wylosuj kierunek ruchu lub stój w miejscu
                bool[] directions = new bool[4] {true, true, true, true};
                int goodDirections = 4;
                if (actualPosition%_columns == 0)
                {
                    //lewy brzeg planszy
                    goodDirections--;
                    directions[0] = false;
                }

                if (actualPosition%_columns + 1 == 0)
                {
                    //prawy brzeg planszy
                    goodDirections--;
                    directions[1] = false;
                }
                if (actualPosition < _columns)
                {
                    //górny brzeg planszy
                    goodDirections--;
                    directions[2] = false;
                }
                if (actualPosition >= (_rows - 1)*_columns)
                {
                    //dolny brzeg planszy
                    goodDirections--;
                    directions[3] = false;
                }
                int newKey = -1;
                if (goodDirections == 1)
                {
                    for (int i = 0; i < directions.Length; i++)
                    {
                        if (directions[i])
                        {
                            switch (i)
                            {
                                case 0:
                                    //lewo
                                    if (_boradBlocksTypes[actualPosition - 1] == BlockType.White)
                                        newKey = actualPosition - 1;
                                    break;
                                case 1:
                                    // prawo
                                    if (_boradBlocksTypes[actualPosition + 1] == BlockType.White)
                                        newKey = actualPosition + 1;
                                    break;
                                case 2:
                                    // góra
                                    if (_boradBlocksTypes[actualPosition - _columns] == BlockType.White)
                                        newKey = actualPosition - _columns;
                                    break;
                                case 3:
                                    // dół
                                    if (_boradBlocksTypes[actualPosition + _columns] == BlockType.White)
                                        newKey = actualPosition + _columns;
                                    break;
                            }
                        }
                    }
                }
                if (goodDirections > 1)
                {
                    int a = _random.Next(goodDirections - 1);
                    a++;
                    int index = 0;
                    for (int i = 0; i < directions.Length; i++)
                    {
                        if (directions[i])
                        {
                            if (index == a) break;
                            switch (i)
                            {
                                case 0:
                                    //lewo
                                    if (_boradBlocksTypes[actualPosition - 1] == BlockType.White)
                                    {
                                        index++;
                                        newKey = actualPosition - 1;
                                    }
                                    break;
                                case 1:
                                    // prawo
                                    if (_boradBlocksTypes[actualPosition + 1] == BlockType.White)
                                    {
                                        newKey = actualPosition + 1;
                                        index++;
                                    }
                                    break;
                                case 2:
                                    // góra
                                    if (_boradBlocksTypes[actualPosition - _columns] == BlockType.White)
                                    {
                                        newKey = actualPosition - _columns;
                                        index++;
                                    }
                                    break;
                                case 3:
                                    // dół
                                    if (_boradBlocksTypes[actualPosition + _columns] == BlockType.White)
                                    {
                                        newKey = actualPosition + _columns;
                                        index++;
                                    }
                                    break;
                            }
                        }
                    }
                }
                if (goodDirections != 0 && newKey != -1 && _boradBlocksTypes[newKey] == BlockType.White)
                {
                    if (!tmpDictionary.ContainsKey(newKey))
                    {
                        tmpDictionary.Add(newKey, new List<CharacterType>());
                    }
                    tmpDictionary[newKey].Add(character);
                }
                else
                {
                    if (!tmpDictionary.ContainsKey(actualPosition))
                    {
                        tmpDictionary.Add(actualPosition, new List<CharacterType>());
                    }
                    tmpDictionary[actualPosition].Add(character);
                }
            }
        }


        /// <summary>
        /// Szczegółowa analiza ruchu, który powinien wykonać duch.
        /// </summary>
        /// <param name="tmpDictionary"></param>
        /// <param name="actualPosition"></param>
        /// <param name="character"></param>
        private void GenerateOneMoveForGhoast(ref Dictionary<int, List<CharacterType>> tmpDictionary, int actualPosition,
            CharacterType character)
        {
            //sprwadź czy ośmiornica widzi gracza
            if (CheckIfOctopusSeesPlayer(actualPosition, _boardEngine.PlayerLocation))
            {
                int octopusPosition = actualPosition;
                int playerPosition = _boardEngine.PlayerLocation;
                if (octopusPosition > playerPosition)
                {
                    if ((octopusPosition - playerPosition)%_columns == 0)
                    {
                        //ośmiornica i gracz w tej samej kolumnie
                        if (!tmpDictionary.ContainsKey(octopusPosition - _columns))
                        {
                            tmpDictionary.Add(octopusPosition - _columns, new List<CharacterType>());
                        }
                        tmpDictionary[octopusPosition - _columns].Add(character);
                    }
                    else
                    {
                        // ośmiornica i gracz w tym samym wierszu
                        if (!tmpDictionary.ContainsKey(actualPosition - 1))
                        {
                            tmpDictionary.Add(actualPosition - 1, new List<CharacterType>());
                        }
                        tmpDictionary[actualPosition - 1].Add(character);
                    }
                }
                else
                {
                    if ((playerPosition - octopusPosition)%_columns == 0)
                    {
                        //ośmiornica i gracz w tej samej kolumnie
                        if (!tmpDictionary.ContainsKey(actualPosition + _columns))
                        {
                            tmpDictionary.Add(actualPosition + _columns, new List<CharacterType>());
                        }
                        tmpDictionary[actualPosition + _columns].Add(character);
                    }
                    else
                    {
                        // ośmiornica i gracz w tym samym wierszu
                        if (!tmpDictionary.ContainsKey(actualPosition + 1))
                        {
                            tmpDictionary.Add(actualPosition + 1, new List<CharacterType>());
                        }
                        tmpDictionary[actualPosition + 1].Add(character);
                    }
                }
            }
            else
            {
                //wylosuj kierunek ruchu lub stój w miejscu
                bool[] directions = new bool[4] {true, true, true, true};
                int goodDirections = 4;
                if (actualPosition%_columns == 0)
                {
                    //lewy brzeg planszy
                    goodDirections--;
                    directions[0] = false;
                }

                if (actualPosition%_columns + 1 == 0)
                {
                    //prawy brzeg planszy
                    goodDirections--;
                    directions[1] = false;
                }
                if (actualPosition < _columns)
                {
                    //górny brzeg planszy
                    goodDirections--;
                    directions[2] = false;
                }
                if (actualPosition >= (_rows - 1)*_columns)
                {
                    //dolny brzeg planszy
                    goodDirections--;
                    directions[3] = false;
                }
                int newKey = -1;
                if (goodDirections == 1)
                {
                    for (int i = 0; i < directions.Length; i++)
                    {
                        if (directions[i])
                        {
                            switch (i)
                            {
                                case 0:
                                    //lewo
                                    if (_boradBlocksTypes[actualPosition - 1] == BlockType.White)
                                        newKey = actualPosition - 1;
                                    break;
                                case 1:
                                    // prawo
                                    if (_boradBlocksTypes[actualPosition + 1] == BlockType.White)
                                        newKey = actualPosition + 1;
                                    break;
                                case 2:
                                    // góra
                                    if (_boradBlocksTypes[actualPosition - _columns] == BlockType.White)
                                        newKey = actualPosition - _columns;
                                    break;
                                case 3:
                                    // dół
                                    if (_boradBlocksTypes[actualPosition + _columns] == BlockType.White)
                                        newKey = actualPosition + _columns;
                                    break;
                            }
                        }
                    }
                }
                if (goodDirections > 1)
                {
                    int a = _random.Next(goodDirections - 1);
                    a++;
                    int index = 0;
                    for (int i = 0; i < directions.Length; i++)
                    {
                        if (directions[i])
                        {
                            if (index == a) break;
                            switch (i)
                            {
                                case 0:
                                    //lewo
                                    if (_boradBlocksTypes[actualPosition - 1] == BlockType.White)
                                    {
                                        index++;
                                        newKey = actualPosition - 1;
                                    }
                                    break;
                                case 1:
                                    // prawo
                                    if (_boradBlocksTypes[actualPosition + 1] == BlockType.White)
                                    {
                                        newKey = actualPosition + 1;
                                        index++;
                                    }
                                    break;
                                case 2:
                                    // góra
                                    if (_boradBlocksTypes[actualPosition - _columns] == BlockType.White)
                                    {
                                        newKey = actualPosition - _columns;
                                        index++;
                                    }
                                    break;
                                case 3:
                                    // dół
                                    if (_boradBlocksTypes[actualPosition + _columns] == BlockType.White)
                                    {
                                        newKey = actualPosition + _columns;
                                        index++;
                                    }
                                    break;
                            }
                        }
                    }
                }
                if (goodDirections != 0 && newKey != -1 && _boradBlocksTypes[newKey] == BlockType.White)
                {
                    if (!tmpDictionary.ContainsKey(newKey))
                    {
                        tmpDictionary.Add(newKey, new List<CharacterType>());
                    }
                    tmpDictionary[newKey].Add(character);
                }
                else
                {
                    if (!tmpDictionary.ContainsKey(actualPosition))
                    {
                        tmpDictionary.Add(actualPosition, new List<CharacterType>());
                    }
                    tmpDictionary[actualPosition].Add(character);
                }
            }
        }

        /// <summary>
        /// Sprawdż czy ośmiornica widzi gracza w lini prostej bez przeszkód. Nie jest ona samobujcą więc nie wiejdzie
        /// na pole czerwone, które by ją spaliło.
        /// </summary>
        /// <param name="octopusPosition">pozycja ośmiornicy</param>
        /// <param name="playerPosition">pozycja gracza</param>
        /// <returns></returns>
        private bool CheckIfOctopusSeesPlayer(int octopusPosition, int playerPosition)
        {
            int octopusRow = octopusPosition/_columns;
            int octopusColumn = octopusPosition - _columns*octopusRow;
            int playerRow = playerPosition/_columns;
            int playerColumn = playerPosition - _columns*octopusRow;
            if (playerRow != octopusRow && playerColumn != octopusColumn) return false;
            if (playerColumn == octopusColumn)
            {
                int minRow = Math.Min(playerRow, octopusRow);
                int maxRow = Math.Max(playerRow, octopusRow);
                //ośmiornica musi widzieć gracza
                for (int i = minRow + 1; i < maxRow; i++)
                {
                    if (_boradBlocksTypes[i*_columns + playerColumn].Equals(BlockType.Black)
                        || _boradBlocksTypes[i*_columns + playerColumn].Equals(BlockType.Grey))
                        return false;
                }
                // ośmiornica nie chce iść prosto na czerwone pole
                if (octopusRow < playerRow)
                {
                    if (_boradBlocksTypes[(octopusRow + 1)*_columns + playerColumn].Equals(BlockType.Red))
                        return false;
                }
                // ośmiornica nie chce iść prosto na czerwone pole
                if (octopusRow > playerRow)
                {
                    if (_boradBlocksTypes[(octopusRow - 1)*_columns + playerColumn].Equals(BlockType.Red))
                        return false;
                }
            }
            if (playerRow == octopusRow)
            {
                int minColumn = Math.Min(playerColumn, octopusColumn);
                int maxColumn = Math.Max(playerColumn, octopusColumn);
                //ośmiornica musi widzieć gracza
                for (int i = minColumn + 1; i < maxColumn; i++)
                {
                    if (_boradBlocksTypes[playerRow*_columns + i].Equals(BlockType.Black)
                        || _boradBlocksTypes[playerRow*_columns + i].Equals(BlockType.Grey))
                        return false;
                }
                // ośmiornica nie chce iść prosto na czerwone pole
                if (octopusColumn < playerColumn)
                {
                    if (_boradBlocksTypes[(octopusRow)*_columns + octopusColumn + 1].Equals(BlockType.Red))
                        return false;
                }
                // ośmiornica nie chce iść prosto na czerwone pole
                if (octopusRow > playerRow)
                {
                    if (_boradBlocksTypes[(octopusRow)*_columns + octopusColumn - 1].Equals(BlockType.Red))
                        return false;
                }
            }
            return true;
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
                if (Utils.User.KeyboardOption.Equals(KeyboardOption.Arrows))
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
                        case Keys.Back:
                        case Keys.B:
                        case Keys.Escape:
                            _backButton.Click();
                            break;
                    }
                }
                else
                {
                    switch (k)
                    {
                        case Keys.W:
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
                        case Keys.S:
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
                        case Keys.A:
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
                        case Keys.D:
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
                        case Keys.Back:
                        case Keys.B:
                        case Keys.Escape:
                            _backButton.Click();
                            break;
                    }
                }

                // sprawdź czy nie trzeba postawić nowej bomby
                switch (k)
                {
                    case Keys.Space:
                        if (Utils.User.BombKeyboardOption.Equals(BombKeyboardOption.Spcace))
                        {
                            _bombLocations.Add(gamer);
                        }
                        break;
                    case Keys.P:
                        if (Utils.User.BombKeyboardOption.Equals(BombKeyboardOption.P))
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
        private GameDao CreateNewGame()
        {
            GenerateGameForSpecifiedLevel(0);
            GameDao gameDao = new GameDao()
            {
                Level = 0,
                Life = 100,
                Finished = false,
                PlayerXLocation = (uint) 1,
                PlayerYLocation = (uint) 1,
                Points = 0,
                SaveTime = DateTime.Now,
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
            _rows = rows;
            _columns = columns;
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
        /// Sprawdza czy gracz może dojść do każdego pola nie czarnego i na nie wejść.
        /// Wystarczy sprawdzić czy z dowolnego nie czarnego pola można dojść do wszystkich nie czarnych pól.
        /// </summary>
        /// <returns></returns>
        private bool CheckIfBoardIsNiceGenerated(int blackBlockAmount, int colummns)
        {
            int verticles = 0;
            bool[] visited = new bool[_boradBlocksTypes.Count];
            int start = 0;
            while (_boradBlocksTypes[start] == BlockType.Black)
            {
                start ++;
            }
            int max = visited.Length - blackBlockAmount;
            WalkOnBoard(ref visited, ref verticles, start, max, colummns);
            if (verticles == max) return true;
            return false;
        }

        /// <summary>
        /// Pomocnicza funkjca wywoływana rekurencyjnie aby sprawdzić czy można dojść z
        /// dowolnego pola planszy (nie czarnego) do dowolnego pola planszy (nie czarnego)
        /// </summary>
        /// <param name="visited">pola odwiedzone</param>
        /// <param name="verticles">ilość wierchołków, które można odiwedzić</param>
        /// <param name="index">index wierchołka, na którym jesteśmy</param>
        /// <param name="max">oczekiwana ilość odwiedzonych wierzchołków</param>
        /// <param name="columns">ilość kolumn w jednym wierszu planszy</param>
        private void WalkOnBoard(ref bool[] visited, ref int verticles, int index, int max, int columns)
        {
            if (index == visited.Length) return;
            if (_boradBlocksTypes[index] == BlockType.Black) return;
            if (verticles == max) return;
            if (visited[index]) return;
            // zwiększamy ilośc odwiedzonych wierzchołków
            if (!visited[index])
            {
                visited[index] = true;
                verticles++;
            }
            if ((index + 1)%columns != 0) WalkOnBoard(ref visited, ref verticles, index + 1, max, columns);
            if (index%columns != 0) WalkOnBoard(ref visited, ref verticles, index - 1, max, columns);
            if (index - columns >= 0) WalkOnBoard(ref visited, ref verticles, index - columns, max, columns);
            if (index + columns < visited.Length)
                WalkOnBoard(ref visited, ref verticles, index + columns, max, columns);
        }

        /// <summary>
        /// Wylosuj pola, które powinny być niezniszczalne lub zniszczalne, pozostałe ustaw na białe, zwykłe
        /// Pola ustawiane jedynie na wartości <value>GREY</value>, <value>BLACK</value>, <value>WHITE</value>
        /// Do wszystkich pól white/grey da się dojść
        /// </summary>
        /// <param name="rows">ilość wierszy pól jednostkowch na planszy</param>
        /// <param name="columns">ilość kolumn pól jednostkowych na planszy</param>
        private void RandomBlocks(int rows, int columns)
        {
            int percentage = PercentageOfSolidBlocks*rows*columns;
            int blackBlocks = percentage%100 == 0 ? percentage/100 : percentage/100 + 1;
            CreateBoardEngine(rows, columns);
            do
            {
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
            } while (!CheckIfBoardIsNiceGenerated(blackBlocks, columns));
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
