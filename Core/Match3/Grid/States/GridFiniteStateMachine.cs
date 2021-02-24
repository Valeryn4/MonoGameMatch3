using Match3MonoGame.Core.StateMachine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.Grid.States
{
    using Grid = Match3MonoGame.Core.Match3.Grid.Grid;
    public class GridFiniteStateMachine : FiniteStateMachine
    {
        private Grid _grid = null;
        public GridFiniteStateMachine(Grid grid) : base()
        {
            _grid = grid;
        }

        public Grid GetGrid() => _grid;
    }
}
