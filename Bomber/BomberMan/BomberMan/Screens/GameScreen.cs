using BomberMan.Common.Components.StateComponents;
using BomberMan.Common.Engines.DynamicEngines;
using BomberManModel;
using BomberManViewModel.DataAccessObjects;
using BomberManViewModel.DataBaseServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Screens
{
    public class GameScreen : Screen
    {
        private GameDAO game;
        public BoardEngine boardEngine;
        private List<BlockKind> blocksKind;

        public List<Texture2D> OpponentsTxtures { get; set; }
        public List<Texture2D> BlockTextures { get; set; }
        public Texture2D PlayerTexture { get; set; }
        public Texture2D BombTexture { get; set; }

        public GameScreen(GameDAO game = null)
        {
            blocksKind = new List<BlockKind>();
            if(game != null)
            {
                this.game = game;
                List<BoardElementLocationDAO> blocks = BoardService.GetAllBlocksForGame(game);
                for(int i = 0; i< blocks.Count; i++)
                {
                    BlockKind blockKind = BlockKind.White;
                    switch (blocks.ElementAt<BoardElementLocationDAO>(i).BoardElement.ElementType)
                    {
                        case BoardElementType.WhiteBlock:
                            blockKind = BlockKind.White;
                            break;
                        case BoardElementType.RedBlock:
                            blockKind = BlockKind.Red;
                            break;
                        case BoardElementType.GrayBlock:
                            blockKind = BlockKind.Grey;
                            break;
                        case BoardElementType.BlackBlock:
                            blockKind = BlockKind.Black;
                            break;
                    }
                    blocksKind.Add(blockKind);
                }
                List<BoardElementDAO> bonuses = BoardService.GetAllBonusesForGame(game);
                List<BoardElementDAO> bombs = BoardService.GetAllBombsForGame(game);
            }
            // najpierw generujemy blocki
            // potem bomby + inne lementy znaczące
            // potem generujemy opponentów
            // na koniec gracza
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            boardEngine.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime, int windowWidth, int windowHeight)
        {
            // dla każdej bomby która nie ma ustawione <0  w czaie do wybuchu zmniejszyć czas o interval jak zmniejszymy i pojawi się mniej niż zero
            // "usuwamy obiekt" z listy do rysowania

            //gameTime.ElapsedGameTime.Milliseconds;
            boardEngine.Update(blocksKind, windowWidth, windowHeight);
        }

        public void HandleKeyboard()
        {
            //throw new NotImplementedException();
        }
    }
}
