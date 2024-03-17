using AWDBiuBiu.Resource;
using AWDBiuBiu.Util;
using AWDBiuBiu.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AWDBiuBiu.ViewModel
{
    public class RequestViewModel : HttpBaseViewModel
    {
        private string urlliststring { get; set; } = string.Empty;
        private string mode { get; set; }
        private UrlSource urlsource { get; set; } = UrlSource.拼接;

        private string iprange { get; set; } = "1-10";
        private string portrange { get; set; } = "80";
        private string flagreg { get; set; } = "flag{[a-zA-Z0-9]{32}}";
        private ObservableCollection<string> urllist { get; set; } = new ObservableCollection<string>();
        private string flagcommitnameselect { get; set; } = string.Empty;


        public string UrlListString { get { return urlliststring; } set { urlliststring = value; OnPropertyChanged("UrlListString"); } }
        [JsonIgnore]
        public string Mode { get { return mode; } set { mode = value; OnPropertyChanged("Mode"); } }
        public UrlSource _UrlSource { get { return urlsource; } set { urlsource = value; OnPropertyChanged("_UrlSource"); OnPropertyChanged("UrlList"); } }

        public string IpRange { get { return iprange; } set { iprange = value.Trim(); OnPropertyChanged("IpRange"); OnPropertyChanged("UrlList"); } }
        public string PortRange { get { return portrange; } set { portrange = value.Trim(); OnPropertyChanged("PortRange"); OnPropertyChanged("UrlList"); } }
        public string FlagReg { get { return flagreg; } set { flagreg = value.Trim(); OnPropertyChanged("FlagReg"); } }
        public string FlagCommitNameSelect { get { return flagcommitnameselect; } set { flagcommitnameselect = value.Trim(); OnPropertyChanged("FlagCommitNameSelect"); } }



        [JsonIgnore]
        public string Attackurl { get; set; }
        [JsonIgnore]
        public string Host { get { return string.IsNullOrEmpty(Attackurl) ? string.Empty : new Uri(Attackurl).Host; } }
        [JsonIgnore]
        public ObservableCollection<string> UrlList
        {
            get
            {
                switch (_UrlSource)
                {
                    case UrlSource.拼接:
                        urllist.Clear();
                        var ips = Utils.BuildStartAndEnd(iprange.Trim());
                        int ipstart = ips.Key;
                        int ipend = ips.Value;
                        var ports = Utils.BuildStartAndEnd(portrange.Trim());
                        int portstart = ports.Key;
                        int portend = ports.Value;

                        for (int ip = ipstart; ip <= ipend; ip++)
                        {
                            for (int port = portstart; port <= portend; port++)
                            {
                                urllist.Add(Url.Replace("{ip}", ip.ToString()).Replace("{port}", port.ToString()));
                            }
                        }
                        return urllist;
                    case UrlSource.来自列表:
                        urllist = new ObservableCollection<string>(
                         UrlListString.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                     );
                        return urllist;
                    default:
                        return urllist;
                }
            }
        }

        [JsonIgnore]
        public ObservableCollection<string> FlagCommitNameList
        {
            get { return new ObservableCollection<string>(MainWindow.configModel.CommitConfigModel.CommitList.Select(c => c.FlagCommitName)); }
        }


        private ICommand startCommand;

        [JsonIgnore]
        public ICommand StartCommand
        {
            get
            {
                return startCommand ?? (startCommand = new CommandHandler(() => Start()));
            }
        }

        private void Start()
        {
            var _commitViewModel = MainWindow.configModel.CommitConfigModel.CommitList.FirstOrDefault(p => p.FlagCommitName.Equals(this.FlagCommitNameSelect));
            if (_commitViewModel == null)
            {
                MessageBox.Show("Flag提交对象不存在");
            }

            MainWindow.configModel.SaveConfig(MainWindow.configModel.BuildConfig(), Resource.FilePath.defaultConfigPath);


            AttackDialog attackDialog = new AttackDialog(this, _commitViewModel);
            attackDialog.Show();

        }

        private ICommand editCommand;

        [JsonIgnore]
        public ICommand EditCommand
        {
            get
            {
                return editCommand ?? (editCommand = new CommandHandler(() => Edit()));
            }
        }

        private void Edit()
        {
            AddRequestDialog addRequestDialog = new AddRequestDialog(this);
            if (addRequestDialog.ShowDialog() == true)
            {
            }
        }


        private ICommand deleteCommand;

        [JsonIgnore]
        public ICommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new CommandHandler(() => Delete()));
            }
        }

        private void Delete()
        {
            MainWindow.configModel.RequestConfigModel.RequestList.Remove(this);
        }

        public RequestViewModel()
        {
            Url = "http://192.168.{ip}.1:{port}/";
        }
    }
}
