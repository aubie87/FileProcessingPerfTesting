// Establish processing folder.
using FileProcessingPerfTesting.DataGenerator;

string processingFolderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FileProcessingPerfTesting");
Directory.CreateDirectory(processingFolderName);

var processingDirectory = new DirectoryInfo(processingFolderName);

string testFile = Path.Combine(processingDirectory.FullName, "text.xml");
StatementGenerator.SaveStatementsToXml(testFile, 1000, 100_000);
