using SampleProject.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("SampleProject.Api application starting up");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.MapPost("/api/authenticate", (AuthenticationRequest request, ILogger<Program> logger) =>
{
    logger.LogInformation("Authentication attempt for username: {Username}", request.Username);
    
    const string validUsername = "admin";
    const string validPassword = "123456";
    
    var isValid = request.Username == validUsername && request.Password == validPassword;
    
    if (isValid)
    {
        logger.LogInformation("Authentication successful for username: {Username}", request.Username);
    }
    else
    {
        logger.LogWarning("Authentication failed for username: {Username}", request.Username);
    }
    
    return Results.Ok(new AuthenticationResponse { IsValid = isValid });
})
.WithName("Authenticate");

logger.LogInformation("SampleProject.Api application started");

app.Run();

public partial class Program { }
