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
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gbxmlviewer
{
    /// <summary>
    /// Interaction logic for Viewport.xaml
    /// </summary>
    public partial class ViewportControl : UserControl
    {
        public ViewportControl()
        {
            InitializeComponent();

            //_testItems();
        }


        private void _testItems()
        {
            _viewportControl.Children.Clear();

            var light = new AmbientLight()
            {
                Color = Color.FromRgb(255, 255, 255)
            };

            var indices = new Int32Collection() { 0, 1, 2 };
            var positions = new Point3DCollection()
            {
                new Point3D( 1.0, 0.0, 0.0),
                new Point3D( 0.5, 0.5, 0.0),
                new Point3D(-1.0, 0.0, 0.0)
            };
            var mesh = new MeshGeometry3D()
            {
                TriangleIndices = indices,
                Positions = positions
            };
            var geom = new GeometryModel3D()
            {
                Geometry = mesh,
                Material = new DiffuseMaterial(new SolidColorBrush(Colors.BlueViolet))
            };

            var group = new Model3DGroup();
            group.Children.Add(light);
            group.Children.Add(geom);
            _viewportControl.Children.Add(new ModelVisual3D() { Content = group });

        }
    }
}
