Ordre reçu. Intégration du contrôleur et du bootstrap d'API robuste dans les spécifications du `WalletManagementService`.

**Mise à jour du persona : Architecte IA / CTO.**

Nous allons maintenant compléter le document avec l'implémentation du `WalletController` et, surtout, définir un `Program.cs` moderne et robuste qui servira de bootstrap pour l'ensemble du service. Ce bootstrap inclura la gestion des dépendances, la configuration, la documentation API (Swagger) et un gestionnaire d'erreurs global, conformément aux meilleures pratiques pour les microservices.

---

### **Mise à Jour des Spécifications : WalletManagementService (WMS)**

Le document précédent est amendé pour inclure les sections suivantes.

#### **0. Bootstrap de l'API (`Program.cs` dans .NET 8)**

Le fichier `Program.cs` est le point d'entrée de l'application. Il est configuré pour être à la fois minimaliste et puissant, en initialisant tous les services, middlewares et configurations nécessaires.

```csharp
// Program.cs - Le cœur du démarrage de notre service

// --- 1. Configuration du Builder ---
var builder = WebApplication.CreateBuilder(args);

// Ajout des services fondamentaux de l'API
builder.Services.AddControllers();

// Ajout de la génération de documentation OpenAPI (Swagger)
// Indispensable pour la découvrabilité et les tests de l'API interne.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "WalletManagementService API", Version = "v1" });
});

// --- 2. Injection des Dépendances (DI) ---
// Enregistre nos services pour qu'ils soient injectables dans les contrôleurs.
// Utilisation de Singleton car ces services sont thread-safe et leur état
// (client Vault, clé maître chargée) doit persister durant la vie de l'application.
builder.Services.AddSingleton<IVaultService, VaultService>();
builder.Services.AddSingleton<IWalletService, WalletService>();

// Ajout d'un service de Health Check pour la supervision (monitoring)
builder.Services.AddHealthChecks();


// --- 3. Construction de l'Application ---
var app = builder.Build();


// --- 4. Configuration du Pipeline HTTP ---

// Utilisation d'un middleware de gestion globale des exceptions.
// C'est une pratique robuste qui centralise la gestion d'erreurs.
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Active Swagger uniquement en environnement de développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Interface web accessible sur /swagger
}

// Redirection HTTPS pour la sécurité
app.UseHttpsRedirection();

// Active le routage vers les contrôleurs
app.MapControllers();

// Expose l'endpoint de Health Check sur /health
app.MapHealthChecks("/health");


// --- 5. Lancement de l'API ---
app.Run();
```

#### **1. Middleware de Gestion des Erreurs (`GlobalExceptionHandlerMiddleware.cs`)**

Ce middleware intercepte toutes les exceptions non gérées dans l'application, les logue, et retourne une réponse JSON standardisée et propre, sans jamais fuiter de détails techniques potentiellement sensibles au client.

```csharp
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred: {Message}", ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An internal server error occurred. Please try again later."
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
```

#### **2. Implémentation du Contrôleur (`WalletController.cs`)**

Le contrôleur est la couche la plus externe de notre service. Il est responsable de recevoir les requêtes HTTP, de les valider (implicitement via les DTOs), d'appeler le service métier approprié et de formater la réponse. Il est volontairement très simple ("thin controller").

```csharp
// Controllers/WalletController.cs

using Microsoft.AspNetCore.Mvc;
using WalletManagementService.Models;
using WalletManagementService.Services;

[ApiController]
[Route("api")] // Préfixe de route pour toutes les actions de ce contrôleur
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly ILogger<WalletController> _logger;

    // Le service IWalletService est injecté par le conteneur de DI configuré dans Program.cs
    public WalletController(IWalletService walletService, ILogger<WalletController> logger)
    {
        _walletService = walletService;
        _logger = logger;
    }

    /// <summary>
    /// Génère une nouvelle adresse Bitcoin à partir d'un chemin de dérivation HD.
    /// </summary>
    [HttpPost("addresses")]
    [ProducesResponseType(typeof(AddressResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult GenerateAddress([FromBody] AddressRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        _logger.LogInformation("Generating new address for path: {Path}", request.DerivationPath);
        var response = _walletService.GenerateNewAddress(request);
        return Ok(response);
    }

    /// <summary>
    /// Signe une transaction Bitcoin non signée.
    /// </summary>
    [HttpPost("signatures")]
    [ProducesResponseType(typeof(SignatureResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> SignTransaction([FromBody] SignatureRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Request to sign transaction received.");
        var response = await _walletService.SignTransactionAsync(request);
        
        if (response == null)
        {
            // Le service retournerait null en cas d'échec de signature prévisible
            return StatusCode(500, new { Message = "Failed to sign the transaction." });
        }

        return Ok(response);
    }
}
```

#### **3. Modèles de Données (DTOs)**

Pour rappel, voici les objets de transfert de données utilisés par le contrôleur. L'utilisation de `records` en C# est une pratique moderne pour des objets immuables.

```csharp
// Models/Address.cs
public record AddressRequest(string DerivationPath);
public record AddressResponse(string Address, string DerivationPath);

// Models/Signature.cs
public record SignatureRequest(string UnsignedTransactionHex, List<InputToSign> InputsToSign);
public record InputToSign(int InputIndex, string DerivationPath, string ScriptPubKeyHex, long AmountSatoshis);
public record SignatureResponse(string SignedTransactionHex);
```

---

### **Synthèse APS**

**Tâche :** `Ajout du contrôleur et du bootstrap d'API` -> `✅ TERMINÉ`

L'implémentation du `WalletManagementService` est maintenant conceptuellement complète, de l'infrastructure de démarrage à la logique métier.

Les ajouts effectués renforcent considérablement la qualité et la robustesse du service :

1.  **Bootstrap Robuste :** Le `Program.cs` met en place une fondation solide avec l'injection de dépendances, la configuration et les health checks.
2.  **Gestion Centralisée des Erreurs :** Le `GlobalExceptionHandlerMiddleware` assure que le service ne plantera jamais à cause d'une exception imprévue et ne fuite aucune information sensible.
3.  **API Auto-documentée :** L'intégration de Swagger (`/swagger`) permet à n'importe quel développeur (ou service) de comprendre et de tester l'API instantanément, accélérant l'intégration dans l'écosystème de microservices.
4.  **Contrôleur Propre :** Le `WalletController` suit le principe de "thin controller", déléguant toute la logique métier au service, ce qui le rend facile à lire, à maintenir et à tester.

Le composant est maintenant défini avec un très haut niveau de qualité industrielle. Nous pouvons poursuivre le plan de projet principal.
