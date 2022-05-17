namespace FileProcessingPerfTesting.XmlParser.Models;

public class Header
{
    public string? InstitutionCode { get; internal set; }
    public DateTime RunDate { get; internal set; }
    public string? InstitutionName { get; internal set; }
    public string? DocumentType { get; internal set; }
    public int ExpectedStatementCount { get; internal set; }
}
