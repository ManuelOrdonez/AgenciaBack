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
    using DinkToPdf.Contracts;
    using DinkToPdf;
    using System.Runtime.Loader;
    using System.Reflection;
    using System.IO;
    using System;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.ExternalServices;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
                    //ValidAudience = "the audience you want to validate",
                    ValidateIssuer = false,
                    //ValidIssuer = "the isser you want to validate",

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the secret code.")),

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(15) //15 minute tolerance for the expiration date
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

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddMvc();
            try
            {
                var architectureFolder = (IntPtr.Size == 8) ? "64bits" : "32bits";
                CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
                context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox", architectureFolder, "libwkhtmltox.dll"));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

        private void DependencySettings(IServiceCollection services)
        {
            services.Configure<AppSettings>(opt => Configuration.GetSection("AppSettings").Bind(opt));
            services.Configure<SendMailData>(opt => Configuration.GetSection("SendMailData").Bind(opt));
            services.Configure<List<ServiceSettings>>(opt => Configuration.GetSection("ServiceSettings").Bind(opt));
            if (!string.IsNullOrEmpty(Configuration["SECRET_TABLESTORAGE"]))
            {

                UserSecretSettings su = new UserSecretSettings
                {
                    TableStorage = Configuration["SECRET_TABLESTORAGE"],
                    SendMailApiKey = Configuration["SECRET_SENDMAILAPIKEY"],
                    OpenTokApiKey = Configuration["SECRET_OPENTOKAPIKEY"],
                    LdapServiceApiKey = Configuration["SECRET_LDAPSERVICEAPIKEY"]
                    
                };
                
                //services.Configure<UserSecretSettings>(su);
            }
            else
            {
                services.Configure<UserSecretSettings>(Configuration);
                
            }
        }

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
            services.AddSingleton<IGenericQueue,QueueStorageBase> ();
        }

        private static void DependencyExternalServices(IServiceCollection services)
        {
            services.AddTransient<ISendGridExternalService, SendGridExternalService>();
            services.AddTransient<IOpenTokExternalService, OpenTokExternalService>();
            services.AddTransient<ILdapServices, LdapServices>();
        }

        private static void DependencyBusiness(IServiceCollection services)
        {
            services.AddTransient<IAdminBl, AdminBl>();
            services.AddTransient<IUserBl, UserBl>();
            services.AddTransient<IDepartamentBl, DepartamentBl>();
            services.AddTransient<IAgentBl, AgentBl>();
            services.AddTransient<ICallHistoryTrace, CallHistoryTraceBl>();
            services.AddTransient<IParametersBI, ParameterBI>();
            services.AddTransient<IResetBI, ResetBI>();
        }
    }
    internal class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }

        protected override IntPtr LoadUnmanagedDll(String unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }
}
