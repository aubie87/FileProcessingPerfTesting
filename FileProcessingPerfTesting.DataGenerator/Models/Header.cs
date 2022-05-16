using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileProcessingPerfTesting.DataGenerator.Models;
public class Header
{
    public string InstitutionCode { get; init; } = string.Empty;
    public string InstitutionName { get; set; } = string.Empty;

    [XmlElement(DataType = "date")]
    public DateTime RunDate { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public int StatementCount { get; init; } = 0;

}
