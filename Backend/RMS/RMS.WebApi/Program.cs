using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RMS.Application;
using RMS.Application.Implementations;
using RMS.Application.Interfaces;
using RMS.Infrastructure.Persistences;
using RMS.WebApi.Filters;
using System.Text;
using RMS.WebApi.Services;
using RMS.WebApi.Configurations; // Added
using Microsoft.Extensions.Configuration; // Added
using Microsoft.Extensions.DependencyInjection; // Added

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationAndInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<IImageService, ImageService>();

// Configure ImageSettings
builder.Services.Configure<ImageSettings>(builder.Configuration.GetSection("ImageSettings")); // Added

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero // Optional: reduce time tolerance for expiration
    };

    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }

            Console.WriteLine($"JWT authentication failed: {context.Exception.Message}");

            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();

            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var result = System.Text.Json.JsonSerializer.Serialize(new
                {
                    message = "Unauthorized. Please login again.",
                    errorCode = "AUTH_TOKEN_INVALID"
                });

                return context.Response.WriteAsync(result);
            }

            return Task.CompletedTask;
        }
    };
});

//builder.Services.AddAuthorization(options =>
//{
//    options.FallbackPolicy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//});


builder.Services.AddScoped<ITokenService, TokenService>(provider =>
    new TokenService(
        builder.Configuration["Jwt:Key"],
        builder.Configuration["Jwt:Issuer"],
        builder.Configuration["Jwt:Audience"],
        provider.GetRequiredService<IUserService>()
    )
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:4200", "http://localhost:5173", "http://localhost:5229/", "https://localhost:7237/", "http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("USER_VIEW", policy => policy.RequireClaim("Permission", "USER_VIEW"));
    options.AddPolicy("USER_CREATE", policy => policy.RequireClaim("Permission", "USER_CREATE"));
    options.AddPolicy("USER_UPDATE", policy => policy.RequireClaim("Permission", "USER_UPDATE"));
    options.AddPolicy("USER_DELETE", policy => policy.RequireClaim("Permission", "USER_DELETE"));
    options.AddPolicy("USER_ASSIGN_ROLE", policy => policy.RequireClaim("Permission", "USER_ASSIGN_ROLE"));
    options.AddPolicy("USER_UNASSIGN_ROLE", policy => policy.RequireClaim("Permission", "USER_UNASSIGN_ROLE"));
    options.AddPolicy("USER_ASSIGN_ROLES", policy => policy.RequireClaim("Permission", "USER_ASSIGN_ROLES"));
    options.AddPolicy("USER_UNASSIGN_ROLES", policy => policy.RequireClaim("Permission", "USER_UNASSIGN_ROLES"));
    options.AddPolicy("USER_UPLOAD_PROFILE_PICTURE", policy => policy.RequireClaim("Permission", "USER_UPLOAD_PROFILE_PICTURE"));
    options.AddPolicy("USER_VIEW_MENU_PERMISSIONS", policy => policy.RequireClaim("Permission", "USER_VIEW_MENU_PERMISSIONS"));
    options.AddPolicy("MENU_VIEW", policy => policy.RequireClaim("Permission", "MENU_VIEW"));
    options.AddPolicy("MENU_CREATE", policy => policy.RequireClaim("Permission", "MENU_CREATE"));
    options.AddPolicy("MENU_UPDATE", policy => policy.RequireClaim("Permission", "MENU_UPDATE"));
    options.AddPolicy("MENU_DELETE", policy => policy.RequireClaim("Permission", "MENU_DELETE"));
    options.AddPolicy("MENU_ASSIGN_ROLE", policy => policy.RequireClaim("Permission", "MENU_ASSIGN_ROLE"));
    options.AddPolicy("MENU_UNASSIGN_ROLE", policy => policy.RequireClaim("Permission", "MENU_UNASSIGN_ROLE"));
    options.AddPolicy("AUDIT_LOG_VIEW", policy => policy.RequireClaim("Permission", "AUDIT_LOG_VIEW"));
    options.AddPolicy("PERMISSION_VIEW", policy => policy.RequireClaim("Permission", "PERMISSION_VIEW"));
    options.AddPolicy("PERMISSION_CREATE", policy => policy.RequireClaim("Permission", "PERMISSION_CREATE"));
    options.AddPolicy("PERMISSION_UPDATE", policy => policy.RequireClaim("Permission", "PERMISSION_UPDATE"));
    options.AddPolicy("PERMISSION_DELETE", policy => policy.RequireClaim("Permission", "PERMISSION_DELETE"));
    options.AddPolicy("ROLE_VIEW", policy => policy.RequireClaim("Permission", "ROLE_VIEW"));
    options.AddPolicy("ROLE_CREATE", policy => policy.RequireClaim("Permission", "ROLE_CREATE"));
    options.AddPolicy("ROLE_UPDATE", policy => policy.RequireClaim("Permission", "ROLE_UPDATE"));
    options.AddPolicy("ROLE_DELETE", policy => policy.RequireClaim("Permission", "ROLE_DELETE"));
    options.AddPolicy("ROLE_ASSIGN_PERMISSION", policy => policy.RequireClaim("Permission", "ROLE_ASSIGN_PERMISSION"));
    options.AddPolicy("ROLE_UNASSIGN_PERMISSION", policy => policy.RequireClaim("Permission", "ROLE_UNASSIGN_PERMISSION"));
    options.AddPolicy("ROLE_VIEW_MENUS", policy => policy.RequireClaim("Permission", "ROLE_VIEW_MENUS")); // Added
    options.AddPolicy("CATEGORY_VIEW", policy => policy.RequireClaim("Permission", "CATEGORY_VIEW"));
    options.AddPolicy("CATEGORY_CREATE", policy => policy.RequireClaim("Permission", "CATEGORY_CREATE"));
    options.AddPolicy("CATEGORY_UPDATE", policy => policy.RequireClaim("Permission", "CATEGORY_UPDATE"));
    options.AddPolicy("CATEGORY_DELETE", policy => policy.RequireClaim("Permission", "CATEGORY_DELETE"));
    options.AddPolicy("UNIT_VIEW", policy => policy.RequireClaim("Permission", "UNIT_VIEW"));
    options.AddPolicy("UNIT_CREATE", policy => policy.RequireClaim("Permission", "UNIT_CREATE"));
    options.AddPolicy("UNIT_UPDATE", policy => policy.RequireClaim("Permission", "UNIT_UPDATE"));
    options.AddPolicy("UNIT_DELETE", policy => policy.RequireClaim("Permission", "UNIT_DELETE"));
    options.AddPolicy("SUPPLIER_VIEW", policy => policy.RequireClaim("Permission", "SUPPLIER_VIEW"));
    options.AddPolicy("SUPPLIER_CREATE", policy => policy.RequireClaim("Permission", "SUPPLIER_CREATE"));
    options.AddPolicy("SUPPLIER_UPDATE", policy => policy.RequireClaim("Permission", "SUPPLIER_UPDATE"));
    options.AddPolicy("SUPPLIER_DELETE", policy => policy.RequireClaim("Permission", "SUPPLIER_DELETE"));
    options.AddPolicy("MANUFACTURER_VIEW", policy => policy.RequireClaim("Permission", "MANUFACTURER_VIEW"));
    options.AddPolicy("MANUFACTURER_CREATE", policy => policy.RequireClaim("Permission", "MANUFACTURER_CREATE"));
    options.AddPolicy("MANUFACTURER_UPDATE", policy => policy.RequireClaim("Permission", "MANUFACTURER_UPDATE"));
    options.AddPolicy("MANUFACTURER_DELETE", policy => policy.RequireClaim("Permission", "MANUFACTURER_DELETE"));
    options.AddPolicy("PRODUCT_VIEW", policy => policy.RequireClaim("Permission", "PRODUCT_VIEW"));
    options.AddPolicy("PRODUCT_CREATE", policy => policy.RequireClaim("Permission", "PRODUCT_CREATE"));
    options.AddPolicy("PRODUCT_UPDATE", policy => policy.RequireClaim("Permission", "PRODUCT_UPDATE"));
    options.AddPolicy("PRODUCT_DELETE", policy => policy.RequireClaim("Permission", "PRODUCT_DELETE"));
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "RMS WEB API", Version = "v1" });

    // Add JWT support
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
    
}

app.UseStaticFiles();

// Redirect root URL to Swagger UI
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }

    await next();
});

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RMS.API v1");
    });
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RMS.API v1");
    c.RoutePrefix = "swagger"; // default, but explicitly safe
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Optional: public ping route
//app.MapGet("/", () => Results.Ok("RMS API is running.")).AllowAnonymous();

app.Run();
