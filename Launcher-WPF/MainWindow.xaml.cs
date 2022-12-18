﻿using BackendCommon;
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
            if (fileRequest.ReadUserData())
            {
                if (!fileRequest.SendUserData())
                {
                    InputBox.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                InputBox.Visibility = Visibility.Visible;
            }
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
    }
}
