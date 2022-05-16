namespace FileProcessingPerfTesting.DataGenerator.Models;

public class Transaction
{
    public int Id { get; set; }
    public string TransType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
}