using FileProcessingPerfTesting.DataGenerator;

// Establish processing folder.
string processingFolderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FileProcessingPerfTesting");
Directory.CreateDirectory(processingFolderName);

var processingDirectory = new DirectoryInfo(processingFolderName);
int fileCount = 4;
int statementCount = 1000;

IEnumerable<FileInfo> filelist = GetExistingProcessingFiles(processingDirectory);
//IEnumerable<FileInfo> filelist = GetNewProcessingFiles(processingDirectory, fileCount, statementCount);

foreach (var file in filelist)
{
    Console.WriteLine($"Processing {file.Name}");
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

