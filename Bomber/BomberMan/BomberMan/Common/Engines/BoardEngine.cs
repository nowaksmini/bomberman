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
        private readonly List<Texture2D> _characterTextures;
        private readonly Texture2D _bombTexture;
        private const int Shift = 60;
        /// <summary>
        /// Zwraca lub ustawia lokalizację gracza na planszy.
        /// </summary>
        /// <value>
        /// lokalizacja gracza.
        /// </value>
        public int PlayerLocation { get; set; }

        /// <summary>
        /// Zwaraca ilość wierszy na planszy.
        /// </summary>
        /// <value>
        /// Wiersze.
        /// </value>
        public int Rows
        {
            get { return _rows; }
        }

        /// <summary>
        ///Zwraca ilość kolummna na planszy.
        /// </summary>
        /// <value>
        /// Ilość kolumn
        /// </value>
        public int Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Utwóz nowy silnik odpowiedzialny za generowanie planszy do gry dla każdego poziomu.
        /// </summary>
        /// <param name="textures">dostępne tła dla pól jednostkowych</param>
        /// <param name="bonusTextures">dostępne tła dla bonusów</param>
        /// <param name="characterTextures">dostępne tła dla postaci</param>
        /// <param name="bombTexture">tło bomb zostawianych przez gracza</param>
        /// <param name="rows">ilość wierszy w planszy</param>
        /// <param name="columns">ilość kolumn w planszy</param>
        public BoardEngine(List<Texture2D> textures, List<Texture2D> bonusTextures, List<Texture2D> characterTextures,
            Texture2D bombTexture, int rows, int columns)
            : base(textures, rows*columns)
        {
            _bombTexture = bombTexture;
            _characterTextures = characterTextures;
            _bonusTextures = bonusTextures;
            _rows = rows;
            _columns = columns;
        }

        /// <summary>
        /// Zaktualizuj widok każdego kwadratu jednostkowego planszy.
        /// Na polu plnaszy narysowana jest zawsze na samej górze gracz, potem bomba, następnie przeciwnik, pole lub bonus w zależności od typu
        /// </summary>
        /// <param name="blocksTypes">lista opisująca każde pole planszy ze względu na rodzaj tła kawdratu <example>characterType.Black</example> </param>
        /// <param name="bonusLocations">słownik opisujący rozmieszczenie wszystkich bonusów na planszy, gdzie klucz oznacza miesjce na planszy</param>
        /// <param name="characterLocations">słownik opisujący rozmieszczenie wszystkich postaci na planszy, gdzie klucz oznacza miesjce na planszy</param>
        /// <param name="bombLocations">lista opisująca rozmieszczenie wszystkich bomb na planszy</param>
        /// <param name="windowWidth">szerokość okna gry</param>
        /// <param name="windowHeight">wysokość okna gry</param>
        public void Update(List<BlockType> blocksTypes, Dictionary<int, BonusType> bonusLocations,
            Dictionary<int, List<CharacterType>> characterLocations,
            List<int> bombLocations, int windowWidth, int windowHeight)
        {
            int width = (windowWidth - 2*Shift)/(_columns);
            int height = (windowHeight - 2*Shift)/(_rows);
            int y = height/2 + Shift;
            int counter = 0;
            // zainicjalizuj całą planszę od nowa
            Components.Clear();
            if (Components.Count == 0)
            {
                for (int i = 0; i < _rows; i++)
                {
                    var x = Shift + width/2;
                    for (int j = 0; j < _columns; j++)
                    {
                        // należy zadbać aby zawsze bonus znajdujący się pod szarym polem był dodany po szarym polu
                        // a bonus znajdujący się na białym/czerwonym został dodany do listy przed utworzeniem Block'a
                        if (bonusLocations.ContainsKey(counter))
                        {
                            if (blocksTypes[counter].Equals(BlockType.Grey))
                            {
                                Components.Add(GenerateNewBonus(bonusLocations[counter], x, y, width, height));
                                Components.Add(GenerateNewBlock(blocksTypes[counter], x, y, width, height));
                            }
                            else
                            {
                                Components.Add(GenerateNewBlock(blocksTypes[counter], x, y, width, height));
                                Components.Add(GenerateNewBonus(bonusLocations[counter], x, y, width, height));
                            }
                        }
                        else
                        {
                            Components.Add(GenerateNewBlock(blocksTypes[counter], x, y, width, height));
                        }
                        //bomby znajdującye się na planszy
                        if (bombLocations.Contains(counter))
                        {
                            Vector2 scale = new Vector2(width/(float) _bombTexture.Width,
                                height/(float) _bombTexture.Height);
                            Components.Add(new Component(_bombTexture, Color.White, new Vector2(x, y), scale, 0));
                        }
                        //na samej górze narysowane będą postacie - przeciwnicy oraz gracz
                        bool player = true;
                        if (characterLocations.ContainsKey(counter))
                        {
                            foreach (var character in characterLocations[counter])
                            {
                                if (character == CharacterType.Player && player)
                                {
                                    PlayerLocation = counter;
                                    player = false;
                                    Components.Add(GenerateNewCharacter(character, x, y, width, height));
                                }
                                else
                                {
                                    Components.Add(GenerateNewCharacter(character, x, y, width, height));
                                }
                            }
                        }
                        x += width;
                        counter++;
                    }
                    y += height;
                }
            }
        }

        /// <summary>
        /// Utwórz jednostkowy kwadrat planszy na podstawie rozmiaru i wybranego tła.
        /// </summary>
        /// <param name="blockType">rodzaj kwadratu jednostkowego <example>characterType.White</example></param>
        /// <param name="x">położenie poziome względem okna gry</param>
        /// <param name="y">położenie pionowe względem okna gry</param>
        /// <param name="width">oczekiwana szerokość pola jednostkowego planszy</param>
        /// <param name="height">oczekiwana wysokość pola jednostkowego planszy</param>
        /// <returns>Zwróć nowy jednostkowy kwadrat planszy z ustalonym tłem.</returns>
        private Block GenerateNewBlock(BlockType blockType, int x, int y, int width, int height)
        {
            Texture2D texture = Textures[(int) blockType];
            Vector2 scale = new Vector2(width/(float) texture.Width, height/(float) texture.Height);
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
            Texture2D texture = _bonusTextures[(int) bonusType];
            Vector2 scale = new Vector2(width/(float) texture.Width, height/(float) texture.Height);
            Bonus bonus = new Bonus(texture, Color.White, new Vector2(x, y), scale, 0, bonusType);
            return bonus;
        }

        /// <summary>
        /// Utwórz postać na wybranym jednostkowym kwadracie planszy
        /// </summary>
        /// <param name="characterType">rodzaj postaci</param>
        /// <param name="x">położenie poziome względem okna gry</param>
        /// <param name="y">położenie pionowe względem okna gry</param>
        /// <param name="width">oczekiwana szerokość bonusa</param>
        /// <param name="height">oczekiwana wysokość bonusa</param>
        /// <returns>zwróć nową postać</returns>
        private Character GenerateNewCharacter(CharacterType characterType, int x, int y, int width, int height)
        {
            Texture2D texture = _characterTextures[(int) characterType];
            Vector2 scale = new Vector2(width/(float) texture.Width, height/(float) texture.Height);
            Character character = new Character(texture, Color.White, new Vector2(x, y), scale, 0, characterType);
            return character;
        }
    }
}
