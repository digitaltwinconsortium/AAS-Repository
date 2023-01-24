namespace AdminShell
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Connections;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Models;
    using System;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddRazorPages();

            services.AddServerSideBlazor();

            services.AddScoped<AssetAdministrationShellEnvironmentService>();

            services.AddSingleton<CarbonReportingService>();

            services.AddSingleton<ProductCarbonFootprintService>();

            services.AddSingleton<VisualTreeBuilderService>();

            services.AddSingleton<AASXPackageService>();

            services.AddSingleton<UANodesetViewer>();

            services.AddLogging(builder => builder.AddConsole());

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddAuthorization();

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

                options.CustomSchemaIds(type => type.ToString());

                options.EnableAnnotations();
            });

            // Setup file storage
            switch (Configuration["HostingPlatform"])
            {
                case "Azure": services.AddSingleton<IFileStorage, AzureFileStorage>(); break;
                default:
                {
                    services.AddSingleton<IFileStorage, LocalFileStorage>();
                    Console.WriteLine("WARNING: Using local filesystem for storage as HostingPlatform environment variable not specified or invalid!");
                    break;
                }
            }

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapBlazorHub(options =>
                {
                    // turn off Websocket transport
                    options.Transports =
                        HttpTransportType.ServerSentEvents |
                        HttpTransportType.LongPolling;
                });

                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
