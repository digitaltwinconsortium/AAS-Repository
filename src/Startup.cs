
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Opc.Ua;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AdminShell
{
    public class Startup
    {
        public class ShouldSerializeContractResolver : DefaultContractResolver
        {
            public static readonly ShouldSerializeContractResolver Instance = new ShouldSerializeContractResolver();

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty property = base.CreateProperty(member, memberSerialization);

                if (property.PropertyType != typeof(string))
                {
                    if (property.PropertyType.GetInterface(nameof(IEnumerable)) != null)
                    {
                        property.ShouldSerialize = instance => (instance?.GetType().GetProperty(property.UnderlyingName).GetValue(instance) as IEnumerable<object>)?.Count() > 0;
                    }
                }

                return property;
            }
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson();

            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = ShouldSerializeContractResolver.Instance;
            });

            services.AddRazorPages();

            services.AddServerSideBlazor();

            services.AddSingleton<AssetAdministrationShellEnvironmentService>();

            services.AddSingleton<ProductCarbonFootprintService>();

            services.AddSingleton<ADXDataService>();

            services.AddSingleton<SMIPDataService>();

            services.AddSingleton<UAClient>();

            services.AddLogging(builder => builder.AddConsole());

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddAuthorization();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v3", new OpenApiInfo
                {
                    Title = "AAS Repository REST Service",
                    Version = "v3",
                    Description = "A REST-full interface to the Asset Administration Shell Repository",
                    Contact = new OpenApiContact
                    {
                        Name = "Digital Twin Consortium",
                        Email = string.Empty,
                        Url = new Uri("https://www.digitaltwinconsortium.org"),
                    }
                });

                options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });

                options.CustomSchemaIds(type => type.FullName);

                options.EnableAnnotations();

                options.MapType<MessageTypeEnum>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetNames(typeof(MessageTypeEnum)).Select(enumName => new OpenApiString(enumName)).Cast<IOpenApiAny>().ToList(),
                    Nullable = false
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v3/swagger.json", "AAS Repository REST Service");
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHsts();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Browser}/{action=Index}/{id?}");

                endpoints.MapBlazorHub(options =>
                {
                    // turn off Websocket transport
                    options.Transports =
                        HttpTransportType.ServerSentEvents |
                        HttpTransportType.LongPolling;
                });

                endpoints.MapFallbackToPage("/_Host");
            });

            // run the OPC UA server in a separate thread
            Task.Run(() => StartServerAsync().GetAwaiter().GetResult());

            Console.WriteLine("Preparing for PCF service start...");
            Program.PCFService = app.ApplicationServices.GetRequiredService<ProductCarbonFootprintService>();
            Console.WriteLine("PCF service started.");
        }

        private async Task StartServerAsync()
        {
            Console.WriteLine("Preparing for OPC UA server start...");

            try
            {
                // Wait 10 seconds for the HTTP server to start
                Thread.Sleep(10000);

                // load the application configuration
                ApplicationConfiguration config = await Program.App.LoadApplicationConfiguration(Path.Combine(Directory.GetCurrentDirectory(), "Application.Config.xml"), false).ConfigureAwait(false);

                // check the application certificate
                await Program.App.CheckApplicationInstanceCertificate(false, 0).ConfigureAwait(false);

                // create cert validator
                config.CertificateValidator = new CertificateValidator();
                config.CertificateValidator.CertificateValidation += new CertificateValidationEventHandler(CertificateValidator_CertificateValidation);
                config.CertificateValidator.Update(config.SecurityConfiguration).GetAwaiter().GetResult();

                // start the server
                await Program.App.Start(new SimpleServer()).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("StartServerAsync: " + ex.Message);
                return;
            }

            Console.WriteLine("OPC UA server started.");
        }

        private static void CertificateValidator_CertificateValidation(CertificateValidator validator, CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
            {
                // accept all OPC UA client certificates
                e.Accept = true;
            }
        }
    }
}
