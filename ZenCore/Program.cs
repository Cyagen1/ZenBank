using Microsoft.EntityFrameworkCore;
using Quartz;
using ZenCore.DataAccess;
using ZenCore.Services;
using ZenCore.Services.ZenReporting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add settings
var zenReportingSettings = builder.Configuration.GetRequiredSection(nameof(ZenReportingSettings)).Get<ZenReportingSettings>()
    ?? throw new InvalidOperationException($"{nameof(ZenReportingSettings)} not set");
builder.Services.AddSingleton(zenReportingSettings);
var dataAccessSettings = builder.Configuration.GetRequiredSection(nameof(DataAccessSettings)).Get<DataAccessSettings>()
    ?? throw new InvalidOperationException($"{nameof(DataAccessSettings)} not set");
builder.Services.AddSingleton(dataAccessSettings);

builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add DB Context
builder.Services.AddDbContext<BankContext>(options =>
{
    options.UseSqlServer(dataAccessSettings.ConnectionString);
}, ServiceLifetime.Transient);

// Add injections
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IZenReportingClient, ZenReportingHttpClient>();
builder.Services.AddScoped<IReportingService, ReportingService>();


// Register Quartz services
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("ReportingJob");
    q.AddJob<ReportingJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("ReportingJob-trigger")
        .WithCronSchedule(zenReportingSettings.CronConfiguration));
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<BankContext>().Database.Migrate();
}

app.Run();
