using Match3MonoGame.Core.StateMachine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.Grid.States
{
    public class GridStateSpawn : State<GridFiniteStateMachine>
    {
        public bool WithoutMatch { get; set; } = false;
        public GridStateSpawn(GridFiniteStateMachine fsm) : base(fsm)
        {
        }

        public override void Process(GameTime gameTime)
        {
            var grid = GetFsm().GetGrid();
            for (int x = 0; x < grid.X; x++)
            {
                for (int y = 0; y < grid.Y; y++ )
                {
                    if (grid[x, y] == null)
                    {
                        grid.FillCell(x, y);
                        var countLoop = 0;
                        while (MatchAt(x, y) && countLoop < 100)
                        {
                            grid[x, y].CellType = grid.GetRandomType();
                            countLoop++;
                        }
                    }
                }
            }

            GetFsm().PopState();
            GetFsm().PushState(new GridStateMove(GetFsm()));
        }

        public bool MatchAt(int x, int y)
        {
            var grid = GetFsm().GetGrid();
            var cell = grid[x, y];
            if (cell != null)
            {
                if (x > 1)
                {
                    if (grid[x - 1, y] != null && grid[x - 2, y] != null)
                        if (grid[x - 1, y].CellType == cell.CellType && grid[x - 2, y].CellType == cell.CellType)
                            return true;
                }
                if (y > 1)
                {
                    if (grid[x, y - 1] != null && grid[x, y - 2] != null)
                        if (grid[x, y - 1].CellType == cell.CellType && grid[x, y - 2].CellType == cell.CellType)
                            return true;
                }
            }

            return false;
        }

    }
}
