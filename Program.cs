using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PRHApiClient;
using MyDbContext.Db;
using Models;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        string connectionString = Configuration.GetConnectionString("MyDatabase");
        services.AddDbContext<MyDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddControllers();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<MyDbContext>();
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseRouting();

        app.MapControllers();

        await GetCompaniesAsync();

        app.Run();
    }

    private static async Task GetCompaniesAsync()
    {
        var apiClient = new PrhApiClient();
        var connectionString = "Host=locallhost;Username=postgres;Password=zzjjjhh;Database=PRHapidb";
        var companies = await apiClient.GetCompaniesByPostalCode("00700", connectionString);

        foreach (var company in companies)
        {
            Console.WriteLine(company.Name);
        }
    }
}