using AWDBiuBiu.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace AWDBiuBiu.View
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        MainPageViewModel mainPageViewModel = new MainPageViewModel();

        public DispatcherTimer stepBarTimer = new DispatcherTimer();

        public MainPage()
        {
            InitializeComponent();

            page.DataContext = mainPageViewModel;

            stepBarTimer.Tick -= stepBarTimerExpiredTasks;
            stepBarTimer.Stop();
            stepBarTimer.Tick += stepBarTimerExpiredTasks;
            stepBarTimer.Interval = TimeSpan.FromSeconds(2);
            stepBarTimer.Start();
        }

        private void stepBarTimerExpiredTasks(object sender, EventArgs e)
        {
            if (mainPageViewModel.Index < 3)
            {
                mainPageViewModel.Index++;
            }
            else
            {
                stepBarTimer.Stop();
            }
        }

        private void Shield_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/yyx290799684/AWDBiuBiu"));
        }
    }
}
