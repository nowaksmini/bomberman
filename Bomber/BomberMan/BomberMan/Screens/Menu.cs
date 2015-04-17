using BomberMan.Common.Components.StateComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Screens
{
    public abstract class Menu : Screen
    {
        public Button[] OptionButtons { get; set; }
        public int Options { get; set; }


        public Menu(int options, List<Texture2D> textures)
        {
            prevSelectedOption = -1;
            Options = options;
            OptionButtons = new Button[options];
            for(int i = 0 ; i < textures.Count; i++)
            {
                OptionButtons[i] = new Button();
                OptionButtons[i].Angle = 0;
                OptionButtons[i].Color = Color.Transparent;
                OptionButtons[i].Texture = textures.ElementAt<Texture2D>(i);
            }

        }
    }
}
