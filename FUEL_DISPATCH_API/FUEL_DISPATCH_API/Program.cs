using FluentValidation;
using FMP_DISPATCH_API.Services.Emails;
using FUEL_DISPATCH_API.Auth;
using FUEL_DISPATCH_API.Auth.AuthRepository;
using FUEL_DISPATCH_API.DataAccess.DTOs;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Services;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Middlewares;
using FUEL_DISPATCH_API.Swagger;
using FUEL_DISPATCH_API.Swagger.SwaggerExamples;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
const string swaggerTitle = "FUEL_DISPATCH_API";
const string swaggerVersion = "v1";
const string corsName = "MyPolicy";
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
    .AddJsonOptions
    (opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerExamplesFromAssemblyOf<UserSwaggerExample>()
                .AddSwaggerExamplesFromAssemblyOf<ArticleSwaggerExample>();

builder.Services.AddSwaggerGen(
    info =>
{
    info.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = swaggerTitle,
        Version = swaggerVersion,
        Description = SwaggerSpecs.GetSwaggerSpecs()
    });
    info.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    info.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    info.IncludeXmlComments(xmlPath);
    info.ExampleFilters();

});
builder.Services.AddDbContext<FUEL_DISPATCH_DBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));
// Call to the secret key.
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
#region ServicesContainers
builder.Services.AddScoped<IValidator<Zone>, ZoneValidator>()
                .AddScoped<IValidator<Vehicle>, VehiclesValidator>()
                .AddScoped<IValidator<EmployeeConsumptionLimits>, EmployeeComsuptionLimitsValidator>()
                .AddScoped<IValidator<Booking>, BookingValidator>()
                .AddScoped<IValidator<BranchOffices>, BranchOfficeValidator>()
                .AddScoped<IValidator<BranchIsland>, BranchIslandValidator>()
                .AddScoped<IValidator<Dispenser>, DispenserValidator>()
                .AddScoped<IValidator<Road>, RoadValidator>()
                .AddScoped<IValidator<Companies>, CompanyValidator>()
                .AddScoped<IValidator<WareHouse>, WareHouseValidator>()
                .AddScoped<IValidator<WareHouseMovementRequest>, RequestValidator>()
                .AddScoped<IValidator<Driver>, DriverValidator>()
                .AddScoped<IValidator<WareHouseMovement>, WareHouseMovementValidator>()
                .AddScoped<IValidator<UserRegistrationDto>, RegisterValidator>()
                .AddScoped<IValidator<ArticleDataMaster>, ArticlesValidator>()
                .AddScoped<IValidator<UsersRols>, UsersRolsValidator>()
                .AddScoped<IValidator<UsersCompanies>, UsersCompanyValidator>()
                .AddScoped<IValidator<UsersBranchOffices>, UsersBranchOfficeValidator>()
                .AddScoped<IValidator<EmployeeConsumptionLimits>, EmployeeComsuptionLimitsValidator>()
                .AddScoped<IEmployeeComsuptionLimitsServices, EmployeeComsuptionLimitsServices>()
                .AddScoped<IZoneServices, ZoneServices>()
                .AddScoped<IBookingServices, BookingServices>()
                .AddScoped<IOdometerMeasureServices, OdometerMeasureServices>()
                .AddScoped<IBranchOfficeServices, BranchOfficeServices>()
                .AddScoped<IUsersBranchOfficesServices, UsersBranchOfficessServices>()
                .AddScoped<IBranchIslandServices, BranchIslandServices>()
                .AddScoped<IRoleServices, RoleServices>()
                .AddScoped<IMakeServices, MakeServices>()
                .AddScoped<IModelServices, ModelServices>()
                .AddScoped<IModEngineServices, ModEngineServices>()
                .AddScoped<IGenerationServices, GenerationServices>()
                .AddScoped<IEmployeeComsuptionLimitsServices, EmployeeComsuptionLimitsServices>()
                .AddScoped<IDispenserServices, DispenserServices>()
                .AddScoped<IRoadServices, RoadServices>()
                .AddScoped<ICompaniesServices, CompaniesServices>()
                .AddScoped<IAllComsuptionServices, AllComsuptionServices>()
                .AddScoped<ICalculatedComsuptionServices, CalculatedComsuptionServices>()
                .AddScoped<IComsuptionByDayServices, ComsuptionByDayServices>()
                .AddScoped<IComsuptionByMonthServices, ComsuptionByMonthServices>()
                .AddScoped<ICompaniesUsersServices, UsersCompaniesServices>()
                .AddScoped<IUsersRolesServices, UsersRolesServices>()
                .AddScoped<IWareHouseMovementServices, WareHouseMovementServices>()
                .AddScoped<IActualStockServices, ActualStockServices>()
                .AddScoped<IWareHouseHistoryServices, WareHouseHistoryServices>()
                .AddScoped<IDriverMethodOfComsuptionServices, DriverMethodOfComsuptionServices>()
                .AddScoped<IStockServices, StockServices>()
                .AddScoped<IArticleServices, ArticleDataMasterServices>()
                .AddScoped<IRequestServices, RequestServices>()
                .AddScoped<IWareHouseServices, WareHouseServices>()
                .AddScoped<IVehiclesServices, VehiclesServices>()
                .AddScoped<IDriversServices, DriversServices>()
                .AddScoped<IUsersAuth, UsersAuth>()
                .AddScoped<IUserServices, UsersServices>()
                .AddScoped<ISAPService, SAPService>()
                .AddTransient<IEmailSender, EmailSender>();
#endregion
// Ignore cycles in the object that is actually serializing.
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
# region AuthMiddlewareInLine
/*app.Use(async (context, next) =>
{
    var c = context.Request.HttpContext.User;
    if (c.Identity?.IsAuthenticated ?? false)
    {
        string? companyId, branchId;

        companyId = c.Claims
        .FirstOrDefault(x => x.Type == "CompanyId")?
        .Value;

        branchId = c.Claims
        .FirstOrDefault(x => x.Type == "BranchOfficeId")?
        .Value;


    }
    await next();
});*/
#endregion
app.UseMiddleware<AuthMiddleware>();
app.UseCors(corsName);
app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.InjectStylesheet(@"C:\repositorio_local\FUEL_DISPATCH_API\FUEL_DISPATCH_API\Swagger\SwaggerSpecs.cs");
    });
}
app.UseReDoc(c =>
{
    c.DocumentTitle = "FUEL_DISPATCH_API Doc";
    c.RoutePrefix = "redoc";
    c.SpecUrl = "/swagger/v1/swagger.json";
    c.HeadContent = "<img src=\"https://www.fmp.com.do/images/logo/Logo3.png\" alt=\"FMP Logo\">";
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();