using BomberMan.Common.Components;
using BomberMan.Common.Components.MovingComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Engines.StateEngines
{
    class PlanetEngine: Engine
    {
        private const int SHIFT = 50;
        public float MaxHeight { get; set; }
        public float MaxWidth { get; set; }
        private int prevWindowWidth, prevWindowHeight;

        public PlanetEngine(List<Texture2D> textures, int objectsCount)
            : base(textures, objectsCount)
        {

        }

        public void Update(int windowWidth, int windowHeight)
        {
            if(components.Count == 0)
            {
                for (int i = 0; i < objectsAmount; i++)
                {
                    components.Add(GenerateNewPlanet(windowWidth, windowHeight));
                }
            }
            else
            {
                for(int i = 0; i< objectsAmount; i++)
                {
                    ((StateComponent)components[i]).Rectangle = new Rectangle(((StateComponent)components[i]).Rectangle.Left * windowWidth / prevWindowWidth,
                        ((StateComponent)components[i]).Rectangle.Top * windowHeight / prevWindowHeight,
                        ((StateComponent)components[i]).Rectangle.Width, ((StateComponent)components[i]).Rectangle.Height);
                }
            }
            prevWindowWidth = windowWidth;
            prevWindowHeight = windowHeight;
        }

        private StateComponent GenerateNewPlanet(int windowWidth, int windowHeight)
        {
            prevWindowWidth = windowWidth;
            prevWindowHeight = windowHeight;
            Texture2D texture = textures[random.Next(textures.Count)];
            int scale = random.Next(4) + 1;
            Rectangle rectangle = new Rectangle(random.Next(windowWidth), random.Next(windowHeight),
                texture.Bounds.Width / scale, texture.Bounds.Height / scale);
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            return new StateComponent(texture, color, rectangle);
        }
    }
}
