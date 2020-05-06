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
    class ViewportHelper
    {
        /// <summary>
        /// Update the viewport from the element view model
        /// </summary>
        public static void UpdateViewport(NavigationRootVM elem, ViewportVM viewport)
        {
            Material surfMaterial = MaterialHelper.CreateMaterial(Color.FromArgb(32, 64, 64, 128));

            viewport.Clear();
            foreach (BuildingVM bldg in elem.Children.Where(e => e is BuildingVM))
            {
                var bldg3D = new ViewportElementVM(bldg, new List<GeometryModel3D>()); // No geom, only grouping
                var bldg3DChilds = new List<ViewportElementVM>();
                foreach (SpaceVM space in bldg.Children)
                {
                    var geom = MakeSpaceGeometry(space.Data);
                    bldg3DChilds.Add(new ViewportElementVM(space, geom));
                }
                bldg3D.SetChildren(bldg3DChilds);
                viewport.AddElem(bldg3D);
            }
            foreach (SurfaceCollectionVM surfColl in elem.Children.Where(e => e is SurfaceCollectionVM))
            {   // There is supposed to be only on surface collection
                var surfColl3D = new ViewportElementVM(surfColl, new List<GeometryModel3D>()); // No geom, only grouping
                var surfColl3DChilds = new List<ViewportElementVM>();
                foreach (SurfaceVM surf in surfColl.Children)
                {
                    var geom = MakeSurfaceGeometry(surf.Data);
                    surfColl3DChilds.Add(new ViewportElementVM(surf, geom, surfMaterial));
                }
                surfColl3D.SetChildren(surfColl3DChilds);
                viewport.AddElem(surfColl3D);
            }
        }


        /// <summary>
        /// Make geometry for a Space
        /// </summary>
        public static List<GeometryModel3D> MakeSpaceGeometry(XElement elem)
        {
            var geomList = new List<GeometryModel3D>();

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
        /// Make geometry for a Surface
        /// </summary>
        public static List<GeometryModel3D> MakeSurfaceGeometry(XElement elem)
        {
            var geomList = new List<GeometryModel3D>();

            // Planar geometry
            foreach (var planar in elem.Elements().Where(e => e.Name.LocalName == "PlanarGeometry"))
            {
                // Polyloop elements
                foreach (var poly in planar.Elements().Where(e => e.Name.LocalName == "PolyLoop"))
                {
                    var geom = _polyLoopGeometry(poly);
                    if (geom != null)
                    {
                        geomList.Add(geom);
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
