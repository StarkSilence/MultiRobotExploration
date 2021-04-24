using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MultiRobotExploration.Algorithm;
using MultiRobotExploration.Model;
using Environment = MultiRobotExploration.Model.Environment;

namespace MultiRobotExploration.Exploration
{
    public static class EnvironmentGen
    {
        private static readonly Random Random = new Random();
        
        public static Environment CreateEnvironmentEmpty(int size, IEnumerable<Robot> robots)
        {
            var env = new Environment(size, size);

            CreateEmpty(env);
            CreateRobots(env, robots);
            
            return env;
        }

        public static Environment CreateEnvironmentWithObstacles(int size, IEnumerable<Robot> robots, double obstacleDensity)
        {
            var env = new Environment(size, size);

            CreateEmpty(env);
            CreateObstacles(env, obstacleDensity);
            CreateRobots(env, robots);
            
            return env;
        }

        private static void CreateEmpty(Environment env)
        {
            for (var x = 0; x < env.Real.Width; x++)
            {
                for (var y = 0; y < env.Real.Height; y++)
                {
                    var cell = Cell.Traversable;

                    if (x == 0 || x == env.Real.Width - 1 || y == 0 || y == env.Real.Height - 1)
                    {
                        cell = Cell.Obstacle;
                    }

                    env.Real[x, y] = cell;
                    env.Known[x, y] = Cell.Unknown;
                }
            }
        }

        private static void CreateObstacles(Environment env, double obstacleDensity)
        {
            var obstacleCount = (int)(env.Real.Width * env.Real.Height * obstacleDensity / 100);

            for (var i = 0; i < obstacleCount; i++)
            {
                CreateObstacle(env);
            }
        }

        private static void CreateObstacle(Environment env)
        {
            var vertices = Random.Next(4) + 1;

            switch (vertices)
            {
                case 1:
                    CreateCircleObstacle(env);
                    break;
                case 2:
                    CreateWallObstacle(env);
                    break;
                case 4:
                    CreateRectangleObstacle(env);
                    break;
                default:
                    CreateShapeObstacle(env, vertices);
                    break;
            }
        }

        private static void CreateCircleObstacle(Environment env, double minRadius = 3, double maxRadius = 6)
        {
            var center = new Point(Random.Next(env.Real.Width), Random.Next(env.Real.Height));
            var radius = Random.NextDouble() * (maxRadius - minRadius) + minRadius;

            var tiles = BreadthFirstSearch.GetTilesInRange(center, env.Real, radius);
            SetTilesAsObstacles(env, tiles);
        }

        private static void CreateWallObstacle(Environment env, double minLength = 5, double maxLength = 10)
        {
            var start = new Point(Random.Next(env.Real.Width), Random.Next(env.Real.Height));
            
            var distance = Random.NextDouble() * (maxLength - minLength) + minLength;
            var direction = Random.NextDouble() * 2 * Math.PI;

            var x = (int)(start.X + distance * Math.Cos(direction));
            var y = (int)(start.Y + distance * Math.Sin(direction));
            var end = new Point(x, y);

            var tiles = Bresenham.TilesInLine(start, end, env.Real);
            SetTilesAsObstacles(env, tiles);
        }

        private static void CreateRectangleObstacle(Environment env, double minLength = 5, double maxLength = 10)
        {
            var length = Random.NextDouble() * (maxLength - minLength) + minLength;
            var lDirection = Random.NextDouble() * 2 * Math.PI;
            
            var width = Random.NextDouble() * (maxLength - minLength) + minLength;
            var wDirection = lDirection - Math.PI / 2;

            var corners = new List<Point>();
            corners.Add(new Point(Random.Next(env.Real.Width), Random.Next(env.Real.Height)));
            corners.Add(GetNextPoint(corners[0], length, lDirection));
            corners.Add(GetNextPoint(corners[1], width, wDirection));
            corners.Add(GetNextPoint(corners[0], width, wDirection));

            var walls = corners.Select((p, i) => Bresenham.TilesInLine(p, corners[i < corners.Count - 1 ? i + 1 : 0], env.Real));
            var wallsSet = walls.SelectMany(w => w).ToHashSet();
            var center = new Point((int)corners.Average(p => p.X), (int)corners.Average(p => p.Y));
            
            var tiles = BreadthFirstSearch.GetTilesInRoom(center, env.Real, wallsSet);
            SetTilesAsObstacles(env, tiles);

            Point GetNextPoint(Point start, double distance, double direction)
            {
                var x = (int)(start.X + distance * Math.Cos(direction));
                var y = (int)(start.Y + distance * Math.Sin(direction));

                return new Point(x, y);
            }
        }

        private static void CreateShapeObstacle(Environment env, int vertices, double minLength = 5, double maxLength = 10)
        {
            var angle = (vertices - 2) * Math.PI / vertices;
            
            var length = Random.NextDouble() * (maxLength - minLength) + minLength;
            var direction = Random.NextDouble() * 2 * Math.PI;
            
            
        }

        private static void SetTilesAsObstacles(Environment env, IEnumerable<Point> tiles)
        {
            foreach (var tile in tiles.Where(tile => Util.InBounds(tile, env.Real)))
            {
                env.Real[tile] = Cell.Obstacle;
            }
        }

        private static void CreateRobots(Environment env, IEnumerable<Robot> robots)
        {
            foreach (var robot in robots)
            {
                env.Robots.Add(robot);

                while (true)
                {
                    var point = new Point(Random.Next(env.Real.Width), Random.Next(env.Real.Height));

                    if (!(env.Real[point].Is(Cell.Obstacle) || env.Real[point].Is(Cell.Robot)))
                    {
                        robot.CurrentPosition = point;
                    
                        env.Known[point] = Cell.Robot;
                        env.Real[point] = Cell.Robot;
                        break;
                    }
                }
            }
        }
    }
}