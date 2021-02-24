using Match3MonoGame.Core.NodeGraph;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Match3MonoGame.Core.Match3.CellGrid;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Match3MonoGame.Core.Match3.Grid.States;
using Match3MonoGame.Core.StateMachine;
using Match3MonoGame.Core.Match3.CellGrid.States;

namespace Match3MonoGame.Core.Match3.Grid
{

    /// <summary>
    /// Data gid info from match board game
    /// </summary>
    public class GridData : ICloneable
    {
        private List<Cell> _grid;
        private int _x;
        private int _y;

        public int X { get => _x; }
        public int Y { get => _y; }

        public Cell this[int x, int y]
        {
            get => _grid[y * X + x];
            set
            {
                _grid[y * X + x] = value;
            }
        }
        public GridData(int x, int y)
        {
            _x = x;
            _y = y;
            _grid = new List<Cell>(x * y);
            for (int i = 0; i < x * y; i++)
                _grid.Add(null);
        }

        public GridData(GridData copy)
        {
            _x = copy.X;
            _y = copy.Y;
            _grid = new List<Cell>(copy._grid);
        }
        public object Clone() => new GridData(this);
        public void Clear() => _grid.Clear();
    }
    
    /// <summary>
    /// implimentation grid board game
    /// </summary>
    public class Grid : Node2D
    {
        private delegate void ReleaseSelection();
        private event ReleaseSelection EventReleaseSelection; // call release select

        public delegate void Finished(int score);
        public event Finished EventFinished; // call end game

        // consts and MAGIC NUMBERS
        private const int IntervalPixel = 10;
        public const int CountVariantCell = 5;
        public const float ScaleBackgroundGridCell = 0.7f;
        private const float HalfMulti = 0.5f;
        
        private GridData _grid;
        private Vector2 _cellSize = Vector2.Zero;
        private Vector2 _sizeGridPixel = Vector2.Zero;
        private Vector2 _halfCellSize = Vector2.Zero;
        private Vector2 _halfSizeGridPixel = Vector2.Zero;

        private Cell _fistPressed = null;
        private Cell _secondPressed = null;
        private GridFiniteStateMachine _fsm = null;

        /// <summary>
        /// Draw game state machine if debug dont null
        /// </summary>
        public Label DebugLabel { get; set; }
        
        /// <summary>
        /// Current score
        /// </summary>
        public int Score { get; set; }
        
        /// <summary>
        /// Game time limit 
        /// </summary>
        public int Time { get; set; } = 60;
       
        //---------- GETTERS AND SATTER SIZE GRID
        public Size SizeGrid { get; private set; }
        public int X { get => SizeGrid.Width; }
        public int Y { get => SizeGrid.Height; }

        //---- crutch :(
        public bool IsFirstSpawn { get; set; }

        //getter cell from grid, by index [x,y]
        public Cell this[int x, int y]
        {
            get => _grid[x, y];
            set
            {
                _grid[x, y] = value;
            }
        }

        
        private Node2D _nodeBackground; //bacground tiles core
        private Node2D _nodeCells; //cell tile cores
        private Label _infoLabel; //draw time and score
        private Timer _gameTimer; //game timer loop

        
        readonly private int _countVariantCell = Math.Min(Enum.GetNames(typeof(CellType)).Length, CountVariantCell);

        /// <summary>
        /// Create grid node. Grid is scenter game board!!!
        /// </summary>
        /// <param name="spriteBatch">Draw sprite butch from MonoGame</param>
        /// <param name="size">size game board</param>
        public Grid(SpriteBatch spriteBatch, Size size) : base(spriteBatch)
        {
            SizeGrid = size;
            var capacity = size.Width * size.Height;
            _grid = new GridData(size.Width, size.Height);

            _fsm = new GridFiniteStateMachine(this);
            _fsm.EventChangeState += OnChangeState;
            _fsm.PushState(new GridStateStand(_fsm));
            Processing = true;
        }

        /// <summary>
        /// Convert grid cods to local world Position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector2 GetPositionCell(int x, int y)
        {
            var pos = new Vector2(x * _cellSize.X, y * _cellSize.Y) + _halfCellSize;
            pos -= _halfSizeGridPixel;
            return pos;
        }

        protected override void OnEnteredTree()
        {

            base.OnEnteredTree();

            _nodeBackground = new Node2D(GetSpriteBatch());
            AddChild(_nodeBackground);

            _nodeCells = new Node2D(GetSpriteBatch());
            AddChild(_nodeCells);

            InitBackground();

            //delay first spawn
            var timerDelay = new Timer()
            {
                Autostart = true,
                Oneshot = true,
                WaitTime = 1.0f,
            };
            AddChild(timerDelay);

            //game loop timer
            _gameTimer = new Timer()
            {
                Autostart = false,
                Oneshot = true,
                WaitTime = Time
            };
            AddChild(_gameTimer);
            _gameTimer.EventTimeout += OnFinished;

            timerDelay.EventTimeout += FirstSpawn;
            timerDelay.EventTimeout += timerDelay.QueueFree; //free some this

            _infoLabel = new Label(_spriteBatch)
            {
                Font = Match3TextureManager.GetDefaultFont(),
                Centred = false,
                Scale = 1.5f,
                Text = "TIME:\nSCORE:\n"
            };
            Parent.AddChild(_infoLabel);
        }

