using System.Collections.Generic;
using System.Drawing;
using MultiRobotExploration.Model;

namespace MultiRobotExploration.Model
{
    public class Environment
    {
        public Map Known { get; set; }
        public Map Real { get; set; }
        public List<Robot> Robots { get; set; }

        public Environment(int width, int height)
        {
            Known = new Map(width, height);
            Real = new Map(width, height);
            Robots = new List<Robot>();
        }
    }
}