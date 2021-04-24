using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using C5;
using MultiRobotExploration.Model;

namespace MultiRobotExploration.Algorithm
{
    public static class AStar
    {
        public static Stack<Point> FindPath(Point start, System.Collections.Generic.HashSet<Point> goals, Map map)
        {
            if (!goals.Any()) return null;
            AStarNode goal = null;
            
            var open = new IntervalHeap<AStarNode>(new AStarComparer());
            var closed = new C5.HashSet<Point>();
            
            var handles = new Dictionary<Point, IPriorityQueueHandle<AStarNode>>();

            open.Add(new AStarNode(null, start, 0, GetHCost(start)));
            while (open.Any())
            {
                var current = open.DeleteMin();
                closed.Add(current.Position);

                if (goals.Contains(current.Position))
                {
                    goal = current;
                    break;
                }

                var neighbors = Util.GetCardinalNeighbors(current.Position, map);
                foreach (var neighbor in neighbors)
                {
                    if (closed.Contains(neighbor) || map[neighbor].Is(Cell.Obstacle)) continue;

                    var gCost = current.GCost + Util.DistanceBetween(current.Position, neighbor);
                    
                    if (handles.TryGetValue(neighbor, out var handle) && open.Find(handles[neighbor], out var other))
                    {
                        handle = null;
                        if (other.GCost < gCost) continue;
                    }
                    
                    var hCost = GetHCost(current.Position);
                    var node = new AStarNode(current, neighbor, gCost, hCost);

                    if (handles.ContainsKey(neighbor))
                    {
                        handle = handles[neighbor];
                        handles.Remove(neighbor);

                        open.Delete(handle);
                        handle = null;
                    }

                    open.Add(ref handle, node);
                    handles.Add(neighbor, handle);
                }
            }

            var path = new Stack<Point>();
            while (goal?.Previous != null)
            {
                path.Push(goal.Position);
                goal = goal.Previous;
            }
            
            return path;

            double GetHCost(Point p)
            {
                return goals.Min(g => Util.DistanceBetween(g, p));
            }
        }
    }
}