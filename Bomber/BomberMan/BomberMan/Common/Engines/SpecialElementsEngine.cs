using BomberManViewModel.DataAccessObjects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Engines
{
    public class SpecialElementsEngine
    {
        public List<OponentDAO> Oponents;
        public List<BoardElementDAO> BoardElements;
        private int velocity;
        public int MaxVelocity;
        public SpecialElementsEngine(List<Texture2D> oponentsTextures, List<Texture2D> boardElementsTextures)
        {
            ;
        }

        public void Update(List<List<int>> fieldValues, int windowWidth, int windowHeight, int max)
        {

        }

        public void Draw(SpriteBatch spritebatch)
        {

        }
    }
}
