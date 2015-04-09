using BomberMan.Common;
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
    public class StarsEngine : Engine
    {
        public Vector2 EmitterLocation { get; set; }

        public StarsEngine(List<Texture2D> textures, Vector2 location) : base(textures)
        {
            EmitterLocation = location;
            objectsAmount = 7;
        }

        public void Update()
        {
            for (int i = 0; i < objectsAmount; i++)
            {
                components.Add(GenerateNewParticle());
            }
            for (int particle = 0; particle < components.Count; particle++)
            {
                ((Star)components[particle]).Update();
                if (((Star)components[particle]).TTL <= 0)
                {
                    components.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Star GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(random.NextDouble() * 2 - 1),
                                    1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            float size = (float)random.NextDouble() / (float)(3.0);
            int ttl = 20 + random.Next(40);
            return new Star(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

    }
}