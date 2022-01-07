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
    /// Interaction logic for Spectrum.xaml
    /// </summary>
    public partial class Spectrum : Window
    {
        private SpectrumViewModel viewModel;

        public Spectrum(SpectrumViewModel vm)
        {
            DataContext = vm;
            viewModel = DataContext as SpectrumViewModel;
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            viewModel.StopCommand.Execute(null);
        }
    }
}
