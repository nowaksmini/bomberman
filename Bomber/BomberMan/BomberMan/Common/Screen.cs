using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan
{
    public interface Screen
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime, int windowWidth, int windowHeight);
        void HandleKeyboard();
    }
}
