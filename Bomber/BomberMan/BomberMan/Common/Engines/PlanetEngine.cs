using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Engines
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
                    components[i].Position = new Vector2((float)windowWidth * components[i].Position.X / (float)prevWindowWidth,
                        (float)windowHeight * components[i].Position.Y / (float)prevWindowHeight);
                }
            }
            prevWindowWidth = windowWidth;
            prevWindowHeight = windowHeight;
        }

        private Component GenerateNewPlanet(int windowWidth, int windowHeight)
        {
            prevWindowWidth = windowWidth;
            prevWindowHeight = windowHeight;
            Texture2D texture = textures[random.Next(textures.Count)];
            float scale = (float)1/ (float) (random.Next(4) + 1);
            Vector2 position = new Vector2(random.Next(windowWidth), random.Next(windowHeight));
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            return new Component(texture, color, position, new Vector2(scale,scale), 0);
        }
    }
}
