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
using HelixToolkit.Wpf;

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
        /// Map of current elements by its RefElem
        /// </summary>
        private Dictionary<object, ViewportElementVM> _elementMap = new Dictionary<object, ViewportElementVM>();

        /// <summary>
        /// Geometry items
        /// </summary>
        private Model3DCollection _geometryCollection = new Model3DCollection();
        public Model3DCollection GeometryCollection
        {
            get
            {
                return _geometryCollection;
            }
        }

        public void Clear()
        {
            _elementMap.Clear();
            _geometryCollection.Clear();
            NotifyPropertyChanged(nameof(GeometryCollection));
        }

        public void AddElem(ViewportElementVM elem)
        {
            if(!_elementMap.ContainsKey(elem.RefElem))
            {   // Not already added
                _elementMap[elem.RefElem] = elem;
                foreach (var geom in elem.GeometryElements)
                {
                    _geometryCollection.Add(geom);
                }
            }
            foreach(var child in elem.Children)
            {
                AddElem(child);
            }
            NotifyPropertyChanged(nameof(GeometryCollection));
        }

        /// <summary>
        /// Sets visibility based on reference element type of the contained ViewportElementVMs
        /// </summary>
        public void SetRefElemVisibility<TNavVM>(bool visible)
        {
            foreach(var elem in _elementMap.Where(e => e.Key is TNavVM).Select(e => e.Value))
            {
                elem.IsVisible = visible;
            }
        }
    }
}
