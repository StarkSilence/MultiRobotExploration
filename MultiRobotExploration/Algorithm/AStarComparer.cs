using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MultiRobotExploration.Algorithm
{
    public class AStarComparer : IComparer<AStarNode>
    {
        public int Compare(AStarNode x, AStarNode y)
        {
            return x.FCost.CompareTo(y.FCost);
        }
    }
}