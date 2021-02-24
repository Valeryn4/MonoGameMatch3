using Match3MonoGame.Core.Match3.CellGrid;
using Match3MonoGame.Core.StateMachine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.Grid.States
{
    public class GridStateSwap : State<GridFiniteStateMachine>
    {
        private Cell _cell1 = null;
        private Cell _cell2 = null;
        private int _swapCount = 0;
        public GridStateSwap(GridFiniteStateMachine fsm, Cell cell1, Cell cell2) : base(fsm)
        {
            _cell1 = cell1;
            _cell2 = cell2;
        }

        public override void Process(GameTime gameTime)
        {
            if (_swapCount < 2)
            {
                var grid = GetFsm().GetGrid();
                if (!grid.SwapSelected())
                {
                    GetFsm().PopState();
                    GetFsm().GetGrid().Release();
                    return;
                }
                var pos1 = _cell1.PosGrid;
                var pos2 = _cell2.PosGrid;
                _cell1.PosGrid = pos2;
                _cell2.PosGrid = pos1;

                grid.SetCell(_cell1);
                grid.SetCell(_cell2);

                GetFsm().PushState(new GridStateMove(GetFsm()));
                _swapCount += 1;
            }
            else
            {
                GetFsm().PopState();
                GetFsm().GetGrid().Release();
            }

        }
    }
}
