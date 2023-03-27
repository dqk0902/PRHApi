using System;
using PRHApiClient;

public void ConfigureServices(IServiceCollection services)
{
    string connectionString = Configuration.GetConnectionString("MyDatabase");

   
    services.AddDbContext<MyDbContext>(options =>
        options.UseNpgsql(connectionString));
}
var apiClient = new PrhApiClient();
var connectionString = "Host=locallhost;Username=postgres;Password=zzjjjhh;Database=PRHapidb";
var companies = await apiClient.GetCompaniesByPostalCode("00700", connectionString);

foreach (var company in companies)
{
    Console.WriteLine(company.Name);
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();