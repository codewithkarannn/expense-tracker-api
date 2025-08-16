using Budget_Tracker_WebAPI.Models;
using Budget_Tracker_WebAPI.Repositories;
using Budget_Tracker_WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// Register IUserRepository and AuthService
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAuthService, AuthService>();



// JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

builder.Services.AddDbContext<Db15765Context>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var myAngularAppUrl = builder.Configuration["AllowedOrigins:MyAngularAppUrl"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        // This line now dynamically uses the correct URL for dev or prod
        policy.WithOrigins(myAngularAppUrl)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
// The order of the following middleware is critical for proper functioning.

// 1. Exception handling (early in the pipeline)
if (app.Environment.IsDevelopment())
{
    // Use developer-friendly tools ONLY in the Development environment.
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // In Production, use a generic, secure error handler that does not leak details.
    // This returns a simple 500 error to the client. The actual error should be logged.
    app.UseExceptionHandler("/Error"); // A minimal API can be set up to handle this

    // Use HSTS to enforce HTTPS connections for better security.
    app.UseHsts();
}

// 2. HTTPS Redirection (before static files and routing)
app.UseHttpsRedirection();

// 3. Routing (before CORS and Authentication/Authorization)
app.UseRouting();

// 4. CORS (after routing and before authentication/authorization and endpoints)
app.UseCors("AllowSpecificOrigin");

// 5. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 6. Endpoints
app.MapControllers();

app.Run();