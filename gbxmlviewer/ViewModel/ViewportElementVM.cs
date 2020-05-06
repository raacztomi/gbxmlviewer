using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace gbxmlviewer.ViewModel
{
    /// <summary>
    /// View model for geometry elements in the viewport
    /// </summary>
    public class ViewportElementVM : INotifyPropertyChanged
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
        /// Construct with reference element
        /// </summary>
        public ViewportElementVM(GbXmlElementVM refElem, List<GeometryModel3D> geom, Material material = null)
        {
            RefElem = refElem;
            GeometryElements = geom;
            Material = material != null ? material : ViewportVM.DefaultMaterial;

            // Sync selection
            refElem.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (sender == refElem && e.PropertyName == "IsSelected")
                {
                    this.IsSelected = refElem.IsSelected;
                }
            };
        }

        /// <summary>
        /// The children viewport elements belonging to this one conceptually
        /// Allows changing visualization in groups
        /// Tree like structure expected (one elem should be child to one parent)
        /// </summary>
        private List<ViewportElementVM> _children = new List<ViewportElementVM>();
        public IReadOnlyList<ViewportElementVM> Children
        {
            get
            {
                return _children;
            }
        }

        /// <summary>
        /// Set the children elements
        /// </summary>
        /// <param name="children"></param>
        public void SetChildren(List<ViewportElementVM> children)
        {
            _children = children;
            NotifyPropertyChanged("Children");
        }

        /// <summary>
        /// Reference to element shown
        /// </summary>
        public GbXmlElementVM RefElem
        {
            get;
            private set;
        }

        /// <summary>
        /// Material of the element
        /// </summary>
        private Material _material;
        public Material Material
        {
            get
            {
                return _material;
            }
            set
            {
                if(_material != value)
                {
                    _material = value;
                    if(!_isSelected)
                    {
                        foreach (var elem in GeometryElements)
                        {
                            elem.Material = _material;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Indicates whether the element is selected in the viewport
        /// </summary>
        private bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if(_isSelected != value)
                {
                    _isSelected = value;
                    foreach (var elem in GeometryElements)
                    {
                        elem.Material = _isSelected ? ViewportVM.SelectionMaterial : _material;
                    }
                    NotifyPropertyChanged();

                    foreach (var child in Children)
                    {
                        child.IsSelected = value;
                    }
                }
            }
        }

        /// <summary>
        /// The geometry elements assotiated with this item
        /// Might be empty
        /// </summary>
        public List<GeometryModel3D> GeometryElements
        {
            get;
            private set;
        }
    }
}
