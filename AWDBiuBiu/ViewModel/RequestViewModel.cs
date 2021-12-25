using AWDBiuBiu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWDBiuBiu.ViewModel
{
    public class RequestViewModel : BaseViewModel
    {
        private int id { get; set; }
        private string url { get; set; }
        private string mode { get; set; }
        private string header { get; set; }
        private string param { get; set; }
        private string filepath { get; set; }
        private string fileparam { get; set; }

        public int Id { get { return id; } set { id = value; OnPropertyChanged("ID"); } }
        public string Url { get { return url; } set { url = value; OnPropertyChanged("Url"); } }
        public string Mode { get { return mode; } set { mode = value; OnPropertyChanged("Mode"); } }
        public string Header { get { return string.IsNullOrEmpty(header) ? string.Empty : header; } set { header = value; OnPropertyChanged("Header"); } }
        public string Param { get { return string.IsNullOrEmpty(param) ? string.Empty : param; } set { param = value; OnPropertyChanged("Param"); } }
        public List<KeyValuePair<string, string>> HeaderList { get { return Utils.BuildKeyValuePairList(Header.Trim().Replace('\r', ' '), '\n', ':'); } }
        public List<KeyValuePair<string, string>> ParamList { get { return Utils.BuildKeyValuePairList(Param.Trim(), '&', '='); } }
        public string Filepath { get { return filepath; } set { filepath = value; OnPropertyChanged("Filepath"); } }
        public string Fileparam { get { return fileparam; } set { fileparam = value; OnPropertyChanged("Fileparam"); } }
        public string Attackurl { get; set; }
        public string Host { get { return new Uri(Attackurl).Host; } }

        public RequestViewModel()
        {
            Url = "http://192.168.{0}.1:{1}";
            Mode = "POST";
            Fileparam = "file";
        }
    }
}
