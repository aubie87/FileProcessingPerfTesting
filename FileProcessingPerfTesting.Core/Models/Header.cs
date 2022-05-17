using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessingPerfTesting.Core.Models;

/// <summary>
/// Originating file name and header info for each statement.
/// </summary>
public class Header
{
    public int HeaderId { get; set; }
    public int JobId { get; set; }


    public string? InstitutionCode { get; set; }
    public DateTime RunDate { get; set; }
    public string? InstitutionName { get; set; }
    public string? DocumentType { get; set; }
    public int ExpectedStatementCount { get; set; }
    public string? Filename { get; set; }

    public DateTime Saved { get; set; }
}
