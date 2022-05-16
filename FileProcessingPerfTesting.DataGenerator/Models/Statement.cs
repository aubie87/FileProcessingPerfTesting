using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FileProcessingPerfTesting.DataGenerator.Models;
public class Statement
{
    public int Id { get; set; }

    public string Name { get; init; } = string.Empty;
    public string Address1 { get; init; } = string.Empty;
    public string Address2 { get; init; } = string.Empty;
    public string? Address3 { get; init; }
    public string? Address4 { get; init; }
    public string AccountName { get; init; } = string.Empty;
    public string AccountNumber { get; init; } = string.Empty;

    [XmlIgnore]
    public string? MessageAsString { get; init; }

    [XmlElement("Message")]
    public XmlCDataSection? MessageAsCdata
    {
        get { return string.IsNullOrEmpty(MessageAsString) ? null : new XmlDocument().CreateCDataSection(MessageAsString); }
        init => MessageAsString = value?.Value;
    }
    public List<Transaction> Transactions { get; set; } = new();

    [XmlElement("Page")]
    public List<Page> Pages { get; set; } = new List<Page>();


    public override string ToString()
    {
        return $"{Name} {Address1} {Address2} {Address3} {AccountName} {AccountNumber}{Environment.NewLine}    [{MessageAsString}]{Environment.NewLine}    Transactions: {Transactions.Count}";
    }

}
