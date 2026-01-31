using Microsoft.EntityFrameworkCore;
using EventRegistrationAPI.Data;
using EventRegistrationAPI.Services;

var builder = WebApplication.CreateBuilder(args);
// This automatically reads appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<RegistrationService>();

// Email Service Configuration (choose one based on appsettings)
var emailServiceType = builder.Configuration["EmailService:Type"];
if (emailServiceType == "SendGrid")
{
    builder.Services.AddScoped<IEmailService, SendGridEmailService>();
}
else if (emailServiceType == "Mailtrap")
{
    builder.Services.AddHttpClient<MailtrapEmailService>();
    builder.Services.AddScoped<IEmailService, MailtrapEmailService>();
}
else
{
    // Default to Console Email Service
    builder.Services.AddScoped<IEmailService, ConsoleEmailService>();
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();