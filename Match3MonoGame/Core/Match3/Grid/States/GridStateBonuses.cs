using Match3MonoGame.Core.Match3.CellGrid.States;
using Match3MonoGame.Core.StateMachine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.Grid.States
{
    public class GridStateBonuses : State<GridFiniteStateMachine>
    {
        public GridStateBonuses(GridFiniteStateMachine fsm) : base(fsm)
        {
        }

        public override void Process(GameTime gameTime)
        {
            if (CellStateSemaphore.ActiveBonuses == 0)
            {
                GetFsm().PopState();
            }
        }
    }
}
