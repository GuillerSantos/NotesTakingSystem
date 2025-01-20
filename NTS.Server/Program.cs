using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NTS.Server.Data;
using NTS.Server.Services;
using NTS.Server.Services.Contracts;
using NTS.Server.Entities.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// DB Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Configure Swagger/OpenAPI with JWT Bearer Authentication
builder.Services.AddSwaggerGen(options =>
{
    // Add Security Definition for Bearer Token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Require the Bearer token for all API endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Configuration CORS For Blazor Server App
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("AllowBlazorApp", policy =>
    {
        policy
        .WithOrigins("https://localhost:5000", "http://localhost:5001")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// JWT Bearer Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });


// Dependency Injection for services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INotesService, NotesService>();
builder.Services.AddScoped<IFavoriteNoteService, FavoriteNoteService>();
builder.Services.AddScoped<IImpotantNotesService, ImportantNotesService>();
builder.Services.AddScoped<ISharedNotesService, SharedNotesService>();
builder.Services.AddScoped<IStarredNotesService, StarredNoteesService>();
builder.Services.AddTransient<IEmailService, EmailService>();


builder.Services.Configure<EmailSettingsDto>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline in development
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();