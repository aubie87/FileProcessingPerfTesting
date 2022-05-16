using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileProcessingPerfTesting.DataGenerator.Models;
public class StatementRoot
{
    public StatementRoot()
    {
        Header = new Header();
        Statements = new List<Statement>();
    }

    public Header Header { get; init; }

    /// <summary>
    /// Force results to root instead of child collection.
    /// </summary>
    [XmlElement("Statement")]
    public List<Statement> Statements { get; init; }

}
