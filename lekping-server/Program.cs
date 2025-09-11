using lekping.server.Domain.Entities;
using lekping.server.Features.Auth;
using lekping.server.Features.Meds.Services;
using lekping.server.Features.Push.Service;
using lekping.server.Infrastructure.Persistence;
using lekping.server.Options;
using LekPing.Server.Features.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json.Serialization;
using static lekping.server.Features.Push.Service.PushService;

const string CorsPolicyName = "FrontendDev";

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

// Meds
builder.Services.AddScoped<IMedsService, MedsService>();

// Push Notifications
builder.Services.Configure<VapidOptions>(
    builder.Configuration.GetSection("Vapid"));
builder.Services.AddScoped<PushService>();

// Db + Identity hasher
var cs = builder.Configuration.GetConnectionString("Default");

// sanity log bez hasła
var safe = new Npgsql.NpgsqlConnectionStringBuilder(cs) { Password = "***" }.ToString();
Console.WriteLine($"[DB] {safe}");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(cs));


builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// JWT Authentication
builder.Services.AddOptions<JwtOptions>().BindConfiguration("Jwt").ValidateOnStart();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
          ?? throw new InvalidOperationException("Missing Jwt config");

// JSON + Enum as string
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// AuthN + AuthZ
builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(o =>
  {
      o.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidIssuer = jwt.Issuer,
          ValidateAudience = true,
          ValidAudience = jwt.Audience,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
          ValidateLifetime = true,
          ClockSkew = TimeSpan.FromMinutes(1)
      };
  });

builder.Services.AddAuthorization(); // policies should be added later!

// CORS
builder.Services.AddCors(o => o.AddPolicy(CorsPolicyName, p => p
    .WithOrigins("http://localhost:3000", "http://127.0.0.1:3000", "https://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
));

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    // MAPUJEMY DOKŁADNIE RAZ (jawny wzorzec ścieżki)
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

// Sanity check
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.OpenConnectionAsync();
        await db.Database.CloseConnectionAsync();
        Console.WriteLine("[DB] Connection OK");
    }
    catch (Exception ex)
    {
        Console.WriteLine("[DB] Connection FAILED: " + ex.Message);
        throw;
    }
}

app.UseCors(CorsPolicyName);

// AuthN + AuthZ
app.UseAuthentication();
app.UseAuthorization();

// Health & Metrics
app.MapHealthChecks("/healthz");
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

// Controllers
app.MapControllers();

// Run
app.Run();

namespace lekping.server
{
    public partial class Program { }
}
