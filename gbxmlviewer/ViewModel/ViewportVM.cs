using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Controls;

namespace gbxmlviewer.ViewModel
{
    public class ViewportVM : INotifyPropertyChanged
    {
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
        /// Construct with defaults
        /// </summary>
        public ViewportVM()
        {
            _perspectiveCamera = new PerspectiveCamera()
            {
                Position = new Point3D(0, -100, 140),
                LookDirection = new Vector3D(0, 1, -1),
                FieldOfView = 60
            };

            var light = new AmbientLight()
            {
                Color = Color.FromRgb(255, 255, 255)
            };
            _lights.Add(light);
        }


        /// <summary>
        /// The camera
        /// </summary>
        private PerspectiveCamera _perspectiveCamera;
        public PerspectiveCamera Camera
        {
            get
            {
                return _perspectiveCamera;
            }
        }

        /// <summary>
        /// Lights
        /// </summary>
        private Model3DCollection _lights = new Model3DCollection();
        public Model3DCollection Lights
        {
            get
            {
                return _lights;
            }
        }

        /// <summary>
        /// Geometry items
        /// </summary>
        private Model3DCollection _geometry = new Model3DCollection();
        public Model3DCollection Geometry
        {
            get
            {
                return _geometry;
            }
        }

        /// <summary>
        /// The XML data viewed
        /// </summary>
        protected XElement _data = null;
        public XElement Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                _updateAfterDataChanged();
                //_testContent();
                NotifyPropertyChanged();
                NotifyPropertyChanged("Geometry");
            }
        }

        private void _updateAfterDataChanged()
        {
            var campus = Data.Elements().Where(e => e.Name.LocalName == "Campus").FirstOrDefault();
            if (campus != null)
            {
                // Building elements
                foreach (var bldg in campus.Elements().Where(e => e.Name.LocalName == "Building"))
                {
                    // Spaces elements
                    foreach (var space in bldg.Elements().Where(e => e.Name.LocalName == "Space"))
                    {
                        // Shell geometry
                        foreach (var shell in space.Elements().Where(e => e.Name.LocalName == "ShellGeometry"))
                        {
                            // Closed shell elements
                            foreach (var cShell in shell.Elements().Where(e => e.Name.LocalName == "ClosedShell"))
                            {
                                // Polyloop elements
                                foreach (var poly in cShell.Elements().Where(e => e.Name.LocalName == "PolyLoop"))
                                {
                                    var geom = _polyLoopGeometry(poly);
                                    if(geom != null)
                                    {
                                        _geometry.Add(geom);
                                    }
                                }
                            }
                        }
                    }
                }

                // Surface elements
                //foreach (var surf in campus.Elements().Where(e => e.Name.LocalName == "Surface"))
                //{
                //    children.Add(new SurfaceVM() { Data = surf });
                //}
            }
        }

        private void _testContent()
        {
            _geometry.Clear();

            var indices = new Int32Collection() { 0, 1, 2 };
            var positions = new Point3DCollection()
            {
                new Point3D( 1.0, 0.0, 0.0),
                new Point3D( 0.5, 0.5, 0.0),
                new Point3D(-1.0, 0.0, 0.0)
            };
            _geometry.Add(_makeMesh(positions, indices));
        }

        private static GeometryModel3D _makeMesh(Point3DCollection points, Int32Collection indices)
        {
            var geom = new GeometryModel3D()
            {
                Geometry = new MeshGeometry3D()
                {
                    TriangleIndices = indices,
                    Positions = points
                },
                Material = new DiffuseMaterial(new SolidColorBrush(Colors.BlueViolet))
            };

            return geom;
        }

        private static GeometryModel3D _polyLoopGeometry(XElement poly)
        {
            var points = new Point3DCollection();
            var indices = new Int32Collection();

            // Cartesian points
            foreach (var point in poly.Elements().Where(e => e.Name.LocalName == "CartesianPoint"))
            {
                var ptCoords = new List<double>();
                foreach (var coord in point.Elements().Where(e => e.Name.LocalName == "Coordinate"))
                {
                    ptCoords.Add(double.Parse(coord.Value));
                }
                if(ptCoords.Count > 2)
                {
                    indices.Add(points.Count);
                    points.Add(new Point3D(ptCoords[0], ptCoords[1], ptCoords[2]));
                }
            }

            if(points.Count < 3)
            {
                return null;
            }

            var geom = new GeometryModel3D()
            {
                Geometry = new MeshGeometry3D()
                {
                    TriangleIndices = indices,
                    Positions = points
                },
                Material = new DiffuseMaterial(new SolidColorBrush(Colors.BlueViolet))
            };

            return geom;
        }
    }
}
