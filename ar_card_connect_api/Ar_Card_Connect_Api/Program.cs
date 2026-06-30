using System;
using System.Text;
using Ar_Card_Connect_Api.DataBase;
using Ar_Card_Connect_Api.Interfaces;
using Ar_Card_Connect_Api.JWT;
using Ar_Card_Connect_Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddScoped<JwtGenerator>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = false, 
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"] ?? "ArCardConnect",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? throw new Exception("JWT Key not found")))
        };
    });
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<JwtGenerator>();
builder.Services.AddEndpointsApiExplorer();

// ========== НАСТРОЙКА SWAGGER ДЛЯ JWT ==========
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите токен в формате: Bearer {ваш_токен}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});
// ===============================================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();  
app.UseCors("AllowAll");
var cardsFolder = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "cards");
var modelsFolder = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "models");
var avatarsFolder = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "avatars");

if (!Directory.Exists(cardsFolder)) Directory.CreateDirectory(cardsFolder);
if (!Directory.Exists(modelsFolder)) Directory.CreateDirectory(modelsFolder);
if (!Directory.Exists(avatarsFolder)) Directory.CreateDirectory(avatarsFolder);

app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Run();