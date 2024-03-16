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
    /// AddCommitDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AddCommitDialog : Window
    {

        public CommitViewModel _commitViewModel { get; set; } = new CommitViewModel();
        public AddCommitDialog()
        {
            InitializeComponent();
            window.DataContext = _commitViewModel;
        }

        public AddCommitDialog(CommitViewModel commitViewModel)
        {
            InitializeComponent();
            window.DataContext = commitViewModel;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

    }
}
