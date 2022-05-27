using FileProcessingPerfTesting.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessingPerfTesting.Data;


public class JobContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Header> Headers => Set<Header>();
    public DbSet<Statement> Statements => Set<Statement>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Line> Lines => Set<Line>();

    public static Job StartJob(string dbFilepath)
    {
        using var context = Create(dbFilepath);
        context.Jobs.Add(new Job());
        context.SaveChanges();
        return context.Jobs.First();
    }

    public static void FinishJob(string dbFilepath)
    {
        using var context = Create(dbFilepath);
        var job = context.Jobs.First();
        job.Finish = DateTime.UtcNow;
        context.SaveChanges();
    }

    public static void SaveHeader(string dbFilepath, Header header)
    {
        using var context = Create(dbFilepath);
        context.Add(header);
        context.SaveChanges();
    }

    public static int SaveStatements(string dbFilepath, IEnumerable<Statement> statementList)
    {
        using var context = Create(dbFilepath);
        context.AddRange(statementList);
        return context.SaveChanges();
    }

    private static async Task<JobContext> CreateAsync(string dbFilename)
    {
        string connectionString = $"Data Source={dbFilename}";
        var context = new JobContext(connectionString);
        await context.Database.EnsureCreatedAsync();

        return context;
    }

    private static JobContext Create(string dbFilename)
    {
        string connectionString = $"Data Source={dbFilename}";
        var context = new JobContext(connectionString);
        context.Database.EnsureCreated();

        return context;
    }

    private JobContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite(_connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>()
            .Property(e => e.Start)
            .HasDefaultValueSql("datetime('now')");

        modelBuilder.Entity<Header>()
            .Property(e => e.Saved)
            .HasDefaultValueSql("datetime('now')");

        modelBuilder.Entity<Statement>()
            .Property(e => e.Saved)
            .HasDefaultValueSql("datetime('now')");
    }
}
