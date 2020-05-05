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
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace gbxmlviewer
{
    /// <summary>
    /// Interaction logic for Navigation.xaml
    /// </summary>
    public partial class NavigationControl : UserControl
    {
        public NavigationControl()
        {
            InitializeComponent();
        }

        //public IEnumerable<object> Itemss 
        //{ 
        //    get
        //    {
        //        XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
        //        nsmgr.AddNamespace(string.Empty, "http://www.gbxml.org/schema");
        //        var xi = Data.XPathSelectElements("Campus", nsmgr).ToList();

        //        var xitems = Data.Elements()
        //            .Where(x => x.Name.LocalName == "Campus").ToList();
        //            //.Where(x => x.Name.LocalName == "Building").ToList();
        //        return xitems.Select(i => new BuildingVM() { Data = i });
        //    }
        //}
    }
}
