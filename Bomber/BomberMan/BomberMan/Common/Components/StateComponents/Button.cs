using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BomberMan.Common.Components.StateComponents
{
    public class Button : StateComponent
    {
        public BState State { get; set; }
        public double Timer { get; set; }

        bool mpressed, prevMpressed = false;
        int mx, my;
        double frameTime;

        public Button(int height, int width, Color color, BState _state, Texture2D texture,
            Rectangle rectangle, double _timer) : base (texture, color, height, width, rectangle)
        {
            State = _state;
            Timer = _timer;
        }

        public Button()
        {
            // TODO: Complete member initialization
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
                    Color = Color.Blue;
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
                    Color = Color.LightBlue;
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
                    Color = Color.White;
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


        bool CheckIfButtonContainsPoint(int x, int y)
        {
            return CheckIfRectangleContainsPoint(0, 0, Texture.Width * (x - Rectangle.X) /
                Rectangle.Width, Texture.Height * (y - Rectangle.Y) / Rectangle.Height);
        }

        bool CheckIfTextureContainsPoint(float tx, float ty, int x, int y)
        {
            return (x >= tx &&
                x <= tx + Texture.Width &&
                y >= ty &&
                y <= ty + Texture.Height);
        }

        bool CheckIfRectangleContainsPoint(float tx, float ty, int x, int y)
        {
            if (CheckIfTextureContainsPoint(tx, ty, x, y))
            {
                uint[] data = new uint[Texture.Width * Texture.Height];
                Texture.GetData<uint>(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    Texture.Width < Texture.Width * Texture.Height)
                {
                    return ((data[
                        (x - (int)tx) + (y - (int)ty) * Texture.Width] & 0xFF000000) >> 24) > 20;
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
