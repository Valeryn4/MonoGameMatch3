using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.StateMachine
{
    public interface IState 
    {
        public void Process(GameTime gameTime);
    }
}
