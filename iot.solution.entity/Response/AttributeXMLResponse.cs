using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iot.solution.entity
{
    public class AttributeXMLResponse
    {
        public string key { get; set; }
        public decimal value { get; set; }
    }

   

    public class AttributeXMLData
    {
        [XmlElement("attribute")]
        public List<AttributeXMLResponse> AttributeList { get; set; }
    }
}
