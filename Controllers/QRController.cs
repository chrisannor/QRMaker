using Microsoft.AspNetCore.Mvc;
using QRMaker.Services;

namespace QRMaker.Controllers;

[ApiController]
[Route("[controller]")]
public class QRController : ControllerBase
{
    private readonly ILogger<QRController> _logger;
    private readonly IQRCodeGeneratorService _qrCodeGeneratorService;

    public QRController(ILogger<QRController> logger, IQRCodeGeneratorService qrCodeGeneratorService)
    {
        _logger = logger;
        _qrCodeGeneratorService = qrCodeGeneratorService;
    }

    [HttpGet("GenerateQRCodeFromUrl")]
    public async Task<string> Get(string url, string fileName, CancellationToken cancellationToken)
    {
        return await _qrCodeGeneratorService.GenerateQRCodeFromUrlAsync(url, fileName, cancellationToken);
    }

    [HttpGet("DownloadQRCode")]
    public async Task<IActionResult> Download(string url, string fileName, CancellationToken cancellationToken)
    {
        string filePath = await _qrCodeGeneratorService.GenerateQRCodeFromUrlAsync(url, fileName, cancellationToken);

        if (System.IO.File.Exists(filePath))
        {
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath, cancellationToken);
            return File(fileBytes, "image/svg+xml", fileName);
        }

        return NotFound();
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        _logger.LogInformation("Health check endpoint hit.");
        return Ok("Service is running");
    }
}