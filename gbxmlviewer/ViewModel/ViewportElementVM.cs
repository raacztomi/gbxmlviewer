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
        public ViewportElementVM(NavigationElementVM refElem, List<GeometryModel3D> geom, Material material = null)
        {
            RefElem = refElem;
            GeometryElements = geom != null ? geom : new List<GeometryModel3D>();
            _material = material != null ? material : DefaultMaterial;
            _setGeometryMaterial();

            // Mechanism to sync selection
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
            NotifyPropertyChanged(nameof(Children));
        }

        /// <summary>
        /// Reference to element shown
        /// </summary>
        public NavigationElementVM RefElem
        {
            get;
            private set;
        }

        /// <summary>
        /// Queries and sets the visibility of the element
        /// Visibility is stored in a flag and achieved by setting the material null, the two are synchronized,
        /// which enables to reset hidden state after selection is removed
        /// </summary>
        private bool _isVisible = true;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                if(_isVisible != value)
                {
                    _isVisible = value;
                    _setGeometryMaterial();
                    NotifyPropertyChanged();

                    foreach (var child in Children)
                    {
                        child.IsSelected = value;
                    }
                }
            }
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
                    _setGeometryMaterial();
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
                    _setGeometryMaterial();
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
            protected set;
        }

        /// <summary>
        /// Should be called when settings affecting material change (visibility, selection, material itself)
        /// </summary>
        private void _setGeometryMaterial()
        {
            Material material = _material;
            if(!_isVisible)
            {
                material = null;
            }
            if(_isSelected)
            {
                material = SelectionMaterial;
            }
            foreach (var geom in GeometryElements)
            {
                geom.Material = material;
                geom.BackMaterial = material;
            }
        }

        /// <summary>
        /// Default material of the viewport elements
        /// </summary>
        private static Material _defaultMaterial = MaterialHelper.CreateMaterial(Color.FromArgb(64, 192, 192, 192));
        public virtual Material DefaultMaterial
        {
            get { return _defaultMaterial; }
        }

        /// <summary>
        /// Selection material of the viewport elements
        /// </summary>
        private static Material _selectionMaterial = MaterialHelper.CreateMaterial(Colors.Yellow);
        public virtual Material SelectionMaterial
        {
            get { return _selectionMaterial; }
        }

    }
}
