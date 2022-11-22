using Launcher;
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
        private int height = 160;

        private FontFamily RobotoBold = new FontFamily(new Uri("file://Fonts/Roboto-Bold.ttf"), "RobotoBold");
        private FontFamily RobotoRegular = new FontFamily(new Uri("file://Fonts/Roboto-Regular.ttf"), "RobotoRegular");

        goConn GoConn = new goConn("127.0.0.1", 8081);

        public MainWindow()
        {
            InitializeComponent();
            LoadServerList();
        }

        private void LoadServerList()
        {
            var test = GoConn.GetServers();
            for (int i = 0; i < test.GetLength(0); i++)
            {
                for (int j = 0; j < test.GetLength(1); j++)
                {
                    MessageBox.Show(test[i, j] + " ");
                }
            }
            LinearGradientBrush GradientBrush = new LinearGradientBrush();
            GradientBrush.StartPoint = new Point(0, 0);
            GradientBrush.EndPoint = new Point(1, 2);

            for (int i = 0; i < 3; i++)
            {
                CreateServerCard(GradientCreator(GradientBrush, Color.FromRgb(91, 195, 255), Color.FromRgb(58, 160, 255)), "WastCraft", 4, 10, "1.19.2", "Ванильный приватный майнкрафт сервер");
            }
        }

        private void CreateServerCard(LinearGradientBrush GradientBrush, string Name, int CurrentPlayers, int MaxPlayer, string Version, string Desc)
        {
            Border bord = new Border()
            {
                Padding = new Thickness(12, 12, 12, 12),
                Margin = new Thickness(0, 0, 0, 20),
                Width = ServersCol.Width.Value - 20,
                Height = height,
                Background = GradientBrush,
                CornerRadius = new CornerRadius(16, 16, 16, 16),
            };
            StackPanel rec = new StackPanel();

            var ServerName = CreateTexBlock(Name, 30, RobotoBold);
            var Players = CreateTexBlock($"{CurrentPlayers} / {MaxPlayer}", 18, RobotoBold);
            var Ver = CreateTexBlock(Version, 18, RobotoBold);

            bord.Child = rec;
            rec.Children.Add(ServerName);
            rec.Children.Add(Players);
            rec.Children.Add(Ver);

            ServerList.Children.Add(bord);
            ServerList.HorizontalAlignment = HorizontalAlignment.Center;
            ServerList.CanVerticallyScroll = true;
        }

        private TextBlock CreateTexBlock(string text, int fontsize, FontFamily font)
        {
            TextBlock value = new TextBlock()
            {
                Text = text,
                FontSize = fontsize,
                Foreground = Brushes.White,
                FontFamily = font,
            };
            return value;
        }

        private LinearGradientBrush GradientCreator(LinearGradientBrush GradientBrush, Color stop1, Color stop2)
        {
            GradientBrush.GradientStops.Add(new GradientStop(stop1, 0.0));
            GradientBrush.GradientStops.Add(new GradientStop(stop2, 1));
            return GradientBrush;
        }

        private void GetServerData()
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
