using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using UltraSystem.API.Middleware.TokenHandle;
using UltraSystem.Application;
using UltraSystem.Application.Interface;
using UltraSystem.Application.Service;
using UltraSystem.Core;
using UltraSystem.Core.Database;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;
using UltraSystem.Core.Repositories;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var env = builder.Environment;
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetSection("JwtIssuerOptions:Issuer").Value,
                ValidAudience = builder.Configuration.GetSection("JwtIssuerOptions:Audience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtIssuerOptions:SecretKey").Value))
            };
        });
        services.AddCors();
        // Add services to the container.
        services.AddControllers().AddJsonOptions(x =>
        {
            // serialize enums as strings in api responses (e.g. Role)
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

            // ignore omitted parameters on models to enable optional params (e.g. User update)
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
        services.Configure<JwtIssuerOptions>(builder.Configuration.GetSection("JwtIssuerOptions"));
        services.Configure<MailAppSetting>(builder.Configuration.GetSection("MailAppSetting"));
        //services.AddSingleton(typeof(IDbContext<>), typeof(SqlServerContext<>));
        services.AddSingleton(typeof(IDbContext<>), typeof(MySQLContext<>));
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepositories<>));
        services.AddTransient(typeof(IBaseService<>), typeof(BaseService<>));
        services.AddScoped<IAuthenService, AuthenService>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<ILicenseRepository, LicenseRepository>();
        services.AddTransient<IPurchasedProductRepository, PurchasedProductRepository>();
        services.AddTransient<IKeyRepository, KeyRepository>();
        services.AddTransient<IHardwareRepository, HardwareRepository>();
        services.AddTransient<IHardwareService, HardwareService>();
        services.AddTransient<IProductService, ProductsService>();
        services.AddTransient<IPurchasedProductService, PurchasedProductService>();
        services.AddTransient<ILicenseService, LicenseService>();
        services.AddTransient<IHardwareRepository, HardwareRepository>();
        services.AddTransient<IUserService, UsersService>();
        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IJwtUtils, JwtUtils>();
        services.AddTransient<IClaimProvider, ClaimProvider>();
        services.AddTransient<IRolePermisstionRepository, RolePermisstionRepository>();
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        
        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        {
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // global error handler
            app.UseMiddleware<AuthHandleMiddleware>();

            app.MapControllers();
        }
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}