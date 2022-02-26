using CustomerService.Configuration;
using CustomerService.Domain.CustomerAggregate;
using CustomerService.Domain.CustomerAggregate.CommandHandlers;
using CustomerService.Repositories;
using EventDriven.DependencyInjection.URF.Mongo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add automapper
builder.Services.AddAutoMapper(typeof(Program));

// Add database settings
builder.Services.AddSingleton<CustomerCommandHandler>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddMongoDbSettings<CustomerDatabaseSettings, Customer>(builder.Configuration);

// Add Dapr event bus
builder.Services.AddDaprEventBus(builder.Configuration, true);
builder.Services.AddDaprMongoEventCache(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();