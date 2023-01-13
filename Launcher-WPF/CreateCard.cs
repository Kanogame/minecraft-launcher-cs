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
using System.Windows.Media.Imaging;
using System.IO;

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

        private string[,] LocalServersList;

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
                    ServersList[i, 3], i);
            }
            SetState(0, ServersList);
            LocalServersList = ServersList;
        }

        public void CreateServerCard(LinearGradientBrush GradientBrush, string Name, string ip, string Version, string Desc, int id)
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
            Button bnt = new Button();
            StackPanel rec = new StackPanel();

            var ServerName = CreateTexBlock(Name, 30, RobotoBold);
            var Players = CreateTexBlock(ip, 18, RobotoBold);
            var Ver = CreateTexBlock(Version, 18, RobotoBold);
            var Descr = CreateTexBlock(Desc, 15, RobotoRegular);
            var ID = CreateTexBlock(id.ToString(), 1, RobotoRegular);

            bord.Child = rec;
            rec.MouseLeftButtonUp += Rec_MouseLeftButtonUp;
            rec.Children.Add(ID);
            rec.Children.Add(ServerName);
            rec.Children.Add(Players);
            rec.Children.Add(Ver);
            rec.Children.Add(Descr);

            ServerList.Children.Add(bord);
            ServerList.HorizontalAlignment = HorizontalAlignment.Center;
            ServerList.CanVerticallyScroll = true;
        }

        private void Rec_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StackPanel clicked = (StackPanel)sender;
            var data = clicked.Children.Cast<TextBlock>();
            var textBlocks  = data.ToArray();
            SetState(int.Parse(textBlocks[0].Text), LocalServersList);
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

        private void SetState(int id, string[,] ServersList)
        {
            Text.Children.Clear();
            CreateImages(id);
            CreateText(ServersList[id, 0], ServersList[id, 4]);
        }

        public void CreateImages(int id)
        {
            for (int i = 0; i < 6; i++)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "kanoCraft", "temp", (id + 1).ToString(), (i + 1).ToString() + ".png")));
                
                Images.Children.Add(img);
            }
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
