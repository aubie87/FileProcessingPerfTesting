using FileProcessingPerfTesting.Core.Models;
using FileProcessingPerfTesting.XmlParser.Readers;
using System.Xml;

namespace FileProcessingPerfTesting.XmlParser;
public class XmlParser : IDisposable
{
    private bool _disposedValue = false;
    private readonly XmlReader _reader;

    public Header Header { get; }

    private XmlParser(XmlReader reader, Header header)
    {
        _reader = reader;
        Header = header;
    }

    static public XmlParser CreateFromFile(FileInfo xmlFile)
    {
        var reader = XmlReader.Create(xmlFile.FullName);
        reader.MoveToContent();
        
        Header header = LoadHeader(reader);
        header.Filename = xmlFile.Name;

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

    public IEnumerable<Statement> Statements()
    {
        while (_reader.Read())
        {
            if (_reader.NodeType == XmlNodeType.Element && _reader.Name == "Statement")
            {
                Statement statement = StatementReader.Load(_reader);
                yield return statement;
            }
            else if (_reader.NodeType == XmlNodeType.Element)
            {
                // any other element at this level is an error
                throw new InvalidOperationException($"Unexpected element {_reader.Name}.");
            }
        }
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
                        Console.WriteLine($"Uknown <Header> element {reader.Name}");
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
