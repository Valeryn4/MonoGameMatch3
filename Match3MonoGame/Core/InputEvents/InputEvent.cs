using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.InputEvents
{
    /// <summary>
    /// Base InputEvent struct. 
    /// Used in NodeGraph.Input(InputEvent ev) 
    /// </summary>
    public class InputEvent
    {

        public bool Pressed { get; set; } = false;
        
        public float Strength { get; set; } = 0f; 
    }
}
