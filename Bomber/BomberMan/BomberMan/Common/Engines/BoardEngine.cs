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
            int width = (windowWidth + columns * SHIFT) / (columns);
            int height = (windowHeight + rows * SHIFT) / (rows);
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
                        ((StateComponent)(components.ElementAt<Component>(counter))).Rectangle = new Rectangle(x, y, width, height);
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
            Rectangle rectangle = new Rectangle(x, y, width, height);
            Block block = new Block(texture, Color.White, new Rectangle(x, y, width, height), blockType);
            return block;
        }
    }
}
