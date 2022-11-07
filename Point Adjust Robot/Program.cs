using Point_Adjust_Robot.Controllers;
using System.Globalization;
//using Sentry;

//using (SentrySdk.Init(o =>
//{
//    o.Dsn = "https://049c3391ea74466b90039be81f529cfc@o4504115449495552.ingest.sentry.io/4504120015585280";
//    o.MaxBreadcrumbs = 50;
//    o.Debug = true;
//    // Enable Global Mode if running in a client app
//    o.IsGlobalModeEnabled = true;
//    o.TracesSampleRate = 1.0;
//}))
//{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddSingleton<SingletonWorkshift>();
    builder.Services.AddHostedService<SingletonWorkshift>(provider => provider.GetService<SingletonWorkshift>());
    //builder.WebHost.UseSentry();

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
    //app.UseSentryTracing();

    app.UseCors("corsapp");
    app.UseAuthorization();

    app.MapControllers();

    var cultureInfo = new CultureInfo("pt-BR");
    cultureInfo.NumberFormat.CurrencySymbol = "R$";

    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


    app.Run();

//}