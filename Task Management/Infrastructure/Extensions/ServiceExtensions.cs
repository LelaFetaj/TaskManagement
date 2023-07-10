using System;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Task_Management.Data.Context;
using Task_Management.Models.Configurations.Tokens;
using Task_Management.Models.Entities.Roles;
using Task_Management.Models.Entities.Users;
using Task_Management.Respositories.Categories;
using Task_Management.Respositories.Emails;
using Task_Management.Respositories.Projects;
using Task_Management.Respositories.SignIn;
using Task_Management.Respositories.Tasks;
using Task_Management.Respositories.Users;
using Task_Management.Respositories.UserTasks;
using Task_Management.Services.Foundations.Categories;
using Task_Management.Services.Foundations.Emails;
using Task_Management.Services.Foundations.Notifications;
using Task_Management.Services.Foundations.Projects;
using Task_Management.Services.Foundations.SignIn;
using Task_Management.Services.Foundations.Tasks;
using Task_Management.Services.Foundations.Users;
using Task_Management.Services.Foundations.UserTaks;
using Task_Management.Services.Orchestration.Categories;
using Task_Management.Services.Orchestration.Projects;
using Task_Management.Services.Orchestration.Users;
using Task_Management.Services.Orchestration.UserTasks;

namespace WorkHub.API.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddControllersWithFilters(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var openApiInfo = new OpenApiInfo
                {
                    Title = "WorkHub.Api",
                    Version = "v1",
                };

                options.SwaggerDoc(
                    name: "v1",
                    info: openApiInfo);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
        }

        public static void AddIdentityServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtConfiguration = configuration
               .GetSection(nameof(JwtConfiguration))
               .Get<JwtConfiguration>();

            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddEntityFrameworkStores<TaskManagementDbContext>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtConfiguration.Key)),
                    };
                });
        }

        public static void AddContext(this IServiceCollection services)
        {
            services.AddDbContext<TaskManagementDbContext>();
            services.AddScoped<TaskManagementDbContext>();
        }

        public static void AddCustomHealthChecks(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString(
                name: "DefaultConnection");

            services
                .AddHealthChecks()
                .AddDbContextCheck<TaskManagementDbContext>(nameof(TaskManagementDbContext))
                .AddNpgSql(
                    connectionString,
                    name: "PostgreSQL");
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISignInRepository, SignInRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<IUserTaskRepository, UserTaskRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISignInService, SignInService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IUserTaskService, UserTaskService>();
            services.AddTransient<ICategoryOrchestrationService, CategoryOrchestrationService>();
            services.AddTransient<IProjectOrchestrationService, ProjectOrchestrationService>();
            services.AddTransient<IUserOrchestrationService, UserOrchestrationService>();
            services.AddTransient<IUserTaskOrchestrationService, UserTaskOrchestrationService>();

        }

        public static void UseSwaggerService(this IApplicationBuilder builder)
        {
            builder.UseSwagger();

            builder.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    url: $"./v1/swagger.json",
                    name: "WorkHuba.Api v1");
            });
        }
    }
}