using Application;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<LeaveManagementContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<LeaveRequestRepository>();


builder.Services.AddAutoMapper(typeof(Application.Mappings.MappingProfile));



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();
