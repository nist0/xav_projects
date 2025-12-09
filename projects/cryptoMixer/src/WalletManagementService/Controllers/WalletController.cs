using CryptoMixer.Shared;
using CryptoMixer.WalletManagementService.Models;
using CryptoMixer.WalletManagementService.Services;
using Microsoft.AspNetCore.Mvc;
using NBitcoin;

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

    [HttpPost("wallets/new")]
    [ProducesResponseType(typeof(CreateWalletResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateWallet([FromBody] CreateWalletRequest? request)
    {
        _logger.LogInformation("Create wallet request received.");
        var network = request?.Network;
        var import = request?.ImportToVaultIfDisabled ?? true;

        var result = await _walletService.CreateWalletAsync(network, import);
        if (!result.IsSuccess || result.Value is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = result.Error ?? "Failed to create wallet." });
        }

        return Ok(result.Value);
    }

    [HttpPost("wallets/verify")]
    [ProducesResponseType(typeof(VerifyWalletResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult VerifyWallet([FromBody] VerifyWalletRequest request)
    {
        try
        {
            var net = request.Network is not null && request.Network.Equals("testnet", StringComparison.OrdinalIgnoreCase)
                ? Network.TestNet
                : Network.Main;

            ExtKey? extKey = null;

            if (!string.IsNullOrWhiteSpace(request.Mnemonic))
            {
                var m = new Mnemonic(request.Mnemonic);
                extKey = m.DeriveExtKey();
            }
            else if (!string.IsNullOrWhiteSpace(request.XPrv))
            {
                extKey = ExtKey.Parse(request.XPrv, net);
            }

            string? derivedXPrv = null;
            string? derivedXPub = null;
            string? derivedFirstAddress = null;

            if (extKey is not null)
            {
                derivedXPrv = extKey.ToString(net);
                derivedXPub = extKey.Neuter().ToString(net);
                var keyPath = new KeyPath("m/0/0/0");
                derivedFirstAddress = extKey.Neuter().Derive(keyPath).PubKey.GetAddress(ScriptPubKeyType.Segwit, net).ToString();
            }

            var resp = new VerifyWalletResponse(
                MnemonicMatchesXPrv: !string.IsNullOrWhiteSpace(request.Mnemonic) && derivedXPrv is not null && derivedXPrv == request.XPrv,
                XPrvMatches: !string.IsNullOrWhiteSpace(request.XPrv) && derivedXPrv is not null && derivedXPrv == request.XPrv,
                XPubMatches: !string.IsNullOrWhiteSpace(request.XPub) && derivedXPub is not null && derivedXPub == request.XPub,
                AddressMatches: !string.IsNullOrWhiteSpace(request.FirstAddress) && derivedFirstAddress is not null && derivedFirstAddress == request.FirstAddress,
                DerivedXPrv: derivedXPrv,
                DerivedXPub: derivedXPub,
                DerivedFirstAddress: derivedFirstAddress,
                Message: "Verification completed"
            );

            return Ok(resp);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
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
