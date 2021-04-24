using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MultiRobotExploration.Model;

namespace MultiRobotExploration.Algorithm
{
    public static class DepthLimitedSearch
    {
        public static List<Point> GetVisibleTiles(Robot robot, Map map)
        {
            var open = new Stack<Point>();
            var closed = new HashSet<Point>();
            open.Push(robot.CurrentPosition);

            while (open.Any())
            {
                var current = open.Pop();
                closed.Add(current);

                var neighbors = Util.GetCardinalNeighbors(current, map);
                foreach (var neighbor in neighbors)
                {
                    if (!IsValid(robot.CurrentPosition, neighbor)) continue;

                    open.Push(neighbor);
                }
            }

            return closed.ToList();

            bool IsValid(Point robotPos, Point next)
            {
                return !closed.Contains(next) && Util.DistanceBetween(robotPos, next) <= robot.MaxRange && Bresenham.IsPointVisible(robotPos, next, map);
            }
        }
    }
}