using Microsoft.EntityFrameworkCore;
using TestWebApi321.DatabaseContext;
using TestWebApi321.Interfaces;
using TestWebApi321.Service;
using TestWebApi321.Hubs;
using System.IO; 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();


var connectionString = builder.Configuration.GetConnectionString("TestDbString");
builder.Services.AddDbContext<ContextDb>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUsersService, UserLoginService>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorOrigin",
        policy => policy.WithOrigins("http://localhost:5157", "https://localhost:5157") 
            .AllowAnyHeader()
            .AllowAnyMethod()
        .AllowCredentials());
});


var app = builder.Build();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowBlazorOrigin"); 

app.UseHttpsRedirection();
app.UseSession();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chat"); 

app.Run();

