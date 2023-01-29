using BackendCommon;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.VersionLoader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
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
        ScrollDragger ServerListDragger;
        ScrollDragger ImageDragger;

        bool connectionSuccesful = true;
        private int loginCnt = 0;

        public MainWindow()
        {
            InitializeComponent();
            GoConn = new goConn("127.0.0.1", 8081);
            fileRequest = new FileRequest(GoConn, progress);
            fileRequest.DownloadCompleted += FileRequest_DownloadCompleted;
            createCard = new CreateCard(ServerList, ServersCol, Images, Text);
            ServerListDragger = new ScrollDragger(ServerList, ScrollServerList, true);
            ImageDragger = new ScrollDragger(Images, ScrollImages, false);
            try
            {
                Initialize();
            }
            catch (Exception)
            {
                createCard.CreateServerCard(new LinearGradientBrush(Color.FromRgb(39, 37, 55), Color.FromRgb(39, 37, 55), 0), "Error Occured", "", "", "Server is currently not anvalible", 0);
                connectionSuccesful = false;
                InputBox.Visibility = Visibility.Collapsed;
            }
        }

        private void FileRequest_DownloadCompleted(string instanceName)
        {
            progressBar.Visibility = Visibility.Collapsed;
            Launcher(instanceName);
        }

        private void Initialize()
        {
            InputBox.Visibility = Visibility.Visible;
            if (fileRequest.ReadUserData() && fileRequest.SendUserData())
            {
                InputBox.Visibility = Visibility.Collapsed;
                //fileRequest.GetImages();
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
                createCard.CreateServerCard(new LinearGradientBrush(Color.FromRgb(39, 37, 55), Color.FromRgb(39, 37, 55), 0),"Error Occured", "", "", "Server is currently not anvalible", 0);
            }
        }

        private void Start()
        {
            var instanceName = createCard.GetinstanceName();
            var versionName = createCard.GetVersionName();
            if (fileRequest.CheckFile(instanceName))
            {
                Launcher(instanceName);
            } else
            {
                progressBar.Visibility = Visibility.Visible;
                fileRequest.GetFile("placeHolder", versionName, instanceName);
                fileRequest.GetFile("placeHolder", instanceName, instanceName);
                Launcher(instanceName);
            }
        }

        async void Launcher(string instanceName)
        {
            string name = GoConn.GetMCname();
            if (name != null)
            {
                var session = MSession.GetOfflineSession(name);
                var path = new MinecraftPath(fileRequest.GetInstPath(instanceName));
                progressBar.Visibility = Visibility.Visible;
                progressBarText.Text = "запуск";

                var launcher = new CMLauncher(path);
                launcher.ProgressChanged += (s, e) =>
                {
                    progress.Value = e.ProgressPercentage;
                };

                var versions = await launcher.GetAllVersionsAsync();
                foreach (var item in versions)
                {
                    Console.WriteLine(item.Name);
                }

                var launchOption = new MLaunchOption
                {
                    MaximumRamMb = 1024,
                    Session = session,
                    ScreenWidth = 1600,
                    ScreenHeight = 900,
                    ServerIp = "toblet.lox"
                };

                var process = await launcher.CreateProcessAsync("1.12.2-forge-14.23.5.2859", launchOption);
                process.Start();
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

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Start();
        }
    }
}
