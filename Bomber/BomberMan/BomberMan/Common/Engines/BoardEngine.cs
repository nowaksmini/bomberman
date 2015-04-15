using BomberMan.Common.Components;
using BomberMan.Common.Components.MovingComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Engines.DynamicEngines
{
    public class BoardEngine : Engine
    {
        private const int SHIFT = 50;

        public BoardEngine(List<Texture2D> textures, int objectsAmount)
            : base(textures, objectsAmount)
        {
           
        }

        public void Update(int windowWidth, int windowHeight)
        {
            
        }

        private StateComponent GenerateNewBlock()
        {
            Texture2D texture = textures[random.Next(textures.Count)];

            return new StateComponent(texture, Color.AliceBlue, texture.Bounds);
        }
    }
}
