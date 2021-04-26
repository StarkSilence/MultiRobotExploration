using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MultiRobotExploration.Exploration;
using MultiRobotExploration.Model;
using Color = System.Windows.Media.Color;
using Environment = MultiRobotExploration.Model.Environment;
using Point = System.Drawing.Point;

namespace MultiRobotExploration.ViewModel
{
    public class MainWindowViewModel
    {
        public EnvironmentBitmap EnvironmentBitmap { get; private set; }
        public bool DrawReal { get; set; }
        
        public int EnvironmentSize { get; set; } = 100;
        public int RobotCount { get; set; } = 3;
        public double RobotMaxRange { get; set; } = 5;
        public double ObstacleDensity { get; set; } = 2;

        private Controller Controller;
        private Environment Environment;

        public MainWindowViewModel()
        {
            CreateNewEnvironment();
        }

        public void CreateNewEnvironment()
        {
            var robots = Enumerable.Range(0, RobotCount).Select(i => new Robot(Point.Empty, RobotMaxRange));

            EnvironmentBitmap = new EnvironmentBitmap(EnvironmentSize, EnvironmentSize);
            Environment = EnvironmentGen.CreateEnvironmentWithObstacles(EnvironmentSize, robots, ObstacleDensity);
            Controller = new Controller(Environment);
            
            UpdateEnvironment();
        }

        public void TakeStep()
        {
            Controller.TakeStep();
            UpdateEnvironment();
        }

        public void UpdateEnvironment()
        {
            DrawEnvironment(DrawReal ? Environment.Real : Environment.Known);
        }

        private void DrawEnvironment(Map map)
        {
            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    EnvironmentBitmap.DrawPixel(x, y, map[x, y]);
                }
            }
        }
    }
}