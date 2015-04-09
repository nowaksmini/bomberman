﻿using BomberMan.Common.Components;
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
        protected Random random;
        protected List<Texture2D> textures;
        protected int objectsAmount;
        protected List<MovingComponent> components;


        public Engine(List<Texture2D> textures)
        {
            this.textures = textures;
            random = new Random();
            components = new List<MovingComponent>();
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