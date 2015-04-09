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
    public class RocketEngine : Engine
    {
        public float MaxHeight { get; set; }
        public float MaxWidth { get; set; }

        public RocketEngine(List<Texture2D> textures) : base(textures)
        {
            objectsAmount = 6;
            for (int i = 0; i < objectsAmount; i++)
            {
                components.Add(GenerateNewRocket());
            }
        }

        public void Update()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Update();
                if (
                    components[i].Position.X> MaxWidth + 30||
                    components[i].Position.X  < 0 - 30||
                    components[i].Position.Y  < 0 -30 ||
                    components[i].Position.Y > MaxHeight + 30)
                {
                    components.RemoveAt(i);
                    i--;
                }
            }
            for(int i = objectsAmount - components.Count; i > 0; i--)
            {
                components.Add(GenerateNewRocket());
            }
        }

        private Rocket GenerateNewRocket()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            int randomX = random.Next(2);
            int randomY = random.Next(2);
            float positionX, positionY;
            switch(randomX)
            {
                case 0:
                    positionX = (float)0;
                    break;
                default:
                    positionX = (float)MaxWidth;
                    break;
            }
            switch (randomY)
            {
                case 0:
                    positionY = (float)0;
                    break;
                default:
                    positionY = (float)MaxHeight;
                    break;
            }
            Vector2 position = new Vector2(positionX, positionY);
            float a = 1f * (float)(random.NextDouble() * 2 - 1);
            float b = 1f * (float)(random.NextDouble() * 2 - 1);
            Vector2 velocity = new Vector2(a,b);
            float angle = MathHelper.Pi - (float)Math.Atan2(a, b);  
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            float size = (float)random.NextDouble() + (float)0.05;
            return new Rocket(texture, position, velocity, angle, color, size);
        }
    }
}
