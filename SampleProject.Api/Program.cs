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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.MapPost("/api/authenticate", (AuthenticationRequest request) =>
{
    const string validUsername = "admin";
    const string validPassword = "123456";
    
    var isValid = request.Username == validUsername && request.Password == validPassword;
    
    return Results.Ok(new AuthenticationResponse { IsValid = isValid });
})
.WithName("Authenticate");

app.Run();

public partial class Program { }
