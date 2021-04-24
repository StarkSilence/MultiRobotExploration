using System.Collections.Generic;
using System.Drawing;

namespace MultiRobotExploration.Model
{
    public class Map
    {
        private readonly Cell[,] _Cells;

        public readonly int Width;
        public readonly int Height;

        public readonly HashSet<Point> Frontier;
        
        public Cell this[Point p]
        {
            get => this[p.X, p.Y];
            set => this[p.X, p.Y] = value;
        }

        public Cell this[int x, int y]
        {
            get => _Cells[x, y];
            set
            {
                CheckFrontier(x, y, value);
                _Cells[x, y] = value;
            }
        }

        private void CheckFrontier(int x, int y, Cell val)
        {
            if (this[x, y] == Cell.Frontier)
            {
                Frontier.Remove(new Point(x, y));
            }
            if (val == Cell.Frontier)
            {
                Frontier.Add(new Point(x, y));
            }
        }
        
        public Map(int width, int height)
        {
            _Cells = new Cell[width, height];
            
            Width = width;
            Height = height;
            
            Frontier = new HashSet<Point>();
        }
    }
}