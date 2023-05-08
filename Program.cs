using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using apartease_backend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using apartease_backend.Services.AmenityService;
using apartease_backend.Services.JwtService;
using apartease_backend.Services.PasswordService;
using apartease_backend.Services.ResidentService;
using apartease_backend.Services.AmenityBookingService;
using apartease_backend.Services.WorkOrderService;
using apartease_backend.Services.VendorService;

namespace apartease_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApartEaseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ApartEaseContext") ?? throw new InvalidOperationException("Connection string 'ApartEaseContext' not found.")));

            // Add services to the container.
            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(builder.Configuration.GetSection("AppSettings:JwtSecret").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //builder.Services.AddCors(options => options.AddPolicy(name: "AllowOrigins",
            //    policy =>
            //    {
            //        policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
            //    }));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigins", policy =>
                {
                    policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:5173")
                    .AllowCredentials();
                });
            });

            //Adding all services to project scope

            builder.Services.AddScoped<IAmenityService, AmenityServiceImpl>();
            builder.Services.AddScoped<IJwtService, JwtServiceImpl>();
            builder.Services.AddScoped<IPasswordService, PasswordServiceImpl>();
            builder.Services.AddScoped<IResidentService, ResidentServiceImpl>();
            builder.Services.AddScoped<IAmenityBookingService, AmenityBookingServiceImpl>();
            builder.Services.AddScoped<IWorkOrderService, WorkOrderServiceImpl>();
            builder.Services.AddScoped<IVendorService, VendorServiceImpl>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowOrigins");

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}