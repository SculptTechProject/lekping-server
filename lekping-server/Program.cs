using LekPing.Server.Features.Items;
using Scalar.AspNetCore;

const string CorsPolicyName = "FrontendDev";

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services.AddSingleton<IItemsService, ItemsServiceInMemory>();

builder.Services.AddCors(o => o.AddPolicy(CorsPolicyName, p => p
    .WithOrigins("http://localhost:3000", "http://127.0.0.1:3000", "https://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
));

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    // MAPUJEMY DOK£ADNIE RAZ (jawny wzorzec œcie¿ki)
    app.MapOpenApi("/openapi/v1.json");

    // Scalar pod /scalar, czyta z /openapi/v1.json
    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "/openapi/v1.json";
        options.Title = "LekPing API";
    });
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(CorsPolicyName);

app.MapHealthChecks("/healthz");
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.Run();
