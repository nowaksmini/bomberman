using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BomberManViewModel;
using System.Collections.Generic;
using BomberMan.Screens;
using BomberMan.Common.Engines;
using Microsoft.Xna.Framework.Media;
using BomberMan.Common.Components.StateComponents;
using System;
using BomberMan.Screens.Menus;
using BomberManModel;
using BomberManViewModel.DataAccessObjects;


namespace BomberMan
{
    public class GameManager : Game
    {
        private SpriteBatch _spriteBatch;
        private StarsEngine _starsEngine;
        private RocketsEngine _rocketeEngine;
        private PlanetEngine _planetEngine;
        private Texture2D _background;
        public static ScreenType ScreenType = ScreenType.Game;
        private MainMenuScreen _mainMenu;
        private GameScreen _game;
        private HelpMenuScreen _help;
        private HighScoresScreen _highScores;
        private LoginScreen _login;
        private Song _song;
        private SpriteFont _font;
        private readonly TextInput textInput = new TextInput();
        private Random random;
        private bool showCursorForInput;

        public GameManager()
        {
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            DataManager.InitContext();
        }

        protected override void Initialize()
        {
            showCursorForInput = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadBackGround();
            LoadMainMenu();
            LoadGame();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            UpdateBackground(Window.ClientBounds.Width, Window.ClientBounds.Height);
            switch (ScreenType)
            {
                case ScreenType.MainMenu:
                    _mainMenu.Update(gameTime, Window.ClientBounds.Width, Window.ClientBounds.Height);
                    textInput.ProcessKeyboard(
                        System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock));
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
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                Color.White);
            _spriteBatch.End();
            _planetEngine.Draw(_spriteBatch);
            _rocketeEngine.Draw(_spriteBatch);
            switch (ScreenType)
            {
                case ScreenType.MainMenu:
                    IsMouseVisible = true;
                    showCursorForInput = ((gameTime.TotalGameTime.TotalSeconds*2)%2 < 1);
                    _mainMenu.Draw(_spriteBatch);
                    textInput.Draw(_spriteBatch, _font, showCursorForInput);
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
                    break;
            }
            _starsEngine.Draw(_spriteBatch);
            base.Draw(gameTime);
        }

        #region LoadResources
        private void LoadMainMenu()
        {
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>(@"Images/MainMenu/NewGame"));
            textures.Add(Content.Load<Texture2D>(@"Images/MainMenu/LoadGame"));
            textures.Add(Content.Load<Texture2D>(@"Images/MainMenu/HighScores"));
            textures.Add(Content.Load<Texture2D>(@"Images/MainMenu/settings"));
            textures.Add(Content.Load<Texture2D>(@"Images/MainMenu/LogOut"));
            _mainMenu = new MainMenuScreen(5, textures);
            //mainMenu.Title.Texture = Content.Load<Texture2D>(@"Images/AllMenus/BomberMan");
        }

        private void LoadHighScores()
        {
        }

        private void LoadSettings()
        {
        }

        private void LoadHelp()
        {
        }

        private void LoadLogin()
        {
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
            _game = new GameScreen(blockTextures, bonusTextures, Content.Load<Texture2D>(@"Images/Game/bomb"),
                characterTextures);
        }

        private void LoadBackGround()
        {
            random = new Random();
            List<Texture2D> textures = new List<Texture2D>();
            _background = Content.Load<Texture2D>(@"Images/Common/stars");
            textures.Add(Content.Load<Texture2D>(@"Images/Common/heart"));
            textures.Add(Content.Load<Texture2D>(@"Images/Common/star"));
            textures.Add(Content.Load<Texture2D>(@"Images/Common/circle"));
            _starsEngine = new StarsEngine(textures,
                new Vector2((float)Window.ClientBounds.Width/2, (float)Window.ClientBounds.Height/2), random.Next(5) + 5);
            List<Texture2D> rocket = new List<Texture2D>();
            rocket.Add(Content.Load<Texture2D>(@"Images/Common/Shuttle"));
            _rocketeEngine = new RocketsEngine(rocket, random.Next(5) + 3);
            List<Texture2D> planets = new List<Texture2D>();
            planets.Add(Content.Load<Texture2D>(@"Images/Common/earth"));
            planets.Add(Content.Load<Texture2D>(@"Images/Common/saturn"));
            planets.Add(Content.Load<Texture2D>(@"Images/Common/dyingstar"));
            planets.Add(Content.Load<Texture2D>(@"Images/Common/redstar"));
            _planetEngine = new PlanetEngine(planets, random.Next(4) + 4);
            _song = (Content.Load<Song>(@"Music/OneRepublic"));
            MediaPlayer.Play(_song);
            _font = Content.Load<SpriteFont>(@"Fonts/Input");
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
    }

    public enum ScreenType
    {
        MainMenu,
        Help,
        HighScores,
        Game,
        LoadGame,
        Login
    }
}
