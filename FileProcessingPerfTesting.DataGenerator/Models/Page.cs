using System.Xml.Serialization;

namespace FileProcessingPerfTesting.DataGenerator.Models;

public class Page
{
    [XmlElement("Line")]
    public List<Line> Lines { get; set; } = new List<Line>();
}