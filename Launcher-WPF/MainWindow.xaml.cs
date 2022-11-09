using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Launcher_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadServerList();
        }

        private void LoadServerList()
        {
            int height = 100;


            for (int i = 0; i < 3; i++)
            {
                LinearGradientBrush GradientBrush = new LinearGradientBrush();
                GradientBrush.StartPoint = new Point(0, 0);
                GradientBrush.EndPoint = new Point(1, 2);
                Rectangle rec = new Rectangle()
                {
                    Width = ServersCol.Width.Value - 20,
                    Height = height,
                    Fill = Brushes.Green,
                    RadiusX = 16,
                    RadiusY = 16,
                };
                ServerList.Children.Add(rec);
                ServerList.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
