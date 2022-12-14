using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using BackendCommon;
using System.Xml.Linq;

namespace Launcher_WPF
{
    internal class CreateCard
    {
        private int height = 160;

        private string TextState;

        private Colors Cols;

        private FontFamily RobotoBold = new FontFamily(new Uri("file://Fonts/Roboto-Bold.ttf"), "RobotoBold");
        private FontFamily RobotoRegular = new FontFamily(new Uri("file://Fonts/Roboto-Regular.ttf"), "RobotoRegular");

        private StackPanel ServerList;
        private StackPanel Images;
        private StackPanel Text;
        private ColumnDefinition ServersCol;

        public CreateCard(StackPanel ServerList, ColumnDefinition ServersCol, StackPanel Images, StackPanel Text)
        {
            this.ServersCol = ServersCol;
            this.ServerList = ServerList;
            this.Images = Images;
            this.Text = Text;
        }

        public void InitCards(string[,] ServersList)
        {
            TextState = ServersList[0, 0];
            Cols = new Colors();
            var gradientColors = Cols.GetColor();
            for (int i = 0; i < ServersList.GetLength(0); i++)
            {
                CreateServerCard(GradientCreator(gradientColors[0], gradientColors[1]),
                    ServersList[i, 0],
                    ServersList[i, 1],
                    ServersList[i, 2],
                    ServersList[i, 3]);
            }
            StateManager(ServersList);
        }

        public void CreateServerCard(LinearGradientBrush GradientBrush, string Name, string ip, string Version, string Desc)
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
            var Players = CreateTexBlock(ip, 18, RobotoBold);
            var Ver = CreateTexBlock(Version, 18, RobotoBold);
            var Descr = CreateTexBlock(Desc, 15, RobotoRegular);

            bord.Child = rec;
            rec.Children.Add(ServerName);
            rec.Children.Add(Players);
            rec.Children.Add(Ver);
            rec.Children.Add(Descr);

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

        public LinearGradientBrush GradientCreator(Color stop1, Color stop2)
        {
            LinearGradientBrush GradientBrush = new LinearGradientBrush();
            GradientBrush.StartPoint = new Point(0, 0);
            GradientBrush.EndPoint = new Point(1, 2);
            GradientBrush.GradientStops.Add(new GradientStop(stop1, 0.0));
            GradientBrush.GradientStops.Add(new GradientStop(stop2, 1));
            return GradientBrush;
        }

        private void ChangeState(string newState, string[,] ServersList)
        {
            Text.Children.Clear();
            TextState = newState;
            StateManager(ServersList);
        }

        private void StateManager(string[,] ServersList)
        {
            for (int i = 0; i < ServersList.GetLength(0); i++)
            {
                if (TextState == ServersList[i, 0])
                {
                    CreateText(ServersList[i, 0], ServersList[i, 4]);
                    break;
                }
            }
        }

        public void CreateImages()
        {
        }

        public void CreateText(string name, string text)
        {
            var ServerName = CreateTexBlock(name, 30, RobotoBold);
            var serverDesc = CreateTexBlock(text, 15, RobotoBold);
            serverDesc.TextWrapping = TextWrapping.Wrap;
            Text.Children.Add(ServerName);
            Text.Children.Add(serverDesc);
        }
    }
}
