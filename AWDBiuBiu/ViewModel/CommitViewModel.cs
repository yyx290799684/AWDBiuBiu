using AWDBiuBiu.Resource;
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
    public class CommitViewModel : HttpBaseViewModel
    {
        private string flagidrange { get; set; } = string.Empty;
        private string flag { get; set; } = string.Empty;
        private string flagcommitname { get; set; } = string.Empty;
        private FlagPostMode flagpostmode { get; set; } = FlagPostMode.键值对;

        public string FlagIdRange { get { return flagidrange; } set { flagidrange = value.Trim(); OnPropertyChanged("FlagIdRange"); } }
        public string Flag { get { return flag; } set { flag = value.Trim(); OnPropertyChanged("Flag"); } }
        public string FlagCommitName { get { return flagcommitname; } set { flagcommitname = value.Trim(); OnPropertyChanged("FlagCommitName"); } }
        public FlagPostMode _FlagPostMode { get { return flagpostmode; } set { flagpostmode = value; OnPropertyChanged("_FlagPostMode"); OnPropertyChanged("FlagPostParamString"); } }

        [JsonIgnore]
        public string FlagPostParamString
        {
            get
            {
                switch (_FlagPostMode)
                {
                    case FlagPostMode.键值对:
                        return Param;
                    case FlagPostMode.JSON:
                        return ParamJson;
                    default:
                        return string.Empty;
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
            AddCommitDialog addCommitDialog = new AddCommitDialog(this);
            if (addCommitDialog.ShowDialog() == true)
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
            MainWindow.configModel.CommitConfigModel.CommitList.Remove(this);
        }

        public CommitViewModel()
        {
            Url = "http://www.sendflag.com?flag={flag}&id={flagid}";
            Param = "flag={flag}";
            ParamJson = "{\r\n        \"flag\": \"{flag}\"\r\n}";
        }
    }
}
