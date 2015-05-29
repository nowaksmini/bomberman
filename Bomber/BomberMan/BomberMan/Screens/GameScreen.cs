using System;
using System.Collections.Generic;
using BomberMan.Common;
using BomberMan.Common.Components.StateComponents;
using BomberMan.Common.Engines;
using BomberManModel;
using BomberManModel.Entities;
using BomberManViewModel.DataAccessObjects;
using BomberManViewModel.Services;
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
        private const int PercentageOfOpponents = 3;
        private const int DeletingGreyField = 10;
        private const int DeletingOctopus = 150;
        private const int DeletingGhoast = 300;
        private const int ButtonResetGap = 10;
        private const string Level = "Poziom";
        private const string Points = "Punkty";
        private int _rows;
        private int _columns;

        private readonly Random _random;
        private List<OpponentLocationDao> _opponents;
        private List<BoardElementDao> _boardElements;
        private BoardEngine _boardEngine;
        private Label _winFailue;


        /// <summary>
        /// Lista zawierająca wszystkie jednostkowe pola planszy ze wzgędu na typ pola
        /// Przekazywana do BoardEngine w celu narysowania jednostkowych pól
        /// </summary>
        private List<BlockType> _boardBlocksTypes;

        private Dictionary<int, BonusType> _bonusLocations;
        private Dictionary<int, List<CharacterType>> _characterLocations;
        private List<int> _bombLocations;
        private List<ProgressBar> _bonuses;
        private List<ProgressBar> _hearts;
        private readonly Label _levelLabel;
        private float _life = 100f;
        private int _bombAmount = 4;

        /// <summary>
        /// Przechowywane textury ładowane podczas włączania gry
        /// Jedna textura na jeden obrazek
        /// </summary>
        private readonly List<Texture2D> _blockTextures;

        private readonly Texture2D _bombTexture;
        private readonly List<Texture2D> _characterTextures;
        private readonly List<Texture2D> _bonusesTextures;
        private Button _backButton;

        private float _countDuration = 0.1f; //every  0.3s.
        private float _currentTime;
        private float _opponentMoveCycyle = 0.6f; // every 0.6s
        private float _currentOpponentTime;
        private List<float> _currentBombTimes;
        private float _bombCycle = 5.0f;
        private bool isMegaStrength;
        private float _redBlockCycle = 1.0f;
        private float _currentRedBlockTime;
        private readonly Button _saveButton;
        private readonly Button _restartGame;
        private readonly float _buttonRestarWidth = 200;
        private readonly float _spectialButtonsHeight = 90;


        /// <summary>
        /// Utwórz widok gry ze wszystkimi polami jednostkowymi
        /// Jeżeli nie ma utworzonego GameDAO w Utils to wygeneruj nową grę z poziomem 0
        /// W przeciwnym przypadku załąduj grę z Utils i utwórz widok całej planszy
        /// </summary>
        public GameScreen(List<Texture2D> blockTextures, List<Texture2D> bonusesTextures,
            Texture2D bombTexture, List<Texture2D> characterTextures, Texture2D backButtonTexture,
            SpriteFont titleFont, Texture2D saveButtonTexture, Texture2D startAgainTexture)
        {
            _winFailue = new Label(titleFont, "", Color.BlueViolet);
            _saveButton = new Button(saveButtonTexture, Color.White, null, "", Color.White)
            {
                Click = delegate()
                {
                    SaveGame();
                    return Color.Transparent;
                }
            };
            _restartGame = new Button(startAgainTexture, Color.White, null, "", Color.White)
            {
                Click = delegate()
                {
                    _winFailue.Text = "";
                    GenerateGameForSpecifiedLevel(0);
                    return Color.Transparent;
                }
            };
            _currentBombTimes = new List<float>();
            _bonusesTextures = bonusesTextures;
            _blockTextures = blockTextures;
            _bombTexture = bombTexture;
            _characterTextures = characterTextures;
            _random = new Random();
            _boardBlocksTypes = new List<BlockType>();
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
                String message = "";
                List<BoardElementLocationDao> blocks = BoardService.GetAllBlocksForGame(Utils.Game, out message);
                for (int i = 0; i < blocks.Count; i++)
                {
                    BlockType blockKind = BlockType.White;
                    switch (blocks[i].BoardElement.ElementType)
                    {
                        case BoardElementType.WhiteBlock:
                            blockKind = BlockType.White;
                            break;
                        case BoardElementType.RedBlock:
                            blockKind = BlockType.Red;
                            break;
                        case BoardElementType.GrayBlock:
                            blockKind = BlockType.Grey;
                            break;
                        case BoardElementType.BlackBlock:
                            blockKind = BlockType.Black;
                            break;
                    }
                    _boardBlocksTypes.Add(blockKind);
                }
                List<BoardElementDao> bonuses = BoardService.GetAllBonusesForGame(Utils.Game, out message);
                List<BoardElementDao> bombs = BoardService.GetAllBombsForGame(Utils.Game, out message);
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
            _restartGame.Draw(spriteBatch);
            _saveButton.Draw(spriteBatch);
            _winFailue.Draw(spriteBatch);
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
            _currentRedBlockTime += (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (_characterLocations.ContainsKey(_boardEngine.PlayerLocation))
            {
                List<CharacterType> characterTypes = _characterLocations[_boardEngine.PlayerLocation];
                foreach (var character in characterTypes)
                {
                    if (character != CharacterType.Player)
                    {
                        GameFinished(false);
                    }
                }
            }
            if (_bonusLocations.ContainsKey(_boardEngine.PlayerLocation) && 
                _boardBlocksTypes[_boardEngine.PlayerLocation] == BlockType.White)
            {
                BonusType bonusType = _bonusLocations[_boardEngine.PlayerLocation];
                switch (bonusType)
                {
                    case BonusType.BombAmount:
                        _bombAmount++;
                        break;
                    case BonusType.Fast:
                        _countDuration -= 0.05f;
                        break;
                    case BonusType.Inmortal:
                        Utils.Game.Points += 120;
                        break;
                    case BonusType.Slow:
                        _countDuration += 0.05f;
                        break;
                    case BonusType.Life:
                        Utils.Game.Points += 50;
                        break;
                }
                _bonusLocations.Remove(_boardEngine.PlayerLocation);
            }
            if (_currentRedBlockTime >= _redBlockCycle)
            {
                _currentRedBlockTime -= _redBlockCycle;
                for (int i = 0; i < _boardBlocksTypes.Count; i++)
                {
                    if (_boardBlocksTypes[i] == BlockType.Red)
                    {
                        if (_characterLocations.ContainsKey(i))
                        {
                            List<CharacterType> characterTypes = _characterLocations[i];
                            foreach (var character in characterTypes)
                            {
                                switch (character)
                                {
                                    case CharacterType.Ghost:
                                        Utils.Game.Points += DeletingGhoast;
                                        break;
                                    case CharacterType.Octopus:
                                        Utils.Game.Points += DeletingOctopus;
                                        break;
                                    case CharacterType.Player:
                                        GameFinished(false);
                                        break;
                                }
                            }
                        }
                        _boardBlocksTypes[i] = BlockType.White;
                    }
                }
            }
            _winFailue.Position = new Vector2((float) windowWidth/2, (float) windowHeight/2);
            List<float> newCurrentTimes = new List<float>();
            for (int i = 0; i < _currentBombTimes.Count; i++)
            {
                _currentBombTimes[i] += (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (_currentBombTimes[i] >= _bombCycle && _bombLocations.Count > 0)
                {
                    ChangeBlocksAndKillOpponents(_bombLocations[i]);
                    _bombLocations.RemoveAt(i);
                    _bombAmount++;
                }
                else
                {
                    newCurrentTimes.Add(_currentBombTimes[i]);
                }
            }
            _currentBombTimes = newCurrentTimes;
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
                            GenerateOneMoveForOpponent(ref tmpDictionary, record.Key, character);
                        }
                        else if (character == CharacterType.Ghost)
                        {
                            GenerateOneMoveForOpponent(ref tmpDictionary, record.Key, character);
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
            _boardEngine.Update(_boardBlocksTypes, _bonusLocations,
                _characterLocations, _bombLocations, windowWidth, windowHeight);
            if (_currentTime >= _countDuration)
            {
                _currentTime -= _countDuration;
                HandleKeyboard();
            }

            _restartGame.Scale = new Vector2(_buttonRestarWidth/_restartGame.Texture.Width,
                _spectialButtonsHeight/_restartGame.Texture.Height);
            _restartGame.Position = new Vector2(windowWidth - _buttonRestarWidth/2,
                _spectialButtonsHeight/2 - ButtonResetGap);
            _saveButton.Scale = new Vector2(_spectialButtonsHeight/_saveButton.Texture.Width,
                _spectialButtonsHeight/_saveButton.Texture.Height);
            _saveButton.Position = new Vector2(_restartGame.Position.X - _spectialButtonsHeight,
                _spectialButtonsHeight/2);
            _levelLabel.Position = new Vector2(GameManager.BackButtonSize*2, 0);
            _saveButton.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            _restartGame.Update(mouseState.X, mouseState.Y, frameTime, MousePressed, PrevMousePressed);
            _levelLabel.Text = Level + " " + (Utils.Game.Level + 1) + " " + Points + " " + Utils.Game.Points;
        }


        private void ChangeBlocksAndKillOpponents(int bombPosition)
        {
            int row = bombPosition/_columns;
            int column = bombPosition - row*_columns;
            int startRowPosition = row*_columns;
            int endRowPosition = startRowPosition + _columns - 1;
            int startColumnPosition = column;
            int endColumnPosition = (_rows - 1)*_columns + column;
            int c;
            if (!isMegaStrength)
            {
                // pojawiają się rozgwiazdy 3X3
                c = 3;
            }
            else
            {
                c = 4;
                //pojawiają się rozgwiazdy 4X4
            }
            int startHorizontal = startRowPosition;
            if (column >= c) startHorizontal = bombPosition - c;
            int endHorizontal = bombPosition + c;
            if (column + c >= _columns) endHorizontal = endRowPosition;
            for (int i = startHorizontal; i <= endHorizontal; i++)
            {
                if (_boardBlocksTypes[i] != BlockType.Black)
                {
                    if (_boardBlocksTypes[i] == BlockType.Grey)
                        Utils.Game.Points += DeletingGreyField;
                    _boardBlocksTypes[i] = BlockType.Red;
                    DeleteCharacters(i);
                }
            }
            int startVertical = startColumnPosition;
            if (row >= c) startVertical = bombPosition - c*_columns;
            int endVertical = endColumnPosition;
            if (row + c < _rows) endVertical = row + c*_columns;
            for (int i = startVertical; i <= endVertical; i += _columns)
            {
                if (_boardBlocksTypes[i] != BlockType.Black)
                {
                    if (_boardBlocksTypes[i] == BlockType.Grey)
                        Utils.Game.Points += DeletingGreyField;
                    _boardBlocksTypes[i] = BlockType.Red;
                    DeleteCharacters(i);
                }
            }
        }

        /// <summary>
        /// Zniszcz znajdujących się na polach czerownych przeciwników.
        /// </summary>
        /// <param name="position"></param>
        private void DeleteCharacters(int position)
        {
            if (_characterLocations.ContainsKey(position))
            {
                List<CharacterType> characters = _characterLocations[position];
                foreach (var character in characters)
                {
                    switch (character)
                    {
                        case CharacterType.Ghost:
                            Utils.Game.Points += DeletingGhoast;
                            break;
                        case CharacterType.Octopus:
                            Utils.Game.Points += DeletingOctopus;
                            break;
                        case CharacterType.Player:
                            GameFinished(false);
                            break;
                    }
                }
                characters.Clear();
                _characterLocations.Remove(position);
            }
        }

        private void GameFinished(bool win)
        {
            if (!win)
            {
                _winFailue.Text = "Spróbuj jeszcze raz";
                _restartGame.Click();
            }
            else
            {
                if (Utils.Game.Level == MaxNumberOfLevel)
                {
                    _winFailue.Text = "Bravo";
                }
                GenerateGameForSpecifiedLevel(++Utils.Game.Level);
            }
        }

        #region algorithms

        /// <summary>
        /// Szczegółowa analiza ruchu, który powinien wykonać przeciwnik.
        /// </summary>
        /// <param name="tmpDictionary">pomocniczy słownik przechowywujący lokalizaje przeciwników</param>
        /// <param name="actualPosition">aktualna pozycja przeciwnika</param>
        /// <param name="character">typ przeciwnika</param>
        private void GenerateOneMoveForOpponent(ref Dictionary<int, List<CharacterType>> tmpDictionary,
            int actualPosition,
            CharacterType character)
        {
            //sprwadź czy ośmiornica widzi gracza
            if (CheckIfOpponentSeesPlayer(actualPosition, _boardEngine.PlayerLocation))
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
                                    if (_boardBlocksTypes[actualPosition - 1] == BlockType.White)
                                        newKey = actualPosition - 1;
                                    break;
                                case 1:
                                    // prawo
                                    if (_boardBlocksTypes[actualPosition + 1] == BlockType.White)
                                        newKey = actualPosition + 1;
                                    break;
                                case 2:
                                    // góra
                                    if (_boardBlocksTypes[actualPosition - _columns] == BlockType.White)
                                        newKey = actualPosition - _columns;
                                    break;
                                case 3:
                                    // dół
                                    if (_boardBlocksTypes[actualPosition + _columns] == BlockType.White)
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
                                    if (actualPosition - 1 > 0 && actualPosition - 1 < _boardBlocksTypes.Count &&
                                        _boardBlocksTypes[actualPosition - 1] == BlockType.White)
                                    {
                                        index++;
                                        newKey = actualPosition - 1;
                                    }
                                    break;
                                case 1:
                                    // prawo
                                    if (actualPosition + 1 < _boardBlocksTypes.Count &&
                                        _boardBlocksTypes[actualPosition + 1] == BlockType.White)
                                    {
                                        newKey = actualPosition + 1;
                                        index++;
                                    }
                                    break;
                                case 2:
                                    // góra
                                    if (actualPosition - _columns > 0 &&
                                        actualPosition - _columns < _boardBlocksTypes.Count &&
                                        _boardBlocksTypes[actualPosition - _columns] == BlockType.White)
                                    {
                                        newKey = actualPosition - _columns;
                                        index++;
                                    }
                                    break;
                                case 3:
                                    // dół
                                    if (actualPosition + _columns < _boardBlocksTypes.Count &&
                                        _boardBlocksTypes[actualPosition + _columns] == BlockType.White)
                                    {
                                        newKey = actualPosition + _columns;
                                        index++;
                                    }
                                    break;
                            }
                        }
                    }
                }
                if (goodDirections != 0 && newKey != -1 && _boardBlocksTypes[newKey] == BlockType.White)
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
        /// Sprawdż czy przeciwnik widzi gracza w lini prostej bez przeszkód. Nie jest ona samobujcą więc nie wejdzie
        /// na pole czerwone, które by go spaliło.
        /// </summary>
        /// <param name="octopusPosition">pozycja przeciwnika</param>
        /// <param name="playerPosition">pozycja gracza</param>
        /// <returns></returns>
        private bool CheckIfOpponentSeesPlayer(int octopusPosition, int playerPosition)
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
                    if (_boardBlocksTypes[i*_columns + playerColumn].Equals(BlockType.Black)
                        || _boardBlocksTypes[i*_columns + playerColumn].Equals(BlockType.Grey))
                        return false;
                }
                // ośmiornica nie chce iść prosto na czerwone pole
                if (octopusRow < playerRow)
                {
                    if (_boardBlocksTypes[(octopusRow + 1)*_columns + playerColumn].Equals(BlockType.Red))
                        return false;
                }
                // ośmiornica nie chce iść prosto na czerwone pole
                if (octopusRow > playerRow)
                {
                    if (_boardBlocksTypes[(octopusRow - 1)*_columns + playerColumn].Equals(BlockType.Red))
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
                    if (_boardBlocksTypes[playerRow*_columns + i].Equals(BlockType.Black)
                        || _boardBlocksTypes[playerRow*_columns + i].Equals(BlockType.Grey))
                        return false;
                }
                // ośmiornica nie chce iść prosto na czerwone pole
                if (octopusColumn < playerColumn)
                {
                    if (_boardBlocksTypes[(octopusRow)*_columns + octopusColumn + 1].Equals(BlockType.Red))
                        return false;
                }
                // ośmiornica nie chce iść prosto na czerwone pole
                if (octopusRow > playerRow)
                {
                    if (_boardBlocksTypes[(octopusRow)*_columns + octopusColumn - 1].Equals(BlockType.Red))
                        return false;
                }
            }
            return true;
        }

        #endregion

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
                                if (_boardBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boardBlocksTypes[tmp].Equals(BlockType.Red))
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
                                if (_boardBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boardBlocksTypes[tmp].Equals(BlockType.Red))
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
                                if (_boardBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boardBlocksTypes[tmp].Equals(BlockType.Red))
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
                                if (_boardBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boardBlocksTypes[tmp].Equals(BlockType.Red))
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
                                if (_boardBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boardBlocksTypes[tmp].Equals(BlockType.Red))
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
                                if (_boardBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boardBlocksTypes[tmp].Equals(BlockType.Red))
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
                                if (_boardBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boardBlocksTypes[tmp].Equals(BlockType.Red))
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
                                if (_boardBlocksTypes[tmp].Equals(BlockType.White) ||
                                    _boardBlocksTypes[tmp].Equals(BlockType.Red))
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
                        if (Utils.User.BombKeyboardOption.Equals(BombKeyboardOption.Spcace) && _bombAmount > 0)
                        {
                            _bombLocations.Add(gamer);
                            _bombAmount--;
                            _currentBombTimes.Add(0);
                        }
                        break;
                    case Keys.P:
                        if (Utils.User.BombKeyboardOption.Equals(BombKeyboardOption.P) && _bombAmount > 0)
                        {
                            _bombLocations.Add(gamer);
                            _bombAmount--;
                            _currentBombTimes.Add(0);
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
            String message;
            if (!UserService.CreateUser(Utils.User, out message))
            {
                UserService.UpdateUser(Utils.User, out message);
            }
            GameService.UpdateGame(Utils.Game, out message);
            //OpponentService.UpdateOpponentLocations()
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
            _boardBlocksTypes = new List<BlockType>();
            _bombLocations = new List<int>();
            _bombAmount = 4;
            _bonusLocations = new Dictionary<int, BonusType>();
            _characterLocations = new Dictionary<int, List<CharacterType>>();
            if (level == 0 && Utils.Game != null) Utils.Game.Points = 0;
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
            bool[] visited = new bool[_boardBlocksTypes.Count];
            int start = 0;
            while (_boardBlocksTypes[start] == BlockType.Black)
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
            if (_boardBlocksTypes[index] == BlockType.Black) return;
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
                _boardBlocksTypes = new List<BlockType>();
                // zapełnij całą listę szarymi zniszczalnymi blokami
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        _boardBlocksTypes.Add(BlockType.White);
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
                    } while (_boardBlocksTypes[x*columns + y].Equals(BlockType.Black));
                    _boardBlocksTypes[x*columns + y] = BlockType.Black;
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
                    } while (_boardBlocksTypes[x*columns + y].Equals(BlockType.Grey) ||
                             _boardBlocksTypes[x*columns + y].Equals(BlockType.Black));
                    _boardBlocksTypes[x*columns + y] = BlockType.Grey;
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
            int maxBonusesAmount = PercentageOfBonuses*_boardBlocksTypes.Count/100 - 1;
            int counter = 0;
            while (counter < maxBonusesAmount)
            {
                int index = _random.Next(_boardBlocksTypes.Count);
                if (_boardBlocksTypes[index].Equals(BlockType.Grey) && !_bonusLocations.ContainsKey(index))
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
            int maxOpponentAmount = PercentageOfOpponents*_boardBlocksTypes.Count/100 - 1;
            int counter = 0;
            while (counter < maxOpponentAmount)
            {
                int index = _random.Next(_boardBlocksTypes.Count);
                if (_boardBlocksTypes[index].Equals(BlockType.White) && !_characterLocations.ContainsKey(index))
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
                int index = _random.Next(_boardBlocksTypes.Count);
                if (_boardBlocksTypes[index].Equals(BlockType.White)
                    && !_characterLocations.ContainsKey(index)
                    && !_characterLocations.ContainsKey(index - 1)
                    && !_characterLocations.ContainsKey(index + 1)
                    && !_characterLocations.ContainsKey(index + columns)
                    && ((index >= columns && !_characterLocations.ContainsKey(index - columns)) || index < columns)
                    && _boardBlocksTypes[index].Equals(BlockType.White))
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
