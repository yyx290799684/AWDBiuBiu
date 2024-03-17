using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWDBiuBiu.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private int index { get; set; } = 0;

        public int Index { get { return index; } set { index = value; OnPropertyChanged("Index"); } }

    }
}
