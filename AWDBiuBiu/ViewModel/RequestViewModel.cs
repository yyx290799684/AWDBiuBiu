using AWDBiuBiu.Resource;
using AWDBiuBiu.Util;
using AWDBiuBiu.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AWDBiuBiu.ViewModel
{
    public class RequestViewModel : BaseViewModel
    {
        private string url { get; set; } = "http://192.168.{0}.1:{1}/";
        private string urlliststring { get; set; }
        private string mode { get; set; }
        private HttpMode httpmode { get; set; } = HttpMode.POST;
        private UrlSource urlsource { get; set; } = UrlSource.拼接;
        private string header { get; set; }
        private string param { get; set; }
        private string filepath { get; set; }
        private string fileparam { get; set; }
        private string iprange { get; set; } = "1-10";
        private string portrange { get; set; } = "80";

        public string Url { get { return url; } set { url = value; OnPropertyChanged("Url"); } }
        public string UrlListString { get { return urlliststring; } set { urlliststring = value; OnPropertyChanged("UrlListString"); } }
        public string Mode { get { return mode; } set { mode = value; OnPropertyChanged("Mode"); } }
        public HttpMode _HttpMode { get { return httpmode; } set { httpmode = value; OnPropertyChanged("_HttpMode"); } }
        public UrlSource _UrlSource { get { return urlsource; } set { urlsource = value; OnPropertyChanged("_UrlSource"); } }
        public string Header { get { return string.IsNullOrEmpty(header) ? string.Empty : header; } set { header = value; OnPropertyChanged("Header"); } }
        public string Param { get { return string.IsNullOrEmpty(param) ? string.Empty : param; } set { param = value; OnPropertyChanged("Param"); } }

        public string FilePath { get { return filepath; } set { filepath = value; OnPropertyChanged("FilePath"); } }
        public string FileParam { get { return fileparam; } set { fileparam = value; OnPropertyChanged("FileParam"); } }
        public string IpRange { get { return iprange; } set { iprange = value; OnPropertyChanged("IpRange"); } }
        public string PortRange { get { return portrange; } set { portrange = value; OnPropertyChanged("PortRange"); } }


        [JsonIgnore]
        public List<KeyValuePair<string, string>> HeaderList { get { return Utils.BuildKeyValuePairList(Header.Trim().Replace('\r', ' '), '\n', ':'); } }
        [JsonIgnore]
        public List<KeyValuePair<string, string>> ParamList { get { return Utils.BuildKeyValuePairList(Param.Trim(), '&', '='); } }
        [JsonIgnore]
        public string Attackurl { get; set; }
        [JsonIgnore]
        public string Host { get { return string.IsNullOrEmpty(Attackurl) ? string.Empty : new Uri(Attackurl).Host; } }





        private ICommand editCommand;

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
