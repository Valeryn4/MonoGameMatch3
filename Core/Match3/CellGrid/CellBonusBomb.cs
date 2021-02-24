using Match3MonoGame.Core.Match3.CellGrid.States;
using Match3MonoGame.Core.NodeGraph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.CellGrid
{

    using Grid = Match3MonoGame.Core.Match3.Grid.Grid;
    public class CellBonusBomb : Sprite
    {
        private readonly List<Texture2D> _frames = new List<Texture2D>();

        const float BombTime = 0.25f;
        private float _leftTime = 0f;
        private Grid _grid;
        private Point _pos;
        public CellBonusBomb(SpriteBatch spriteBatch, Grid grid, Point pos) : base(spriteBatch)
        {
            _grid = grid;
            _pos = pos;
            CellStateSemaphore.ActiveBonuses++;
            _frames = Match3TextureManager.GetTexturesExplotion();
            Scale = 3.0f;
        }

        protected override void OnEnteredTree()
        {
            Processing = true;
            base.OnEnteredTree();
        }

        protected override void OnExitedTree()
        {

            CellStateSemaphore.ActiveBonuses--;
            base.OnExitedTree();
        }

        protected override void Process(GameTime gameTime)
        {
            var countFrames = _frames.Count;
            var currentFrameID = (int)Math.Floor(countFrames * (_leftTime / BombTime));
            currentFrameID = Math.Clamp(currentFrameID, 0, countFrames);
            Texture = _frames[currentFrameID];
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _leftTime += delta;
            if (_leftTime >= BombTime)
            {
                Processing = false;
                for (int x = _pos.X - 1; x < _pos.X + 2; x++)
                {
                    for (int y = _pos.Y -1; y < _pos.Y + 2; y++)
                    {
                        if (x >= 0 && x < _grid.X && y >= 0 && y < _grid.Y)
                        {
                            if (_grid[x, y] != null)
                            {
                                _grid[x, y].Destroy();
                                _grid[x, y] = null;
                            }
                        }
                    }
                }

                Texture = null;
                QueueFree();

            }
        
        }
    }
}
