using BomberMan.Common.Engines.DynamicEngines;
using BomberManViewModel.DataAccessObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Screens
{
    public class Game : Screen
    {
        private GameDAO game;
        private BoardEngine boardEngine;

        public Game(GameDAO game, List<Texture2D> opponentsTxtures, List<Texture2D> blockTextures, Texture2D player, Texture2D bomb)
        {
            this.game = game;
            // najpierw generujemy blocki
            // potem bomby + inne lementy znaczące
            // potem generujemy opponentów
            // na koniec gracza
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            // dla każdej bomby która nie ma ustawione <0  w czaie do wybuchu zmniejszyć czas o interval jak zmniejszymy i pojawi się mniej niż zero
            // "usuwamy obiekt" z listy do rysowania

            //gameTime.ElapsedGameTime.Milliseconds;
        }

        public void HandleKeyboard()
        {
            throw new NotImplementedException();
        }
    }
}
