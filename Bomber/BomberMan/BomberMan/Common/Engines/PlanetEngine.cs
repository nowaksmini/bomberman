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
            if(Components.Count == 0)
            {
                for (int i = 0; i < ObjectsAmount; i++)
                {
                    Components.Add(GenerateNewPlanet(windowWidth, windowHeight));
                }
            }
            else
            {
                for(int i = 0; i< ObjectsAmount; i++)
                {
                    Components[i].Position = new Vector2((float)windowWidth * Components[i].Position.X / (float)prevWindowWidth,
                        (float)windowHeight * Components[i].Position.Y / (float)prevWindowHeight);
                }
            }
            prevWindowWidth = windowWidth;
            prevWindowHeight = windowHeight;
        }

        private Component GenerateNewPlanet(int windowWidth, int windowHeight)
        {
            prevWindowWidth = windowWidth;
            prevWindowHeight = windowHeight;
            Texture2D texture = Textures[Random.Next(Textures.Count)];
            float scale = (float)1/ (float) (Random.Next(4) + 1);
            Vector2 position = new Vector2(Random.Next(windowWidth), Random.Next(windowHeight));
            Color color = new Color(
                        (float)Random.NextDouble(),
                        (float)Random.NextDouble(),
                        (float)Random.NextDouble());
            return new Component(texture, color, position, new Vector2(scale,scale), 0);
        }
    }
}
