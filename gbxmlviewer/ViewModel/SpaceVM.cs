using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbxmlviewer.ViewModel
{
    public class SpaceVM : gbXmlElementVM
    {
        /// <summary>
        /// Ignore sub items
        /// </summary>
        protected override void updateChildrenAfterDataChange()
        {
        }
    }
}
