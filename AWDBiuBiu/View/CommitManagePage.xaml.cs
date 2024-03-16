using AWDBiuBiu.Model;
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

namespace AWDBiuBiu.View
{
    /// <summary>
    /// CommitManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class CommitManagePage : Page
    {
        public CommitManagePage()
        {
            InitializeComponent();
        }

        public CommitManagePage(ConfigModel configModel)
        {
            InitializeComponent();
            page.DataContext = configModel;
        }
    }
}
