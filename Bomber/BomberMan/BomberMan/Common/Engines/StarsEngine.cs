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

        public StarsEngine(List<Texture2D> textures, Vector2 location, int objectsCount) : base(textures, objectsCount)
        {
            EmitterLocation = location;
        }

        public void Update()
        {
            for (int i = 0; i < ObjectsAmount; i++)
            {
                Components.Add(GenerateNewParticle());
            }
            for (int particle = 0; particle < Components.Count; particle++)
            {
                ((Star)Components[particle]).Update();
                if (((Star)Components[particle]).TTL <= 0)
                {
                    Components.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Star GenerateNewParticle()
        {
            Texture2D texture = Textures[Random.Next(Textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(Random.NextDouble() * 2 - 1),
                                    1f * (float)(Random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(Random.NextDouble() * 2 - 1);
            Color color = new Color(
                        (float)Random.NextDouble(),
                        (float)Random.NextDouble(),
                        (float)Random.NextDouble());
            float scale = (float)Random.NextDouble() / (float)(3.0);
            int ttl = 20 + Random.Next(40);
            return new Star(texture, position, velocity, angle, angularVelocity, color, new Vector2(scale), ttl);
        }

    }
}