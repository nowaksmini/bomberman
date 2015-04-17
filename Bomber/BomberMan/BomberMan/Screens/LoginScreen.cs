using BomberMan.Common.Components.StateComponents;
using BomberManViewModel.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Screens
{
    public class LoginScreen : Screen
    {
        public List<Label> Lablels;
        public List<TextInput> Fields;


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

        public UserDAO CreateUser() { return null; }
        public UserDAO LoginUser() { return null; }
    }
}
