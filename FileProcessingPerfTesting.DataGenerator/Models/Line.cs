using System.Xml;
using System.Xml.Serialization;

namespace FileProcessingPerfTesting.DataGenerator.Models;

public class Line
{
    [XmlAttribute("type")]
    public string LineType { get; set; } = string.Empty;

    [XmlAttribute("columns")]
    public int ColumnCount { get; set; }

    [XmlIgnore]
    public string Text { get; set; } = string.Empty;

    [XmlElement("Text")]
    public XmlCDataSection? TextAsCdata
    {
        get { return string.IsNullOrEmpty(Text) ? null : new XmlDocument().CreateCDataSection(Text); }
        init => Text = value == null ? string.Empty : string.IsNullOrEmpty(value.Value) ? string.Empty : value.Value;
    }
}