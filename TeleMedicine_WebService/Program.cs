using CommandHandler;
using Commands.UAM;
using Contract;
using Domains.Entities;
using Domains.Mappers;
using Infrastructure.Core.Caching;
using Infrastructure.Core.Managers;
using Infrastructure.Core.Services;
using Infrastructure.Core.Services.Storage;
using Infrastructure.Core.Services.Test;
using Infrastructure.Core.SmsService;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDbGenericRepository;
using StackExchange.Redis;
using TeleMedicine_WebService.Pipeline;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(CreateUserCommand).Assembly, typeof(CreateUserCommandHandler).Assembly);
builder.Services.AddAutoMapper(typeof(MappingProfiles));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
          builder =>
          {
              builder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     ;
          });
});

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configuration = ConfigurationOptions.Parse(config.GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(configuration);
});


var mongoDbContext = new MongoDbContext(config.GetSection("Mongosettings:Connection").Value, config.GetSection("Mongosettings:DatabaseName").Value);

builder.Services.AddIdentity<TelemedicineAppUser, TelemedicineAppRole>()
  .AddMongoDbStores<IMongoDbContext>(mongoDbContext)
  .AddDefaultTokenProviders();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IOtpService, OtpService>();
builder.Services.AddTransient<IKeyStore, KeyStore>();
builder.Services.AddTransient<ISmsService, SmsService>();
builder.Services.AddTransient<TestServices>();
builder.Services.AddTransient<IUserManagerServices, UserManagerServices>();
builder.Services.AddTransient<IFileStorgaeCommunicationService, FileStorgaeCommunicationService>();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        SaveSigninToken = true,
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Token:Issuer"],
        ValidAudience = config["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Token:Key"])) // Jwt:Key - config value 
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole",
         policy => policy.RequireRole("Administrator"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseRouting();  // first

app.UseCors("AllowAllOrigins");
app.UseCors("AllowAllHeaders");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
