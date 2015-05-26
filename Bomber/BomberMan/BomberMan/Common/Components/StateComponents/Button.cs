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
        private readonly Color _normalColor = Color.White;
        private bool _mpressed, _prevMpressed;
        private int _mx, _my;
        private double _frameTime;
        public Func<string, int> Click;

        public Button(BState state, Texture2D texture, Color color, Vector2 position, Vector2 scale, float angle, double timer)
            : base(texture, color, position, scale, angle)
        {
            _state = state;
            _timer = timer;
        }

        public Button()
        {
            Color = _normalColor;
            _state = BState.Up;
            _timer = 0.0;
        }

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
        }

        public void OnClick(double timer)
        {
            _timer = timer;
            Color = Color.Green;
        }


        private bool CheckIfButtonContainsPoint(int x, int y)
        {
            return CheckIfRectangleContainsPoint(0, 0, (int) ((x - (Position.X - Texture.Width * Scale.X/(float)2 )) / Scale.X),
                (int)((y - (Position.Y - Texture.Height * Scale.Y / 2)) / Scale.Y));
        }

        private bool CheckIfTextureContainsPoint(float tx, float ty, int x, int y)
        {
            return (x >= tx &&
                x <= tx + Texture.Width &&
                y >= ty &&
                y <= ty + Texture.Height);
        }

        private bool CheckIfRectangleContainsPoint(float tx, float ty, int x, int y)
        {
            if (CheckIfTextureContainsPoint(tx, ty, x, y))
            {
                uint[] data = new uint[Texture.Width * Texture.Height];
                Texture.GetData(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    Texture.Width < Texture.Width * Texture.Height)
                {
                    return ((data[(x - (int)tx) + (y - (int)ty) * Texture.Width] & 0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }
    }

    public enum BState
    {
        Hover,
        Up,
        JustReleased,
        Down
    }
}
