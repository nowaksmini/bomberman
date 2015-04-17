using BomberManViewModel.DataAccessObjects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Screens.Menus
{
    public class LoadGameScreen : Menu
    {
        public List<GameDAO> Gamse;

        public LoadGameScreen(List<Texture2D> textures, int options) : base(options, textures)
        {

        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, int windowWidth, int windowHeight)
        {
            throw new NotImplementedException();
        }

        public override void HandleKeyboard()
        {
            throw new NotImplementedException();
        }
    }
}
