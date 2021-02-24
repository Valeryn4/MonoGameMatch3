using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.InputEvents
{
    /// <summary>
    /// Impl keyboard version
    /// </summary>
    public class InputEventKey : InputEvent
    {
        public KeyboardState State { get; set; }
    }
}
