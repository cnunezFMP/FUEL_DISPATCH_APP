using FMP_DISPATCH_API.Services.Emails;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
const string swaggerTitle = "FUEL_DISPATCH_API";
const string swaggerVersion = "v1";
const string corsName = "MyPolicy";
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    info =>
{
    info.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = swaggerTitle,
        Version = swaggerVersion
    });
});
builder.Services.AddDbContext<FUEL_DISPATCH_DBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSql")));
// Llamado a la clave secreta
var secretkey = builder.Configuration.GetSection("settings:secretkey").Value; //.GetSection("secretkey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretkey!);
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(config =>
    {
        config.RequireHttpsMetadata = false;
        config.SaveToken = true;
        config.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddScoped<IDispatchServices, DispatchServices>()
                .AddTransient<IEmailSender, EmailSender>();

// Ignorar ciclos en el objeto que se esta serializando
builder.Services.AddControllers().AddJsonOptions(option =>
{
    option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddCors((options) =>
{
    options.DefaultPolicyName = corsName;
    options.AddPolicy(corsName, p =>
    {
        p.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});
var app = builder.Build();
app.UseExceptionHandler();
// app.AddGlobalErrorHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseReDoc(c =>
{
    c.DocumentTitle = "FUEL_DISPATCH_API Doc";
    c.RoutePrefix = "redoc";
    c.SpecUrl = "/swagger/v1/swagger.json";
    c.HeadContent = "<a href='https://localhost:44363/swagger/index.html' style= 'font-weight: bold; font-family: sans-serif; text-decoration: none; color: blue; margin-left: 100px;'>Swagger</a>";
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();