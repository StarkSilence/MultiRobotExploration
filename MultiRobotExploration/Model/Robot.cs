using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MultiRobotExploration.Model
{
    public class Robot
    {
        private static int _RobotCount = 0;
        
        public int Id { get; }
        public Point CurrentPosition { get; set; }
        public Point? TargetPosition => !Finished && !Stopped ? (Point?)Path.Last() : null;
        public double MaxRange { get; set; }
        public Stack<Point> Path { get; set; }
        
        public bool Finished => Path == null;
        public bool Stopped => Path != null && !Path.Any();

        public Robot(Point currentPosition, double maxRange)
        {
            Id = _RobotCount++;
            
            Path = new Stack<Point>();
            CurrentPosition = currentPosition;
            MaxRange = maxRange;
        }
    }
}