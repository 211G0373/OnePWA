using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnePWA.Helpers;
using OnePWA.Mappers;
using OnePWA.Models;
using OnePWA.Models.DTOs;
using OnePWA.Models.Entities;
using OnePWA.Providers;
using OnePWA.Repositories;
using OnePWA.Services;

var builder = WebApplication.CreateBuilder(args);
// Configuración de JWT
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); //evita el mapeo automático de claims

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"] ?? ""))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/Signalr"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

// Conexión a BD
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OnecgdbContext>(x =>
    x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddSingleton<ISessionContext,SessionContext>();
builder.Services.AddSingleton<SignalrService>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});


builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));


builder.Services.AddTransient<ISessionsRepository, SessionsRepository>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<ICardsService, CardsService>();
builder.Services.AddTransient<ISignUpDTO, SignUpDTO>();
builder.Services.AddTransient<ILoginDTO, LoginDTO>();
builder.Services.AddTransient<ISessionsService, SessionsService>();
builder.Services.AddTransient<EmailService>();
//builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddScoped<IPushNotificationServices, PushNotificationsServices>();



//builder.Services.AddTransient<ITareasService, TareasService>();
builder.Services.AddTransient<JwtHelper>();



// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
//builder.Services.AddSingleton<IUserIdProvider, NameIdentifierUserIdProvider>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseFileServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<SignalrService>("/hubs/Signalr");

app.Run();
