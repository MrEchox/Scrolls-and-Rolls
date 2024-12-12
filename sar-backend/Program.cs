using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DiceService>();

//var connectionString = Environment.GetEnvironmentVariable("SARDefaultConnection");
var connectionString = "Server=tcp:sarsqldb.database.windows.net,1433;Initial Catalog=ScrollsAndRollsDB;Persist Security Info=False;User ID=saradmin;Password=Scr0lling!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No connection string found in environment variables.");
}

// Configure Entity Framework and connect to Azure SQL DB
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure JWT
var key = Environment.GetEnvironmentVariable("SARJwtKey");
if (string.IsNullOrEmpty(key))
{
    throw new InvalidOperationException("No JWT key found in environment variables.");
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Add the allowed origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Player", policy => policy.RequireRole("Player"));
    options.AddPolicy("GameMaster", policy => policy.RequireRole("GameMaster"));
    options.AddPolicy("LoggedIn", policy => policy.RequireAuthenticatedUser());

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(); // Enable CORS

app.UseHttpsRedirection();

// Authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.MapMessageEndpoints();
app.MapSessionEndpoints();
app.MapCharacterEndpoints();
app.MapItemEndpoints();
app.MapAuthEndpoints();
app.MapGameplayEndpoints();

app.Run();
