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

        goConn GoConn;
        CreateCard createCard;

        public MainWindow()
        {
            InitializeComponent();
            GoConn = new goConn("127.0.0.1", 8081);
            createCard = new CreateCard(ServerList, ServersCol);
            LoadServerList();
        }

        private void LoadServerList()
        {
            try
            { 
                var ServersList = GoConn.GetServers();

                for (int i = 0; i < ServersList.GetLength(0); i++)
                {
                    createCard.CreateServerCard(createCard.GradientCreator(Color.FromRgb(91, 195, 255), Color.FromRgb(58, 160, 255)),
                        ServersList[i, 0],
                        ServersList[i, 1],
                        ServersList[i, 2],
                        ServersList[i, 3]);
                }
            }
            catch (Exception)
            {
                createCard.CreateServerCard(new LinearGradientBrush(Color.FromRgb(39, 37, 55), Color.FromRgb(39, 37, 55), 0),"Error Occured", "", "", "Server is currently not anvalible");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
