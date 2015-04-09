using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BomberManViewModel;
using System.Collections.Generic;
using BomberMan.Screens;
using BomberMan.Common.Engines;
using Microsoft.Xna.Framework.Media;
using BomberMan.Common.Components.StateComponents;

  
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
        bool showCursorForInput;

        public GameManager()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            mainMenu = new MainMenu(Window.ClientBounds.Width, Window.ClientBounds.Height);
        }
 
        protected override void Initialize()
        {
            showCursorForInput = true;
            base.Initialize();
        }
 
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice); 
            List<Texture2D> textures = new List<Texture2D>();
            background = Content.Load<Texture2D>(@"Images/AllMenus/stars");
            textures.Add(Content.Load<Texture2D>(@"Images/AllMenus/heart"));
            textures.Add(Content.Load<Texture2D>(@"Images/AllMenus/star"));
            textures.Add(Content.Load<Texture2D>(@"Images/AllMenus/circle"));
            starsEngine = new StarsEngine(textures, new Vector2(400, 240));
            List<Texture2D> rocket = new List<Texture2D>();
            rocket.Add(Content.Load<Texture2D>(@"Images/AllMenus/Shuttle"));
            rocketeEngine = new RocketEngine(rocket);
            song =(Content.Load<Song>(@"Music/OneRepublic"));
            MediaPlayer.Play(song);
            font = Content.Load<SpriteFont>(@"Fonts/Input");
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
            rocketeEngine.MaxWidth = Window.ClientBounds.Width;
            rocketeEngine.MaxHeight = Window.ClientBounds.Height;
            rocketeEngine.Update();
            starsEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            starsEngine.Update();
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
            mainMenu.buttons[0].Texture =
                Content.Load<Texture2D>(@"Images/MainMenu/NewGame");
            mainMenu.buttons[1].Texture =
                Content.Load<Texture2D>(@"Images/MainMenu/LoadGame");
            mainMenu.buttons[2].Texture =
                Content.Load<Texture2D>(@"Images/MainMenu/HighScores");
            mainMenu.buttons[3].Texture =
                Content.Load<Texture2D>(@"Images/MainMenu/settings");
            mainMenu.buttons[4].Texture =
                Content.Load<Texture2D>(@"Images/MainMenu/LogOut");
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
