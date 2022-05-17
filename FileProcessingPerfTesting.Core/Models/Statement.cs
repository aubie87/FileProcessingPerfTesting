using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessingPerfTesting.Core.Models;
public class Statement
{
    public int StatementId { get; set; }
    public int HeaderId { get; set; }
    public int JobId { get; set; }


    public int OriginalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address1 { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public string? Address3 { get; set; }
    public string? Address4 { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? Message { get; set; }

    public DateTime Saved { get; set; }

    public List<Transaction> Transactions { get; set; } = new();
    public List<Line> Lines { get; set; } = new();
}
