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
        public NavigationRootVM NavigationVM { get; private set; } = new NavigationRootVM();
        public ViewportVM ViewportVM { get; private set; } = new ViewportVM();

        private void Start(object sender, StartupEventArgs e)
        {
            Repository.LoadFile();
            NavigationVM.Data = Repository.Root;
            ViewportHelper.UpdateViewport(NavigationVM, ViewportVM);

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
