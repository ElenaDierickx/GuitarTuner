using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class MainViewModel : ObservableObject
    {
        private readonly IMicrophone microphone;
        public MainViewModel(IMicrophone microphone)
        {
            this.microphone = microphone;
        }
    }
}
