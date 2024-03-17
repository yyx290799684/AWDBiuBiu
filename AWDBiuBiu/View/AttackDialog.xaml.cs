using AWDBiuBiu.Util;
using AWDBiuBiu.ViewModel;
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
using System.Windows.Shapes;

namespace AWDBiuBiu.View
{
    /// <summary>
    /// AttackDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AttackDialog : Window
    {

        AttackViewModel attackViewModel = new AttackViewModel();

        public AttackDialog(RequestViewModel requestViewModel, CommitViewModel commitViewModel)
        {
            InitializeComponent();
            attackViewModel._requestViewModel = requestViewModel;
            attackViewModel._commitViewModel = commitViewModel;
            window.DataContext = attackViewModel;
            Init();
        }

        private async void Init()
        {
            await Task.Delay(100);
        }


    }
}
