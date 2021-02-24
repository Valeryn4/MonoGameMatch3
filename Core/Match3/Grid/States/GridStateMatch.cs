using Match3MonoGame.Core.Match3.CellGrid;
using Match3MonoGame.Core.StateMachine;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Match3MonoGame.Core.Match3.Grid.States
{

    /// <summary>
    /// Match result. 
    /// Find bonuses and patterns
    /// </summary>
    public class MatchResult
    {
        public Dictionary<Cell, CellBonus> bonuses = new Dictionary<Cell, CellBonus>();
        public List<List<Cell>> Horizontals = new List<List<Cell>>();
        public List<List<Cell>> Verticals = new List<List<Cell>>();
        
        public void AppendMatchHorizontal(List<Cell> horizontal)
        {
            if (Horizontals.Count == 0)
            {
                Horizontals.Add(horizontal);

            }
            else
            {
                var last = Horizontals[Horizontals.Count - 1];
                var lastCell = last[last.Count - 1];
                var append = false;
                foreach (var cell in horizontal)
                {
                    var cellPointLeftOffset = cell.PosGrid - new Point(-1, 0);
                    if (lastCell.PosGrid == cellPointLeftOffset)
                    {
                        append = true;
                        lastCell = cell;
                        last.Add(cell);

                    }
                }

                if (!append)
                {
                    Horizontals.Add(horizontal);
                }

            }
        }
        public void AppendMatchVerticals(List<Cell> vertical)
        {
            if (Verticals.Count == 0)
            {
                Verticals.Add(vertical);
            }
            else
            {
                var append = false;
                foreach (var v in Verticals)
                {
                    if (append)
                        break;
                    var lastCell = v[v.Count - 1];
                    if (vertical[0].X == lastCell.X)
                    {
                        foreach (var cell in vertical)
                        {
                            var cellPointUpOffset = cell.PosGrid - new Point(0, -1);
                            if (cellPointUpOffset == lastCell.PosGrid)
                            {
                                v.Add(cell);
                                append = true;
                                lastCell = cell;

                            }

                        }
                    }
                }

                if (!append)
                {
                    Verticals.Add(vertical);
                }

            }
        }


        public void CalculateBonuses()
        {
            CalculateBombs();

            CalculateBonusHV(Verticals, CellBonus.VerticalLine);
            CalculateBonusHV(Horizontals, CellBonus.HorizontalLine);
        }

        private void CalculateBonusHV(List<List<Cell>> matchLines, CellBonus bonus)
        {
            foreach (var line in matchLines)
            {
                if (line.Count > 3)
                {
                    Cell cell = null;
                    foreach (var v in line)
                    {
                        if (cell != null)
                            if (v.LastMovedTimestamp < cell.LastMovedTimestamp || v.Bonus != CellBonus.None)
                                continue;
                        cell = v;
                    }

                    if (bonuses.ContainsKey(cell))
                        continue;

                    bonuses.Add(cell, line.Count == 4 ? bonus : CellBonus.Bomb);
                }
            }
        }

        private void CalculateBombs()
        {
            var result = new List<Cell>();
            foreach(var horizontal in Horizontals)
            {
                foreach(var vertocal in Verticals)
                {
                    foreach(var h in horizontal)
                    {
                        foreach(var v in vertocal)
                        {
                            if (h == v)
                            {

                                bonuses.TryAdd(h, CellBonus.Bomb);
                            }
                        }
                    }
                }
            }

        }

        public bool IsEmpty()
        {
            return Horizontals.Count == 0 && Verticals.Count == 0;
        }
        public override string ToString()
        {
            var text = "HORIZONTAL: ";
            foreach (var h in Horizontals)
            {
                text += "{ ";
                foreach (var cell in h)
                {
                    text += $"[{cell.X}:{cell.Y}] ";
                }
                text += "},";
            }
            text += "\nVERTICAL: ";
            foreach (var v in Verticals)
            {
                text += "{ ";
                foreach (var cell in v)
                {
                    text += $"[{cell.X}:{cell.Y}] ";
                }
                text += "},";
            }
            return text;
        }
    }

    /// <summary>
    /// MATCH3 MAGICA!
    /// </summary>
    public class GridStateMatch : State<GridFiniteStateMachine>
    {
        public GridStateMatch(GridFiniteStateMachine fsm) : base(fsm)
        {
            
        }

        /// <summary>
        /// Find all matches and create MatchResult
        /// </summary>
        /// <returns>Macth result object... see up</returns>
        public MatchResult FindMatch()
        {
            var grid = GetFsm().GetGrid();
            var result = new MatchResult();
            for (int x = 0; x < grid.X ; x++)
            {
                for (int y = 0; y < grid.Y; y++)
                {
                    var cell = grid[x, y];
                    if (cell != null)
                    {
                        var type = cell.CellType;
                        if (x > 0 && x < grid.X - 1)
                            if (grid[x - 1, y] != null && grid[x + 1, y] != null)
                                if (grid[x - 1, y].CellType == type && grid[x + 1, y].CellType == type)
                                {
                                    grid[x, y].Matches = true;
                                    grid[x - 1, y].Matches = true;
                                    grid[x + 1, y].Matches = true;
                                    result.AppendMatchHorizontal(new List<Cell>() { grid[x - 1, y], grid[x,y], grid[x + 1, y]});
                                }

                        if (y > 0 && y < grid.Y - 1)
                            if (grid[x, y - 1] != null && grid[x, y + 1] != null)
                                if (grid[x, y - 1].CellType == type && grid[x, y + 1].CellType == type)
                                {
                                    grid[x, y].Matches = true;
                                    grid[x, y - 1].Matches = true;
                                    grid[x, y + 1].Matches = true;
                                    result.AppendMatchVerticals(new List<Cell>() { grid[x, y - 1], grid[x, y], grid[x, y + 1] });
                                }
                    }
                }
            }

            result.CalculateBonuses(); //find bonuses and puch to bonus container
            return result;
        }


        public override void Process(GameTime gameTime)
        {
            var grid = GetFsm().GetGrid();
            var result = FindMatch(); //Macthes

            if (result.IsEmpty())
            {
                GetFsm().PopState(); //match not found.
                return;
            }

            Debug.WriteLine(result.ToString());
            for (int x = 0; x < grid.X; x++)
            {
                for (int y = 0; y < grid.Y; y++)
                {
                    var cell = grid[x, y];
                    if (cell != null)
                    {
                        if (cell.Matches)
                        {
                            //replace cell to bonus 
                            if (result.bonuses.ContainsKey(cell))
                            {
                                cell.Bonus = result.bonuses[cell];
                                cell.Matches = false;
                            }
                            //remove cell
                            else
                            {
                                cell.Destroy();
                                grid[x, y] = null;
                            }
                        }
                    }
                }
            }

            GetFsm().PushState(new GridStateFall(GetFsm()));
            GetFsm().GetGrid().Release();
        }
    }
}
