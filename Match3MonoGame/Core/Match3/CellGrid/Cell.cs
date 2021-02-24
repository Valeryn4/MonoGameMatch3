using Match3MonoGame.Core.InputEvents;
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
    public class Cell : Sprite
    {
        const float BaseFallSpeed = 2000f;
        public float Speed { get; set; } = 1000f;
        public float FallSpeed { get; set; }
        private CellType _type;
        public CellType CellType 
        { 
            get => _type; 
            set
            {
                _type = value;
                Texture = Match3TextureManager.GetTextureTile(_type);
            }
        
        }

        private CellBonus _bonus = CellBonus.None;
        public CellBonus Bonus
        {
            get => _bonus;
            set
            {
                _bonus = value;
                if (_bonus != CellBonus.None)
                {
                    var spriteBonus = new Sprite(_spriteBatch)
                    {
                        Texture = Match3TextureManager.GetTextureBonusTile(_bonus)
                    };
                    AddChild(spriteBonus);
                }
                
            }
                
        }
        
        public int X { get; set; }
        public int Y { get; set; }

        public bool Matches { get; set; } = false;
        public bool Selected { get; set; }

        public double LastMovedTimestamp { get; set; }
        

        public Point PosGrid 
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        
        }

        private Grid _grid;
        private CellFiniteStateMachine _fsm;
        public Cell(SpriteBatch spriteBatch, CellType type, Grid grid) : base(spriteBatch)
        {
            _type = type;
            Texture = Match3TextureManager.GetTextureTile(type);
            _grid = grid;
            Processing = true;
            FallSpeed = GameRandom.Instance.Next((int)(BaseFallSpeed * 0.9), (int)(BaseFallSpeed * 1.2));
            _fsm = new CellFiniteStateMachine(this);
        }

        public Grid GetGrid() => _grid;
        public CellFiniteStateMachine GetFsm() => _fsm;
        
        public void Destroy()
        {
            if (_fsm == null)
                return;
            if (_fsm.CurrentState() is CellStateDestroy)
                return;

            if (_fsm.CurrentState() is CellStateMove)
            {
                CellStateSemaphore.MoveCellCount--;
            }

            if (Bonus == CellBonus.HorizontalLine || Bonus == CellBonus.VerticalLine)
            {
                var bonusLine = new CellBonusLine(_spriteBatch, _grid, PosGrid, Bonus);
                _grid.GetNodeCells().AddChild(bonusLine);
                bonusLine.Position = _grid.GetPositionCell(X, Y);
            }
            else if (Bonus == CellBonus.Bomb)
            {
                var bomb = new CellBonusBomb(_spriteBatch, _grid, PosGrid);
                _grid.GetNodeCells().AddChild(bomb);
                bomb.Position = _grid.GetPositionCell(X, Y);
            }

            _grid.Score++;
            _fsm.PushState(new CellStateDestroy(_fsm));

        }
        public void ForceMove()
        {
            _fsm.PushState(new CellStateMove(_fsm));
        }

        protected override void Process(GameTime gameTime)
        {

            _fsm?.Process(gameTime);
        }

        protected override void OnFree()
        {
            _grid = null;
            _fsm = null;
            base.OnFree();
        }


    }
}
