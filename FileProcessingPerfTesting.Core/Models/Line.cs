namespace FileProcessingPerfTesting.Core.Models;

public class Line
{
    public int LineId { get; set; }
    public int StatementId { get; set; }

    public string LineType { get; set; } = string.Empty;
    public int ColumnCount { get; set; }
    public string Text { get; set; } = string.Empty;
}