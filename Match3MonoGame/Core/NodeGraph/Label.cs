using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.NodeGraph
{
    public class Label : Node2D
    {
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Label(SpriteBatch spriteBatch) : base(spriteBatch)
        {
            Drawing = true;
            Centred = true;
        }
        protected override void Draw(GameTime gameTime)
        {
            if (Font != null)
                DrawText(
                    Font,
                    Text,
                    Vector2.Zero,
                    Vector2.Zero
                    );
        }

        protected override void OnFree()
        {
            Font = null;
            base.OnFree();
        }
    }
}
