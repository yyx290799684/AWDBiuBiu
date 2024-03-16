using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWDBiuBiu.ViewModel
{
    public class DemoConfigModel
    {
        public string flagreg { get; set; }
        public string iprange { get; set; }
        public string portrange { get; set; }

        public ObservableCollection<RequestViewModel> requestList { get; set; } = new ObservableCollection<RequestViewModel>();
        public ObservableCollection<CommitViewModel> commitList { get; set; } = new ObservableCollection<CommitViewModel>();

        public string flagurl { get; set; }
        public string flagidrange { get; set; }
        public string flaghttpmode { get; set; }
        public string flagheader { get; set; }
        public string flagparam { get; set; }
        public string flagjsonparam { get; set; }
    }

    public class RequestConfigModel
    {
        public ObservableCollection<RequestViewModel> requestList { get; set; } = new ObservableCollection<RequestViewModel>();
    }

}
