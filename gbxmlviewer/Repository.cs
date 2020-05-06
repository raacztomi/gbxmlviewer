using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace gbxmlviewer
{
    public class Repository
    {
        /// <summary>
        /// The content of the xml file
        private XElement _data;
        /// </summary>
        public XElement Root { get => _data; }

        /// <summary>
        /// Loading the given xml file
        /// </summary>
        public bool LoadFile(string fileName)
        {
            try
            {
                _data = XElement.Load(fileName);
            }
            catch
            {
                _data = null;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Indicates whether there is gbXML data in the repository
        /// </summary>
        public bool IsGbXml
        {
            get
            {
                return _data != null && _data.Name.LocalName == "gbXML";
            }
        }
    }
}
