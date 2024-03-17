using AWDBiuBiu.Converter;
using AWDBiuBiu.Util;
using AWDBiuBiu.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AWDBiuBiu
{
    /// <summary>
    /// DemoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DemoWindow : Window
    {
        int attackcount = 0;
        int requestcount = 0;
        ObservableCollection<RequestViewModel> requestList = new ObservableCollection<RequestViewModel>();


        int ipstart = -1;
        int ipend = -1;
        int portstart = -1;
        int portend = -1;
        string flagmode = "GET";
        string reg = string.Empty;
        string flagurl = string.Empty;
        List<KeyValuePair<string, string>> headerflagList = null;
        List<KeyValuePair<string, string>> paramflagList = null;
        bool isthread = false;

        public DemoWindow()
        {
            InitializeComponent();

            Init();

        }

        private void Init()
        {
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/default.config"))
            {
                string config = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "/default.config");
                ReadConfig(config);
            }
            else
            {

                requestList.Add(new RequestViewModel() );
                tab.ItemsSource = requestList;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            requestList.Add(new RequestViewModel()
            {
                Url = (tab.SelectedItem as RequestViewModel).Url,
                Header = (tab.SelectedItem as RequestViewModel).Header,
            });
            tab.SelectedIndex = requestList.Count - 1;
        }

        private void delButton_Click(object sender, RoutedEventArgs e)
        {
            if (requestList.Count > 1)
            {
                requestList.RemoveAt(tab.SelectedIndex);
            }

        }


        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ipTextBox.Text) || string.IsNullOrEmpty(portTextBox.Text))
            {
                return;
            }

            if (!GetBaseConfig())
            {
                return;
            }

            foreach (var item in requestList)
            {
                if (!GetConfig(item))
                {
                    return;
                }
            }

            addButton.IsEnabled = false;
            delButton.IsEnabled = false;
            threadCheckBox.IsEnabled = false;
            threadCheckBox.SetBinding(IsEnabledProperty, new Binding("Items.Count") { ElementName = "tab", Converter = new ThreadEnableConverter() });
            attackcount++;
            DoAttack();
        }
        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            attackcount++;
            progressBar.Value = 0;
            addButton.IsEnabled = true;
            delButton.IsEnabled = true;
            threadCheckBox.IsEnabled = true;
            threadCheckBox.SetBinding(IsEnabledProperty, new Binding("Items.Count") { ElementName = "tab", Converter = new ThreadEnableConverter() });
        }


        private bool GetBaseConfig()
        {
            reg = regTextBox.Text.Trim();
            flagurl = flagUrlTextBox.Text.Trim();
            var ips = Utils.BuildStartAndEnd(ipTextBox.Text.Trim());
            ipstart = ips.Key;
            ipend = ips.Value;
            isthread = (bool)threadCheckBox.IsChecked;
            var ports = Utils.BuildStartAndEnd(portTextBox.Text.Trim());
            portstart = ports.Key;
            portend = ports.Value;
            if (ipstart == -1 || ipstart > 256 || ipend > 256 || portstart == -1 || portstart > 65535 || portend > 65535)
            {
                return false;
            }

            flagmode = (bool)getflagRadioButton.IsChecked ? "GET" : "POST";
            return true;

        }


        private bool GetConfig(RequestViewModel requestViewModel)
        {
            if ((!string.IsNullOrEmpty(requestViewModel.Header) && requestViewModel.HeaderList == null) || (!string.IsNullOrEmpty(requestViewModel.Param) && requestViewModel.ParamList == null))
            {
                return false;
            }
            return true;
        }

        private async void DoAttack()
        {
            int oldattackcount = attackcount;
            progressBar.Maximum = (ipend - ipstart + 1) * (portend - portstart + 1) * requestList.Count;
            progressBar.Value = 0;
            logTextBox.Text = string.Empty;
            logTextBox.Text += DateTime.Now + "\r";

            SaveConfig(BuildConfig(), System.AppDomain.CurrentDomain.BaseDirectory + "/default.config");



            for (int ip = ipstart; ip <= ipend; ip++)
            {
                for (int port = portstart; port <= portend; port++)
                {
                    if (oldattackcount != attackcount)
                    {
                        progressBar.Value = 0;
                        return;
                    }
                    foreach (var item in requestList)
                    {
                        item.Attackurl = string.Format(item.Url, ip, port);
                        statusTextBlock.Text = item.Mode + " " + item.Attackurl;
                        if (isthread)
                        {
                            SendAttack(item);
                            await Task.Delay(1);
                        }
                        else
                        {
                            await SendAttack(item);
                        }
                        progressBar.Value++;
                    }


                }
            }


            var configString = JsonConvert.SerializeObject(requestList);
            Debug.WriteLine(configString);


            addButton.IsEnabled = true;
            delButton.IsEnabled = true;
            threadCheckBox.IsEnabled = true;
        }

        private async Task SendAttack(RequestViewModel requestViewModel)
        {
            var ret = await NetWork.getHttpWebRequestOld(requestViewModel.Attackurl, paramList: requestViewModel.ParamList, headerList: requestViewModel.HeaderList, mode: requestViewModel.Mode, filePath: requestViewModel.FilePath, fileParam: requestViewModel.FileParam);
            if (!string.IsNullOrEmpty(ret))
            {
                if (!string.IsNullOrEmpty(reg))
                {
                    string flag = Utils.DoRegex(ret, reg);
                    if (!string.IsNullOrEmpty(flagurl))
                    {
                        try
                        {
                            SubFlag(flag);
                            PutLog(requestViewModel.Host, flag);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        private async void SubFlag(string flag)
        {
            if (string.IsNullOrEmpty(paramJsonflagTextBox.Text))
            {
                if (string.IsNullOrEmpty(flagIDTextBox.Text))
                {
                    headerflagList = Utils.BuildKeyValuePairList(headerflagTextBox.Text.Trim().Replace('\r', ' '), '\n', ':');
                    paramflagList = Utils.BuildKeyValuePairList(string.Format(paramflagTextBox.Text.Trim(), flag), '&', '=');
                    var ret = await NetWork.getHttpWebRequestOld(string.Format(flagurl, flag), paramList: paramflagList, headerList: headerflagList, mode: flagmode);
                    PutLog(flag, ret);
                }
                else
                {
                    var ids = Utils.BuildStartAndEnd(flagIDTextBox.Text.Trim());
                    var idstart = ids.Key;
                    var ipend = ids.Value;

                    for (int id = idstart; id <= ipend; id++)
                    {
                        headerflagList = Utils.BuildKeyValuePairList(headerflagTextBox.Text.Trim().Replace('\r', ' '), '\n', ':');
                        paramflagList = Utils.BuildKeyValuePairList(string.Format(paramflagTextBox.Text.Trim(), flag), '&', '=');
                        var ret = await NetWork.getHttpWebRequestOld(string.Format(flagurl, flag, id), paramList: paramflagList, headerList: headerflagList, mode: flagmode);
                        PutLog(flag, ret);
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(flagIDTextBox.Text))
                {
                    headerflagList = Utils.BuildKeyValuePairList(headerflagTextBox.Text.Trim().Replace('\r', ' '), '\n', ':');
                    var ret = await NetWork.getHttpWebRequestOld(string.Format(flagurl, flag), jsonParam: paramJsonflagTextBox.Text.Trim().Replace("{0}", flag), headerList: headerflagList, mode: flagmode);
                    PutLog(flag, ret);
                }
                else
                {
                    var ids = Utils.BuildStartAndEnd(flagIDTextBox.Text.Trim());
                    var idstart = ids.Key;
                    var ipend = ids.Value;

                    for (int id = idstart; id <= ipend; id++)
                    {
                        headerflagList = Utils.BuildKeyValuePairList(headerflagTextBox.Text.Trim().Replace('\r', ' '), '\n', ':');
                        var ret = await NetWork.getHttpWebRequestOld(string.Format(flagurl, flag), jsonParam: paramJsonflagTextBox.Text.Trim().Replace("{0}", flag), headerList: headerflagList, mode: flagmode);
                        PutLog(flag, ret);
                    }
                }
            }

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
                //requestList.Where(p => p.Id == (int)(sender as Button).Tag).ToList()[0].FilePath = openFileDialog.FileName.ToString();
            }
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
        }

        private void gitLink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));

        }

        private string BuildConfig()
        {
            if (!GetBaseConfig())
            {
                return string.Empty;
            }

            DemoConfigModel configModel = new DemoConfigModel();
            configModel.flagreg = reg;
            configModel.iprange = ipTextBox.Text.Trim();
            configModel.portrange = portTextBox.Text.Trim();

            configModel.requestList = requestList;

            configModel.flagurl = flagurl;
            configModel.flagidrange = flagIDTextBox.Text.Trim();
            configModel.flaghttpmode = flagmode;
            configModel.flagheader = headerflagTextBox.Text.Trim();
            configModel.flagparam = paramflagTextBox.Text.Trim();
            configModel.flagjsonparam = paramJsonflagTextBox.Text.Trim();
            
            var configString = JsonConvert.SerializeObject(configModel);

            return configString;
        }

        private void ReadConfig(string configString)
        {
            try
            {
                var configModel = JsonConvert.DeserializeObject<DemoConfigModel>(configString);

                regTextBox.Text = configModel.flagreg;
                ipTextBox.Text = configModel.iprange;
                portTextBox.Text = configModel.portrange;

                requestList = configModel.requestList;
                tab.ItemsSource = requestList;
                tab.SelectedIndex = requestList.Count - 1;

                flagUrlTextBox.Text = configModel.flagurl;
                flagIDTextBox.Text = configModel.flagidrange;

                if (configModel.flaghttpmode == "GET")
                {
                    getflagRadioButton.IsChecked = true;
                }
                else
                {
                    postflagRadioButton.IsChecked = true;
                }


                headerflagTextBox.Text = configModel.flagheader;
                paramflagTextBox.Text = configModel.flagparam;
                paramJsonflagTextBox.Text = configModel.flagjsonparam;
            }
            catch (Exception)
            {
                MessageBox.Show("配置文件读取错误");
            }
        }

        private bool SaveConfig(string configModel, string path)
        {

            File.Create(path).Close();
            StreamWriter stream = new StreamWriter(path);
            stream.WriteLine(configModel);
            stream.Dispose();//流关闭

            return true;
        }



        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            sfd.Filter = "配置文件|*.json;*.txt;*.config;";
            sfd.FileName = "config";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Debug.WriteLine(sfd.FileName);
                SaveConfig(BuildConfig(), sfd.FileName);
            }

        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
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
