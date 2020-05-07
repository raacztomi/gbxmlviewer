using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbxmlviewer.ViewModel
{
    class SurfaceVM : NavigationElementVM
    {
        /// <summary>
        /// Ignore sub items
        /// </summary>
        protected override void updateChildrenAfterDataChange()
        {
            if (Data == null)
            {
                return;
            }
            // Opening elements
            foreach (var opening in Data.Elements().Where(e => e.Name.LocalName == "Opening"))
            {
                children.Add(new OpeningVM() { Data = opening });
            }
        }
    }
}
