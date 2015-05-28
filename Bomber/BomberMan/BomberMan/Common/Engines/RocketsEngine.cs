using System;
using System.Collections.Generic;
using BomberMan.Common.Components;
using BomberMan.Common.Components.MovingComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Engines
{
    /// <summary>
    /// Klasa generująca latające rakiety w tle aplikacji podczas jej działania.
    /// </summary>
    public class RocketsEngine : Engine
    {
        /// <summary>
        /// Bariera graniczna dla widoku okna, wykorzystywana gdy samolot doleci do końca okna 
        /// symuluje lot o <value>SHIFT</value> jednostek dalej po czym znika
        /// </summary>
        private const int Shift = 50;

        public float MaxHeight { get; set; }
        public float MaxWidth { get; set; }

        /// <summary>
        /// Utwórz nowy silnik odpowiedzialny za generowanie latających rakiet w tle aplikacji.
        /// </summary>
        /// <param name="textures">dostępne tła rakiet</param>
        /// <param name="objectsCount">ilość generowanych rakiet</param>
        public RocketsEngine(List<Texture2D> textures, int objectsCount) : base(textures, objectsCount)
        {
            for (int i = 0; i < ObjectsAmount; i++)
            {
                Components.Add(GenerateNewRocket());
            }
        }

        /// <summary>
        /// Uaktualnij pozycjie wszystkich rakiet w aplikacji.
        /// </summary>
        public void Update()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                ((MovingComponent) Components[i]).Update();
                if (
                    ((MovingComponent) Components[i]).Position.X > MaxWidth + Shift ||
                    ((MovingComponent) Components[i]).Position.X < 0 - Shift ||
                    ((MovingComponent) Components[i]).Position.Y < 0 - Shift ||
                    ((MovingComponent) Components[i]).Position.Y > MaxHeight + Shift)
                {
                    Components.RemoveAt(i);
                    i--;
                }
            }
            for (int i = ObjectsAmount - Components.Count; i > 0; i--)
            {
                Components.Add(GenerateNewRocket());
            }
        }

        /// <summary>
        /// Utwórz nową rakietę. Wyznacz punkt startowy, prędkość, kierunek, kolor.
        /// </summary>
        /// <returns>Zwróć nową rakietę gotową do wyświetlenia</returns>
        private Rocket GenerateNewRocket()
        {
            Texture2D texture = Textures[Random.Next(Textures.Count)];
            float positionY, positionX;
            bool vertical = Random.Next(1) > 0;
            if (vertical)
            {
                positionY = Random.Next((int) MaxHeight);
                bool left = Random.Next(1) > 0;
                if (left)
                    positionX = (float) 0;
                else
                    positionX = MaxWidth;
            }
            else
            {
                positionX = Random.Next((int) MaxWidth);
                bool up = Random.Next(1) > 0;
                if (up)
                    positionY = (float) 0;
                else
                    positionY = MaxHeight;
            }
            Vector2 position = new Vector2(positionX, positionY);
            float a = 1f*(float) (Random.NextDouble()*2 - 1);
            float b = 1f*(float) (Random.NextDouble()*2 - 1);
            Vector2 velocity = new Vector2(a, b);
            float angle = MathHelper.Pi - (float) Math.Atan2(a, b);
            Color color = new Color(
                (float) Random.NextDouble(),
                (float) Random.NextDouble(),
                (float) Random.NextDouble());
            float scale = (float) Random.NextDouble() + (float) 0.05;
            return new Rocket(texture, position, velocity, angle, color, new Vector2(scale, scale));
        }
    }
}
