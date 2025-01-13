using Microsoft.EntityFrameworkCore;
using OrderManagement.Application;
using OrderManagement.Application.Services;
using OrderManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();


// Adaug? conexiunea la baza de date
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configureaz? serviciile ?i controller-ele
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Înregistreaz? serviciile în containerul de DI
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<CancelOrderService>();

builder.Services.AddSingleton(sp =>
    new AzureServiceBusService(configuration["AzureServiceBus:ConnectionString"]));

builder.Services.AddSingleton(sp =>
    new WorkflowProcessorService(configuration["AzureServiceBus:ConnectionString"]));



var app = builder.Build();

// Configureaz? middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
