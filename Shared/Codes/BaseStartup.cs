#region Imports
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NSwag;
using System;
using System.Collections.Generic;
using System.Text; 
#endregion

namespace Shared.Codes {
    public class BaseStartup {

        #region ConfigureServices
        static public void ConfigureServices(IServiceCollection services, OpenApiInfo openApiInfo) {
            services.AddCors(options => {
                options.AddPolicy(SharedDefines.MyAllowSpecificOrigins,
                    builder => {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            SetupJwt(services);
            services.AddMvcCore(options => {
                options.Filters.Add(typeof(GlobalExceptionHandler));
                options.EnableEndpointRouting = false;
            }).AddApiExplorer()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    //options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
            services.AddSwaggerDocument(config => {
                //NSwag documentation here: https://docs.microsoft.com/pt-br/aspnet/core/tutorials/getting-started-with-nswag?view=aspnetcore-3.1&tabs=visual-studio
                config.PostProcess = document => {
                    document.Info = openApiInfo;
                    document.Info.License = new NSwag.OpenApiLicense {
                        Name = "Use under Microservice License",
                        Url = "https://microservice.io"
                    };
                };
            });
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            SetupApiVersioning(services,1,0);
            services.AddControllers().AddNewtonsoftJson();//System.Text.Json had some issues serializing/deserializing numbers. I'm falling back to newtonsoft for a while. 
        }
        #endregion 

        #region Configure
        static public void Configure(IConfiguration Configuration, IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                //app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }
            app.UseCors(SharedDefines.MyAllowSpecificOrigins);
            app.UseResponseCompression();
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseRouting();
            //acho que nao precisa mais app.UseMvcWithDefaultRoute();//adicionado para suportar webapi
            app.UseAuthentication();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
            SharedDefines.ConnectionString = Configuration["Settings:MySQLConnection"];
            SharedDefines.BaseURL = Configuration["Settings:BaseURL"];
            FlurlHttp.Configure(c => {
                c.JsonSerializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            });
        } 
        #endregion

        #region SetupJwt
        static public void SetupJwt(IServiceCollection services) {
            services.AddAuthentication()
                //.AddCookie(cfg => cfg.SlidingExpiration = true)
                .AddJwtBearer(cfg => {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = false;
                    cfg.TokenValidationParameters = new TokenValidationParameters() {
                        ValidIssuer = SharedDefines.TokenIssuer,
                        ValidAudience = SharedDefines.TokenAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SharedDefines.Token))
                    };
                });
        }
        #endregion

        #region SetupApiVersioning
        static public void SetupApiVersioning(IServiceCollection services, int defaultAPIMajor, int defaultAPiMinor) {
            services.AddApiVersioning(
                o => {
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = new ApiVersion(defaultAPIMajor, defaultAPiMinor);
                });
        }
        #endregion
    }
}
