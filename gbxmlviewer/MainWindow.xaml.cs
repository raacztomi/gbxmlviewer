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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace gbxmlviewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private XElement _data;
        public XElement Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                _navigationControl.DataContext = new ViewModel.gbXmlVM() { Data = value };
                _viewportControl.DataContext = new ViewModel.ViewportVM() { Data = value };
            }
        }
    }
}
