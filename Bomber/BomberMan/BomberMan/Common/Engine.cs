using BomberMan.Common.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common
{
    public class Engine
    {
        protected Random Random;
        protected List<Texture2D> Textures;
        protected int ObjectsAmount;
        protected List<Component> Components;

        public Engine(List<Texture2D> textures, int objectsCount)
        {
            this.Textures = textures;
            ObjectsAmount = objectsCount;
            Random = new Random();
            Components = new List<Component>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int index = 0; index < Components.Count; index++)
            {
                Components[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
