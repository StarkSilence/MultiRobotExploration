using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MultiRobotExploration.Model;
using MultiRobotExploration.Algorithm;
using Environment = MultiRobotExploration.Model.Environment;

namespace MultiRobotExploration.Exploration
{
    public class Controller
    {
        private Sensor Sensor { get; }

        private readonly Environment Environment;

        private Map Known => Environment.Known;
        private Map Real => Environment.Real;
        private List<Robot> Robots => Environment.Robots;
        private HashSet<Point> Frontier => Known.Frontier;

        public Controller(Environment env)
        {
            Environment = env;
            Sensor = new Sensor(env);
            
            Sensor.UpdateKnownEnvironment();
        }

        public void TakeStep()
        {
            foreach (var robot in Robots)
            {
                if (robot.Finished) continue;
                
                AssignTargetPosition(robot);
                MoveRobot(robot);
                
                Sensor.UpdateKnownEnvironment();
            }
        }
        
        private void AssignTargetPosition(Robot robot)
        {
            if (!robot.Finished && robot.Stopped)
            {
                GetNewPath(robot);
                RemoveFrontierTilesNearTarget(robot);
            }
        }

        private void RemoveFrontierTilesNearTarget(Robot robot)
        {
            if (robot.TargetPosition is Point target)
            {
                var remove = Known.Frontier.Where(p => Util.DistanceBetween(p, target) < robot.MaxRange).ToList();

                foreach (var tile in remove)
                {
                    Known[tile] = Cell.Traversable;
                }
            }
        }

        private void GetNewPath(Robot robot)
        {
            robot.Path = GetPathToBestFrontierCell(robot);

            if (!robot.Path.Any())
            {
                robot.Path = null;
            }

            if (robot.TargetPosition is Point p)
            {
                Real[p] = Cell.Target;
                Known[p] = Cell.Target;
            }
        }

        private Stack<Point> GetPathToBestFrontierCell(Robot robot)
        {
            return AStar.FindPath(robot.CurrentPosition, Frontier, Known);
        }

        private void MoveRobot(Robot robot)
        {
            if (robot.Finished || robot.Stopped) return;
                
            var lastPosition = robot.CurrentPosition;
            var nextPosition = robot.Path.Pop();
                
            Real[lastPosition] = Cell.Traversable;
            Real[nextPosition] = Cell.Robot;
            robot.CurrentPosition = nextPosition;
        }
    }
}