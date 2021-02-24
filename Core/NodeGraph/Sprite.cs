using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.NodeGraph
{
    public class Sprite : Node2D
    {
        public Texture2D Texture { get; set; }
        public bool FlipH { get; set; }
        public bool FlipV { get; set; }
        public Sprite( SpriteBatch spriteBatch) : base(spriteBatch)
        {
            Centred = true;
            Drawing = true;
        }

        protected override void Draw(GameTime gameTime)
        {
            if (Texture != null)
            {
                var spriteEffect = (FlipH ? SpriteEffects.FlipHorizontally : 0) | (FlipV ? SpriteEffects.FlipVertically : 0);
                DrawTexture(
                    Texture,
                    Vector2.Zero,
                    Vector2.Zero,
                    0,
                    1f,
                    spriteEffect,
                    0
                    );
            }
        }

        protected override void OnFree()
        {
            Texture = null;
            base.OnFree();
        }
    }
}
