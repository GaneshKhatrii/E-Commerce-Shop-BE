using ECommerce.API.Middlewares;
using ECommerce.API.Validators.ProductImages;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Interfaces.Admin;
using ECommerce.Application.Interfaces.Orders;
using ECommerce.Application.Interfaces.Products;
using ECommerce.Application.Interfaces.Storage;
using ECommerce.Application.Validators.Auth;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Repositories.Admin;
using ECommerce.Infrastructure.Repositories.Orders;
using ECommerce.Infrastructure.Repositories.Products;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Services.Admin;
using ECommerce.Infrastructure.Services.Orders;
using ECommerce.Infrastructure.Services.Products;
using ECommerce.Infrastructure.Services.Storage;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register AddFluentValidationAutoValidation() to enable automatic validation of incoming requests using FluentValidation validators.
// It automatically executes the appropriate validators for the incomming request models
builder.Services.AddFluentValidationAutoValidation();

// Register FluentValidators.
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UploadProductImageRequestValidator>();

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

// Register IProductImageRepository
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();

// Register IProductImageService
builder.Services.AddScoped<IProductImageService, ProductImageService>();

// Register IFileStorageService
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();

// Register IInventoryRepository
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// Register IInventoryService
builder.Services.AddScoped<IInventoryService, InventoryService>();

// Register ICartRepository
builder.Services.AddScoped<ICartRepository, CartRepository>();

// Register ICartService
builder.Services.AddScoped<ICartService, CartService>();

// Register IOrderRepository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register IOrderService
builder.Services.AddScoped<IOrderService, OrderService>();

// Register IAdminRepository
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

// Register IAdminService
builder.Services.AddScoped<IAdminService, AdminService>();

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

// ASP.NET Core needs UseStaticFiles() to expose: images, css, js static assets to browser.
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
