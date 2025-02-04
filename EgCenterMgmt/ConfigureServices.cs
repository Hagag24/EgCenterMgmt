
namespace EgCenterMgmt;

public static class ConfigureServices
{
    public static async Task AddSeeding(this IServiceCollection services, ConfigurationManager configuration, WebApplication app)
    {
        try
        {
            var json = File.ReadAllText("tenantsettings.json");
            var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
            TenantSettings options = appSettings!.TenantSettings;

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EgCenterMgmtContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            foreach (var tenant in options.Tenants.Where(t=> t.IsActive == true && t.StartDate <= DateTime.UtcNow && t.EndDate > DateTime.UtcNow))
            {
                dbContext.Database.SetConnectionString(tenant.ConnectionString ?? options.Defaults.ConnectionString);
                await SeedData.Initialize(scope.ServiceProvider, userManager, roleManager, tenant.TId, tenant.Name);
                await dbContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
    }



    public static async Task<IServiceCollection> AddTenancyAsync(this IServiceCollection services, ConfigurationManager configuration)
    {
        var json = File.ReadAllText("tenantsettings.json");
        var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
        TenantSettings options = appSettings!.TenantSettings;

        var defaultDbProvider = options.Defaults.DBProvider;

        if (defaultDbProvider.ToLower() == "mssql")
        {
            services.AddDbContext<EgCenterMgmtContext>(m => m.UseSqlServer());
        }

        foreach (var tenant in options.Tenants.Where(t => t.IsActive == true && t.StartDate <= DateTime.UtcNow && t.EndDate > DateTime.UtcNow))
        {
            var connectionString = tenant.ConnectionString ?? options.Defaults.ConnectionString;

            using var scope =  services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<EgCenterMgmtContext>();

            try
            {
                dbContext.Database.SetConnectionString(connectionString);
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    await dbContext.Database.MigrateAsync();
                    await SeedViews.AddOrUpdateViews(dbContext);
                }

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }

        return services;
    }
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services,ConfigurationManager configuration)
    {
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAttendanceServices, AttendanceServices>();
        services.AddScoped<IPaymentServices, PaymentServices>();
        services.AddScoped<IUserManagementServices, UserManagementServices>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(UserProfile));
        return services;
    }

    public static IServiceCollection AddAuthenticationClean(this IServiceCollection services, ConfigurationManager configuration)
    {
        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();

        services.AddCors(options =>

            options.AddDefaultPolicy(builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(allowedOrigins!)
                    .AllowCredentials()
                    .WithExposedHeaders("X-Pagination")
                    .WithExposedHeaders("Content-Disposition")
            )
        );
        services
            .AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<EgCenterMgmtContext>();
        services.Configure<IdentityOptions>(options =>
        {
            options.User.AllowedUserNameCharacters = string.Empty;
        });

        return services;
    }
    public static IServiceCollection AddSwagger(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "EgCenterMgmt.Api", Version = "v1" });

            // Add Security Definitions for Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' [space] and then your token in the text input below."
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
                Array.Empty<string>()
            }
        });
        });

        return services;
    }
}