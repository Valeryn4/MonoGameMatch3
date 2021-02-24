using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.NodeGraph
{
    public class RootNode : Node
    {
        public RootNode() : base()
        {
            Root = this;
        }

        protected override void OnFree()
        {
            Root = null;
            base.OnFree();
        }

        public void Clear()
        {
            foreach (var child in Children)
                child.QueueFree();
        }
    }
}
