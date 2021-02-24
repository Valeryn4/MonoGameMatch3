using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.NodeGraph
{
    public class Node2D : CanvasItem
    {
        public Node2D(SpriteBatch spriteBatch) : base(spriteBatch)
        {
        }

        protected override void OnFree()
        {
            base.OnFree();
        }


    }
}
