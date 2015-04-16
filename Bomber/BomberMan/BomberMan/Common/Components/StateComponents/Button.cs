using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Components.StateComponents
{
    public class Button : Component
    {
        public BState State { get; set; }
        public double Timer { get; set; }

        private Color pressedColor = Color.DarkBlue;
        private Color hoverColor = Color.LightBlue;
        private Color normalColor = Color.White;
        private bool mpressed, prevMpressed = false;
        private int mx, my;
        private double frameTime;

        public Button(BState _state, Texture2D texture, Color color, Vector2 position, Vector2 scale, float angle, double _timer)
            : base(texture, color, position, scale, angle)
        {
            State = _state;
            Timer = _timer;
        }

        public Button()
        {
            Color = normalColor;
            State = BState.UP;
            Timer = 0.0;
        }

        public void Update(int _mx, int _my, double _frameTime, bool _mPressed, bool _prevMPressed)
        {
            mx = _mx;
            my = _my;
            frameTime = _frameTime;
            mpressed = _mPressed;
            prevMpressed = _prevMPressed;
            if (CheckIfButtonContainsPoint(mx, my))
            {
                Timer = 0.0;
                if (mpressed)
                {
                    State = BState.DOWN;
                    Color = pressedColor;
                }
                else if (!mpressed && prevMpressed)
                {
                    if (State == BState.DOWN)
                    {
                        State = BState.JUST_RELEASED;
                    }
                }
                else
                {
                    State = BState.HOVER;
                    Color = hoverColor;
                }
            }
            else
            {
                State = BState.UP;
                if (Timer > 0)
                {
                    Timer = Timer - frameTime;
                }
                else
                {
                    Color = normalColor;
                }
            }
            if (State == BState.JUST_RELEASED)
            {
                OnClick(Timer);
            }
        }

        public void OnClick(double _timer)
        {
            Color = Color.Green;
            Timer = _timer;
        }


        private bool CheckIfButtonContainsPoint(int x, int y)
        {
            return CheckIfRectangleContainsPoint(0, 0, (int) (((float)x - (Position.X - (float)Texture.Width * Scale.X/(float)2 )) / Scale.X),
                (int)(((float)y - (Position.Y - (float)Texture.Height * Scale.Y / (float)2)) / Scale.Y));
        }

        private bool CheckIfTextureContainsPoint(float tx, float ty, int x, int y)
        {
            return (x >= tx &&
                x <= tx + (float)Texture.Width &&
                y >= ty &&
                y <= ty + (float)Texture.Height);
        }

        private bool CheckIfRectangleContainsPoint(float tx, float ty, int x, int y)
        {
            if (CheckIfTextureContainsPoint(tx, ty, x, y))
            {
                uint[] data = new uint[Texture.Width * Texture.Height];
                Texture.GetData<uint>(data);
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
        HOVER,
        UP,
        JUST_RELEASED,
        DOWN
    }
}
