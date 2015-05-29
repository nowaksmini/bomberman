using System;
using System.CodeDom;
using System.Collections.Generic;
using BomberMan.Common.Components.StateComponents;
using BomberMan.Common.Engines;
using BomberMan.Screens;
using BomberMan.Screens.Menus;
using BomberManViewModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BomberMan
{
    public class GameManager : Game
    {
        private GraphicsDeviceManager _graphics;
        private const int MinWindowWidth = 900;
        private const int MinWindowHeight = 600;
        private SpriteBatch _spriteBatch;
        private StarsEngine _starsEngine;
        private RocketsEngine _rocketeEngine;
        private PlanetEngine _planetEngine;
        private Texture2D _background;
        private Texture2D _back;

        public static ScreenType ScreenType = ScreenType.Login;
        public const float BackButtonSize = 50f;
        private MainMenuScreen _mainMenu;
        private GameScreen _game;
        private HelpMenuScreen _help;
        private HighScoresScreen _highScores;
        private LoginScreen _login;
        private SettingsScreen _settings;

        private Song _song;
        private Random _random;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = MinWindowWidth;
            _graphics.PreferredBackBufferHeight = MinWindowHeight;
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            DataManager.InitContext();
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadLogin();
            LoadBackGround();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
            UpdateBackground(Window.ClientBounds.Width, Window.ClientBounds.Height);
            if (ScreenType != ScreenType.Login && _mainMenu == null)
            {
                LoadRestSreens();
            }
            if (Utils.User != null && Utils.User.IsMusic == false)
            {
                if (MediaPlayer.State != MediaState.Paused)
                {
                    MediaPlayer.Pause();
                }
            }
            else
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(_song);
                }
            }

            switch (ScreenType)
            {
                case ScreenType.MainMenu:
                    _mainMenu.Update(gameTime, Window.ClientBounds.Width, Window.ClientBounds.Height);
                    break;
                case ScreenType.Settings:
                    _settings.Update(gameTime, Window.ClientBounds.Width, Window.ClientBounds.Height);
                    break;
                case ScreenType.Help:
                    break;
                case ScreenType.HighScores:
                    break;
                case ScreenType.Game:
                    _game.Update(gameTime, Window.ClientBounds.Width, Window.ClientBounds.Height);
                    break;
                case ScreenType.LoadGame:
                    break;
                case ScreenType.Login:
                    _login.Update(gameTime, Window.ClientBounds.Width, Window.ClientBounds.Height);
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (ScreenType != ScreenType.Login && _mainMenu == null)
            {
                LoadRestSreens();
            }
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0, 0, Window.ClientBounds.Width,
                Window.ClientBounds.Height), Color.White);
            _spriteBatch.End();
            _planetEngine.Draw(_spriteBatch);
            if (Utils.User == null || Utils.User.IsAnimation)
            {
                _rocketeEngine.Draw(_spriteBatch);
            }
            switch (ScreenType)
            {
                case ScreenType.MainMenu:
                    IsMouseVisible = true;
                    _mainMenu.Draw(_spriteBatch);
                    break;
                case ScreenType.Settings:
                    IsMouseVisible = true;
                    _settings.Draw(_spriteBatch);
                    break;
                case ScreenType.Help:
                    break;
                case ScreenType.HighScores:
                    break;
                case ScreenType.Game:
                    _game.Draw(_spriteBatch);
                    break;
                case ScreenType.LoadGame:
                    break;
                case ScreenType.Login:
                    IsMouseVisible = true;
                    _login.Draw(_spriteBatch);
                    break;
            }
            if (Utils.User == null || Utils.User.IsAnimation)
            {
                _starsEngine.Draw(_spriteBatch);
            }
            base.Draw(gameTime);
        }

        #region LoadResources

        private void LoadRestSreens()
        {
            LoadMainMenu();
            LoadSettings();
            LoadGame();
        }

        private void LoadMainMenu()
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>(@"Images/MainMenu/NewGame"));
            textures.Add(Content.Load<Texture2D>(@"Images/MainMenu/LoadGame"));
            textures.Add(Content.Load<Texture2D>(@"Images/MainMenu/HighScores"));
            textures.Add(Content.Load<Texture2D>(@"Images/MainMenu/settings"));
            textures.Add(Content.Load<Texture2D>(@"Images/MainMenu/LogOut"));
            _mainMenu = new MainMenuScreen(5, textures);
        }

        private void LoadHighScores()
        {
        }

        private void LoadSettings()
        {
            SpriteFont labelSpriteFont = Content.Load<SpriteFont>(@"Fonts/Input");
            SpriteFont additionalFont = Content.Load<SpriteFont>(@"Fonts/AdditionalOptions");
            List<Texture2D> buttonsTextures = new List<Texture2D>()
            {
                Content.Load<Texture2D>(@"Images/Settings/Arrows"),
                Content.Load<Texture2D>(@"Images/Settings/WSAD"),
                Content.Load<Texture2D>(@"Images/Settings/SpaceBomb"),
                Content.Load<Texture2D>(@"Images/Settings/PBomb")
            };
            _settings = new SettingsScreen(labelSpriteFont, additionalFont,
                buttonsTextures, Content.Load<Texture2D>(@"Images/Game/white_block"),
                Content.Load<Texture2D>(@"Images/Settings/SaveChanges"), _back);
        }

        private void LoadHelp()
        {
        }

        private void LoadLogin()
        {
            SpriteFont font = Content.Load<SpriteFont>(@"Fonts/Input");
            Texture2D fieldBackground = Content.Load<Texture2D>(@"Images/Game/white_block");
            Texture2D bombTexture = Content.Load<Texture2D>(@"Images/Game/Bomb");
            _login = new LoginScreen(font, Content.Load<SpriteFont>(@"Fonts/Title"),
                Content.Load<SpriteFont>(@"Fonts/AdditionalOptions"), fieldBackground, bombTexture);
        }

        private void LoadGame()
        {
            List<Texture2D> blockTextures = new List<Texture2D>();
            Texture2D[] blocks = new Texture2D[4];
            blocks[(int) BlockType.Black] = Content.Load<Texture2D>(@"Images/Game/black_block");
            blocks[(int) BlockType.White] = Content.Load<Texture2D>(@"Images/Game/white_block");
            blocks[(int) BlockType.Grey] = Content.Load<Texture2D>(@"Images/Game/grey_block");
            blocks[(int) BlockType.Red] = Content.Load<Texture2D>(@"Images/Game/bomb_block");
            blockTextures.AddRange(blocks);
            List<Texture2D> bonusTextures = new List<Texture2D>();
            Texture2D[] bonuses = new Texture2D[6];
            bonuses[(int) BonusType.Life] = Content.Load<Texture2D>(@"Images/Game/life_bonus");
            bonuses[(int) BonusType.BombAmount] = Content.Load<Texture2D>(@"Images/Game/extra_bomb_bonus");
            bonuses[(int) BonusType.Fast] = Content.Load<Texture2D>(@"Images/Game/fast_bonus");
            bonuses[(int) BonusType.Inmortal] = Content.Load<Texture2D>(@"Images/Game/inmortal_bonus");
            bonuses[(int) BonusType.Slow] = Content.Load<Texture2D>(@"Images/Game/slow_bonus");
            bonuses[(int) BonusType.Strenght] = Content.Load<Texture2D>(@"Images/Game/strength_bonus");
            bonusTextures.AddRange(bonuses);
            List<Texture2D> characterTextures = new List<Texture2D>();
            Texture2D[] characters = new Texture2D[3];
            characters[(int) CharacterType.Octopus] = Content.Load<Texture2D>(@"Images/Game/octopus");
            characters[(int) CharacterType.Ghost] = Content.Load<Texture2D>(@"Images/Game/ghost");
            characters[(int) CharacterType.Player] = Content.Load<Texture2D>(@"Images/Game/robot");
            characterTextures.AddRange(characters);
            SpriteFont titleFont = Content.Load<SpriteFont>(@"Fonts/Title");
            _game = new GameScreen(blockTextures, bonusTextures, Content.Load<Texture2D>(@"Images/Game/bomb"),
                characterTextures, _back, titleFont);
        }

        private void LoadBackGround()
        {
            _random = new Random();
            List<Texture2D> textures = new List<Texture2D>();
            _background = Content.Load<Texture2D>(@"Images/Common/stars");
            textures.Add(Content.Load<Texture2D>(@"Images/Common/heart"));
            textures.Add(Content.Load<Texture2D>(@"Images/Common/star"));
            textures.Add(Content.Load<Texture2D>(@"Images/Common/circle"));
            _starsEngine = new StarsEngine(textures,
                new Vector2((float) Window.ClientBounds.Width/2, (float) Window.ClientBounds.Height/2),
                _random.Next(5) + 5);
            List<Texture2D> rocket = new List<Texture2D>();
            rocket.Add(Content.Load<Texture2D>(@"Images/Common/Shuttle"));
            _rocketeEngine = new RocketsEngine(rocket, _random.Next(5) + 3);
            List<Texture2D> planets = new List<Texture2D>();
            planets.Add(Content.Load<Texture2D>(@"Images/Common/earth"));
            planets.Add(Content.Load<Texture2D>(@"Images/Common/saturn"));
            planets.Add(Content.Load<Texture2D>(@"Images/Common/dyingstar"));
            planets.Add(Content.Load<Texture2D>(@"Images/Common/redstar"));
            _planetEngine = new PlanetEngine(planets, _random.Next(4) + 4);
            _song = (Content.Load<Song>(@"Music/OneRepublic"));
            _back = Content.Load<Texture2D>(@"Images/Common/Back");
            MediaPlayer.Play(_song);
        }

        #endregion

        private void UpdateBackground(int windowWidth, int windowHeight)
        {
            _planetEngine.Update(windowWidth, windowHeight);
            _rocketeEngine.MaxWidth = Window.ClientBounds.Width;
            _rocketeEngine.MaxHeight = Window.ClientBounds.Height;
            _rocketeEngine.Update();
            _starsEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            _starsEngine.Update();
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (Window.ClientBounds.Height < MinWindowHeight)
            {
                _graphics.PreferredBackBufferHeight = MinWindowHeight;
            }
            if (Window.ClientBounds.Width < MinWindowWidth)
            {
                _graphics.PreferredBackBufferWidth = MinWindowWidth;
            }
            _graphics.ApplyChanges();
        }
    }

    public enum ScreenType
    {
        MainMenu,
        Help,
        HighScores,
        Settings,
        Game,
        LoadGame,
        Login
    }
}
