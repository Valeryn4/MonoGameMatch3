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

    public class CellBonusLine : Node2D
    {
        private Grid _grid = null;
        private Sprite _firstDestroyed = null;
        private Sprite _secondDestroyed = null;
        public CellBonus Bonus { get; private set; }
        private Point _posGrid;

        const float SpeedDestroyer = 1500f;
        const float MinDestroyDistance = 25f;

        private Stack<Point> _firstTargets = new Stack<Point>();
        private Stack<Point> _secondTargets = new Stack<Point>();

        public CellBonusLine(SpriteBatch spriteBatch, Grid grid, Point posGrid, CellBonus bonus) : base(spriteBatch)
        {
            CellStateSemaphore.ActiveBonuses++;
            _grid = grid;
            _posGrid = posGrid;
            Bonus = bonus;
            
            if (Bonus == CellBonus.HorizontalLine)
            {
                for (int x = 0; x < posGrid.X; x++)
                {
                    _firstTargets.Push(new Point(x, posGrid.Y));
                }
                for (int x = _grid.X - 1; x > posGrid.X; x--)
                {
                    _secondTargets.Push(new Point(x, posGrid.Y));
                }
            }
            else
            {
                for (int y = 0; y < posGrid.Y; y++)
                {
                    _firstTargets.Push(new Point(posGrid.X, y));
                }
                for (int y = _grid.Y - 1; y > posGrid.Y; y--)
                {
                    _secondTargets.Push(new Point(posGrid.X, y));
                }
            }

           
        }

        protected override void OnEnteredTree()
        {
            _firstDestroyed = new Sprite(_spriteBatch)
            {
                Texture = Match3TextureManager.GetTextureParticle()
            };
            AddChild(_firstDestroyed);
            _firstDestroyed.Position = Vector2.Zero;
            _firstDestroyed.Scale = 1.5f;

            _secondDestroyed = new Sprite(_spriteBatch)
            {
                Texture = Match3TextureManager.GetTextureParticle()
            };
            AddChild(_secondDestroyed);
            _secondDestroyed.Position = Vector2.Zero;
            _secondDestroyed.Scale = 1.5f;
            Processing = true;
            base.OnEnteredTree();
        }

        protected override void OnExitedTree()
        {
            CellStateSemaphore.ActiveBonuses--;
            base.OnExitedTree();
        }

        protected override void OnFree()
        {
            _firstDestroyed = null;
            _secondDestroyed = null;
            _grid = null;
            base.OnFree();
        }


        private void MoveDestroyer(float delta, Stack<Point> targetPull, Sprite destroyer)
        {
            if (destroyer == null)
                return;
            if (targetPull.Count > 0)
            {
                var pos = Position + destroyer.Position;
                var target = targetPull.Peek();
                var targetPos = _grid.GetPositionCell(target.X, target.Y);

                var distance = Vector2.Distance(targetPos, pos);
                if (distance < MinDestroyDistance)
                {
                    _grid[target.X, target.Y]?.Destroy();
                    _grid[target.X, target.Y] = null;
                    targetPull.Pop();
                }
                else
                {
                    var direction = Vector2.Normalize(targetPos - pos);
                    var velocity = direction * SpeedDestroyer * delta;
                    destroyer.Position += velocity;

                }
            }
            else 
            {
                destroyer.Texture = null;
            }
        }

        protected override void Process(GameTime gameTime)
        {
            if (_firstTargets.Count == 0 && _secondTargets.Count == 0)
            {
                Processing = false;
                QueueFree();
            }
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            MoveDestroyer(delta, _firstTargets, _firstDestroyed);
            MoveDestroyer(delta, _secondTargets, _secondDestroyed);


        }
    }
}
