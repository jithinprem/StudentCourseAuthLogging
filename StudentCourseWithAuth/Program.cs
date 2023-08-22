using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage;
using StudentCourseWithAuth.Authorization;
using StudentCourseWithAuth.Logger;
using StudentCourseWithAuth.Models;

namespace StudentCourseWithAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(StudentCourseRepo<>));
            builder.Services.AddScoped(typeof(IMyLogger), typeof(DatabaseLogger));
            builder.Services.AddDbContext<StudentContext>();
            //ServiceLifetime.Singleton

            builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                options.LoginPath = "/Login/LogInAction";
                options.AccessDeniedPath = "/Login/Notauthorized";
                options.ExpireTimeSpan = TimeSpan.FromSeconds(300);

            });

            builder.Services.AddAuthorization(options =>
            {

                options.AddPolicy("AdminOnly",
                    policy => policy.RequireClaim("Admin"));


                options.AddPolicy("Experience", policy => policy
                .Requirements.Add(new CustomCondition(3)));

            });
            builder.Services.AddSingleton<IAuthorizationHandler, CustomConditionHandler>();


            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}