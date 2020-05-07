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
using LibTessDotNet.Double;
using System.Windows.Controls;

namespace gbxmlviewer.ViewModel
{
    class ViewportHelper
    {
        /// <summary>
        /// Update the viewport from the element view model
        /// </summary>
        public static void UpdateViewport(NavigationRootVM elem, ViewportVM viewport)
        {
            viewport.Clear();

            // Loop spaces
            foreach (BuildingVM bldg in elem.Children.Where(e => e is BuildingVM))
            {
                var bldg3D = new ViewportElementVM(bldg, new List<GeometryModel3D>()); // No geom, only grouping
                var bldg3DChilds = new List<ViewportElementVM>();
                foreach (SpaceVM space in bldg.Children)
                {
                    var spacePoly = _spacePolyloops(space.Data);
                    var spaceGeom = _geometryFromPolygons(spacePoly);
                    var space3D = new ViewportSpaceVM(space, spaceGeom);
                    bldg3DChilds.Add(space3D);
                }
                bldg3D.SetChildren(bldg3DChilds);
                viewport.AddElem(bldg3D);
            }

            // Loop surfaces
            foreach (SurfaceCollectionVM surfColl in elem.Children.Where(e => e is SurfaceCollectionVM))
            {   // There is supposed to be only on surface collection
                var surfColl3D = new ViewportElementVM(surfColl, new List<GeometryModel3D>()); // No geom, only grouping
                var surfColl3DChilds = new List<ViewportElementVM>();
                foreach (SurfaceVM surf in surfColl.Children.Where(e => e is SurfaceVM))
                {
                    // Collect polygons or surface and openings
                    var surfVertices = _planarVertices(surf.Data);
                    var openingVerticesList = new Dictionary<OpeningVM, List<Point3DCollection>>();
                    foreach (OpeningVM opening in surf.Children.Where(e => e is OpeningVM))
                    {
                        var openingVertices = _planarVertices(opening.Data);
                        openingVerticesList[opening] = openingVertices;
                    }
                    
                    // Apply openings on surface and create surface VM
                    Point3DCollection vertices;
                    Int32Collection indices;
                    _cutOpenings(surfVertices, openingVerticesList.Values, out vertices, out indices);
                    var surf3D = new ViewportSurfaceVM(surf, _geometryFromTriangles(vertices, indices));
                    surfColl3DChilds.Add(surf3D);
                    
                    // Create opening VMs
                    foreach(var openingRec in openingVerticesList)
                    {
                        var openingGeom = _geometryFromPolygons(openingRec.Value);
                        var opening3D = new ViewportOpeningVM(openingRec.Key, openingGeom);
                        surfColl3DChilds.Add(opening3D);  // Openings are selected individually from surfaces, but not from surface collections
                    }
                }
                surfColl3D.SetChildren(surfColl3DChilds);
                viewport.AddElem(surfColl3D);
            }
        }

        /// <summary>
        /// Apply opening polygons on contour polygons, with triangulation
        /// </summary>
        private static void _cutOpenings(List<Point3DCollection> surfVertices,
                                         IEnumerable<List<Point3DCollection>> openingVertices,
                                         out Point3DCollection vertices,
                                         out Int32Collection indices)
        {
            vertices = new Point3DCollection();
            indices = new Int32Collection();

            var tes = new LibTessDotNet.Double.Tess();
            foreach(var poly in surfVertices)
            {
                var cont = poly.Select(pt => new ContourVertex(new Vec3(pt.X, pt.Y, pt.Z))).ToArray();
                tes.AddContour(cont, ContourOrientation.CounterClockwise);
            }
            foreach(var opening in openingVertices)
            {
                foreach (var poly in opening)
                {
                    var hole = poly.Select(pt => new ContourVertex(new Vec3(pt.X, pt.Y, pt.Z))).ToArray();
                    tes.AddContour(hole, ContourOrientation.Clockwise);
                }
            }
            tes.Tessellate();

            vertices = new Point3DCollection(tes.Vertices.Select(i => new Point3D(i.Position.X, i.Position.Y, i.Position.Z)));
            indices = new Int32Collection(tes.Elements);
        }


