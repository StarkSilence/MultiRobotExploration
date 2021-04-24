using System;
using System.Collections.Generic;
using System.Drawing;
using MultiRobotExploration.Model;

namespace MultiRobotExploration.Algorithm
{
    public static class Bresenham
    {
        public static bool IsPointVisible(Point p1, Point p2, Map map)
        {
            if (!Util.InBounds(p1, map) || !Util.InBounds(p2, map)) return false;
            
            var visible = true;
            Func<Point, bool> check = p =>
            {
                if (!map[p].Is(Cell.Obstacle) || p.Equals(p2)) return true;
                
                visible = false;
                return false;
            };
            
            LineCheck(p1, p2, check);
            return visible;
        }

        public static List<Point> TilesInLine(Point p1, Point p2, Map map)
        {
            var tiles = new List<Point>();

            Func<Point, bool> check = p =>
            {
                tiles.Add(p);
                return true;
            };
            
            LineCheck(p1, p2, check);
            return tiles;
        }
        
        private static void LineCheck(Point p1, Point p2, Func<Point, bool> check)
        {
            var dx = Math.Abs(p2.X - p1.X);
            var sx = p1.X < p2.X ? 1 : -1;
            
            var dy = -Math.Abs(p2.Y - p1.Y);
            var sy = p1.Y < p2.Y ? 1 : -1;

            var err = dx + dy;

            while (check(p1) && (p1.X != p2.X || p1.Y != p2.Y))
            {
                var err2 = 2 * err;

                if (err2 >= dy)
                {
                    err += dy;
                    p1.X += sx;
                }
                if (err2 <= dx)
                {
                    err += dx;
                    p1.Y += sy;
                }
            }
        }
    }
}