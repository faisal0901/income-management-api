using System.IdentityModel.Tokens.Jwt;
using System.Text;
using budget_management_api.Entities;
using budget_management_api.Exceptions;
using budget_management_api.Middlewares;
using budget_management_api.Repositories;
using budget_management_api.Scheduler;
using budget_management_api.Security;
using budget_management_api.Services;
using budget_management_api.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using budget_management_api.Services;
using budget_management_api.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Logging;
using ITrigger = Microsoft.EntityFrameworkCore.Metadata.ITrigger;
using TriggerBuilder = Microsoft.EntityFrameworkCore.Metadata.Builders.TriggerBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("JWT Bearer", new OpenApiSecurityScheme
    {
        Description = "This is a JWT bearer authentication scheme",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Id = "JWT Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// builder.Services.AddQuartz( async q =>
// {
//     q.UseMicrosoftDependencyInjectionScopedJobFactory();
//     
//         
//     using (var context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer("Server=FAISAL;Database=DB_budgetManagemenet;Trusted_Connection=True;TrustServerCertificate=True;").Options))
//     {
//         var intervals = context.Bills.ToList();
//         foreach (var item in intervals)
//         {
//             // Console.Write(
//             //     $"{item.BillDate.Minute} {item.BillDate.Second} {item.BillDate.Hour} {item.BillDate.Day} * ?");
//           
//             var jobKey = new JobKey(item.Id.ToString());
//             q.AddJob<MyJob>(opts => opts.WithIdentity(jobKey));
//             q.AddTrigger(opts => opts
//                 .ForJob(jobKey)
//                 .WithIdentity(item.Id.ToString())
//                 .UsingJobData("list", item.ToString())
//                 .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(item.BillDate.Day, 9, 41)));
//                 // .WithCronSchedule("0 42 10 * * ?")
//         }
//     }
//     // var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
//     // var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
//    
//
// });
// builder.Services.AddQuartzServer(options =>
// {
//     options.WaitForJobsToComplete = true;
// });
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IPersistence, DbPersistence>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IJwtUtil, JwtUtil>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<IWalletService, WalletService>();
builder.Services.AddTransient<ISubCategoryService, SubCategoryService>();
builder.Services.AddTransient<IGoldService, GoldService>();
builder.Services.AddTransient<ISummaryService, SummaryService>();
builder.Services.AddTransient<IBillingService, BillingService>();
builder.Services.AddTransient<IJob, MyJob>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();

            // Mendapatkan header Authorization
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                // Mendapatkan nilai token dari header Authorization
                var bearerToken = authorizationHeader.Substring("Bearer ".Length);
                Console.WriteLine(bearerToken);
                // Memanggil metode di layanan token untuk memeriksa validitas token
                var isTokenRevoked = await tokenService.IsTokenRevoked(bearerToken);

                if (isTokenRevoked)
                {
                    context.Fail("Invalid token");
                }
            }
           
        }
    };

   
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();