using FileProcessingPerfTesting.XmlParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileProcessingPerfTesting.XmlParser;
public class XmlParser : IDisposable
{
    private bool _disposedValue = false;
    private XmlReader _reader;

    public Header Header { get; }

    private XmlParser(XmlReader reader, Header header)
    {
        _reader = reader;
        Header = header;
    }

    static public XmlParser CreateFromFile(FileInfo xmlFile)
    {
        var reader = XmlReader.Create(xmlFile.FullName);
        var node = reader.MoveToContent();
        Header header = LoadHeader(reader);

        return new XmlParser(reader, header);
    }

    private static Header LoadHeader(XmlReader reader)
    {
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element
                && reader.Name == "Header")
            {
                return ReadHeaderElements(reader);
            }
            else if (reader.NodeType is XmlNodeType.Element or XmlNodeType.EndElement)
            {
                break;
            }
        }
        throw new Exception("Invalid XML");
    }

    private static Header ReadHeaderElements(XmlReader reader)
    {
        if (reader.NodeType != XmlNodeType.Element || reader.Name != "Header")
        {
            throw new InvalidOperationException($"Expected <Header> element, found {reader.NodeType}");
        }
        var header = new Header();

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.Name)
                {
                    case "InstitutionCode":
                        header.InstitutionCode = reader.ReadElementContentAsString();
                        break;
                    case "InstitutionName":
                        header.InstitutionName = reader.ReadElementContentAsString();
                        break;
                    case "RunDate":
                        header.RunDate = reader.ReadElementContentAsDateTime();
                        break;
                    case "DocumentType":
                        header.DocumentType = reader.ReadElementContentAsString();
                        break;
                    case "StatementCount":
                        header.ExpectedStatementCount = reader.ReadElementContentAsInt();
                        break;
                    default:
                        System.Console.WriteLine($"Uknown <Header> element {reader.Name}");
                        break;
                }
            }
            else if (reader.NodeType == XmlNodeType.EndElement)
            {
                break;
            }
        }

        return header;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                _reader.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~XmlParser()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
