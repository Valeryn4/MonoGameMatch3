using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.NodeGraph
{
    /// <summary>
    /// Base draw node.
    /// </summary>
    public class CanvasItem : Node
    {


        protected SpriteBatch _spriteBatch = null;


        private Vector2 _position = Vector2.Zero;
        public Vector2 Position
        {
            get => _position;
            set { _position = value; }

        }
        public Vector2 GlobalPosition 
        {
            get
            {
                if (Parent == null)
                    return _position;
                if (Parent is CanvasItem)
                {
                    var parent = Parent as CanvasItem;
                    return parent.GlobalPosition + _position;
                }
                return _position;
            }
        }

        
        private float _rotation = 0f;
        public float Rotation 
        {
            get => _rotation;
            set { _rotation = value; }
        }
        public float GlobalRotation 
        {
            get
            {
                if (Parent == null)
                    return Rotation;

                if (Parent is CanvasItem)
                {
                    var parent = Parent as CanvasItem;
                    return parent.GlobalRotation + _rotation;
                }
                return _rotation;
            }
        }

        
        private float _scale = 1f;
        public float Scale 
        { 
            get => _scale;
            set { _scale = value; }
        }
        public float GlobalScale
        {
            get
            {
                if (Parent == null)
                    return Scale;
                if (Parent is CanvasItem)
                {
                    var parent = Parent as CanvasItem;
                    return parent.GlobalScale * _scale;
                }
                return Scale;
            }
        }
        public bool Centred { get; set; } = false;
        public Color Modulate { get; set; } = Color.White;
        public int LayerDepth { get; set; } = 0;
        public CanvasItem(SpriteBatch spriteBatch) : 
            base()
        {
            _spriteBatch = spriteBatch;
            Centred = false;
            Modulate = Color.White;
            LayerDepth = 0;
        }

        public SpriteBatch GetSpriteBatch() => _spriteBatch;


        protected override void OnEnteredTree()
        {
            Position = _position;
            Scale = _scale;
            Rotation = _rotation;
            base.OnEnteredTree();
        }

        public void DrawTexture(Texture2D texture, Vector2 pos, Vector2 origin, float rot = 0, float scale = 1f, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0)
        {
            var origin_global = Centred ? new Vector2(texture.Width * 0.5f, texture.Height * 0.5f) : Vector2.Zero;
            _spriteBatch.Draw(
                texture,
                pos + GlobalPosition,
                null,
                Modulate,
                rot + GlobalRotation,
                origin_global + origin,
                scale * GlobalScale,
                effects,
                LayerDepth + layerDepth
            );
        }

        public void DrawText(SpriteFont spriteFont, string text, Vector2 position, Vector2 origin, float rotation = 0f, float scale = 1f, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0)
        {
            var size = spriteFont.MeasureString(text);
            var origin_global = Centred ? size * 0.5f : Vector2.Zero;
            _spriteBatch.DrawString(
                spriteFont,
                text,
                position + GlobalPosition,
                Modulate,
                rotation + GlobalRotation,
                origin_global + origin,
                scale * GlobalScale,
                effects,
                LayerDepth + layerDepth
            );
        }

        protected override void OnFree()
        {
            _spriteBatch = null;
            base.OnFree();
        }
    }
}
