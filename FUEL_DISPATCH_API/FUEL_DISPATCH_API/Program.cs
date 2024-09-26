using FluentValidation;
using FUEL_DISPATCH_API.Attributes;
using FUEL_DISPATCH_API.Auth;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Services;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Middlewares;
using FUEL_DISPATCH_API.Swagger;
using FUEL_DISPATCH_API.Swagger.SwaggerExamples;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(config =>
{
    config.Filters.Add<ValidationFilterAttribute>();
})
    .AddJsonOptions
    (opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExamplesFromAssemblyOf<UserSwaggerExample>()
                .AddSwaggerExamplesFromAssemblyOf<ArticleSwaggerExample>();


builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddSwaggerGen(
    info =>
{
    info.SwaggerDoc("v1",
        new OpenApiInfo
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

var secretkey = builder.Configuration.GetSection("settings:secretkey").Value; //.GetSection("secretkey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretkey!);
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
    {
        config.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(ResultPattern<object>.Failure(StatusCodes.Status401Unauthorized,
                    "Usuario no autenticado. "));
            }
        };
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
builder.Services.AddScoped<IValidator<Vehicle>, VehiclesValidator>()
                .AddScoped<IValidator<EmployeeConsumptionLimits>, EmployeeComsuptionLimitsValidator>()
                .AddScoped<IValidator<Booking>, BookingValidator>()
                .AddScoped<IValidator<BranchOffices>, BranchOfficeValidator>()
                .AddScoped<IValidator<BranchIsland>, BranchIslandValidator>()
                .AddScoped<IValidator<Dispenser>, DispenserValidator>()
                .AddScoped<IValidator<WareHouseMovementRequest>, RequestValidator>()
                .AddScoped<IValidator<Driver>, DriverValidator>()
                .AddScoped<IValidator<WareHouseMovement>, WareHouseMovementValidator>()
                .AddScoped<IValidator<ArticleDataMaster>, ArticlesValidator>()
                .AddScoped<IValidator<UsersRols>, UsersRolsValidator>()
                .AddScoped<IValidator<UsersBranchOffices>, UsersBranchOfficeValidator>()
                .AddScoped<IValidator<EmployeeConsumptionLimits>, EmployeeComsuptionLimitsValidator>()
                .AddScoped<IEmployeeComsuptionLimitsServices, EmployeeComsuptionLimitsServices>()
                .AddScoped<IBookingServices, BookingServices>()
                .AddScoped<IOdometerMeasureServices, OdometerMeasureServices>()
                .AddScoped<IBranchOfficeServices, BranchOfficeServices>()
                .AddScoped<IUsersBranchOfficesServices, UsersBranchOfficessServices>()
                .AddScoped<IBranchIslandServices, BranchIslandServices>()
                .AddScoped<IRoleServices, RoleServices>()
                .AddScoped<IMaintenanceServices, MaintenanceServices>()
                .AddScoped<IPartServices, PartServices>()
                .AddScoped<IMaintenanceNotificacionServices, MaintenanceNotificacionServices>()
                .AddScoped<IMakeServices, MakeServices>()
                .AddScoped<IModelServices, ModelServices>()
                .AddScoped<IModEngineServices, ModEngineServices>()
                .AddScoped<IGenerationServices, GenerationServices>()
                .AddScoped<IEmployeeComsuptionLimitsServices, EmployeeComsuptionLimitsServices>()
                .AddScoped<IVw_MaintenanceServices, Vw_MaintenanceServices>()
                .AddScoped<IDispenserServices, DispenserServices>()
                .AddScoped<ICompaniesServices, CompaniesServices>()
                .AddScoped<ILicenseExpDateAlertServices, LicenseExpDateAlertServices>()
                .AddScoped<IVw_NotificationsServices, Vw_NotificationsServices>()
                .AddScoped<IAllComsuptionServices, AllComsuptionServices>()
                .AddScoped<ICalculatedComsuptionServices, CalculatedComsuptionServices>()
                .AddScoped<IComsuptionByDayServices, ComsuptionByDayServices>()
                .AddScoped<ICompanySapParamsServices, CompanySapParamsServices>()
                .AddScoped<IComsuptionByMonthServices, ComsuptionByMonthServices>()
                .AddScoped<IMaintenanceDetailsServices, MaintenanceDetailsServices>()
                .AddScoped<IUsersRolesServices, UsersRolesServices>()
                .AddScoped<IWareHouseMovementServices, WareHouseMovementServices>()
                .AddScoped<IActualStockServices, ActualStockServices>()
                .AddScoped<IWareHouseHistoryServices, WareHouseHistoryServices>()
                .AddScoped<IVehicleMakeModelsServices, VehicleMakeModelsServices>()
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
                .AddScoped<IReportsServices, ReportsServices>();
//.AddTransient<IEmailSender, EmailSender>();
#endregion
// Ignore cycles in the object that is actually serializing.
builder.Services
    .AddControllers()
    .AddJsonOptions(option =>
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
        .WarehouseItemStockValue;

        branchId = c.Claims
        .FirstOrDefault(x => x.Type == "BranchOfficeId")?
        .WarehouseItemStockValue;
    }
    await next();
});*/
#endregion
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
app.UseAuthentication();
app.UseAuthorization();
//app.Use(async (context, next) =>
//{
//    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
//    {
//        context.Response.ContentType = "application/json";
//        await context.Response.WriteAsync("{\"message\": \"Acceso denegado. No tienes permisos suficientes.\"}");
//    }
//    else
//    {
//        await next();
//    }
//});
//app.UseMiddleware<AuthMiddleware>();
//app.UseStatusCodePages(async (x) =>
//{
//    if (x.HttpContext.Response.StatusCode == 403)
//    {
//        var noAuthorizedObj = new
//        {
//            Titulo = "Usuario no autorizado.",
//            Message = "No esta autorizado para hacer esta accion. ",
//            Status = 403

//        };

//        await x.HttpContext.Response.WriteAsJsonAsync(noAuthorizedObj);
//    };
//});
app.MapControllers();
app.Run();