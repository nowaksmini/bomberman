using System.Collections.Generic;
using BomberMan.Common.Components.StateComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Screens.Menus
{
    public class Settings : Menu
    {
        public List<TextInput> TextInputs;

        public Settings(int options, List<Texture2D> textures) : base(options, textures)
        {

        }

        public override void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            HandleKeyboard();
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
           
        }

        public override void HandleKeyboard()
        {
            
        }

    }
}
