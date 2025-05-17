using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SoftCore.BLL;
using SoftCore.Configuration;
using SoftEng.Core.BLL;
using SoftInterface.DLL;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Database connection
var connectionStrings = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();
builder.Services.AddSingleton(connectionStrings);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add token 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddHttpContextAccessor();

// Add cross 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        opt => opt.WithOrigins("https://localhost:44397")   
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ACC_API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Add all service
builder.Services.AddScoped<IEmployeeInfo, EmployeeInfoBLL>();
builder.Services.AddScoped<ITeamInfo, TeamInfoBLL>();
builder.Services.AddScoped<IUserInfo, UserInfoBLL>();
builder.Services.AddScoped<IUserRole, UserRoleBLL>();
builder.Services.AddScoped<IUserLogin, UserLoginBLL>();
builder.Services.AddScoped<ITaskInfo, TaskInfoBLL>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
