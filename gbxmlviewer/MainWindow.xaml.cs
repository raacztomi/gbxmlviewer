using gbxmlviewer.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Implementation of INotifyPropertyChaned with auxiliary function
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Reference to the app
        /// </summary>
        internal App TheApp { get; set; }

        /// <summary>
        /// Indicates if spaces are requested to be visible
        /// </summary>
        private bool _isSpacesVisible = true;
        public bool IsSpacesVisible {
            get
            {
                return _isSpacesVisible;
            }
            set
            {
                if(_isSpacesVisible != value)
                {
                    _isSpacesVisible = value;
                    ViewportVM.SetRefElemVisibility<SpaceVM>(value);
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Indicates if surfaces are requested to be visible
        /// </summary>
        private bool _isSurfacesVisible = true;
        public bool IsSurfacesVisible
        {
            get
            {
                return _isSurfacesVisible;
            }
            set
            {
                if(_isSurfacesVisible != value)
                {
                    _isSurfacesVisible = value;
                    ViewportVM.SetRefElemVisibility<SurfaceVM>(value);
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Indicates if openings are requested to be visible
        /// </summary>
        private bool _isOpeningsVisible = true;
        public bool IsOpeningsVisible
        {
            get
            {
                return _isOpeningsVisible;
            }
            set
            {
                if(_isOpeningsVisible != value)
                {
                    _isOpeningsVisible = value;
                    ViewportVM.SetRefElemVisibility<OpeningVM>(value);
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The view model used for the navigation panel (tree view)
        /// </summary>
        public ViewModel.NavigationRootVM NavigationVM
        {
            get
            {
                return _navigationControl.DataContext as ViewModel.NavigationRootVM;
            }
            set
            {
                _navigationControl.DataContext = value;
            }
        }

        /// <summary>
        /// View model used for the viewport (3D view)
        /// </summary>
        public ViewModel.ViewportVM ViewportVM
        {
            get
            {
                return _viewportControl.DataContext as ViewModel.ViewportVM;
            }
            set
            {
                _viewportControl.DataContext = value;
            }
        }

        /// <summary>
        /// Handle opening gbXML file click
        /// </summary>
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            TheApp.OpenFileCommand();
            _viewportControl.ZoomAll();
        }
    }
}
