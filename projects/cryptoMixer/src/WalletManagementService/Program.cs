using CryptoMixer.WalletManagementService.Middleware;
using CryptoMixer.WalletManagementService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "WalletManagementService API", Version = "v1" });
});

builder.Services.AddSingleton<IVaultService, VaultService>();
builder.Services.AddSingleton<IWalletService, WalletService>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
