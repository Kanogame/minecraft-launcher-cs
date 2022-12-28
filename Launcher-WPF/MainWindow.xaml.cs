using BackendCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
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
    public partial class MainWindow : Window
    {

        goConn GoConn;
        CreateCard createCard;
        FileRequest fileRequest;
        private int loginCnt = 0;

        public MainWindow()
        {
            InitializeComponent();
            GoConn = new goConn("127.0.0.1", 8081);
            fileRequest = new FileRequest(GoConn);
            createCard = new CreateCard(ServerList, ServersCol);
            Initialize();
        }

        private void Initialize()
        {
            InputBox.Visibility = Visibility.Visible;
            if (fileRequest.ReadUserData() && fileRequest.SendUserData())
            {
                InputBox.Visibility = Visibility.Collapsed;
                LoadServerList();
            }
        }

        private void LoadServerList()
        {
            try
            { 
                createCard.InitCards(GoConn.GetServers());
            }
            catch (Exception)
            {
                createCard.CreateServerCard(new LinearGradientBrush(Color.FromRgb(39, 37, 55), Color.FromRgb(39, 37, 55), 0),"Error Occured", "", "", "Server is currently not anvalible");
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (inpBox.Text != "login" && pasBox.Text != "password" && loginCnt <= 3)
            {
                if (loginCnt <= 3)
                {
                    if (!GoConn.VerifyUser(inpBox.Text, pasBox.Text))
                    {
                        MessageBox.Show("Неверный логин или пароль");
                        loginCnt++;
                    }
                    else
                    {
                        InputBox.Visibility = Visibility.Collapsed;
                        if (!fileRequest.ReadUserData())
                        {
                            fileRequest.WriteUserData(inpBox.Text, pasBox.Text);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Вы совершили слишком много попыток, вы были забаненый нахуй");
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
