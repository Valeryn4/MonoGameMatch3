using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Match3MonoGame.Core.Match3.CellGrid.States
{
    static public class CellStateSemaphore
    {
        static public int MoveCellCount = 0;
        static public int DestroySemaphore = 0;
        static public int ActiveBonuses = 0;
    }
}
