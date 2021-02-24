using Match3MonoGame.Core.Match3.CellGrid.States;
using Match3MonoGame.Core.StateMachine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.Grid.States
{
    public class GridStateMove : State<GridFiniteStateMachine>
    {
        public GridStateMove(GridFiniteStateMachine fsm) : base(fsm)
        {
        }

        public override void Process(GameTime gameTime)
        {
            var grid = GetFsm().GetGrid();
            var isFirst = grid.IsFirstSpawn;
            if (CellStateSemaphore.MoveCellCount == 0)
            {
                GetFsm().PopState();
                if (!isFirst)
                {
                    for (int x = 0; x < grid.X; x++)
                    {
                        for (int y = 0; y < grid.Y; y++)
                        {
                            var cell = grid[x, y];
                            if (cell == null)
                            {
                                GetFsm().PushState(new GridStateSpawn(GetFsm()));
                                return;
                            }
                        }
                    }
                    GetFsm().PushState(new GridStateMatch(GetFsm()));
                }
                else
                {
                    grid.IsFirstSpawn = false;
                }
            }
        }
    }
}
