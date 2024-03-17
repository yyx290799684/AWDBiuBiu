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
        string oldFlagCommitName = string.Empty;
        bool isEdit = false;

        public AddCommitDialog()
        {
            InitializeComponent();
            window.DataContext = _commitViewModel;
        }

        public AddCommitDialog(CommitViewModel commitViewModel)
        {
            InitializeComponent();
            oldFlagCommitName = commitViewModel.FlagCommitName;
            _commitViewModel = commitViewModel;
            window.DataContext = commitViewModel;
            isEdit = true;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_commitViewModel.FlagCommitName) || string.IsNullOrEmpty(_commitViewModel.Url))
            {
                return;
            }


            if (isEdit)
            {
                if (MainWindow.configModel.CommitConfigModel.CommitList.Count(p => p.FlagCommitName.Equals(_commitViewModel.FlagCommitName)) > 1)
                {
                    _commitViewModel.FlagCommitName = oldFlagCommitName;
                    return;
                }
            }
            else
            {
                if (MainWindow.configModel.CommitConfigModel.CommitList.Count(p => p.FlagCommitName.Equals(_commitViewModel.FlagCommitName)) > 0)
                {
                    return;
                }
            }

           

            var oldNameUsedList = MainWindow.configModel.RequestConfigModel.RequestList.Where(p => p.FlagCommitNameSelect.Equals(oldFlagCommitName)).ToList();
            if (oldNameUsedList != null && oldNameUsedList.Count > 0)
            {
                foreach (var item in oldNameUsedList)
                {
                    item.FlagCommitNameSelect = _commitViewModel.FlagCommitName;
                }
            }

            DialogResult = true;
            this.Close();
        }

    }
}

