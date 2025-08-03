using Microsoft.EntityFrameworkCore;
using Functions.Data;
using Functions;
using Functions.Services;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<Worker>();
    services.AddDbContext<FunctionContext>(options =>
        options.UseSqlite("Data Source=statusupdates.db"));
    services.AddLogging(logging =>
    {
        logging.AddConsole();
    });
    services.AddHttpClient();
});
// Add logging


var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FunctionContext>();
    context.Database.Migrate();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Migration applied. Database path: " + context.Database.GetDbConnection().ConnectionString);
    logger.LogInformation("Migrations applied for Functions.");


}

app.Run();