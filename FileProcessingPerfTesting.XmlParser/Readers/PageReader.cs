using FileProcessingPerfTesting.Core.Models;
using System.Xml;

namespace FileProcessingPerfTesting.XmlParser.Readers;

internal class PageReader
{
    internal static Page Load(XmlReader reader)
    {
        if (reader.NodeType != XmlNodeType.Element || reader.Name != "Page")
        {
            throw new InvalidOperationException($"Expected <Statement> element, found {reader.NodeType}");
        }

        var page = new Page();

        if (reader.IsEmptyElement)
        {
            return page;
        }

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element
                && reader.Name == "Line")
            {
                // Todo: get this working and test it.
                page.Lines.AddRange(LineReader.Load(reader));
            }

            if (reader.NodeType == XmlNodeType.EndElement)
            {
                break;
            }
        }

        return page;
    }
}