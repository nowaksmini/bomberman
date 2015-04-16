using BomberMan.Common.Components;
using BomberMan.Common.Components.MovingComponents;
using BomberMan.Common.Components.StateComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Engines.DynamicEngines
{
    public class BoardEngine : Engine
    {
        private int rows, columns;
        private const int SHIFT = 50;

        public BoardEngine(List<Texture2D> textures, int rows, int columns) : base(textures, rows * columns)
        {
            this.rows = rows;
            this.columns = columns;
        }

        public void Update(List<BlockKind> blocksType, int windowWidth, int windowHeight)
        {
            int x = SHIFT;
            int y = SHIFT;
            int width = (windowWidth + 2 * SHIFT) / (columns);
            int height = (windowHeight + 2* SHIFT) / (rows);
            int counter = 0;
            if(components.Count == 0)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (counter == blocksType.Count) break;
                        components.Add(GenerateNewBlock(blocksType.ElementAt<BlockKind>(counter), x, y, width, height));
                        x += width;
                        counter++;
                    }
                    y += height;
                }
            }
            else
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (counter == blocksType.Count) break;
                        components.ElementAt<Component>(counter).Scale = new Vector2((float)width / (float)components.ElementAt<Component>(counter).Texture.Width, 
                            (float)height / (float)components.ElementAt<Component>(counter).Texture.Height);
                        x += width ;
                        counter++;
                    }
                    y += height;
                }
            }
            
        }

        private Block GenerateNewBlock(BlockKind blockType, int x, int y, int width, int height)
        {
            Texture2D texture = textures[(int)blockType];
            Vector2 scale = new Vector2((float)width / (float)texture.Width, (float)height / (float)texture.Height);
            Block block = new Block(texture, Color.Transparent, new Vector2(x,y), scale, 0, blockType);
            return block;
        }
    }
}
