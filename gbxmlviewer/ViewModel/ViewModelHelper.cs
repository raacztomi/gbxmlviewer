using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using HelixToolkit.Wpf;

namespace gbxmlviewer.ViewModel
{
    class ViewModelHelper
    {
        /// <summary>
        /// Update the viewport from the element view model
        /// </summary>
        public static void UpdateViewport(gbXmlVM elem, ViewportVM viewport)
        {
            viewport.Clear();
            foreach (BuildingVM bldg in elem.Children)
            {
                var bldg3D = new ViewportElementVM(bldg, new List<GeometryModel3D>()); // No geom, only grouping
                var bldg3DChilds = new List<ViewportElementVM>();
                foreach (SpaceVM space in bldg.Children)
                {
                    var geom = MakeGeometry(space.Data);
                    bldg3DChilds.Add(new ViewportElementVM(space, geom));
                }
                bldg3D.SetChildren(bldg3DChilds);
                viewport.AddElem(bldg3D);
            }
        }


        /// <summary>
        /// Make geometry for the given XElement node
        /// It may produce empty list
        /// </summary>
        public static List<GeometryModel3D> MakeGeometry(XElement elem)
        {
            var geomList = new List<GeometryModel3D>();

            // Determine the object type
            // Shell geometry
            foreach (var shell in elem.Elements().Where(e => e.Name.LocalName == "ShellGeometry"))
            {
                // Closed shell elements
                foreach (var cShell in shell.Elements().Where(e => e.Name.LocalName == "ClosedShell"))
                {
                    // Polyloop elements
                    foreach (var poly in cShell.Elements().Where(e => e.Name.LocalName == "PolyLoop"))
                    {
                        var geom = _polyLoopGeometry(poly);
                        if (geom != null)
                        {
                            geomList.Add(geom);
                        }
                    }
                }
            }

            return geomList;
        }

        /// <summary>
        /// Make geometry for polyloop
        /// </summary>
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
                if (ptCoords.Count > 2)
                {
                    indices.Add(points.Count);
                    points.Add(new Point3D(ptCoords[0], ptCoords[1], ptCoords[2]));
                }
            }

            if (points.Count < 3)
            {
                return null;
            }

            var mbuilder = new MeshBuilder(false, false, false);
            mbuilder.AddPolygon(points);

            var geom = new GeometryModel3D()
            {
                Geometry = mbuilder.ToMesh(),
                Material = ViewportVM.DefaultMaterial
            };

            return geom;
        }
    }
}
