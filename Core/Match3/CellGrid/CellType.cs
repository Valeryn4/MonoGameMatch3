using System;
using System.Collections.Generic;
using System.Text;

namespace Match3MonoGame.Core.Match3.CellGrid
{
    public enum CellBackType
    {
        Green,
        Yellow,
        Blue
    }
    public enum CellType
    {
        Black,
        Blue,
        Green,
        Orange,
        Red,
        Yellow,
        Grey,
        Pink,
    }

    public enum CellBonus
    {
        None,
        VerticalLine,
        HorizontalLine,
        Bomb
    }
}
