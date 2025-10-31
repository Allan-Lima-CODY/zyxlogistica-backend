using Microsoft.EntityFrameworkCore;
using ZyxLogistica.Application;
using ZyxLogistica.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

#region DbContext
var connection = builder.Configuration.GetConnectionString("ZyxLogistica");
builder.Services.AddDbContext<DatabaseContext>(o => o.UseSqlServer(
    connection,
    b => b.MigrationsAssembly("ZyxLogistica.Infrastructure")
    ));
#endregion

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
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

app.UseRouting();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
