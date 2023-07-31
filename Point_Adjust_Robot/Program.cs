using Point_Adjust_Robot.Controllers;
using System.Globalization;
using Sentry;
using Sentry.AspNetCore;
using Point_Adjust_Robot;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SingletonWorkshift>();
    
builder.Services.AddHostedService<SingletonWorkshift>(provider => provider.GetService<SingletonWorkshift>());
builder.WebHost.UseSentry(options =>
{
    options.Dsn = "https://049c3391ea74466b90039be81f529cfc@o4504115449495552.ingest.sentry.io/4504120015585280";
    options.MaxBreadcrumbs = 50;
    options.TracesSampleRate = 1.0;
    options.Debug = true;
    options.IsGlobalModeEnabled = true;
    options.Release = SystemInfo.Release;
    options.AutoSessionTracking = true;

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsapp", policy =>
    {
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Sentry
app.UseSentryTracing();

app.UseCors("corsapp");
app.UseAuthorization();

app.MapControllers();

var cultureInfo = new CultureInfo("pt-BR");
cultureInfo.NumberFormat.CurrencySymbol = "R$";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


app.Run();
