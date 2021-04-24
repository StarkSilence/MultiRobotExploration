using System;
using System.Drawing;

namespace MultiRobotExploration.Algorithm
{
    public class AStarNode
    {
        public Point Position { get; }
        public AStarNode Previous { get; set; }
        public double GCost { get; set; }
        public double HCost { get; set; }
        public double FCost => GCost + HCost;

        public AStarNode(AStarNode previous, Point pos, double gCost, double hCost)
        {
            Previous = previous;
            Position = pos;
            GCost = gCost;
            HCost = hCost;
        }

        public override bool Equals(object obj)
        {
            if (obj is AStarNode other)
            {
                return Position.Equals(other.Position);
            }
            
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position);
        }
    }
}