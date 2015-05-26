using System;
using System.Collections.Generic;
using System.Linq;
using BomberMan.Common.Components.StateComponents;
using BomberManModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Engines
{
    /// <summary>
    /// Klasa rysuje wszystkie jednostkowe pola planszy, każde pole może być narysowane z przeciwnikami, 
    /// bonusami, graczem lub bombami.
    /// Przechowywana jest informacja na temat rodzajów blocków.
    /// Tło pojedynczego pola rysowane jest na podstawie sklejenia textur.
    /// </summary>
    public class BoardEngine : Engine
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly List<Texture2D> _bonusTextures;
        private List<Texture2D> _opponentTextures;
        private Texture2D _playerTexture;
        private Texture2D _bombTexture;
        private const int Shift = 90;
        private const float GapX = 0.57f;
        private const float GapY = 0.79f;
        /// <summary>
        /// Współczynnik opisujący jak powinien być pomniejszony bonus aby zmieścić się w polu planszy
        /// </summary>
        private const float BonusPercentage = 0.5f;
        /// <summary>
        /// Współczynnik opisujący jak powinien być w skali szerokości przesunięty bonus aby zmieścić się w polu planszy
        /// </summary>
        private const float GapBonusX = 0.2f;
        /// <summary>
        /// Współczynnik opisujący jak powinien być w skali wysokości przesunięty bonus aby zmieścić się w polu planszy
        /// </summary>
        private const float GapBonusY = 0.2f;


        public BoardEngine(List<Texture2D> textures, List<Texture2D> bonusTextures, List<Texture2D> opponentTextures,
            Texture2D bombTexture, Texture2D playerTexture, int rows, int columns)
            : base(textures, rows * columns)
        {
            _playerTexture = playerTexture;
            _bombTexture = bombTexture;
            _opponentTextures = opponentTextures;
            _bonusTextures = bonusTextures;
            _rows = rows;
            _columns = columns;
        }

        /// <summary>
        /// Zaktualizuj widok każdego kwadratu jednostkowego planszy.
        /// Na polu plnaszy narysowana jest zawsze na samej górze gracz, potem bomba, następnie przeciwnik, pole lub bonus w zależności od typu
        /// </summary>
        /// <param name="blocksTypes">lista opisująca każde pole planszy ze względu na rodzaj tła kawdratu <example>BlockType.Black</example> </param>
        /// <param name="bonusLocations">słownik opisujący rozmieszczenie wszystkich bonusów na planszy, gdzie klucz oznacza miesjce na planszy</param>
        /// <param name="oponenTypes">słownik opisujący rozmieszczenie wszystkich przeciwników na planszy, gdzie klucz oznacza miesjce na planszy</param>
        /// <param name="bombLocations">lista opisująca rozmieszczenie wszystkich bomb na planszy</param>
        /// <param name="playerLocation">wartość lokalizacji gracza</param>
        /// <param name="windowWidth">szerokość okna gry</param>
        /// <param name="windowHeight">wysokość okna gry</param>
        public void Update(List<BlockType> blocksTypes, Dictionary<int, BonusType> bonusLocations, Dictionary<int, OpponentType> oponenTypes,
            List<int> bombLocations, int playerLocation, int windowWidth, int windowHeight)
        {
            int width = (windowWidth + 2 * Shift) / (_columns);
            int height = (windowHeight) / (_rows);
            int x = Shift + width / 2;
            int y = Shift + height / 2;
            int counter = 0;
            // zainicjalizuj całą planszę pierwszy raz
            if (Components.Count == 0)
            {
                for (int i = 0; i < _rows; i++)
                {
                    x = Shift + width / 2;
                    for (int j = 0; j < _columns; j++)
                    {
                        // należy zadbać aby zawsze bonus znajdujący się pod szarym polem był dodany po szarym polu
                        // a bonus znajdujący się na białym/czerwonym został dodany do listy przed utworzeniem Block'a
                        if (bonusLocations.ContainsKey(counter))
                        {
                            if (blocksTypes[counter].Equals(BlockType.Grey))
                            {
                                Components.Add(GenerateNewBonus(bonusLocations[counter], (int)(x - GapBonusX * width), (int)(y - GapBonusY * height), (int)(width * BonusPercentage), (int)(height * BonusPercentage)));
                                Components.Add(GenerateNewBlock(blocksTypes[counter], x, y, width, height));
                            }
                            else
                            {
                                Components.Add(GenerateNewBlock(blocksTypes[counter], x, y, width, height));
                                Components.Add(GenerateNewBonus(bonusLocations[counter], (int)(x - GapBonusX * width), (int)(y - GapBonusY * height), (int)(width * BonusPercentage), (int)(height * BonusPercentage)));
                            }
                        }
                        else
                        {
                            Components.Add(GenerateNewBlock(blocksTypes[counter], x, y, width, height));
                        }
                        x += (int)(GapX * width);
                        counter++;
                    }
                    y += (int)(GapY * height);
                }
            }
        }

        /// <summary>
        /// Utwórz jednostkowy kwadrat planszy na podstawie rozmiaru i wybranego tła.
        /// </summary>
        /// <param name="blockType">rodzaj kwadratu jednostkowego <example>BlockType.White</example></param>
        /// <param name="x">położenie poziome względem okna gry</param>
        /// <param name="y">położenie pionowe względem okna gry</param>
        /// <param name="width">oczekiwana szerokość pola jednostkowego planszy</param>
        /// <param name="height">oczekiwana wysokość pola jednostkowego planszy</param>
        /// <returns>Zwróć nowy jednostkowy kwadrat planszy z ustalonym tłem.</returns>
        private Block GenerateNewBlock(BlockType blockType, int x, int y, int width, int height)
        {
            Texture2D texture = Textures[(int)blockType];
            Vector2 scale = new Vector2(width / (float)texture.Width, height / (float)texture.Height);
            Block block = new Block(texture, Color.White, new Vector2(x, y), scale, 0, blockType);
            return block;
        }

        /// <summary>
        /// Utwórz jednostkowy kwadrat planszy z tłem wybranego bonusa.
        /// </summary>
        /// <param name="bonusType">rodzaj bonusa <example>BonusType.Fast</example></param>
        /// <param name="x">położenie poziome względem okna gry</param>
        /// <param name="y">położenie pionowe względem okna gry</param>
        /// <param name="width">oczekiwana szerokość bonusa</param>
        /// <param name="height">oczekiwana wysokość bonusa</param>
        /// <returns>Zwróć nowy jednostkowy kwadrat planszy z ustalonym tłem bonusa.</returns>
        private Bonus GenerateNewBonus(BonusType bonusType, int x, int y, int width, int height)
        {
            Texture2D texture = _bonusTextures[(int)bonusType];
            Vector2 scale = new Vector2(width / (float)texture.Width, height / (float)texture.Height);
            Bonus bonus = new Bonus(texture, Color.White, new Vector2(x, y), scale, 0, bonusType);
            return bonus;
        }
    }
}
