using BuildingBlocks.Exceptions.Handler;
using Catalog.API.Data;
using Catalog.API.Settings;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddCarter();
services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));
builder.Services.AddDbContext<CatalogDbContext>((sp, options) =>
    {
        var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
        options.UseMongoDB(
            settings.ConnectionString,
            settings.DatabaseName
        );
    }
);

services.AddExceptionHandler<CustomExceptionHandler>();

services.AddValidatorsFromAssembly(typeof(Program).Assembly);

services.AddHealthChecks();

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.InitializeDatabaseAsync();
}

app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();