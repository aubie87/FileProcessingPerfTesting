using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessingPerfTesting.Core.Models;
public class Transaction
{
    public int TransactionId { get; set; }
    public int StatementId { get; set; }

    public int OriginalId { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date1 { get; set; }
    public DateOnly Date2 { get; set; }
    public string Description { get; set; } = string.Empty;
}
