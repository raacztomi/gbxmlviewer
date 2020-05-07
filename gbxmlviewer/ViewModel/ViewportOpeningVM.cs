using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace gbxmlviewer.ViewModel
{
    class ViewportOpeningVM : ViewportElementVM
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewportOpeningVM(NavigationElementVM refElem, List<GeometryModel3D> geom, Material material = null)
            : base (refElem, geom, material)
        {
        }

        /// <summary>
        /// Default material for spaces in the viewport
        /// </summary>
        private static Material _defaultMaterial = MaterialHelper.CreateMaterial(Color.FromArgb(128, 128, 192, 0));    // Transparent deep green
        public override Material DefaultMaterial => _defaultMaterial;
    }
}
