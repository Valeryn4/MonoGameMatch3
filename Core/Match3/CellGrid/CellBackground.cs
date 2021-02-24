using Match3MonoGame.Core.InputEvents;
using Match3MonoGame.Core.NodeGraph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Match3MonoGame.Core.Match3.CellGrid
{

    using Grid = Match3MonoGame.Core.Match3.Grid.Grid;
    public class CellBackground : Sprite
    {

        public enum SelectType
        {
            Focus,
            FirstSelect,
            SecondSelect,
            None
        }

        public delegate void PressedPos(int x, int y, CellBackground cell);
        public event PressedPos EventPressCell;

        public int X { get; set; }
        public int Y { get; set; }


        public SelectType Select { get; set; }

        public CellBackground(SpriteBatch spriteBatch) : base(spriteBatch)
        {
            InputEnable = true;
            Processing = true;
            Select = SelectType.None;
        }

        public void OnReleaseSelection()
        {
            Select = SelectType.None;
        }

        private bool _is_pressed = false;
        protected override void Input(InputEvent ev)
        {
            if (ev is InputEventMouse)
            {
                var mouse = ev as InputEventMouse;
                var textureSize = new Vector2(Texture.Width, Texture.Height) * Grid.ScaleBackgroundGridCell;
                var halfTextureSize = textureSize * 0.5f;
                var leftTop = GlobalPosition - halfTextureSize;
                var rect = new Rectangle(leftTop.ToPoint(), textureSize.ToPoint());
                var press = mouse.State.LeftButton == ButtonState.Pressed;

                

                if (rect.Contains(mouse.Position.ToPoint()))
                {
                    if (press)
                    {
                        if (!_is_pressed)
                        {
                            _is_pressed = true;
                            Debug.WriteLine($"Pressed [{X}:{Y}]");
                        }
                    }
                    else
                    { 
                        if (_is_pressed)
                        {
                            _is_pressed = false;
                            Debug.WriteLine($"Relese [{X}:{Y}]");
                            EventPressCell?.Invoke(X, Y, this);
                        }
                        else
                        {
                            if (Select == SelectType.None)
                                Select = SelectType.Focus;
                        }
                    }
                }
                else
                {
                    if (!press && _is_pressed)
                    {
                        _is_pressed = false;

                        Debug.WriteLine($"Release [{X}:{Y}] out");
                    }
                    if (Select == SelectType.Focus)
                        Select = SelectType.None;
                }
            }
        }

        protected override void Process(GameTime gameTime)
        {
            switch (Select)
            {
                case SelectType.None:
                    Texture = Match3TextureManager.GetTextureBackgroundTile();
                    break;
                case SelectType.Focus:
                    Texture = Match3TextureManager.GetTextureTileBack(CellBackType.Yellow);
                    break;
                case SelectType.FirstSelect:
                    Texture = Match3TextureManager.GetTextureTileBack(CellBackType.Green);
                    break;
                case SelectType.SecondSelect:
                    Texture = Match3TextureManager.GetTextureTileBack(CellBackType.Blue);
                    break;


            }

        }

        protected override void OnFree()
        {
            EventPressCell = null;
            base.OnFree();
        }
    }
}
