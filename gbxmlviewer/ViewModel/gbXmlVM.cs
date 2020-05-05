using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gbxmlviewer.ViewModel
{
    /// <summary>
    /// Class corresponds to the root level of the gbXML file
    /// It extracts elements relevant to the context of this tool: Building and Surfaces, make those available as child
    /// </summary>
    public class gbXmlVM : gbXmlElementVM
    {
        /// <summary>
        /// Extract building and surfaces
        /// </summary>
        protected override void updateChildrenAfterDataChange()
        {
            var campus = Data.Elements().Where(e => e.Name.LocalName == "Campus").FirstOrDefault();
            if(campus != null)
            {
                // Building elements
                foreach(var bldg in campus.Elements().Where(e => e.Name.LocalName == "Building"))
                {
                    children.Add(new BuildingVM() { Data = bldg });
                }

                // Surface elements
                //foreach (var surf in campus.Elements().Where(e => e.Name.LocalName == "Surface"))
                //{
                //    children.Add(new SurfaceVM() { Data = surf });
                //}
            }
        }
    }
}
