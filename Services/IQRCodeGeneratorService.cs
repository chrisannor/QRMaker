namespace QRMaker.Services;

public interface IQRCodeGeneratorService
{
    Task<string> GenerateQRCodeFromUrlAsync(string url, string fileName, CancellationToken cancellationToken);
}
