using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MultiRobotExploration.Model;
using MultiRobotExploration.Algorithm;
using Environment = MultiRobotExploration.Model.Environment;

namespace MultiRobotExploration.Exploration
{
    public class Sensor
    {
        private readonly Environment Environment;

        private Map Known => Environment.Known;
        private Map Real => Environment.Real;
        private List<Robot> Robots => Environment.Robots;
        private HashSet<Point> Frontier => Known.Frontier;
        
        public Sensor(Environment env)
        {
            Environment = env;
        }

        public void UpdateKnownEnvironment()
        {
            foreach (var robot in Robots)
            {
                var visibleTiles = DepthLimitedSearch.GetVisibleTiles(robot, Real);

                visibleTiles.ForEach(t => Known[t] = Real[t]);
                visibleTiles.ForEach(UpdateTile);
            }
        }

        private void UpdateTile(Point point)
        {
            if (IsFrontier(point))
            {
                Known[point] = Cell.Frontier;
            }
            
            UpdateNeighbors(point);
        }

        private void UpdateNeighbors(Point point)
        {
            var neighbors = Util.GetCardinalNeighbors(point, Known);
            foreach (var neighbor in neighbors.Where(n => Known[n].Is(Cell.Traversable)))
            {
                if (IsFrontier(neighbor))
                {
                    Known[neighbor] = Cell.Frontier;
                }
                else
                {
                    Known[neighbor] = Cell.Traversable;
                }
            }
        }

        private bool IsFrontier(Point point)
        {
            return Util.IsFrontier(point, Known) && !Util.NearRobotTarget(point, Robots);
        }
    }
}