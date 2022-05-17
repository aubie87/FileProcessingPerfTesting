using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessingPerfTesting.Core.Models;
public class Job
{
    public int JobId { get; set; }
    public DateTime Start { get; set; }
    public DateTime? Finish { get; set; }
    public Guid GlobalId { get; set; } = Guid.NewGuid();
    
    public List<Statement> Statements { get; set; } = new();
    public List<Header> Headers { get; set; } = new();
}
