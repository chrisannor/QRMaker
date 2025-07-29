namespace QRMaker.Services;

public class FileService : IFileService
{
    private const string DOWNLOAD_FOLDER = "GeneratedCodes";
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }

    public async Task<string> SaveFileAsync(string content, string fileName, CancellationToken cancellationToken)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), DOWNLOAD_FOLDER, fileName);

        try
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                await writer.WriteAsync(content);
            }
            _logger.LogInformation("File saved successfully at {FilePath}", filePath);
            return filePath;
        }
        catch (IOException ioEx)
        {
            _logger.LogError(ioEx, "IO error while saving file at {FilePath}", filePath);
            throw;
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("File saving was canceled.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving file at {FilePath}", filePath);
            throw;
        }
    }
}

public interface IFileService
{
    Task<string> SaveFileAsync(string content, string fileName, CancellationToken cancellationToken);
}