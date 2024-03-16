using AWDBiuBiu.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWDBiuBiu.ViewModel
{
    public class DrawerListViewModel : BaseViewModel
    {
        public string icon { get; set; }
        public string title { get; set; }
        public DrawerItemType type { get; set; }
        public string subTypeName { get; set; }
        public string routeUrl { get; set; }
        public string routeArg { get; set; }
    }
}
