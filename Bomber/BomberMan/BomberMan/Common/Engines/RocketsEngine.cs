using BomberMan.Common.Components;
using BomberMan.Common.Components.MovingComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Engines
{
    public class RocketsEngine : Engine
    {
        private const int SHIFT = 50;
        public float MaxHeight { get; set; }
        public float MaxWidth { get; set; }

        public RocketsEngine(List<Texture2D> textures, int objectsCount) : base(textures, objectsCount)
        {
            for (int i = 0; i < ObjectsAmount; i++)
            {
                Components.Add(GenerateNewRocket());
            }
        }

        public void Update()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                ((MovingComponent)Components[i]).Update();
                if (
                    ((MovingComponent)Components[i]).Position.X> MaxWidth + SHIFT||
                    ((MovingComponent)Components[i]).Position.X < 0 - SHIFT ||
                    ((MovingComponent)Components[i]).Position.Y < 0 - SHIFT ||
                    ((MovingComponent)Components[i]).Position.Y > MaxHeight + SHIFT)
                {
                    Components.RemoveAt(i);
                    i--;
                }
            }
            for(int i = ObjectsAmount - Components.Count; i > 0; i--)
            {
                Components.Add(GenerateNewRocket());
            }
        }

        private Rocket GenerateNewRocket()
        {
            Texture2D texture = Textures[Random.Next(Textures.Count)];
            float positionY, positionX;
            bool vertical = Random.Next(1) > 0;
            if(vertical)
            {
                positionY = Random.Next((int)MaxHeight);
                bool left = Random.Next(1) > 0;
                if (left)
                    positionX = (float)0;
                else
                    positionX = MaxWidth;
            }
            else
            {
                positionX = Random.Next((int)MaxWidth);
                bool up = Random.Next(1) > 0;
                if (up)
                    positionY = (float)0;
                else
                    positionY = MaxHeight;
            }
            Vector2 position = new Vector2(positionX, positionY);
            float a = 1f * (float)(Random.NextDouble() * 2 - 1);
            float b = 1f * (float)(Random.NextDouble() * 2 - 1);
            Vector2 velocity = new Vector2(a,b);
            float angle = MathHelper.Pi - (float)Math.Atan2(a, b);  
            Color color = new Color(
                        (float)Random.NextDouble(),
                        (float)Random.NextDouble(),
                        (float)Random.NextDouble());
            float scale = (float)Random.NextDouble() + (float)0.05;
            return new Rocket(texture, position, velocity, angle, color, new Vector2(scale, scale));
        }
    }
}
