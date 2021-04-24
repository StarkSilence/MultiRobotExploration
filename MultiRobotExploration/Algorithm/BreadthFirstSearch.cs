using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MultiRobotExploration.Model;

namespace MultiRobotExploration.Algorithm
{
    public static class BreadthFirstSearch
    {
        public static HashSet<Point> GetTilesInRoom(Point point, Map map, HashSet<Point> walls)
        {
            bool Check(Point p) => !walls.Contains(p);

            return Search(point, map, Check);
        }

        public static HashSet<Point> GetTilesInRange(Point point, Map map, double range)
        {
            bool Check(Point p) => Util.DistanceBetween(point, p) < range;

            return Search(point, map, Check);
        }
        
        private static HashSet<Point> Search(Point start, Map map, Func<Point, bool> check)
        {
            var open = new Queue<Point>();
            var closed = new HashSet<Point>();
            open.Enqueue(start);

            while (open.Any())
            {
                var current = open.Dequeue();
                closed.Add(current);
                
                var neighbors = Util.GetCardinalNeighbors(current, map);
                foreach (var neighbor in neighbors)
                {
                    if (!closed.Contains(neighbor) && check(neighbor))
                    {
                        open.Enqueue(neighbor);
                    }
                }
            }
            
            return closed;
        }
    }
}