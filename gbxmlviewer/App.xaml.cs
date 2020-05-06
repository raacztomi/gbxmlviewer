using gbxmlviewer.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;

namespace gbxmlviewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Repository Repository { get; private set; } = new Repository();
        public NavigationRootVM NavigationVM { get; private set; } = new NavigationRootVM();
        public ViewportVM ViewportVM { get; private set; } = new ViewportVM();

        private readonly static string defaultTitle = "[no gbXML data]";

        /// <summary>
        /// The this.MainWindow casted to MainWindow
        /// </summary>
        private MainWindow MainWin
        {
            get { return this.MainWindow as MainWindow; }
        }

        /// <summary>
        /// Called on starting the application
        /// </summary>
        private void Start(object sender, StartupEventArgs e)
        {
            MainWindow win = new MainWindow()
            {
                NavigationVM = NavigationVM,
                ViewportVM = ViewportVM,
                TheApp = this,
                Title = defaultTitle
            };
            MainWindow = win;
            win.Show();
        }

        /// <summary>
        /// Request of opening a new gbXML file
        /// </summary>
        internal void OpenFileCommand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "gbXML Files(*.xml; *.gbxml)| *.xml; *.gbxml | All files(*.*) | *.*";
            if (openFileDialog.ShowDialog() == true)
            {
                MainWin.Title = defaultTitle;

                // Clear view of previous file
                NavigationVM.Data = null;
                ViewportVM.Clear();

                // Load and present current file
                if(Repository.LoadFile(openFileDialog.FileName))
                {
                    if(Repository.IsGbXml)
                    {
                        NavigationVM.Data = Repository.Root;
                        ViewportHelper.UpdateViewport(NavigationVM, ViewportVM);
                        MainWin.Title = openFileDialog.FileName;
                    }
                    else
                    {
                        MessageBox.Show("The file contains no gbXML data.", "No gbXML data", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Error loading the file!\nThe file may be incacessible or containing no XML data", "Loading error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
