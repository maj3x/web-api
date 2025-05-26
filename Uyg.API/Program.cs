using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Uyg.API.Models;
using Uyg.API.Repositories;
using Uyg.API.Helpers;
using Uyg.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "News Portal API",
        Version = "v1",
        Description = "A RESTful API for managing news articles"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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

// Add Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<INewsRepository, NewsRepository>();

// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("sqlCon"));
});

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("sqlCon"));
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Configure CORS
builder.Services.AddCors(p => p.AddPolicy("corspolicy", opt =>
{
    opt.WithOrigins("https://localhost:5001") // UI project URL
       .AllowAnyMethod()
       .AllowAnyHeader()
       .AllowCredentials();
}));

// Configure Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.MaxFailedAccessAttempts = 3;
})
.AddDefaultTokenProviders()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var key = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured");
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval'; style-src 'self' 'unsafe-inline';");
    await next();
});

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("corspolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

async Task SeedData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Create roles if they don't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("Editor"))
    {
        await roleManager.CreateAsync(new IdentityRole("Editor"));
    }
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    // Create admin user if it doesn't exist
    var adminUser = await userManager.FindByNameAsync("adminuser");
    if (adminUser == null)
    {
        adminUser = new AppUser
        {
            UserName = "adminuser",
            Email = "admin@example.com",
            FullName = "Admin User",
            PhotoUrl = "/images/default-avatar.png"
        };
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    // Create categories if they don't exist
    if (!context.Categories.Any())
    {
        var categories = new List<Category>
        {
            new Category { Name = "Technology", Description = "Latest technology news and updates", Slug = "technology" },
            new Category { Name = "Sports", Description = "Sports news and events", Slug = "sports" },
            new Category { Name = "Politics", Description = "Political news and analysis", Slug = "politics" },
            new Category { Name = "Entertainment", Description = "Entertainment news and celebrity updates", Slug = "entertainment" },
            new Category { Name = "Business", Description = "Business and financial news", Slug = "business" }
        };
        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    // Create sample news if they don't exist
    if (!context.News.Any())
    {
        var categories = await context.Categories.ToListAsync();
        var news = new List<News>
        {
            new News
            {
                Title = "New AI Technology Breakthrough",
                Content = "Scientists have made a significant breakthrough in artificial intelligence technology...",
                Summary = "A major advancement in AI research promises to revolutionize the field.",
                CategoryId = categories.First(c => c.Name == "Technology").Id,
                AuthorId = adminUser.Id,
                Created = DateTime.UtcNow.AddDays(-1),
                Updated = DateTime.UtcNow.AddDays(-1),
                PublishedAt = DateTime.UtcNow.AddDays(-1),
                IsPublished = true,
                IsActive = true,
                ImageUrl = "/images/ai-news.jpg",
                TagList = new List<Tag> { new Tag { Name = "AI", Slug = "ai" }, new Tag { Name = "Technology", Slug = "technology" }, new Tag { Name = "Innovation", Slug = "innovation" } }
            },
            new News
            {
                Title = "World Cup Finals",
                Content = "The World Cup finals are set to begin next month...",
                Summary = "The biggest football tournament is about to start.",
                CategoryId = categories.First(c => c.Name == "Sports").Id,
                AuthorId = adminUser.Id,
                Created = DateTime.UtcNow.AddDays(-2),
                Updated = DateTime.UtcNow.AddDays(-2),
                PublishedAt = DateTime.UtcNow.AddDays(-2),
                IsPublished = true,
                IsActive = true,
                ImageUrl = "/images/worldcup.jpg",
                TagList = new List<Tag> { new Tag { Name = "Football", Slug = "football" }, new Tag { Name = "Sports", Slug = "sports" }, new Tag { Name = "World Cup", Slug = "world-cup" } }
            },
            new News
            {
                Title = "Global Economic Summit",
                Content = "World leaders gather for the annual economic summit...",
                Summary = "Top economic leaders meet to discuss global challenges.",
                CategoryId = categories.First(c => c.Name == "Politics").Id,
                AuthorId = adminUser.Id,
                Created = DateTime.UtcNow.AddDays(-3),
                Updated = DateTime.UtcNow.AddDays(-3),
                PublishedAt = DateTime.UtcNow.AddDays(-3),
                IsPublished = true,
                IsActive = true,
                ImageUrl = "/images/economy.jpg",
                TagList = new List<Tag> { new Tag { Name = "Economy", Slug = "economy" }, new Tag { Name = "Politics", Slug = "politics" }, new Tag { Name = "Global", Slug = "global" } }
            }
        };
        await context.News.AddRangeAsync(news);
        await context.SaveChangesAsync();
    }
}

await SeedData(app.Services);

app.Run();
