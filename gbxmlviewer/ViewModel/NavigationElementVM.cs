using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows;

namespace gbxmlviewer.ViewModel
{
    /// <summary>
    /// Base class for gbXML element based view model classes
    /// It implements basic things such as reference to wrapped gbXml element, id, child elements
    /// </summary>
    public class NavigationElementVM : INotifyPropertyChanged
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
        /// Name of the element (typically node name)
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// ID of the gbXML element
        /// (may be null or empty)
        /// </summary>
        public string ID
        {
            get;
            protected set;
        }

        /// <summary>
        /// Indicates if the element is selected in the view
        /// </summary>
        bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// The visibility of the ID (depends on if it is empty or null)
        /// </summary>
        public Visibility IDVisibility
        {
            get
            {
                return string.IsNullOrEmpty(ID) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// Collection of children elements
        /// </summary>
        protected List<object> children = new List<object>();
        public ReadOnlyCollection<object> Children
        {
            get
            {
                return new ReadOnlyCollection<object>(children);
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
                _updateAfterDataChange();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Update the viewmodel on data change
        /// </summary>
        private void _updateAfterDataChange()
        {
            // Update self
            updateAfterDataChanged();

            // Update children
            children.Clear();
            updateChildrenAfterDataChange();
            NotifyPropertyChanged(nameof(Children));
        }

        /// <summary>
        /// Updates Children upon Data change.
        /// It adds items into the protected children field (which is exposed as Childrend property and is cleared at this point)
        /// Default implementation list all immediate child elements but this can be overriden
        /// Watvh for Data being null!
        /// </summary>
        protected virtual void updateChildrenAfterDataChange()
        {
            if(_data == null)
            {
                return;
            }
            foreach(var elem in _data.Elements())
            {
                children.Add(new NavigationElementVM() { Data = elem });
            }
        }


        /// <summary>
        /// Virtual function to implement in child classes following changes in Data
        /// </summary>
        protected virtual void updateAfterDataChanged()
        {
            if(_data == null)
            {
                Name = string.Empty;
                ID = string.Empty;
                children.Clear();
                _isSelected = false;
                NotifyPropertyChanged(nameof(Name));
                NotifyPropertyChanged(nameof(ID));
                NotifyPropertyChanged(nameof(IDVisibility));
                return;
            }

            // Update Name
            Name = _data.Name.LocalName;
            NotifyPropertyChanged(nameof(Name));

            // Update ID
            var id = _data.Attributes().ToList();
            ID = _data.Attribute("id")?.Value;
            NotifyPropertyChanged(nameof(ID));
            NotifyPropertyChanged(nameof(IDVisibility));
        }
    }
}
