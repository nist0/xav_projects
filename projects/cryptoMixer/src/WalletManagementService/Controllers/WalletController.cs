using CryptoMixer.Shared;
using CryptoMixer.WalletManagementService.Models;
using CryptoMixer.WalletManagementService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMixer.WalletManagementService.Controllers;

[ApiController]
[Route("api")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly ILogger<WalletController> _logger;

    public WalletController(IWalletService walletService, ILogger<WalletController> logger)
    {
        _walletService = walletService;
        _logger = logger;
    }

    [HttpPost("addresses")]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GenerateAddress([FromBody] AddressRequest request)
    {
        if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

        _logger.LogInformation("Generating new address for path: {Path}", request.DerivationPath);
        Result<AddressResponse> result = _walletService.GenerateNewAddress(request);

        if (!result.IsSuccess || result.Value is null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { error = result.Error ?? "Failed to generate address." }
                );
            }

        return Ok(result.Value);
    }

    [HttpPost("signatures")]
    [ProducesResponseType(typeof(SignatureResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignTransaction([FromBody] SignatureRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Request to sign transaction received.");
        Result<SignatureResponse> result = await _walletService.SignTransactionAsync(request);

        if (!result.IsSuccess || result.Value is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = result.Error ?? "Failed to sign transaction." });
        }

        return Ok(result.Value);
    }
}
