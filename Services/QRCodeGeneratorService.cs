using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace QRMaker.Services;

public class QRCodeGeneratorService : IQRCodeGeneratorService
{
    private readonly ILogger<QRCodeGeneratorService> _logger;
    private readonly IFileService _fileService;

    public QRCodeGeneratorService(ILogger<QRCodeGeneratorService> logger, IFileService fileService)
    {
        _logger = logger;
        _fileService = fileService;
    }

    public async Task<string> GenerateQRCodeFromUrlAsync(string url, string fileName, CancellationToken cancellationToken)
    {
        QRCodeGenerator gen = new QRCodeGenerator();
        string filePath;

        _logger.LogDebug("Starting QR code generation for URL: {Url}", url);

        using (gen)
        {
            QRCodeData data = gen.CreateQrCode(url, QRCodeGenerator.ECCLevel.H);

            using (data)
            {
                QRCode code = new QRCode(data);
                using (code)
                {
                    try
                    {
                        var svg = new SvgQRCode(data);

                        // string path = Path.Combine(Directory.GetCurrentDirectory(), "generatedCodes");
                        // if (!Directory.Exists(path))
                        // {
                        //     Directory.CreateDirectory(path);
                        // }
                        // if (string.IsNullOrEmpty(fileName))
                        //     fileName = $"{Guid.NewGuid()}";

                        // filePath = Path.Combine(path, $"{fileName}.svg");
                        string graphic = svg.GetGraphic(20);

                        // using (StreamWriter sw = new(filePath))
                        // {
                        //     await sw.WriteAsync(graphic);
                        // }
                        filePath = await _fileService.SaveFileAsync(graphic, $"{fileName}.svg", cancellationToken);
                    }
                    catch (IOException ioEx)
                    {
                        _logger.LogError(ioEx, "IO error while writing QR code to file");
                        throw;
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogInformation("QR code generation was canceled.");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error generating QR code");
                        throw;
                    }
                }
            }
        }

        return filePath;
    }
}
