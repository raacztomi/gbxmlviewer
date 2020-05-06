using gbxmlviewer.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
        public GbXmlVM NavigationVM { get; private set; } = new GbXmlVM();
        public ViewportVM ViewportVM { get; private set; } = new ViewportVM();

        private void Start(object sender, StartupEventArgs e)
        {
            Repository.LoadFile();
            NavigationVM.Data = Repository.Root;
            ViewModelHelper.UpdateViewport(NavigationVM, ViewportVM);

            MainWindow win = new MainWindow()
            {
                NavigationVM = NavigationVM,
                ViewportVM = ViewportVM
            };
            MainWindow = win;
            win.Show();
        }
    }
}
