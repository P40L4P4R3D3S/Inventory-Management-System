using System.IO;

using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Application.Ports.Outbound;
using Inventory_Management_System.Api.Application.Services;
using Inventory_Management_System.Api.Infrastructure.Persistence;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register controllers.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

string dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "Data");

Directory.CreateDirectory(dataDirectory);
string productsFilePath = Path.Combine(dataDirectory, "products.json");
string transactionsFilePath = Path.Combine(dataDirectory, "transactions.json");

builder.Services.AddSingleton<IProductRepository>(_ => new ProductRepository(productsFilePath));
builder.Services.AddSingleton<ITransactionRepository>(_ => new TransactionRepository(transactionsFilePath));
builder.Services.AddSingleton<IInventoryService, InventoryService>();

var app = builder.Build();

// Expose the OpenAPI document only in Development.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Inventory Management API v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
