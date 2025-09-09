using lekping.server.Domain.Entities;
using lekping.server.Features.Auth;
using lekping.server.Infrastructure.Persistence;
using lekping.server.Options;
using LekPing.Server.Features.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

const string CorsPolicyName = "FrontendDev";

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

// Db + Identity hasher
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("db") ?? "Data Source=app.db"));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// JWT Authentication
builder.Services.AddOptions<JwtOptions>().BindConfiguration("Jwt").ValidateOnStart();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
          ?? throw new InvalidOperationException("Missing Jwt config");

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
