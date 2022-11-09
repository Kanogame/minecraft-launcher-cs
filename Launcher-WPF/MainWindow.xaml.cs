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
            int width = 10;
            int height = 20;
            for (int i = 0; i < 3; i++)
            {
                Rectangle rec = new Rectangle()
                {
                    Width = width,
                    Height = height,
                    Fill = Brushes.Green,
                    Stroke = Brushes.Red,
                    StrokeThickness = 2,
                };

                G
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
