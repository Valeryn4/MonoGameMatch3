using Match3MonoGame.Core.InputEvents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.NodeGraph
{
    public class ShapeButton : Node2D
    {
        public delegate void Pressed();
        public delegate void Released();

        public event Pressed EventPressed;
        public event Released EventReleased;

        public RectangleF ShapeRect { get; set; }


        private bool isPressed = false;
        
        public ShapeButton(SpriteBatch spriteBatch) : base(spriteBatch)
        {
            InputEnable = true;
        }


        protected override void Input(InputEvent ev)
        {

            if (ev is InputEventMouse)
            {
                var mouseEvent = ev as InputEventMouse;
                if (mouseEvent.Pressed)
                {
                    if (!isPressed)
                    {
                        var locationRect = new Vector2(ShapeRect.Position.X * GlobalScale, ShapeRect.Position.Y * GlobalScale);
                        locationRect += GlobalPosition;
                        var localRect = new RectangleF(locationRect.X, locationRect.Y, ShapeRect.Width * GlobalScale, ShapeRect.Height * GlobalScale);
                         
                        if (localRect.Contains(mouseEvent.Position))
                        {
                            isPressed = true;
                            EventPressed?.Invoke();
                        }
                    }
                }
                else
                {
                    if (isPressed)
                    {
                        isPressed = false;
                        EventReleased?.Invoke();
                    }
                }
            }
            base.Input(ev);
        }
    }
}
