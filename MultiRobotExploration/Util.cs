using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MultiRobotExploration.Model;

namespace MultiRobotExploration
{
    public static class Util
    {
        public static double DistanceBetween(Point p1, Point p2)
        {
            var dx = p2.X - p1.X;
            var dy = p2.Y - p1.Y;

            return Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
        }

        public static bool InBounds(Point p, Map map)
        {
            return p.X >= 0 && p.Y >= 0 && p.X < map.Width && p.Y < map.Height;
        }
        
        public static bool IsFrontier(Point point, Map map)
        {
            if (!map[point].Is(Cell.Traversable)) return false;

            var neighbors = GetCardinalNeighbors(point, map);
            return neighbors.Any(n => map[n].Is(Cell.Unknown));
        }

        public static bool NearRobotTarget(Point point, List<Robot> robots)
        {
            foreach (var robot in robots)
            {
                if (robot.TargetPosition is Point target && DistanceBetween(point, target) < robot.MaxRange)
                {
                    return true;
                }
            }

            return false;
        }

        public static List<Point> GetCardinalNeighbors(Point p, Map map)
        {
            var neighbors = new List<Point>();

            neighbors.Add(new Point(p.X + 1, p.Y));
            neighbors.Add(new Point(p.X - 1, p.Y));
            neighbors.Add(new Point(p.X, p.Y + 1));
            neighbors.Add(new Point(p.X, p.Y - 1));
            
            return neighbors.Where(n => InBounds(n, map)).ToList();
        }

        public static List<Point> GetNeighbors(Point p)
        {
            var neighbors = new List<Point>();

            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    
                    neighbors.Add(new Point(p.X + dx, p.Y + dy));
                }
            }
            
            return neighbors;
        }
    }
}