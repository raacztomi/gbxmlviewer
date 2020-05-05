using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace gbxmlviewer
{
    public class Repository
    {
        public XElement Root { get => _data; }

        public void LoadFile()
        {
            _data = XElement.Load("c:/Dev/OfficeBuilding.xml");
        }

        #region Private members

        private XElement _data;

        #endregion
    }
}
