

using System.Collections.Generic;
using System.Diagnostics;
using Match3MonoGame.Core.InputEvents;
using Match3MonoGame.Core.Match3;
using Match3MonoGame.Core.Match3.CellGrid;
using Match3MonoGame.Core.Match3.GameOver;
using Match3MonoGame.Core.Match3.Grid;
using Match3MonoGame.Core.Match3.Menu;
using Match3MonoGame.Core.NodeGraph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Match3MonoGame
{
    public class Match3PrototypeGame : Game
    {
        private float _aspect = 0.3f; //Viweport aspect factor
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //ROOT SCENE GRAPH
        private RootNode _root = null;

        public Match3PrototypeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        //DoLoading resource. Init
        protected override void Initialize()
        {

            _root = new RootNode();
            base.Initialize();
           
        }

        private void LoadGame()
        {
            var viweportSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            var scaleviweport = (viweportSize / _aspect);
            var grid = new Grid(_spriteBatch, new Size(8, 8));
            grid.Position = scaleviweport * 0.5f;
            grid.EventFinished += OnGameFinished; //Event to gameover
            _root.AddChild(grid);
        }

        private void LoadMenu()
        {
            var viweportSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            var scaleviweport = (viweportSize / _aspect);
            var menu = new Menu(_spriteBatch, Content);
            menu.Scale = 5.0f;
            menu.Position = scaleviweport * 0.5f;
            menu.EventPressed += OnPressPlay; //Event to game
            _root.AddChild(menu);
        }


        private void LoadGameover(int score)
        {
            var viweportSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            var scaleviweport = (viweportSize / _aspect);
            var menu = new GameOver(_spriteBatch, Content, score);
            menu.Scale = 5.0f;
            menu.Position = scaleviweport * 0.5f;
            menu.EventPressed += OnGameOverOK; //Event to menu
            _root.AddChild(menu);
        }

        private void OnPressPlay()
        {
            _root.Clear();
            LoadGame();

        }
        
        private void OnGameFinished(int score)
        {
            _root.Clear();
            LoadGameover(score);
        }

        private void OnGameOverOK()
        {
            _root.Clear();
            LoadMenu();
        }

        //Load resource and content
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Match3TextureManager.LoadTextures(Content);

            LoadMenu();

            base.LoadContent();
        }


        //Tick update in game
        protected override void Update(GameTime gameTime)
        {
            var gamepadState = GamePad.GetState(PlayerIndex.One);
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
          
            if (gamepadState.Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            // Create InputEvent and parse to tree
            var mousePressed = (
                    mouseState.LeftButton == ButtonState.Pressed ||
                    mouseState.RightButton == ButtonState.Pressed ||
                    mouseState.MiddleButton == ButtonState.Pressed
                    );
            var inputMouse = new InputEventMouse()
            {
                Position = new Vector2(mouseState.X, mouseState.Y) / _aspect,
                Pressed = mousePressed,
                State = mouseState,
                Strength = mousePressed ? 1f : 0f
            };

            var keyPressed = keyboardState.GetPressedKeys().Length > 0;
            var inputKeybard = new InputEventKey()
            {
                Pressed = keyPressed,
                Strength = keyPressed ? 1f : 0f,
                State = keyboardState
            };


            //Recurcive process handle
            _root.ProcessRecturcive(gameTime);
            _root.InputRecurcive(inputKeybard);
            _root.InputRecurcive(inputMouse);
            _root.AddChildRequrciveProcess();
            Node.FreeProcess(gameTime);

            base.Update(gameTime);
        }

        //Tick draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var matrix = Matrix.CreateScale(_aspect);
            _spriteBatch.Begin(transformMatrix: matrix);
            _root.DrawRecurcive(gameTime);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
