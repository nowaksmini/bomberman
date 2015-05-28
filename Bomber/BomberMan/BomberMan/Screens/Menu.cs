using System.Collections.Generic;
using System.Linq;
using BomberMan.Common;
using BomberMan.Common.Components.StateComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Screens
{
    public abstract class Menu : Screen
    {
        public Button[] OptionButtons { get; set; }
        public int Options { get; set; }

        /// <summary>
        /// Utwóz nową instancję menu.
        /// </summary>
        /// <param name="options">ilość dostępnych opcji</param>
        /// <param name="textures">dostępne tła dla przycisków</param>
        protected Menu(int options, List<Texture2D> textures)
        {
            PrevSelectedOption = -1;
            Options = options;
            OptionButtons = new Button[options];
            for (int i = 0; i < textures.Count; i++)
            {
                OptionButtons[i] = new Button
                {
                    Angle = 0,
                    Color = Color.Transparent,
                    Texture = textures.ElementAt(i)
                };
            }
        }
    }
}
