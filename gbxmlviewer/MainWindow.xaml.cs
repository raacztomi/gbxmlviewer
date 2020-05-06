﻿using System;
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

        /// <summary>
        /// The view model used for the navigation panel (tree view)
        /// </summary>
        public ViewModel.GbXmlVM NavigationVM
        {
            get
            {
                return _navigationControl.DataContext as ViewModel.GbXmlVM;
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
    }
}
