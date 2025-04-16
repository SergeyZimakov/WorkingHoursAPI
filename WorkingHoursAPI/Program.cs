using BLL.Manager;
using Core.Interface.Repository;
using Core.Service;
using DAL;
using DAL.Repoitory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WorkingHoursAPI.Helper;
using WorkingHoursAPI.Mapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EntityDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DB")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

builder.Services.AddTransient<AuthManager>();
builder.Services.AddTransient<DayTypeManager>();
builder.Services.AddTransient<ShiftsManager>();

builder.Services.AddTransient<ValidationService>();

builder.Services.AddTransient<IUserRepository, UserRepoitory>();
builder.Services.AddTransient<IDayTypeRepository, DayTypeRepository>();
builder.Services.AddTransient<IShiftRepository, ShiftRepository>();

builder.Services.AddAutoMapper(typeof(AuthManager));
builder.Services.AddAutoMapper(typeof(DayTypeMapper));
builder.Services.AddAutoMapper(typeof(ShiftMapper));

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = $"Hours API" });
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
    opt.CustomSchemaIds(type => type.ToString());
});

var app = builder.Build();
TokenHelper.GetConfiguration(app.Services.GetRequiredService<IConfiguration>());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
//app.UseAuthentication();
app.MapControllers();
app.Run();
