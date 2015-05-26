using System;
using System.Collections.Generic;
using BomberMan.Common.Components.StateComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Screens
{
    /// <summary>
    /// Ekran pojawiający się na starcie aplikacji.
    /// Weryfikuje poprawność danych logowania do aplikacji.
    /// Pozwala założyć konto i zalogować się do aplikacji.
    /// </summary>
    public class LoginScreen : Screen
    {
        public List<Label> Labels;
        public List<TextInput> Fields;


        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            throw new NotImplementedException();
        }

        public override void HandleKeyboard()
        {
            throw new NotImplementedException();
        }

        public void CreateUser() { }
        public void LoginUser() { }
    }
}
