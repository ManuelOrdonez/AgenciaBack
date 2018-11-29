namespace AgenciaDeEmpleoVirutal.Services
{
    using Business;
    using Contracts.Business;
    using Contracts.Referentials;
    using Entities;
    using Entities.Referentials;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using DataAccess.Referentials;
    using Swashbuckle.AspNetCore.Swagger;
    using System.Collections.Generic;
    using System;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.ExternalServices;

    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Interface to configuration eweb apies
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Method to Configur eServices
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            DependencySettings(services);
            DependencyRepositories(services);
            DependencyExternalServices(services);
            DependencyBusiness(services);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Jwt";
                options.DefaultChallengeScheme = "Jwt";
            }).AddJwtBearer("Jwt", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    /// ValidAudience = "the audience you want to validate",
                    ValidateIssuer = false,
                    /// ValidIssuer = "the isser you want to validate",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the secret code.")),
                    ValidateLifetime = true, /// validate the expiration and not before values in the token
                    ClockSkew = TimeSpan.FromMinutes(15) /// 15 minute tolerance for the expiration date
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolitic",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetPreflightMaxAge(TimeSpan.FromSeconds(2520)).Build());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Services Agencia de Empleo Virtual", Version = "v1" });
            });
            services.AddMvc();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolitic");
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Services Agencia Virtual de Empleo");
            });
            app.UseAuthentication();
            app.UseMvc();
        }

        /// <summary>
        /// Method to register Dependency Settings
        /// </summary>
        /// <param name="services"></param>
        private void DependencySettings(IServiceCollection services)
        {
            services.Configure<AppSettings>(opt => Configuration.GetSection("AppSettings").Bind(opt));
            services.Configure<SendMailData>(opt => Configuration.GetSection("SendMailData").Bind(opt));
            services.Configure<List<ServiceSettings>>(opt => Configuration.GetSection("ServiceSettings").Bind(opt));
            //if (!string.IsNullOrEmpty(Configuration["SECRET_TABLESTORAGE"]))
            //{

            //    UserSecretSettings su = new UserSecretSettings
            //    {
            //        TableStorage = Configuration["SECRET_TABLESTORAGE"],
            //        SendMailApiKey = Configuration["SECRET_SENDMAILAPIKEY"],
            //        OpenTokApiKey = Configuration["SECRET_OPENTOKAPIKEY"],
            //        LdapServiceApiKey = Configuration["SECRET_LDAPSERVICEAPIKEY"]
                    
            //    };                
            //}
            //else
            //{
                services.Configure<UserSecretSettings>(Configuration);
                
            //}
        }

        /// <summary>
        /// Method to register Dependency Repositories
        /// </summary>
        /// <param name="services"></param>
        private static void DependencyRepositories(IServiceCollection services)
        {
            services.AddSingleton<IGenericRep<User>, TableStorageBase<User>>();
            services.AddSingleton<IGenericRep<DepartamenCity>, TableStorageBase<DepartamenCity>>();
            services.AddSingleton<IGenericRep<CallHistoryTrace>, TableStorageBase<CallHistoryTrace>>();
            services.AddSingleton<IGenericRep<Agent>, TableStorageBase<Agent>>();
            services.AddSingleton<IGenericRep<Parameters>, TableStorageBase<Parameters>>();
            services.AddSingleton<IGenericRep<ResetPassword>, TableStorageBase<ResetPassword>>();
            services.AddSingleton<IGenericRep<PDI>, TableStorageBase<PDI>>();
            services.AddSingleton<IGenericRep<BusyAgent>, TableStorageBase<BusyAgent>>();
            services.AddSingleton<IGenericRep<Menu>, TableStorageBase<Menu>>();
            services.AddSingleton<IGenericRep<Subsidy>, TableStorageBase<Subsidy>>();
        }

        /// <summary>
        /// Method to register Dependency External Services
        /// </summary>
        /// <param name="services"></param>
        private static void DependencyExternalServices(IServiceCollection services)
        {
            services.AddTransient<ISendGridExternalService, SendGridExternalService>();
            services.AddTransient<IOpenTokExternalService, OpenTokExternalService>();
            services.AddTransient<ILdapServices, LdapServices>();
            services.AddTransient<IPDFConvertExternalService, PDFConvertExternalService>();
        }

        /// <summary>
        /// Method to register Dependency Business Logic
        /// </summary>
        /// <param name="services"></param>
        private static void DependencyBusiness(IServiceCollection services)
        {
            services.AddTransient<IAdminBl, AdminBl>();
            services.AddTransient<IUserBl, UserBl>();
            services.AddTransient<IDepartamentBl, DepartamentBl>();
            services.AddTransient<IAgentBl, AgentBl>();
            services.AddTransient<ICallHistoryTrace, CallHistoryTraceBl>();
            services.AddTransient<IParametersBI, ParameterBI>();
            services.AddTransient<IResetBI, ResetBI>();
            services.AddTransient<IPdiBl, PdiBl>();
            services.AddTransient<IMenuBl, MenuBl>();
            services.AddTransient<ISubsidyBl, SubsidyBl>();
        }
    }
}
