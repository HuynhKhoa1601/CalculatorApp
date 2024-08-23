using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace CalculatorApp.Models
{
    [XmlRoot("Maths")]
    public class Maths
    {
        [XmlElement("Operation")]
        [JsonPropertyName("Operations")]
        public List<Operation> Operations { get; set; } = new();

        public class Operation
        {
            [XmlAttribute("ID")]
            public string Id { get; set; }

            [XmlElement("Value")]
            public List<string> Values { get; set; } = new();

            [XmlElement("Operation")]
            public List<Operation> NestedOperations { get; set; } = new();
        }
    }
}
