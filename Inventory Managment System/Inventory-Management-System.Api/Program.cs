using System;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Application.Ports.Outbound;
using Inventory_Management_System.Api.Application.Services;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Management_System.Api.Infrastructure.Auth;
using Inventory_Management_System.Api.Infrastructure.Persistence;
using Inventory_Management_System.Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Register controllers and JSON configuration
builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo { Title = "Inventory Management API", Version = "v1" }
    );

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter only the JWT token. " + "Swagger will add the Bearer prefix.",
        }
    );

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
    });
});

//Data files
string dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "Data");

Directory.CreateDirectory(dataDirectory);
string productsFilePath = Path.Combine(dataDirectory, "products.json");
string transactionsFilePath = Path.Combine(dataDirectory, "transactions.json");
string usersFilePath = Path.Combine(dataDirectory, "users.json");

//Rpositories
builder.Services.AddSingleton<IProductRepository>(_ => new ProductRepository(productsFilePath));
builder.Services.AddSingleton<ITransactionRepository>(_ => new TransactionRepository(
    transactionsFilePath
));
builder.Services.AddSingleton<IUserRepository>(_ => new UserRepository(usersFilePath));

//Services
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSingleton<IInventoryService, InventoryService>();

// JWT configuration
string jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT key is not configured.");

string jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("JWT issuer is not configured.");

string jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("JWT audience is not configured.");

if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException("JWT key must contain at least 32 bytes.");
}

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            ValidateAudience = true,
            ValidAudience = jwtAudience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();

// Expose the OpenAPI document only in Development.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();

// This middleware reads the authenticated user's
app.UseMiddleware<CurrentUserMiddleware>();
app.UseAuthorization();
app.UseMiddleware<AuthorizationResponseMiddleware>();

app.MapControllers();

app.Run();
