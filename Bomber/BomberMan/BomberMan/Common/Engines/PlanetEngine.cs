using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Engines
{
    /// <summary>
    /// Silnik generujący planety znajdujące się w tle aplikacji.
    /// </summary>
    internal class PlanetEngine : Engine
    {
        private const int Shift = 50;
        public float MaxHeight { get; set; }
        public float MaxWidth { get; set; }
        private int _prevWindowWidth, _prevWindowHeight;

        public PlanetEngine(List<Texture2D> textures, int objectsCount)
            : base(textures, objectsCount)
        {
        }

        /// <summary>
        /// Uaktualnij pozycje i rozmiary planet podczas resize okna aplikacji.
        /// </summary>
        /// <param name="windowWidth">nowa szerokość okna aplikacji</param>
        /// <param name="windowHeight">nowa wysokość okna aplikacji</param>
        public void Update(int windowWidth, int windowHeight)
        {
            if (Components.Count == 0)
            {
                for (int i = 0; i < ObjectsAmount; i++)
                {
                    Components.Add(GenerateNewPlanet(windowWidth, windowHeight));
                }
            }
            else
            {
                for (int i = 0; i < ObjectsAmount; i++)
                {
                    Components[i].Position =
                        new Vector2(windowWidth*Components[i].Position.X/_prevWindowWidth,
                            windowHeight*Components[i].Position.Y/_prevWindowHeight);
                }
            }
            _prevWindowWidth = windowWidth;
            _prevWindowHeight = windowHeight;
        }

        /// <summary>
        /// Utwórz nową planetę gotową do wyświetlenia w aplikacji.
        /// </summary>
        /// <param name="windowWidth">szerokość okna aplikacji</param>
        /// <param name="windowHeight">wysokosć okna aplikacji</param>
        /// <returns></returns>
        private Component GenerateNewPlanet(int windowWidth, int windowHeight)
        {
            _prevWindowWidth = windowWidth;
            _prevWindowHeight = windowHeight;
            Texture2D texture = Textures[Random.Next(Textures.Count)];
            float scale = (float) 1/(float) (Random.Next(4) + 1);
            Vector2 position = new Vector2(Random.Next(windowWidth), Random.Next(windowHeight));
            Color color = new Color(
                (float) Random.NextDouble(),
                (float) Random.NextDouble(),
                (float) Random.NextDouble());
            return new Component(texture, color, position, new Vector2(scale, scale), 0);
        }
    }
}
