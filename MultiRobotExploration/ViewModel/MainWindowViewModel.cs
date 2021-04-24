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
        private static Color RobotColor => Colors.OrangeRed;
        private static Color TraversableColor => Colors.White;
        private static Color FrontierColor => Colors.LightGray;
        private static Color ObstacleColor => Colors.SaddleBrown;
        private static Color UnknownColor => Color.FromRgb(48, 48, 48);
        private static Color TargetColor => Colors.GreenYellow;
        
        public WriteableBitmap EnvironmentBmp { get; private set; }
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
            
            EnvironmentBmp = new WriteableBitmap(EnvironmentSize, EnvironmentSize, 96, 96, PixelFormats.Bgr32, null);
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
                    var cell = map[x, y];

                    var color = cell switch
                    {
                        Cell.Robot => RobotColor,
                        Cell.Traversable => TraversableColor,
                        Cell.Frontier => FrontierColor,
                        Cell.Obstacle => ObstacleColor,
                        Cell.Unknown => UnknownColor,
                        Cell.Target => TargetColor,
                        _ => Colors.MediumPurple
                    };

                    DrawPixel(color, x, y);
                }
            }
        }

        private void DrawPixel(Color color, int x, int y)
        {
            byte[] colorData = {color.B, color.G, color.R, color.A};
            var rect = new Int32Rect(x, y, 1, 1);
            
            EnvironmentBmp.WritePixels(rect, colorData, 4, 0);
        }
    }
}