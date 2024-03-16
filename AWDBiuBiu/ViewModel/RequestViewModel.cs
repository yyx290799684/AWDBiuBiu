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
        private ObservableCollection<string> urllist { get; set; } = new ObservableCollection<string>();


        public string UrlListString { get { return urlliststring; } set { urlliststring = value; OnPropertyChanged("UrlListString"); } }
        [JsonIgnore]
        public string Mode { get { return mode; } set { mode = value; OnPropertyChanged("Mode"); } }
        public UrlSource _UrlSource { get { return urlsource; } set { urlsource = value; OnPropertyChanged("_UrlSource"); } }

        public string IpRange { get { return iprange; } set { iprange = value.Trim(); OnPropertyChanged("IpRange"); OnPropertyChanged("UrlList"); } }
        public string PortRange { get { return portrange; } set { portrange = value.Trim(); OnPropertyChanged("PortRange"); OnPropertyChanged("UrlList"); } }


        [JsonIgnore]
        public List<KeyValuePair<string, string>> HeaderList { get { return Utils.BuildKeyValuePairList(Header.Trim().Replace('\r', ' '), '\n', ':'); } }
        [JsonIgnore]
        public List<KeyValuePair<string, string>> ParamList { get { return Utils.BuildKeyValuePairList(Param.Trim(), '&', '='); } }
        [JsonIgnore]
        public string Attackurl { get; set; }
        [JsonIgnore]
        public string Host { get { return string.IsNullOrEmpty(Attackurl) ? string.Empty : new Uri(Attackurl).Host; } }
        [JsonIgnore]
        public ObservableCollection<string> UrlList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(UrlListString))
                {
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
                            urllist.Add(string.Format(Url, ip, port));
                        }
                    }
                    return urllist;
                }
                else
                {
                    urllist = new ObservableCollection<string>(
                        UrlListString.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                    );
                    return urllist;
                }
            }
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
    }
}
