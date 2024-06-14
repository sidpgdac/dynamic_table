//using Microsoft.EntityFrameworkCore;
//using SmbcApp.Data;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddSwaggerGen();

//// Add CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowSpecificOrigin",
//        builder => builder
//            .WithOrigins("http://localhost:3000") // Replace with your frontend's URL
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .AllowCredentials());
//});



//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.UseCors("AllowSpecificOrigin");

//app.MapControllers();

//app.Run();



using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmbcApp.Data;
using smbcbackend.Audit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using smbcbackend.ValidationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidationService,ValidationService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:3000") // Replace with your frontend's URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Add HttpContextAccessor to access the current user
builder.Services.AddHttpContextAccessor();

// Register the AppDbContext with the current user and audit interceptor
builder.Services.AddScoped<AppDbContext>(provider =>
{
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var currentUser = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "unknown";
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(new AuditInterceptor(currentUser))
        .Options;
    return new AppDbContext(options, currentUser);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
