using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.StateMachine
{
    /// <summary>
    /// State template.
    /// </summary>
    /// <typeparam name="T"> is FinitaeStateMachine</typeparam>
    public abstract class State<T> : IState
        where T : FiniteStateMachine
    {
        protected T _fsm = null;

        public State(T fsm)
        {
            _fsm = fsm;
        }


        public T GetFsm() => _fsm;

        /// <summary>
        /// Process tick update
        /// </summary>
        /// <param name="gameTime">delta time info</param>
        public abstract void Process(GameTime gameTime);
    }
}
