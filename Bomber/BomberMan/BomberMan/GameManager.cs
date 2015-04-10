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

  
namespace BomberMan
{

    public class GameManager : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StarsEngine starsEngine;
        RocketEngine rocketeEngine;
        Texture2D background;
        ScreenType screen = ScreenType.MainMenu;
        MainMenu mainMenu;
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
            mainMenu = new MainMenu();
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
        }
 
        protected override void UnloadContent()
        {
            Content.Unload();
        }
 
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            UpdateBackground();
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
            mainMenu.Title.Texture = Content.Load<Texture2D>(@"Images/AllMenus/BomberMan");
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
            rocketeEngine = new RocketEngine(rocket, random.Next(5) + 3);
            song = (Content.Load<Song>(@"Music/OneRepublic"));
            MediaPlayer.Play(song);
            font = Content.Load<SpriteFont>(@"Fonts/Input");
        }

        private void UpdateBackground()
        {
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
