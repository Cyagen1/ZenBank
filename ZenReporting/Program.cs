using ZenReporting.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add settings
var mailServiceSettings = builder.Configuration.GetRequiredSection(nameof(MailServiceSettings)).Get<MailServiceSettings>()
    ?? throw new InvalidOperationException($"{nameof(MailServiceSettings)} not set");
builder.Services.AddSingleton(mailServiceSettings);

// Add injections
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IPdfService, PdfService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
