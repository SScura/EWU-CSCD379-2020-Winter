using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecretSanta.Business;
using SecretSanta.Business.Services;
using SecretSanta.Data;
using Microsoft.EntityFrameworkCore;

namespace SecretSanta.Api
{
    public class Startup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
        public static void ConfigureServices(IServiceCollection services)
        {
            SqliteConnection sqliteConnection = new SqliteConnection("DataSource=:memory:");

            sqliteConnection.Open();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.EnableSensitiveDataLogging()
                    .UseSqlite(sqliteConnection);
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGiftService, GiftService>();
            services.AddScoped<IGroupService, GroupService>();

            System.Type profileType = typeof(AutomapperConfigurationProfile);
            System.Reflection.Assembly assembly = profileType.Assembly;
            services.AddAutoMapper(new[] { assembly });

            services.AddMvc(opts => opts.EnableEndpointRouting = false);

            services.AddSwaggerDocument();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseMvc();
        }
    }
}