using AWDBiuBiu.Model;
using AWDBiuBiu.Resource;
using AWDBiuBiu.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;

namespace AWDBiuBiu.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ConfigModel configModel = null;
        MainPage mainPage = new MainPage();
        RequestManagePage requestManagePage;
        CommitManagePage commitManagePage;


        public MainWindow()
        {
            InitializeComponent();
            InitMainPage();
        }


        private void InitMainPage()
        {
            drawerListView.ItemsSource = DrawerMenu.drawerMenuList;
            mainFrame.Navigate(mainPage);

            if (File.Exists(FilePath.defaultConfigPath))
            {
                string config = File.ReadAllText(FilePath.defaultConfigPath);
                ReadConfig(config);
            }
            else
            {
                InitConfig();
            }
        }

        private void ReadConfig(string configString)
        {
            try
            {
                configModel = JsonConvert.DeserializeObject<ConfigModel>(configString);
                SetDataContext();
            }
            catch (Exception)
            {
                MessageBox.Show("配置文件读取错误");
                File.Delete(FilePath.defaultConfigPath);
                InitConfig();
            }
        }

        private void SetDataContext()
        {
            window.DataContext = configModel;
            if (requestManagePage != null)
                requestManagePage.DataContext = configModel;
            if (commitManagePage != null)
                commitManagePage.DataContext = configModel;
        }

        private void InitConfig()
        {
            configModel = new ConfigModel();
            SetDataContext();
        }

        private void drawerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine((sender as ListView).Name + "->" + (sender as ListView).SelectedIndex);
            if ((sender as ListView).SelectedIndex == -1)
                return;
            var item = ((sender as ListView).SelectedItem as DrawerListViewModel);
            switch (item.type)
            {
                case DrawerItemType.Home: //首页
                    ReturnHomePage();
                    break;
                case DrawerItemType.RequestManage: //请求管理
                    mainFrame.Source = null;
                    if (requestManagePage == null)
                        requestManagePage = new RequestManagePage(configModel);
                    mainFrame.Navigate(requestManagePage);
                    break;
                case DrawerItemType.CommitManage: //提交管理
                    mainFrame.Source = null;
                    if (commitManagePage == null)
                        commitManagePage = new CommitManagePage(configModel);
                    mainFrame.Navigate(commitManagePage);
                    break;
                    //case DrawerItemType.AttackManage: //攻击管理
                    //    mainFrame.Source = null;
                    //    if (settingPage == null)
                    //        settingPage = new SettingPage();
                    //    mainFrame.Navigate(settingPage);
                    //    break;

            }
        }

        private void ReturnHomePage()
        {
            if (mainFrame.Content == null)
                return;
            mainFrame.Source = null;
            while (mainFrame.CanGoBack)
            {
                mainFrame.NavigationService.RemoveBackEntry();
            }
            mainFrame.Navigate(mainPage);
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.InitialDirectory = FilePath.baseDir;
            ofd.Filter = "配置文件|*.json;*.txt;*.config;";
            ofd.FileName = "config";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string config = File.ReadAllText(ofd.FileName);
                ReadConfig(config);
            }

        }

       
    }
}
