using Match3MonoGame.Core.StateMachine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.CellGrid.States
{
    public class CellStateStand : State<CellFiniteStateMachine>
    {
        public CellStateStand(CellFiniteStateMachine fsm) : base(fsm)
        {
        }

        public override void Process(GameTime gameTime)
        {
            var cell = GetFsm().GetCell();
            var grid = cell.GetGrid();
            var pos = grid.GetPositionCell(cell.X, cell.Y);
            var selfPos = cell.Position;
            var distance = Vector2.Distance(selfPos, pos);
            if (distance > CellFiniteStateMachine.MinDistance)
            {
                GetFsm().PushState(new CellStateMove(GetFsm()));
            }


        }
    }
}
