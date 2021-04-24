using System;

namespace MultiRobotExploration.Model
{
    [Flags]
    public enum Cell
    {
        Robot = 1,
        Traversable = 1 << 1,
        Frontier = Traversable + (1 << 2),
        Obstacle = 1 << 3,
        Unknown = Obstacle + (1 << 4),
        Target = 1 << 5
    }
}