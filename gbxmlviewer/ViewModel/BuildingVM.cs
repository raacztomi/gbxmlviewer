using gbxmlviewer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbxmlviewer
{
    public class BuildingVM : NavigationElementVM
    {
        /// <summary>
        /// Extract spaces 
        /// </summary>
        protected override void updateChildrenAfterDataChange()
        {
            // Spaces elements
            foreach (var space in Data.Elements().Where(e => e.Name.LocalName == "Space"))
            {
                children.Add(new SpaceVM() { Data = space });
            }
        }
    }
}
