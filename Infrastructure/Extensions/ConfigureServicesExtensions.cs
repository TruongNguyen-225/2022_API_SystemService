using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Text.Json.Serialization;
using SystemServiceAPI.Bo;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Helpers;
using SystemServiceAPICore3.Bo;
using SystemServiceAPICore3.Bo.Interface;

namespace SystemServiceAPICore3.Infrastructure.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static void AddConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersService();
            services.AddDistributedMemoryCache();
            services.AddDal(configuration);
            services.AddEntityService();
            services.AddMappingProfile();
            services.AddAuthentication(configuration);
            services.AddCors();
            services.AddSession();
        }

        public static void AddControllersService(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // ignore omitted parameters on models to enable optional params (e.g. User update)
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        }

        public static void AddEntityService(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBillBo, BillBo>();
            services.AddScoped<ICustomer, CustomerBo>();
            services.AddScoped<IMasterData, MasterDataBo>();
            services.AddScoped<IReport, ReportBo>();
            services.AddScoped<IDashboard, DashboardBo>();
            services.AddScoped<IAdminConfig, AdminConfigBo>();
            services.AddScoped<IBillTempBo, BillTempBo>();
            services.AddScoped<IAuthenticationBo, AuthenticationBo>();
        }

        private static void AddDal(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<AppDbContext>();
            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
            //services.AddScoped<IUnitOfWork<EssiHRPUow>, EssiHRPUow>();
        }

        private static void AddMappingProfile(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(SystemServiceAPICore3.Profiles.MappingProfile));
        }

        private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
        }

        public static void AddCors(this IServiceCollection services)
        {
            services.AddCors((setup) =>
            {
                setup.AddPolicy("default", (options) =>
                {
                    options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });
        }

        public static void AddSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }
    }
}
