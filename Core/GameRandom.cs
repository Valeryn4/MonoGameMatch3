using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core
{
    public class GameRandom : Random
    {
        private static readonly Random random = new Random();
        public static Random Instance { get => random; }
    }
}
