using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GuitarBuddy
{
    /// <summary>
    /// Interaction logic for Tuner.xaml
    /// </summary>
    public partial class Tuner : Window
    {
        private TunerViewModel viewModel;
        public Tuner(TunerViewModel vm)
        {
            DataContext = vm;
            viewModel = DataContext as TunerViewModel;
            InitializeComponent();
        }
    }
}
