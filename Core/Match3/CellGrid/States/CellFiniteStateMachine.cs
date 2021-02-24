using Match3MonoGame.Core.StateMachine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.CellGrid.States
{
    public class CellFiniteStateMachine : FiniteStateMachine
    {
        public const float MinDistance = 0.1f;
        private readonly Cell _cell;
        public CellFiniteStateMachine(Cell cell) : base()
        {
            _cell = cell;
            PushState(new CellStateStand(this));
        }

        public Cell GetCell() => _cell;
    }
}
