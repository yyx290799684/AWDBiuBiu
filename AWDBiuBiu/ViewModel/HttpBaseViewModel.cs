using AWDBiuBiu.Resource;
using AWDBiuBiu.Util;
using AWDBiuBiu.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AWDBiuBiu.ViewModel
{
    public class HttpBaseViewModel : BaseViewModel
    {
        private HttpMode httpmode { get; set; } = HttpMode.POST;
        private PostMode postmodel { get; set; } = PostMode.键值对;
        private string filepath { get; set; } = string.Empty;
        private string fileparam { get; set; } = string.Empty;
        private string header { get; set; }
        private string param { get; set; }
        private string paramjson { get; set; }
        private string url { get; set; } 


        public PostMode _PostMode { get { return postmodel; } set { postmodel = value; OnPropertyChanged("_PostMode"); OnPropertyChanged("PostParamString"); } }
        public HttpMode _HttpMode { get { return httpmode; } set { httpmode = value; OnPropertyChanged("_HttpMode"); } }
        public string FilePath { get { return filepath; } set { filepath = value; OnPropertyChanged("FilePath"); OnPropertyChanged("PostParamString"); } }
        public string FileParam { get { return fileparam; } set { fileparam = value; OnPropertyChanged("FileParam"); OnPropertyChanged("PostParamString"); } }
        public string Header { get { return string.IsNullOrEmpty(header) ? string.Empty : header.Trim(); } set { header = value.Trim(); OnPropertyChanged("Header"); } }
        public string Param { get { return string.IsNullOrEmpty(param) ? string.Empty : param.Trim(); } set { param = value.Trim(); OnPropertyChanged("Param"); OnPropertyChanged("PostParamString"); } }
        public string ParamJson { get { return string.IsNullOrEmpty(paramjson) ? string.Empty : paramjson.Trim(); } set { paramjson = value.Trim(); OnPropertyChanged("ParamJson"); OnPropertyChanged("PostParamString"); } }
        public string Url { get { return url; } set { url = value; OnPropertyChanged("Url"); OnPropertyChanged("UrlList"); } }

        [JsonIgnore]
        public List<KeyValuePair<string, string>> HeaderList { get { return Utils.BuildKeyValuePairList(Header.Trim().Replace('\r', ' '), '\n', ':'); } }
        [JsonIgnore]
        public List<KeyValuePair<string, string>> ParamList { get { return Utils.BuildKeyValuePairList(Param.Trim(), '&', '='); } }

        [JsonIgnore]
        public string PostParamString
        {
            get
            {
                switch (_PostMode)
                {
                    case PostMode.键值对:
                        return Param;
                    case PostMode.FormData:
                        return $@"{FileParam} {FilePath}";
                    case PostMode.JSON:
                        return ParamJson;
                    default:
                        return string.Empty;
                }
            }
        }


        private ICommand postFileSelectCommand;

        [JsonIgnore]
        public ICommand PostFileSelectCommand
        {
            get
            {
                return postFileSelectCommand ?? (postFileSelectCommand = new CommandHandler(() => PostFileSelect()));
            }
        }

        private void PostFileSelect()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = openFileDialog.FileName.ToString();
            }
        }
    }
}
