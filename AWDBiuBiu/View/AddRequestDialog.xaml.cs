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
    /// AddRequestDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AddRequestDialog : Window
    {
        public RequestViewModel _requestViewModel { get; set; } = new RequestViewModel();
        public AddRequestDialog()
        {
            InitializeComponent();
            window.DataContext = _requestViewModel;
        }

        public AddRequestDialog(RequestViewModel requestViewModel)
        {
            InitializeComponent();
            _requestViewModel = requestViewModel;
            window.DataContext = requestViewModel;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_requestViewModel.FlagCommitNameSelect) || string.IsNullOrEmpty(_requestViewModel.FlagReg))
            {
                return;
            }

            if (_requestViewModel._UrlSource == Resource.UrlSource.拼接)
            {
                if (string.IsNullOrEmpty(_requestViewModel.Url) || string.IsNullOrEmpty(_requestViewModel.IpRange) || string.IsNullOrEmpty(_requestViewModel.PortRange))
                {
                    return;
                }
                else
                {
                    var ips = Utils.BuildStartAndEnd(_requestViewModel.IpRange.Trim());
                    var ports = Utils.BuildStartAndEnd(_requestViewModel.PortRange.Trim());

                    if (ips.Key == -1 || ips.Value == -1 || ports.Key == -1 || ports.Value == -1)
                    {
                        return;
                    }
                    else if (ips.Key > ips.Value || ports.Key > ports.Value)
                    {
                        return;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_requestViewModel.UrlListString))
                {
                    return;
                }
            }



            DialogResult = true;
            this.Close();
        }
    }
}
