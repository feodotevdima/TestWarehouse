using Application.Interfaces;
using Application.Repository;
using Application.Services;
using TestWarehouse.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IUnitRepository, UnitRepository>();
builder.Services.AddTransient<IResourceRepository, ResourceRepository>();
builder.Services.AddTransient<IBalanceRepository, BalanceRepository>();
builder.Services.AddTransient<IIncomeRepository, IncomeRepository>();
builder.Services.AddTransient<IIncomeResourceRepository, IncomeResourceRepository>();
builder.Services.AddTransient<IShipmentRepository, ShipmentRepository>();
builder.Services.AddTransient<IShipmentResourceRepository, ShipmentResourceRepository>();

builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<IUnitService, UnitService>();
builder.Services.AddTransient<IResourceService, ResourceService>();
builder.Services.AddTransient<IBalanceService, BalanceService>();
builder.Services.AddTransient<IIncomeService, IncomeService>();
builder.Services.AddTransient<IShipmentService, NewShipmentService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseStaticFiles();

app.MapControllers();

app.Run();
