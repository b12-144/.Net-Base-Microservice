#region Imports
using Microservice.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using Shared.Codes;
#endregion

namespace Microservice {
    public class Startup {
        #region Fields
        public IConfiguration Configuration { get; } 
        #endregion

        #region Constructor
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        } 
        #endregion

        #region ConfigureServices
        public void ConfigureServices(IServiceCollection services) {
            OpenApiInfo info = new OpenApiInfo { Version = "v1", Title = ".Net Core Microservice API", Description = ".Net Core API" };            
            BaseStartup.ConfigureServices(services,info);
            AddSingletons(services);
        }
        #endregion

        #region Configure
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            BaseStartup.Configure(Configuration, app, env);
        }
        #endregion


        #region AddSingletons
        private void AddSingletons(IServiceCollection services) {
            //add all singletons below
            services.AddSingleton<UsersService>();
        } 
        #endregion
    }
}
