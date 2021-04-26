using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MultiRobotExploration.Model
{
    public class EnvironmentBitmap
    {
        private static Color RobotColor => Colors.OrangeRed;
        private static Color TraversableColor => Colors.White;
        private static Color FrontierColor => Colors.LightGray;
        private static Color ObstacleColor => Colors.SaddleBrown;
        private static Color TargetColor => Colors.GreenYellow;
        private static Color UnknownColor => Color.FromRgb(48, 48, 48);
        
        public WriteableBitmap Bitmap { get; }

        public EnvironmentBitmap(int width, int height)
        {
            Bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
        }

        public void DrawPixel(int x, int y, Cell cell)
        {
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
            
            DrawPixel(x, y, color);
        }

        private void DrawPixel(int x, int y, Color color)
        {
            byte[] colorData = {color.B, color.G, color.R, color.A};
            var rect = new Int32Rect(x, y, 1, 1);
            
            Bitmap.WritePixels(rect, colorData, 4, 0);
        }
    }
}