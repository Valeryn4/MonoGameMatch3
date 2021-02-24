using Match3MonoGame.Core.InputEvents;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Match3MonoGame.Core.NodeGraph
{

    /// <summary>
    /// NodeGraph base Node
    /// See: https://en.wikipedia.org/wiki/Node_graph_architecture
    /// And see: https://docs.godotengine.org/ru/stable/classes/class_node.html
    /// </summary>
    public class Node : IDisposable
    {

        /// <summary>
        /// Root or core nodergaph
        /// </summary>
        private Node _root = null;
       
        /// <summary>
        /// Getter setter root.
        /// Call recursive change root to all children
        /// </summary>
        public Node Root 
        {
            get => _root;
            protected set
            {
                _root = value;
                foreach (Node node in Children)
                {
                    node.Root = value;
                }
            } 
        }
        
        /// <summary>
        /// Parent node. if parent null then node in not inside tree
        /// </summary>
        public Node Parent { get; protected set; }

        /// <summary>
        /// Onwer
        /// TODO:
        /// NOT IMPLIMENTATION! Need creating prebuf mini-scene and create resource file from tree.
        /// See: https://docs.godotengine.org/ru/stable/classes/class_node.html#class-node-property-owner
        /// </summary>
        public Node Owner { get; set; }

        /// <summary>
        /// Returned true if this node inside tree and not free/
        /// </summary>
        public bool IsInsideTree { get; private set; }
        
        /// <summary>
        /// Allow call 'Draw(delta)' function
        /// </summary>
        public bool Drawing { get; set; }
        
        /// <summary>
        /// Allow call 'Process(delta)' function
        /// </summary>
        public bool Processing { get; set; }
        /// <summary>
        /// Allow call 'Input(event)' function
        /// </summary>
        public bool InputEnable { get; set; }
        /// <summary>
        /// Stop InputEvent in this Node. Dont call Input to childrens
        /// </summary>
        public bool InputStopped { get; set; }

        /// <summary>
        /// Set collection children. Fast find children.
        /// </summary>
        readonly public HashSet<Node> Children = new HashSet<Node>();

        /// <summary>
        /// async free node
        /// </summary>
        static readonly protected Queue<Node> _queueFree = new Queue<Node>();

        /// <summary>
        /// async add new node to tree
        /// </summary>
        private readonly Queue<Node> _queueAddChild = new Queue<Node>();

        private bool _erased = false;
        
        /// <summary>
        /// Use ONLY in root scene
        /// Asyn free process
        /// </summary>
        /// <param name="_gameTime"></param>
        public static void FreeProcess(GameTime _gameTime)
        {
            while (_queueFree.Count > 0)
            {
                _queueFree.Dequeue().Free();
            }
        }


        public Node()
        {
            Parent = null;
            Owner = null;
            Drawing = false;
            Processing = false;
            InputStopped = false;
            InputEnable = false;
        }


        /// <summary>
        /// Call on Entered tree scene. Call this function pos insideTree
        /// </summary>
        protected virtual void OnEnteredTree()
        {

            Debug.WriteLine($"Node entered tree {this}");
        }

        /// <summary>
        /// Call on exted tree scene. Call this function pos remove tree
        /// </summary>
        protected virtual void OnExitedTree()
        {

            Debug.WriteLine($"Node exit tree {this}");
        }

        /// <summary>
        /// Forse free node. WARRNING! 
        /// </summary>
        public void Free()
        {
            _erased = true;
            foreach (Node child in Children)
            {
                child.Free();
            }
            Children.Clear();

            if (Parent != null)
            {
                if (!Parent._erased)
                    Parent.RemoveChild(this);
            }
            Parent = null;
            Dispose();

        }

        /// <summary>
        /// async free resource. Use at function, please!
        /// </summary>
        public void QueueFree()
        {
            _queueFree.Enqueue(this);
        }


        /// <summary>
        /// Add new node in tree.
        /// The current node will be the parent
        /// </summary>
        /// <param name="child">Node to add tree</param>
        /// <returns></returns>
        public bool AddChild(Node child)
        {

            if (Children.Contains(child) || child.Parent != null)
                return false;
            _queueAddChild.Enqueue(child);
            return true;
        }

        /// <summary>
        /// Async process add to tree
        /// Use in Process tick or another thread
        /// </summary>
        public void AddChildRequrciveProcess()
        {
            while (_queueAddChild.Count > 0)
            {
                var child = _queueAddChild.Dequeue();
                child.Parent = this;
                child.Root = Root;
                Children.Add(child);
                child.IsInsideTree = true;
                child.OnEnteredTree();
            }

            foreach (Node child in Children)
            {
                child.AddChildRequrciveProcess(); 
            }
        }

        /// <summary>
        /// Remove child from this node.
        /// Child node DONT FREE. 
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public bool RemoveChild(Node child)
        {
            if (Children.Remove(child))
            {
                child.Root = null;
                child.Owner = null;
                child.Parent = null;
                child.IsInsideTree = false;
                child.OnExitedTree();
                return true;
            }
            return false;
        }
    
        /// <summary>
        /// Recursive call 'Process' all tree
        /// </summary>
        /// <param name="gameTime"></param>
        public void ProcessRecturcive(GameTime gameTime)
        {
            if (Processing)
                Process(gameTime);
            foreach (Node node in Children)
                node.ProcessRecturcive(gameTime);
        }
        
        /// <summary>
        /// Recursive call 'Draw' all tree
        /// </summary>
        /// <param name="gameTime"></param>
        public void DrawRecurcive(GameTime gameTime)
        {

            if (Drawing)
                Draw(gameTime);
            foreach (Node node in Children)
            {
                node.DrawRecurcive(gameTime);
            }
        }
       
        /// <summary>
        /// Recursive call 'Input' all tree
        /// </summary>
        /// <param name="ev"></param>
        public void InputRecurcive(InputEvent ev)
        {
            if (InputEnable)
                Input(ev);
            if (!InputStopped)
            {
                foreach (Node node in Children)
                {
                    node.InputRecurcive(ev);
                }
            }
        }

        /// <summary>
        /// Process game tick
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void Process(GameTime gameTime)
        {
        }

        /// <summary>
        /// Process game draw (fps tick)
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void Draw(GameTime gameTime)
        {
        }

        /// <summary>
        /// Process input tick
        /// </summary>
        /// <param name="ev"></param>
        protected virtual void Input(InputEvent ev)
        {
        }

        /// <summary>
        /// Analog destructor. Call post free resource.
        /// </summary>
        protected virtual void OnFree()
        {}
        public void Dispose()
        {
            OnFree();
        }
    }
}
