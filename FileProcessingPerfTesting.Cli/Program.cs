using FileProcessingPerfTesting.Core.Models;
using FileProcessingPerfTesting.Data;
using FileProcessingPerfTesting.DataGenerator;
using FileProcessingPerfTesting.XmlParser;

// Establish processing folder.
string processingFolderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FileProcessingPerfTesting");
Directory.CreateDirectory(processingFolderName);

int dateTicks = (int)DateTime.Now.Ticks;  // & 0x00000000FFFFFFFF;
string dbName = $"Job_{dateTicks:X08}.sqlite3";
string dbFilepath = Path.Combine(processingFolderName, dbName);
var job = JobContext.StartJob(dbFilepath);


var processingDirectory = new DirectoryInfo(processingFolderName);

// ToDo: Convert to service/DI based CLI.

IEnumerable<FileInfo> filelist = GetExistingProcessingFiles(processingDirectory);

//int fileCount = 4;
//int statementCount = 3000;
//IEnumerable<FileInfo> filelist = GetNewProcessingFiles(processingDirectory, fileCount, statementCount);

foreach (var file in filelist)
{
    ProcessFile(file);
}

JobContext.FinishJob(dbFilepath);
Console.WriteLine("Processing complete!");

void ProcessFile(FileInfo file)
{
    using var xmlParser = XmlParser.CreateFromFile(file);
    Console.WriteLine($"Loaded {xmlParser.Header.InstitutionName} {xmlParser.Header.Filename}");
    var header = xmlParser.Header;
    header.JobId = job.JobId;
    JobContext.SaveHeader(dbFilepath, header);

    var statementList = xmlParser.Statements().Take(100).ToList();
    while (statementList.Any())
    {
        statementList
            .ForEach(s => 
            {
                s.JobId = job.JobId;
                s.HeaderId = header.HeaderId;
            });
        
        Console.WriteLine($"Saving to DB - {statementList[0].Name}");
        JobContext.SaveStatements(dbFilepath, statementList);
        statementList = xmlParser.Statements().Take(100).ToList();
    }

    //foreach (var statement in xmlParser.Statements())
    //{
    //    statement.JobId = job.JobId;
    //    statement.HeaderId = header.HeaderId;
    //    statementList.Add(statement);
    //    if (statement.OriginalId % 100 == 0)
    //    {
    //        Console.WriteLine($"Saving to DB - {statement.Name}");
    //        JobContext.SaveStatements(dbFilepath, statementList);
    //        statementList.Clear();
    //    }
    //}

    if(statementList.Any())
    {
        Console.WriteLine($"Found {statementList.Count} unsaved statements!");
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

