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
using BomberMan.Common.Engines.StateEngines;
using BomberManViewModel.DataAccessObjects;
using BomberMan.Common.Engines.DynamicEngines;

  
namespace BomberMan
{

    public class GameManager : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StarsEngine starsEngine;
        RocketsEngine rocketeEngine;
        PlanetEngine planetEngine;
        Texture2D background;
        ScreenType screen = ScreenType.Game;
        MainMenu mainMenu;
        GameScreen game;
        Song song;
        SpriteFont font;
        TextInput textInput = new TextInput();
        Random random;
        bool showCursorForInput;

        public GameManager()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            DataManager.InitContext();
            mainMenu = new MainMenu();
            game = new GameScreen(new GameDAO() { ID = 1 });
        }
 
        protected override void Initialize()
        {
            showCursorForInput = true;
            base.Initialize();
        }
 
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            switch (screen)
            {
                case ScreenType.MainMenu:
                    mainMenu.Update(gameTime, Window.ClientBounds.Width, Window.ClientBounds.Height);
                    textInput.ProcessKeyboard(System.Windows.Forms.Control.IsKeyLocked(System.Windows.Forms.Keys.CapsLock));
                    break;
                case ScreenType.Help:
                    break;
                case ScreenType.HighScores:
                    break;
                case ScreenType.Game:
                    game.Update(gameTime, Window.ClientBounds.Width, Window.ClientBounds.Height);
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
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
            spriteBatch.End();
            planetEngine.Draw(spriteBatch);
            rocketeEngine.Draw(spriteBatch);
            switch (screen)
            {
                case ScreenType.MainMenu:
                    IsMouseVisible = true;
                    showCursorForInput = ((gameTime.TotalGameTime.TotalSeconds * 2) % 2 < 1);
                    mainMenu.Draw(spriteBatch);
                    textInput.Draw(spriteBatch, font, showCursorForInput);
                    break;
                case ScreenType.Help:
                    break;
                case ScreenType.HighScores:
                    break;
                case ScreenType.Game:
                    game.Draw(spriteBatch);
                    break;
                case ScreenType.LoadGame:
                    break;
                case ScreenType.Login:
                    break;
            }
            starsEngine.Draw(spriteBatch);
            base.Draw(gameTime);
        }

        private void LoadMainMenu()
        {
            //mainMenu.Title.Texture = Content.Load<Texture2D>(@"Images/AllMenus/BomberMan");
            mainMenu.optionButtons[0].Texture =
                Content.Load<Texture2D>(@"Images/MainMenu/NewGame");
            mainMenu.optionButtons[1].Texture =
                Content.Load<Texture2D>(@"Images/MainMenu/LoadGame");
            mainMenu.optionButtons[2].Texture =
                Content.Load<Texture2D>(@"Images/MainMenu/HighScores");
            mainMenu.optionButtons[3].Texture =
                Content.Load<Texture2D>(@"Images/MainMenu/settings");
            mainMenu.optionButtons[4].Texture =
                Content.Load<Texture2D>(@"Images/MainMenu/LogOut");
        }

        private void LoadGame()
        {
            game.BlockTextures = new List<Texture2D>();
            Texture2D[] blocks = new Texture2D[4];
            blocks[(int)BlockKind.Black] =  Content.Load<Texture2D>(@"Images/Game/SolidBlock");
            blocks[(int)BlockKind.White] = Content.Load<Texture2D>(@"Images/Game/Block");
            blocks[(int)BlockKind.Grey] = Content.Load<Texture2D>(@"Images/Game/DeletableBlock");
            blocks[(int)BlockKind.Red] =  Content.Load<Texture2D>(@"Images/Game/BombBlock");
            for (int i = 0; i < blocks.Length; i++ )
            {
                game.BlockTextures.Add(blocks[i]);
            }
            game.BombTexture = Content.Load<Texture2D>(@"Images/Game/Bomb");
            game.boardEngine = new BoardEngine(game.BlockTextures, 12, 16);

        }

        private void LoadBackGround()
        {
            random = new Random();
            List<Texture2D> textures = new List<Texture2D>();
            background = Content.Load<Texture2D>(@"Images/AllMenus/stars");
            textures.Add(Content.Load<Texture2D>(@"Images/AllMenus/heart"));
            textures.Add(Content.Load<Texture2D>(@"Images/AllMenus/star"));
            textures.Add(Content.Load<Texture2D>(@"Images/AllMenus/circle"));
            starsEngine = new StarsEngine(textures, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), random.Next(5) + 5);
            List<Texture2D> rocket = new List<Texture2D>();
            rocket.Add(Content.Load<Texture2D>(@"Images/AllMenus/Shuttle"));
            rocketeEngine = new RocketsEngine(rocket, random.Next(5) + 3);
            List<Texture2D> planets = new List<Texture2D>();
            planets.Add(Content.Load<Texture2D>(@"Images/AllMenus/earth"));
            planets.Add(Content.Load<Texture2D>(@"Images/AllMenus/saturn"));
            planets.Add(Content.Load<Texture2D>(@"Images/AllMenus/dyingstar"));
            planets.Add(Content.Load<Texture2D>(@"Images/AllMenus/redstar"));
            planetEngine = new PlanetEngine(planets, random.Next(4) + 4);
            song = (Content.Load<Song>(@"Music/OneRepublic"));
            MediaPlayer.Play(song);
            font = Content.Load<SpriteFont>(@"Fonts/Input");
        }

        private void UpdateBackground(int windowWidth, int windowHeight)
        {
            planetEngine.Update(windowWidth, windowHeight);
            rocketeEngine.MaxWidth = Window.ClientBounds.Width;
            rocketeEngine.MaxHeight = Window.ClientBounds.Height;
            rocketeEngine.Update();
            starsEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            starsEngine.Update();
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
