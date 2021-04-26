using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MultiRobotExploration.ViewModel;

namespace MultiRobotExploration.View
{
    public partial class MainWindow : Window
    {
        public int AutoStepInterval { get; set; } = 100;
        
        public readonly MainWindowViewModel ViewModel = new MainWindowViewModel();
        
        public MainWindow()
        {
            InitializeComponent();
            
            RenderOptions.SetBitmapScalingMode(MainEnvironment, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(MainEnvironment, EdgeMode.Aliased);
            
            MainEnvironment.Source = ViewModel.EnvironmentBitmap.Bitmap;
            DataContext = ViewModel;
        }

        private void TakeStepButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.TakeStep();
        }

        private void CreateEnvButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.CreateNewEnvironment();
            MainEnvironment.Source = ViewModel.EnvironmentBitmap.Bitmap;
        }

        private async void AutoStepButton_OnClick(object sender, RoutedEventArgs e)
        {
            while (sender is ToggleButton button && button.IsChecked.HasValue && button.IsChecked.Value)
            {
                TakeStepButton_OnClick(null, null);

                await Task.Factory.StartNew(() => Thread.Sleep(AutoStepInterval));
            }
        }

        private void RobotCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                SetTextBoxToSliderLimit(textBox, RobotCountSlider);
            }
        }

        private void MapSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                SetTextBoxToSliderLimit(textBox, MapSizeSlider);
            }
        }

        private void RobotMaxRange_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                SetTextBoxToSliderLimit(textBox, RobotMaxRangeSlider);
            }
        }

        private void ObstacleDensity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                SetTextBoxToSliderLimit(textBox, ObstacleDensitySlider);
            }
        }

        private void SetTextBoxToSliderLimit(TextBox textBox, Slider slider)
        {
            if (int.TryParse(textBox.Text, out var value))
            {
                value = Math.Max((int)slider.Minimum, value);
                value = Math.Min((int)slider.Maximum, value);

                textBox.Text = value.ToString();
            }
        }

        private void MoveFocusOnEnter(object sender, KeyEventArgs e)
        {
            if (sender is Control control && e.Key == Key.Enter)
            {
                control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void DrawReal_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggle)
            {
                ViewModel.DrawReal = toggle.IsChecked.HasValue && toggle.IsChecked.Value;
                ViewModel.UpdateEnvironment();
            }
        }
    }
}