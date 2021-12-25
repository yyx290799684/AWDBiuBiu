using AWDBiuBiu.Util;
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

namespace AWDBiuBiu
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int attackcount = 0;

        string url = string.Empty;
        int ipstart = -1;
        int ipend = -1;
        int portstart = -1;
        int portend = -1;
        string mode = "GET";
        string flagmode = "GET";
        string reg = string.Empty;
        string filepath = string.Empty;
        string fileparam = string.Empty;
        List<KeyValuePair<string, string>> headerList = null;
        List<KeyValuePair<string, string>> paramList = null;
        string flagurl = string.Empty;
        List<KeyValuePair<string, string>> headerflagList = null;
        List<KeyValuePair<string, string>> paramflagList = null;
        bool isthread = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(urlTextBox.Text) || string.IsNullOrEmpty(ipTextBox.Text) || string.IsNullOrEmpty(portTextBox.Text))
            {
                return;
            }

            if (!GetConfig())
            {
                return;
            }
            attackcount++;
            // url = "http://124.160.12.17:18890/flag.html";
            DoAttack();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            attackcount++;
            progressBar.Value = 0;
        }

        private bool GetConfig()
        {
            url = urlTextBox.Text.Trim();
            reg = regTextBox.Text.Trim();
            flagurl = flagUrlTextBox.Text.Trim();
            var ips = Utils.BuildStartAndEnd(ipTextBox.Text.Trim());
            ipstart = ips.Key;
            ipend = ips.Value;
            isthread = (bool)threadCheckBox.IsChecked;
            filepath = pathTextBox.Text;
            fileparam = fileparamTextBox.Text.Trim();

            var ports = Utils.BuildStartAndEnd(portTextBox.Text.Trim());
            portstart = ports.Key;
            portend = ports.Value;

            if (ipstart == -1 || ipstart > 256 || ipend > 256 || portstart == -1 || portstart > 65535 || portend > 65535)
            {
                return false;
            }

            mode = (bool)getRadioButton.IsChecked ? "GET" : "POST";
            flagmode = (bool)getflagRadioButton.IsChecked ? "GET" : "POST";

            headerList = Utils.BuildKeyValuePairList(headerTextBox.Text.Trim().Replace('\r', ' '), '\n', ':');
            paramList = Utils.BuildKeyValuePairList(paramTextBox.Text.Trim(), '&', '=');
            if ((!string.IsNullOrEmpty(headerTextBox.Text) && headerList == null) || (!string.IsNullOrEmpty(paramTextBox.Text) && paramList == null))
            {
                return false;
            }
            return true;
        }

        private async void DoAttack()
        {
            int oldattackcount = attackcount;
            progressBar.Maximum = ipend - ipstart + 1;
            progressBar.Value = 0;
            logTextBox.Text = string.Empty;


            for (int ip = ipstart; ip <= ipend; ip++)
            {
                for (int port = portstart; port <= portend; port++)
                {
                    if (oldattackcount != attackcount)
                    {
                        return;
                    }
                    string attackurl = string.Format(url, ip, port);
                    statusTextBlock.Text = mode + " " + attackurl;
                    if (isthread)
                    {
                        SendAttack(attackurl);
                        await Task.Delay(1);
                    }
                    else
                    {
                        await SendAttack(attackurl);
                    }

                }
                progressBar.Value++;
            }
        }

        private async Task SendAttack(string attackurl)
        {
            var ret = await NetWork.getHttpWebRequest(attackurl, paramList: paramList, headerList: headerList, mode: mode, filePath: filepath, fileParam: fileparam);
            if (!string.IsNullOrEmpty(ret))
            {
                if (!string.IsNullOrEmpty(reg))
                {
                    string flag = Utils.DoRegex(ret, reg);
                    if (!string.IsNullOrEmpty(flagurl))
                    {
                        SubFlag(flag);
                    }
                    PutLog(new Uri(attackurl).Host, flag);
                }
            }
        }

        private async void SubFlag(string flag)
        {
            headerflagList = Utils.BuildKeyValuePairList(headerflagTextBox.Text.Trim().Replace('\r', ' '), '\n', ':');
            paramflagList = Utils.BuildKeyValuePairList(string.Format(paramflagTextBox.Text.Trim(), flag), '&', '=');
            var ret = await NetWork.getHttpWebRequest(string.Format(flagurl, flag), paramList: paramflagList, headerList: headerflagList, mode: flagmode);
        }

        private void PutLog(string host, string flag)
        {
            logTextBox.Text += (host + " " + flag + "\r");
        }

        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pathTextBox.Text = openFileDialog.FileName.ToString();
            }
        }
    }
}
