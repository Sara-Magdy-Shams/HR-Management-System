using HRMS.Models;
using HRMS.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<ISalaryReport, SalaryReport>();
builder.Services.AddScoped<HRMS.services.ILogger, HRMS.services.LogIn>();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreateAttendance", policy =>
                      policy.RequireClaim("CreateAttendance", "True"));
    options.AddPolicy("CreateEmp", policy =>
                     policy.RequireClaim("CreateEmp", "True"));
    options.AddPolicy("CreateSettings", policy =>
                     policy.RequireClaim("CreateSettings", "True"));
    options.AddPolicy("ReadAttendance", policy =>
                     policy.RequireClaim("ReadAttendance", "True"));
    options.AddPolicy("ReadEmp", policy =>
                     policy.RequireClaim("ReadEmp", "True"));
    options.AddPolicy("ReadSettings", policy =>
                     policy.RequireClaim("ReadSettings", "True"));
    options.AddPolicy("ReadReport", policy =>
                     policy.RequireClaim("ReadReport", "True"));
    options.AddPolicy("UpdateAttendance", policy =>
                     policy.RequireClaim("UpdateAttendance", "True"));
    options.AddPolicy("UpdateEmp", policy =>
                     policy.RequireClaim("UpdateEmp", "True"));
    options.AddPolicy("UpdateSettings", policy =>
                     policy.RequireClaim("UpdateSettings", "True"));
    options.AddPolicy("DeleteAttendance", policy =>
                     policy.RequireClaim("DeleteAttendance", "True"));
    options.AddPolicy("DeleteEmp", policy =>
                     policy.RequireClaim("DeleteEmp", "True"));
    options.AddPolicy("DeleteSettings", policy =>
                     policy.RequireClaim("DeleteSettings", "True"));
    options.AddPolicy("HRrole", policy =>
                     policy.RequireClaim("GrpName", "HR"));
});

builder.Services.AddDbContext<HRMSContext>(options =>
options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString:cs")));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
