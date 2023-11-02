using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWDBiuBiu.ViewModel
{
    public class ConfigModel
    {
        public string flagreg { get; set; }
        public string iprange { get; set; }
        public string portrange { get; set; }

        public ObservableCollection<RequestViewModel> requestList { get; set; }

        public string flagurl { get; set; }
        public string flagidrange { get; set; }
        public string flaghttpmode { get; set; }
        public string flagheader { get; set; }
        public string flagparam { get; set; }
        public string flagjsonparam { get; set; }
    }
}
