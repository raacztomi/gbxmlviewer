using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbxmlviewer.ViewModel
{
    class SurfaceVM : gbXmlElementVM
    {
        /// <summary>
        /// Ignore sub items
        /// </summary>
        protected override void updateChildrenAfterDataChange()
        {
        }
    }
}
