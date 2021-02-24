using Match3MonoGame.Core.StateMachine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.CellGrid.States
{
    public class CellStateDestroy : State<CellFiniteStateMachine>
    {
        private const float _timeDead = 0.2f;
        private float _timeLeft = 0f;
        private bool _isEnd = false;

        private float _beginScale = 1.0f;
        public CellStateDestroy(CellFiniteStateMachine fsm) : base(fsm)
        {
            _beginScale = fsm.GetCell().Scale;
            CellStateSemaphore.DestroySemaphore++;
        }

        public override void Process(GameTime gameTime)
        {
            if (_isEnd)
                return;
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timeLeft += delta;

            var factor = Math.Clamp(1.0f - (_timeLeft / _timeDead), 0f, 1f);
            var scale = factor * _beginScale;
            GetFsm().GetCell().Scale = scale;
            if (_timeLeft >= _timeDead)
            {
                _isEnd = true;

                CellStateSemaphore.DestroySemaphore--;
                GetFsm().GetCell().QueueFree();
            }

        }
    }
}
