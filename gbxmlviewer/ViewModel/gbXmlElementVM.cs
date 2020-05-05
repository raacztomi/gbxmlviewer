﻿using System;
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
    public class gbXmlElementVM : INotifyPropertyChanged
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

        private void _updateAfterDataChange()
        {
            // Update Name
            Name = _data.Name.LocalName;
            NotifyPropertyChanged("Name");

            // Update ID
            var id = _data.Attributes().ToList();
            ID = _data.Attribute("id")?.Value;
            NotifyPropertyChanged("ID");
            NotifyPropertyChanged("IDVisibility");

            // Update children
            children.Clear();
            updateChildrenAfterDataChange();
            NotifyPropertyChanged("Children");

            // Call child implementations
            updateAfterDataChanged();
        }

        /// <summary>
        /// Updates Children upon Data change.
        /// It adds items into the protected children field (which is exposed as Childrend property and is cleared at this point)
        /// Default implementation list all immediate child elements but this can be overriden
        /// </summary>
        protected virtual void updateChildrenAfterDataChange()
        {
            foreach(var elem in _data.Elements())
            {
                children.Add(new gbXmlElementVM() { Data = elem });
            }
        }


        /// <summary>
        /// Virtual function to implement in child classes following changes in Data
        /// </summary>
        protected virtual void updateAfterDataChanged()
        {
            // Default implementation is empty
        }
    }
}
