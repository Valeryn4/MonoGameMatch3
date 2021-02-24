using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.NodeGraph
{
    /// <summary>
    /// Simple timer
    /// </summary>
    public class Timer : Node
    {
        public delegate void Timeout();
        /// <summary>
        /// Call timeout event
        /// </summary>
        public event Timeout EventTimeout;


        public float WaitTime { get; set; } = 1f;
        public bool Oneshot { get; set; } = false;
        public bool Autostart { get; set; } = false;

        public float LeftTime { get; private set; } = 0f;
        public Timer() : base()
        {
        }

        protected override void OnEnteredTree()
        {
            if (Autostart)
                Start();
            else
                Stop();
            base.OnEnteredTree();
        }
        public void Start()
        {
            Processing = true;
        }

        public void Stop()
        {
            Processing = false;
        }


        protected override void OnFree()
        {
            EventTimeout = null;
            base.OnFree();
        }
        protected override void Process(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            LeftTime += delta;
            if (LeftTime >= WaitTime)
            {
                EventTimeout.Invoke();
                LeftTime = 0;
                if (Oneshot)
                {
                    Processing = false;
                }
            }
        }

    }
}
