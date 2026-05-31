using ECommerce.API.Middlewares;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Validators.Auth;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using ECommerce.Application.Interfaces.Products;
using ECommerce.Infrastructure.Repositories.Products;
using ECommerce.Infrastructure.Services.Products;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register FluentValidators.
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestDtoValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ECommerce.API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration
.GetConnectionString("DefaultConnection"));
});

// Register IUserRepository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register IJwtService
builder.Services.AddScoped<IJwtService, JwtService>();

// Register IEmailService
builder.Services.AddScoped<IEmailService, EmailService>();

// Register IAuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// Register IUserService
builder.Services.AddScoped<IUserService, UserService>();

// Register IProductRepository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register IProductService
builder.Services.AddScoped<IProductService, ProductService>();

// Register IProductVariantRepository
builder.Services.AddScoped<IproductVariantRepository, ProductVariantRepository>();

// Register IproductVariantService
builder.Services.AddScoped<IproductVariantService, ProductVariantService>();

// Configure Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!);
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

var app = builder.Build();

// Seed Admin
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await AdminSeeder.SeedAdminAsync(context);
}


// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register ExceptionMiddleware
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
