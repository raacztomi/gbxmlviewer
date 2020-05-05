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

        private void Start(object sender, StartupEventArgs e)
        {
            Repository.LoadFile();

            MainWindow win = new MainWindow();
            win.Data = Repository.Root;
            MainWindow = win;
            win.Show();

        }
    }
}
