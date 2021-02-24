using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Match3MonoGame.Core.StateMachine
{

    /// <summary>
    /// Simple FiniteStateMachine
    /// </summary>
    public class FiniteStateMachine
    {
        public delegate void ChangeState(IState state);
        public event ChangeState EventChangeState; //Emit event state change. Use parse swintch (state) { case MyState : { break;}


        private readonly Stack<IState> _stateMem = new Stack<IState>();
        public FiniteStateMachine()
        {
        }
        
        //Pop current state
        public IState PopState()
        {
            EventChangeState?.Invoke(CurrentState());
            return _stateMem.Pop();
        }

        //Push and set current state 
        public void PushState(IState state)
        {

            _stateMem.Push(state);
            EventChangeState?.Invoke(CurrentState());
        }

        //Return current state
        public IState CurrentState()
        {
            return _stateMem.Peek();
        }

        public IState[] GetMem()
        {
            return _stateMem.ToArray();
        }

        public void Process(GameTime delta)
        {
            if (_stateMem.Count > 0)
                CurrentState().Process(delta);
        }
    }
}