        private void FirstSpawn()
        {
            IsFirstSpawn = true;
            _fsm.PushState(new GridStateSpawn(_fsm));
            _gameTimer.Start();
        }
        
        private void InitBackground()
        {
            var bgTexture = Match3TextureManager.GetTextureBackgroundTile();
            _cellSize = new Vector2(bgTexture.Width * ScaleBackgroundGridCell, bgTexture.Height * ScaleBackgroundGridCell);
            _cellSize.X += IntervalPixel;
            _cellSize.Y += IntervalPixel;
            _halfCellSize = _cellSize * HalfMulti;
            var widthGridPixel = _cellSize.X * X;
            var heightGridPixel = _cellSize.Y * Y;
            _sizeGridPixel = new Vector2(widthGridPixel, heightGridPixel);
            _halfSizeGridPixel = _sizeGridPixel * HalfMulti;

            for (int x = 0; x < X; x++)
            {
                for (int y = 0; y < Y; y++)
                {
                    var pos = GetPositionCell(x, y);
                    var backgroundTile = new CellBackground(GetSpriteBatch())
                    {
                        X = x,
                        Y = y,
                        Position = pos,
                        Texture = bgTexture,
                        Centred = true,
                        Scale = ScaleBackgroundGridCell
                    };
                    backgroundTile.EventPressCell += OnPressCell;
                    EventReleaseSelection += backgroundTile.OnReleaseSelection;

                    _nodeBackground.AddChild(backgroundTile);
                    Debug.WriteLine($"add node to position: x:{_nodeBackground.Position.X} y:{_nodeBackground.Position.Y}");

                }
            }
        }
        
        public CellType GetRandomType() => (CellType)GameRandom.Instance.Next(_countVariantCell);
        
        //set random cell to position grid
        public void FillCell(int x, int y)
        {
            var pos = GetPositionCell(x, y);
            var typeCell = GetRandomType();
            var cell = new Cell(GetSpriteBatch(), (CellType)typeCell, this)
            {
                Position = pos,
                X = x,
                Y = y
            };
            _nodeCells.AddChild(cell);
            this[x, y] = cell;
            cell.Position = new Vector2(cell.Position.X, -(_sizeGridPixel.Y));
        }

        //get cells parent node (i need :()
        public Node2D GetNodeCells() => _nodeCells;


        private void OnFinished()
        {
            Processing = false; //Stop game process. All game FREEZ!
            
            EventFinished?.Invoke(Score); 
        }

        // Call click cell game 
        private void OnPressCell(int x, int y, CellBackground cell)
        {
            Debug.WriteLine($"Press cell [{x}:{y}]");
            if (!(_fsm.CurrentState() is GridStateStand))
                return;
            if (_fistPressed == null)
            {
                _fistPressed = this[x, y];
                cell.Select = CellBackground.SelectType.FirstSelect;
            }
            else if (_secondPressed == null)
            {
                var second = this[x, y];
                if (second != _fistPressed)
                {
                    var vertical = Math.Abs(_fistPressed.X - x);
                    var horizontal = Math.Abs(_fistPressed.Y - y);
                    var sum = vertical + horizontal;
                    if (sum == 1)
                    {

                        _secondPressed = this[x, y];
                        cell.Select = CellBackground.SelectType.SecondSelect;
                        Swap();
                    }
                    else
                    {
                        Release();
                    }
                }
            }
        }

        // swap firsh and second objects
        private void Swap()
        {
            _fsm.PushState(new GridStateSwap(_fsm, _fistPressed, _secondPressed));
        }

        // inside cell to grid, remplace current cell 
        public void SetCell(Cell cell)
        {
            this[cell.X, cell.Y] = cell;
        }

        // check is selected pair
        public bool SwapSelected()
        {
            return _fistPressed != null && _secondPressed != null;

        }

        //free selected pair
        public void Release()
        {
            Debug.WriteLine("Release");
            _fistPressed = null;
            _secondPressed = null;
            EventReleaseSelection?.Invoke();
        }

        //DEPERDICATE!
        public GridData GetCopyGridData()
        {
            return _grid.Clone() as GridData;
        }

        // call from state machine event
        private void OnChangeState(IState state)
        {
            Debug.WriteLine($"Change state to {state}");

          
        }

        protected override void Process(GameTime gameTime)
        {
            _fsm.Process(gameTime); // to FSM

            if (DebugLabel != null) // DEBUG STATE MEM
            {
                string text = "";
                foreach (var state in _fsm.GetMem())
                    text += $"{state.GetType().Name.Split("State")[1]}\n";
                text += $"FALL: {CellStateSemaphore.MoveCellCount}\n";
                text += $"BONUS: {CellStateSemaphore.ActiveBonuses}\n";
                DebugLabel.Text = text;
            }

            if (_infoLabel != null) // DRAW INFO IN GAME
            {
                _infoLabel.Text = $"TIME: {(int)(_gameTimer.WaitTime - _gameTimer.LeftTime)}\n" +
                    $"SCORE:{Score}";
            }
        }

        protected override void OnFree()
        {
            EventReleaseSelection = null;
            _grid.Clear();
            _grid = null;
            _fistPressed = null;
            _secondPressed = null;
            _fsm = null;
            base.OnFree();
        }

    }
}
