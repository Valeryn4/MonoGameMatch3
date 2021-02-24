using Match3MonoGame.Core.Match3.CellGrid.States;
using Match3MonoGame.Core.StateMachine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.Grid.States
{
    public class GridStateFall : State<GridFiniteStateMachine>
    {
        public GridStateFall(GridFiniteStateMachine fsm) : base(fsm)
        {
            
        }

        public override void Process(GameTime gameTime)
        {
            if (CellStateSemaphore.ActiveBonuses != 0)
                return;
            var grid = GetFsm().GetGrid();
            for (int x = 0; x < grid.X; x++)
            {
                for (int y = grid.Y - 1; y > -1; y--)
                {
                    var cell = grid[x, y];
                    if (cell == null)
                    {
                        for (int topY = y - 1; topY > -1; topY--)
                        {
                            var topCell = grid[x, topY];
                            if (topCell != null)
                            {
                                topCell.X = x;
                                topCell.Y = y;
                                grid[x, topY] = null;
                                grid[x, y] = topCell;
                                topCell.ForceMove();
                                break;
                            }
                        }
                    }
                }
            }

            GetFsm().PopState();
            GetFsm().PushState(new GridStateSpawn(GetFsm()));
        }
    }
}
