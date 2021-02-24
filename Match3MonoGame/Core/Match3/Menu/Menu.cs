using Match3MonoGame.Core.Match3.CellGrid;
using Match3MonoGame.Core.NodeGraph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Match3MonoGame.Core.Match3.Menu
{
    public class Menu : Node2D
    {

        public event ShapeButton.Pressed EventPressed;

        private Sprite _panel;
        private Sprite _playButton;
        private Texture2D _pressedTexture;
        public Menu(SpriteBatch spriteBatch, ContentManager content) : base(spriteBatch)
        {
            _panel = new Sprite(spriteBatch)
            {
                Texture = content.Load<Texture2D>("UI/yellow_panel")
            };
            AddChild(_panel);
            _panel.Scale = 3.0f;

            _playButton = new Sprite(spriteBatch)
            {
                Texture = content.Load<Texture2D>("UI/blue_button00")
            };
            AddChild(_playButton);

            _pressedTexture  = content.Load<Texture2D>("UI/green_button00");

            var text = new Label(spriteBatch)
            {
                Text = "PLAY",
                Font = Match3TextureManager.GetDefaultFont()
            };
            _playButton.AddChild(text);

            var pressShape = new ShapeButton(spriteBatch);
            var halfSize = new Vector2(_playButton.Texture.Width * 0.5f, _playButton.Texture.Height * 0.5f);
            pressShape.ShapeRect = new RectangleF(-halfSize.X, -halfSize.Y, _playButton.Texture.Width, _playButton.Texture.Height);
            _playButton.AddChild(pressShape);
            pressShape.EventReleased += OnReleased;
            pressShape.EventPressed += OnPressed;
        }

        private void OnPressed()
        {

            _playButton.Texture = _pressedTexture;
        }

        private void OnReleased()
        {
            Debug.WriteLine("Press PLAY");
            EventPressed?.Invoke();
            Processing = false;
            InputEnable = false;
        }

        protected override void OnEnteredTree()
        {



            base.OnEnteredTree();
        }
    }
}
