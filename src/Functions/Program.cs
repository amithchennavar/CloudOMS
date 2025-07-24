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
});

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FunctionContext>();
    context.Database.Migrate();
    Console.WriteLine("Migration applied. Database path: " + context.Database.GetDbConnection().ConnectionString);
}

app.Run();