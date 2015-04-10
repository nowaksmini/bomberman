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
        // TO DO Split engine for moving and state
        protected Random random;
        protected List<Texture2D> textures;
        protected int objectsAmount;
        protected List<Component> components;


        public Engine(List<Texture2D> textures, int objectsCount)
        {
            this.textures = textures;
            objectsAmount = objectsCount;
            random = new Random();
            components = new List<Component>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int index = 0; index < components.Count; index++)
            {
                components[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
