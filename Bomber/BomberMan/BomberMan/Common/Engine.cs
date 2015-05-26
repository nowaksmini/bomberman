using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common
{
    /// <summary>
    /// Bazowa klasa reprezentująca silniki generujące zbiory komponentów w aplikacji.
    /// </summary>
    public class Engine
    {
        protected Random Random;
        protected List<Texture2D> Textures;
        protected int ObjectsAmount;
        protected List<Component> Components;

        /// <summary>
        /// Utwórz silnik do generowania zbiorów komponentów.
        /// </summary>
        /// <param name="textures">dostępne tła dla komponentów</param>
        /// <param name="objectsCount">ilość komponentów</param>
        public Engine(List<Texture2D> textures, int objectsCount)
        {
            Textures = textures;
            ObjectsAmount = objectsCount;
            Random = new Random();
            Components = new List<Component>();
        }

        /// <summary>
        /// Narysuj wszystkie komponenty przechowywane w liście komponentów w obiekcie.
        /// </summary>
        /// <param name="spriteBatch">Obiekt, w którym rysujemy komponenty</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int index = 0; index < Components.Count; index++)
            {
                Components[index].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
