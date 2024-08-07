using api.Data;
using api.Interfaces;
using api.Mappers;
using api.Repositories;
using api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ISheepRepository, SheepRepository>();
builder.Services.AddScoped<IExcelExportService, ExcelExportService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddHostedService<SheepDataHostedService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x
     .AllowAnyMethod()
     .AllowAnyHeader()
     .AllowCredentials()
      .WithOrigins("http://localhost:5173")
      .SetIsOriginAllowed(origin => true));
app.UseRouting();

app.UseAuthorization(); // If you have authorization, this is needed

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
