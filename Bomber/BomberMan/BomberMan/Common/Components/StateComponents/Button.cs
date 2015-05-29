using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BomberMan.Common.Components.StateComponents
{
    /// <summary>
    /// Reprezentacja przyciska, możliwośc najechania na przycisk, wciśnięcia, wykonania akcji
    /// </summary>
    public class Button : Component
    {
        private BState _state;
        private double _timer;
        private readonly Color _pressedColor = Color.DarkBlue;
        private readonly Color _hoverColor = Color.LightBlue;
        private Color _normalColor = Color.White;
        private bool _mpressed, _prevMpressed;
        private int _mx, _my;
        private double _frameTime;
        public Func<Color> Click;
        private readonly Label _textLabel;

        public String Text
        {
            get { return _textLabel.Text; }
            set { _textLabel.Text = value; }
        }

        public Color NormalColor { set { _normalColor = value; } }

        /// <summary>
        /// Utwórz nowy przycisk z napisem na nim.
        /// </summary>
        /// <param name="state">stan startowy przycisku <example>Wciśnięty</example></param>
        /// <param name="texture">tło przycisku</param>
        /// <param name="color">kolor przycisku</param>
        /// <param name="position">pozycja przycisku</param>
        /// <param name="scale">skala rozmiaru przycisku</param>
        /// <param name="angle">kąt nachylenia przycisku</param>
        /// <param name="timer">najkrótszy czas od jednej zmiany stanu przycisku do drugiej</param>
        /// <param name="spriteFont">czcionka napisu na przycisku</param>
        /// <param name="text">napis na przycisku</param>
        /// <param name="textColor">kolor tekstu na przycisku</param>
        public Button(BState state, Texture2D texture, Color color, Vector2 position, Vector2 scale, float angle,
            double timer, Color textColor, SpriteFont spriteFont = null, String text = "")
            : base(texture, color, position, scale, angle)
        {
            _textLabel = new Label(spriteFont, text, textColor, position, scale, angle);
            _state = state;
            _timer = timer;
        }

        /// <summary>
        /// Utwórz przycisk z tekstem znajdującym się na nim.
        /// Nie obracaj przycisku, ustaw pozycję startową na 0,0
        /// a skalę na 1,1
        /// </summary>
        /// <param name="texture">tło przycisku</param>
        /// <param name="color">kolor przycisku</param>
        /// <param name="spriteFont">czcionka napisu</param>
        /// <param name="text">napis</param>
        /// <param name="textColor">kolor tekstu znajdującego się na przycisku</param>
        public Button(Texture2D texture, Color color, SpriteFont spriteFont, String text, Color textColor)
            : base(texture, color, new Vector2(0, 0), new Vector2(1, 1), 0f)
        {
            _textLabel = new Label(spriteFont, text, textColor, new Vector2(0, 0), new Vector2(1, 1), 0);
            _state = BState.Up;
            _timer = 2f;
        }

        /// <summary>
        /// Utwórz przycisk bez napisu.
        /// </summary>
        public Button()
        {
            Color = _normalColor;
            _state = BState.Up;
            _timer = 0.0;
        }

        /// <summary>
        /// Uaktualnij widok przycisku w zależności od położenia myszki na ekranie.
        /// </summary>
        /// <param name="mx">współrzędna x myszki na ekranie</param>
        /// <param name="my">współrzędna y myszki na ekranie</param>
        /// <param name="frameTime">czas trwania gry</param>
        /// <param name="mPressed">stan myszki <true>wciśnięta</true></param>
        /// <param name="prevMPressed">poprzedni stan myszki <value>true</value> wciśnięta</param>
        public void Update(int mx, int my, double frameTime, bool mPressed, bool prevMPressed)
        {
            _mx = mx;
            _my = my;
            _frameTime = frameTime;
            _mpressed = mPressed;
            _prevMpressed = prevMPressed;
            if (CheckIfButtonContainsPoint(_mx, _my))
            {
                _timer = 0.0;
                if (_mpressed)
                {
                    _state = BState.Down;
                    Color = _pressedColor;
                }
                else if (!_mpressed && _prevMpressed)
                {
                    if (_state == BState.Down)
                    {
                        _state = BState.JustReleased;
                    }
                }
                else
                {
                    _state = BState.Hover;
                    Color = _hoverColor;
                }
            }
            else
            {
                _state = BState.Up;
                if (_timer > 0)
                {
                    _timer = _timer - _frameTime;
                }
                else
                {
                    Color = _normalColor;
                }
            }
            if (_state == BState.JustReleased)
            {
                OnClick(_timer);
            }
            if (_textLabel != null && _textLabel.Text.Length > 0)
            {
                _textLabel.Position = new Vector2(Position.X - Texture.Width*Scale.X/2,
                    Position.Y - Texture.Height*Scale.Y/2);
            }
        }

        /// <summary>
        /// Narysuj na podanym SpriteBatch komponent z labelką.
        /// </summary>
        /// <param name="spriteBatch">Obiekt, do ktorego dorysowujemy własny komponent</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (_textLabel != null && _textLabel.Text.Length > 0)
            {
                _textLabel.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Metoda wykonywana podczas kliknięcia na przycisk.
        /// </summary>
        /// <param name="timer"></param>
        public void OnClick(double timer)
        {
            _timer = timer;
            //Color = Color.Green;
            if (Click != null)
            {
                Color color = Click();
                if (color != Color.Transparent)
                    _normalColor = color;
            }
        }

        /// <summary>
        /// Metoda sprawdza czy dany punkt należy do przycisku.
        /// Wykorzystywane do sprawdzenia czy myszka najeżdża na przycisk.
        /// </summary>
        /// <param name="x">współrzędna x punktu</param>
        /// <param name="y">współrzędna y punktu</param>
        /// <returns>zwróć <value>true</value> jeżeli punkt zawiera się w przeciwnym przypadku zwróć <value>false</value></returns>
        private bool CheckIfButtonContainsPoint(int x, int y)
        {
            return CheckIfRectangleContainsPoint(0, 0,
                (int) ((x - (Position.X - Texture.Width*Scale.X/(float) 2))/Scale.X),
                (int) ((y - (Position.Y - Texture.Height*Scale.Y/2))/Scale.Y));
        }

        /// <summary>
        /// Metoda sprawdza czy wydzielone tło przyciska zawiera punkt.
        /// </summary>
        /// <param name="tx">współrzędna startowa x na tle porównywania</param>
        /// <param name="ty">współrzędna startowa y na tle porónywania</param>
        /// <param name="x">współrzędna x punktu</param>
        /// <param name="y">współrzędna y punktu</param>
        /// <returns>zwróć <value>true</value> jeżeli punkt zawiera się w przeciwnym przypadku zwróć <value>false</value></returns>
        private bool CheckIfTextureContainsPoint(float tx, float ty, int x, int y)
        {
            return (x >= tx &&
                    x <= tx + Texture.Width &&
                    y >= ty &&
                    y <= ty + Texture.Height);
        }

        /// <summary>
        /// Sprawdź czy przycisk zawiera dany punkt, jedynie w miejscach gdzie nie jest on prześwitujący.
        /// </summary>
        /// <param name="tx">współrzędna startowa x na tle porównywania</param>
        /// <param name="ty">współrzędna startowa y na tle porónywania</param>
        /// <param name="x">współrzędna x punktu</param>
        /// <param name="y">współrzędna y punktu</param>
        /// <returns>zwróć <value>true</value> jeżeli punkt zawiera się w przeciwnym przypadku zwróć <value>false</value></returns>
        private bool CheckIfRectangleContainsPoint(float tx, float ty, int x, int y)
        {
            if (CheckIfTextureContainsPoint(tx, ty, x, y))
            {
                uint[] data = new uint[Texture.Width*Texture.Height];
                Texture.GetData(data);
                if ((x - (int) tx) + (y - (int) ty)*
                    Texture.Width < Texture.Width*Texture.Height)
                {
                    return ((data[(x - (int) tx) + (y - (int) ty)*Texture.Width] & 0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Reprezentuje stan przycisku: najechanie, brak czynności, właśnie puszczony, przyciśnięty
    /// </summary>
    public enum BState
    {
        Hover,
        Up,
        JustReleased,
        Down
    }
}