        /// <summary>
        /// Extact polygons of the given element (e.g. surfaces or openings)
        /// </summary>
        public static List<Point3DCollection> _planarVertices(XElement elem)
        {
            var verticesList = new List<Point3DCollection>();

            // Planar geometry
            foreach (var planar in elem.Elements().Where(e => e.Name.LocalName == "PlanarGeometry"))
            {
                // Polyloop elements
                foreach (var poly in planar.Elements().Where(e => e.Name.LocalName == "PolyLoop"))
                {
                    var geom = _polyLoopVertices(poly);
                    if (geom != null)
                    {
                        verticesList.Add(geom);
                    }
                }
            }

            return verticesList;
        }

        /// <summary>
        /// Extrac polygon from a single Polyloop
        /// </summary>
        private static Point3DCollection _polyLoopVertices(XElement poly)
        {
            var points = new Point3DCollection();

            // Cartesian points
            foreach (var point in poly.Elements().Where(e => e.Name.LocalName == "CartesianPoint"))
            {
                var pt = new List<double>();
                foreach (var coord in point.Elements().Where(e => e.Name.LocalName == "Coordinate"))
                {
                    pt.Add(double.Parse(coord.Value));
                }
                if(pt.Count > 2)
                {
                    points.Add(new Point3D(pt[0], pt[1], pt[2]));
                }
            }

            return points;
        }

        /// <summary>
        /// Make viewport geometry for triangles
        /// </summary>
        private static List<GeometryModel3D> _geometryFromTriangles(Point3DCollection vertices, Int32Collection triangles)
        {
            var material = MaterialHelper.CreateMaterial(Colors.LightGray); // Just in case, not to accidentally remain invisible (consequence of null)
            var mesh = new MeshGeometry3D()
            {
                Positions = vertices,
                TriangleIndices = triangles

            };
            var geom = new GeometryModel3D()
            {
                Geometry = mesh,
                Material = material,
                BackMaterial = null
            };

            var geomList = new List<GeometryModel3D>();
            geomList.Add(geom);
            return geomList;
        }

        /// <summary>
        /// Create viewport geometry for the polygons (with tesselation)
        /// </summary>
        private static List<GeometryModel3D> _geometryFromPolygons(List<Point3DCollection> polyList)
        {
            var geomList = new List<GeometryModel3D>();

            foreach(var poly in polyList)
            {
                var tes = new LibTessDotNet.Double.Tess();
                tes.AddContour(poly.Select(pt => new ContourVertex(new Vec3(pt.X, pt.Y, pt.Z))).ToArray());
                tes.Tessellate();

                var positions = new Point3DCollection(tes.Vertices.Select(i => new Point3D(i.Position.X, i.Position.Y, i.Position.Z)));
                var triangleIndices = new Int32Collection(tes.Elements);

                var material = MaterialHelper.CreateMaterial(Colors.LightGray); // Just in case, not to accidentally remain invisible (consequence of null)
                var mesh = new MeshGeometry3D()
                {
                    Positions = positions,
                    TriangleIndices = triangleIndices

                };
                var geom = new GeometryModel3D()
                {
                    Geometry = mesh,
                    Material = material,
                    BackMaterial = null
                };
                geomList.Add(geom);
            }

            return geomList;
        }

        /// <summary>
        /// Extract polygons from a space
        /// </summary>
        public static List<Point3DCollection> _spacePolyloops(XElement elem)
        {
            var geomList = new List<Point3DCollection>();

            // Shell geometry
            foreach (var shell in elem.Elements().Where(e => e.Name.LocalName == "ShellGeometry"))
            {
                // Closed shell elements
                foreach (var cShell in shell.Elements().Where(e => e.Name.LocalName == "ClosedShell"))
                {
                    // Polyloop elements
                    foreach (var poly in cShell.Elements().Where(e => e.Name.LocalName == "PolyLoop"))
                    {
                        var geom = _polyLoopVertices(poly);
                        geomList.Add(geom);
                    }
                }
            }

            return geomList;
        }

    }
}
