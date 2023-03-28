using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Db;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace PRHapi.Controllers
{
    [Route("postal_codes")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CompaniesController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("{postalCode}/companies")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompaniesByPostalCode(string postalCode)
        {
            var connectionString = "Host=localhost;Username=postgres;Password=zzjjjhh;Database=postgres"; 
            var query = $"SELECT * FROM companies WHERE postal_code = '{postalCode}'";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var companies = new List<Company>();

                        while (await reader.ReadAsync())
                        {
                            companies.Add(new Company
                            {
                                Id = reader.GetInt32(0),
                                BusinessId = reader.GetString(1),
                                Name = reader.GetString(2),
                                CompanyForm = reader.GetString(3),
                                DetailsUri = reader.GetString(4),
                                RegistrationDate = reader.GetDateTime(5),
                                PostalCode = reader.GetString(6)
                            });
                        }

                        if (companies.Count == 0)
                        {
                            return NotFound();
                        }

                        return companies;
                    }
                }
            }
        }
    }
}
