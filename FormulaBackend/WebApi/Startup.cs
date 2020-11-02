using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using WebApi.Services;
using Microsoft.Data.Sqlite;
using AutoMapper;
using WebApi.DataAccess;
using WebApi.DataAccess.Data;
using WebApi.DataAccess.Models;
using WebApi.Api;

namespace WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = "DataSource=myshareddb;mode=memory;cache=shared";
            var keepAliveConnection = new SqliteConnection(connectionString);
            keepAliveConnection.Open();

            services.AddDbContext<FormulaContext>(opt =>
               opt.UseSqlite(keepAliveConnection, b => b.MigrationsAssembly("WebApi.Api")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<FormulaContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
                mc.AddProfile(new ServiceMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["SecurityKey"]))
                };
            });

            services.AddControllers();
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<FormulaContext>();
                context.Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                context.Teams.Add(new Team { Name = "Test Team 1", IsEntryFeePaid = false, WonWorldChampionshipsCount = 4, YearOfFoundation = 1990 });
                context.Teams.Add(new Team { Name = "Test Team 2", IsEntryFeePaid = true, WonWorldChampionshipsCount = 2, YearOfFoundation = 1994 });
                context.Teams.Add(new Team { Name = "Test Team 3", IsEntryFeePaid = false, WonWorldChampionshipsCount = 3, YearOfFoundation = 1998 });

                context.SaveChanges();

                await userManager.CreateAsync(new ApplicationUser { UserName = "admin" }, "f1test2018");
            }
        }
    }
}
