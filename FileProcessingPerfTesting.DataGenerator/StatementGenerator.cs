using Bogus;
using FileProcessingPerfTesting.DataGenerator.Models;
using System.Xml;
using System.Xml.Serialization;

namespace FileProcessingPerfTesting.DataGenerator;
public class StatementGenerator
{
    private readonly string[] _lineTypeList =
    {
            "decimal",
            "datetime",
            "string",
        };
    private readonly Faker<Line> _fakerLine;
    private readonly Faker<Page> _fakerPage;
    private readonly Faker<Transaction> _fakerTransaction;
    private readonly Faker<Statement> _fakerStatement;
    private int _index;

    public StatementGenerator(int startIndex)
    {
        _index = startIndex;

        _fakerLine = new Faker<Line>()
            .RuleFor(x => x.LineType, x => x.PickRandom(_lineTypeList))
            .RuleFor(x => x.ColumnCount, x => x.Random.Number(1, 4))
            .RuleFor(x => x.Text, x => $"{x.Lorem.Sentence(3, 8)} {x.Finance.Amount(0, 10000):n} {x.Lorem.Sentence(3, 9)}");

        _fakerPage = new Faker<Page>()
            .RuleFor(x => x.Lines, _fakerLine.Generate(30));

        _fakerTransaction = new Faker<Transaction>()
            .RuleFor(x => x.Id, x => x.IndexFaker)
            .RuleFor(x => x.TransType, x => x.Finance.TransactionType())
            .RuleFor(x => x.Amount, x => x.Finance.Amount(-50, 500))
            .RuleFor(x => x.Date, x => x.Date.Between(DateTime.UtcNow.AddDays(-40), DateTime.UtcNow.AddDays(-10)))
            .RuleFor(x => x.Description, x => x.Lorem.Sentence(4, 12));

        _fakerStatement = new Faker<Statement>()
            .RuleFor(p => p.Id, x => _index++)
            .RuleFor(p => p.Name, f => f.Name.FullName())
            .RuleFor(p => p.Address1, f => f.Address.StreetAddress())
            .RuleFor(p => p.Address2, f => f.Address.SecondaryAddress())
            .RuleFor(p => p.Address3, f => $"{f.Address.City()}, {f.Address.StateAbbr()}  {f.Address.ZipCode()}")
            .RuleFor(p => p.Address4, f => f.Address.Country().OrNull(f, 0.9f))
            .RuleFor(p => p.AccountName, f => f.Finance.AccountName())
            .RuleFor(p => p.AccountNumber, f => f.Finance.Account())
            .RuleFor(p => p.MessageAsString, f => f.Lorem.Sentences().OrNull(f, 0.1f))
            .RuleFor(p => p.Transactions, f => _fakerTransaction.GenerateBetween(0, 20))
            .RuleFor(p => p.Pages, f => _fakerPage.GenerateBetween(0, 4));
    }

    /// <summary>
    /// </summary>
    /// <param name="statementCount"></param>
    /// <returns></returns>
    internal IEnumerable<Statement> GenerateStatements()
    {
        while (true)
        {
            yield return _fakerStatement.Generate();
        }
    }

    public static void SaveStatementsToXml(string xmlFilepath, int count, int startIndex)
    {
        var generator = new StatementGenerator(startIndex);

        var statements = generator.GenerateStatements().Take(count).ToList();

        var header = new Header()
        {
            DocumentType = "Statement",
            InstitutionCode = "01010220202",
            InstitutionName = "Mega Bank & Trust",
            RunDate = DateTime.Now.Date, // new DateOnly(),
            StatementCount = statements.Count()
        };

        var stmtRoot = new StatementRoot()
        {
            Header = header,
            Statements = statements.ToList()
        };

        var serializer = new XmlSerializer(typeof(StatementRoot));
        var xmlSettings = new XmlWriterSettings()
        {
            Indent = true  // for legibility
        };
        using var xmlStream = XmlWriter.Create(xmlFilepath, xmlSettings);
        //using var xmlStream = XmlWriter.Create(Console.Out, xmlSettings);

        // forces root element name to be "Statements"
        //var rootStatements = new Statements();
        //rootStatements.AddRange(statements);

        serializer.Serialize(xmlStream, stmtRoot);
        xmlStream.Close();
    }
}
