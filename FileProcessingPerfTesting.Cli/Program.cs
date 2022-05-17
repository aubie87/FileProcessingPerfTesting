using FileProcessingPerfTesting.Core.Models;
using FileProcessingPerfTesting.DataGenerator;
using FileProcessingPerfTesting.XmlParser;

// Establish processing folder.
string processingFolderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FileProcessingPerfTesting");
Directory.CreateDirectory(processingFolderName);

var processingDirectory = new DirectoryInfo(processingFolderName);

// IEnumerable<FileInfo> filelist = GetExistingProcessingFiles(processingDirectory);

int fileCount = 4;
int statementCount = 3000;
IEnumerable<FileInfo> filelist = GetNewProcessingFiles(processingDirectory, fileCount, statementCount);

foreach (var file in filelist)
{
    ProcessFile(file);
}

Console.WriteLine("Processing complete!");



void ProcessFile(FileInfo file)
{
    using var xmlParser = XmlParser.CreateFromFile(file);
    Console.WriteLine($"Loaded {xmlParser.Header.InstitutionName} {xmlParser.Header.Filename}");
    foreach(var statement in xmlParser.Statements())
    {
        if(statement.OriginalId % 100 == 0)
        {
            Console.WriteLine($"{statement.Name}");
        }
    }
}

IEnumerable<FileInfo> GetNewProcessingFiles(DirectoryInfo processingDirectory, int fileCount, int statementCount)
{
    processingDirectory
        .EnumerateFiles("*.xml")
        .ToList()
        .ForEach(file => file.Delete());

    for(int i=1; i<= fileCount; i++)
    {
        string filename = Path.Combine(processingDirectory.FullName, $"statement{i}.xml");
        StatementGenerator.SaveStatementsToXml(filename, statementCount, i * 100_000);
    }

    return GetExistingProcessingFiles(processingDirectory);
}

IEnumerable<FileInfo> GetExistingProcessingFiles(DirectoryInfo processingDirectory)
{
    return processingDirectory.EnumerateFiles("*.xml");
}

