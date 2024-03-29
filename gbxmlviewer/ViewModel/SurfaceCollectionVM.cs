﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbxmlviewer.ViewModel
{
    /// <summary>
    /// Collection element for surfces in the navigation tree
    /// It has no corresponding gbXML node but takes the parent of all Surfaces, the Campus node
    /// </summary>
    class SurfaceCollectionVM : NavigationElementVM
    {
        /// <summary>
        /// Extract surface childs
        /// </summary>
        protected override void updateChildrenAfterDataChange()
        {
            // Surface elements
            foreach (var surf in Data.Elements().Where(e => e.Name.LocalName == "Surface"))
            {
                children.Add(new SurfaceVM() { Data = surf });
            }
        }

        /// <summary>
        /// Override update on data change
        /// </summary>
        protected override void updateAfterDataChanged()
        {
            Name = "Surfaces";
        }
    }
}
