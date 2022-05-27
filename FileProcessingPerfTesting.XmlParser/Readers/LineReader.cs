using FileProcessingPerfTesting.Core.Models;
using System.Xml;

namespace FileProcessingPerfTesting.XmlParser.Readers;

internal class LineReader
{
    internal static List<Line> Load(XmlReader reader)
    {
        if (reader.NodeType != XmlNodeType.Element || reader.Name != "Line")
        {
            throw new InvalidOperationException($"Expected <Line> element, found {reader.NodeType}");
        }

        var lines = new List<Line>();

        if (reader.IsEmptyElement)
        {
            return lines;
        }

        // we are already on a line - parse it
        lines.Add(LoadLine(reader));

        // read more lines
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element
                && reader.Name == "Line")
            {
                lines.Add(LoadLine(reader));
            }
            else if (reader.NodeType == XmlNodeType.EndElement)
            {
                break;
            }
        }

        return lines;
    }

    private static Line LoadLine(XmlReader reader)
    {
        if (reader.NodeType != XmlNodeType.Element || reader.Name != "Line")
        {
            throw new InvalidOperationException($"Expected <Transactions> element, found {reader.NodeType}");
        }

        var line = new Line();
        line.LineType = reader.GetAttribute("type") ?? string.Empty;
        line.ColumnCount = int.Parse(reader.GetAttribute("columns") ?? "0");

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element
                && reader.Name == "Text")
            {
                // ToDo: Parse for a possible decimal value from the line and store in Amount.
                line.Text = reader.ReadElementContentAsString();
            }
            else if (reader.NodeType == XmlNodeType.EndElement
                && reader.Name == "Line")
            {
                break;
            }
        }

        return line;
    }
}