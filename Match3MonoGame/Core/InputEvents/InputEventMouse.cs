using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.InputEvents
{

    /// <summary>
    /// Impl mouse verion
    /// </summary>
    public class InputEventMouse : InputEvent
    {
        public Vector2 Position { get; set; }
        public MouseState State { get; set; }
    }
}
