using Scalar.AspNetCore;

const string CorsPolicyName = "FrontendDev";

var builder = WebApplication.CreateBuilder(args);

// --- Services ---
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
        policy
            .WithOrigins(
                "http://localhost:3000",
                "http://127.0.0.1:3000",
                "https://localhost:3000") // gdy uruchomisz front na https
            .AllowAnyHeader()
            .AllowAnyMethod()
    // .AllowCredentials() // w³¹cz, jeœli u¿ywasz cookies; pamiêtaj o HTTPS + SameSite=None
    );
});

var app = builder.Build();

// --- Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();           // /openapi/v1.json
    app.MapOpenApi();             // /openapi/v1.json
    app.MapScalarApiReference();  // UI pod /scalar
}
else
{
    app.UseHttpsRedirection();  // w DEV nie wymuszamy HTTPS
}

app.UseCors(CorsPolicyName);

app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.MapHealthChecks("/healthz");
app.MapControllers();

app.Run();
