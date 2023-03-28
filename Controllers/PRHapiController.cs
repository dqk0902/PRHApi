using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PRHApiClient;
using Models;
namespace PRHapi.Controllers
{
    [Route("postal_codes")]
    [ApiController]
    public class PRHapiController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly PrhApiClient _prhApiClient;

        public PRHapiController(string connectionString, PrhApiClient prhApiClient)
        {
            _connectionString = connectionString;
            _prhApiClient = prhApiClient;
        }

        [HttpGet("{postalCode}/companies")]
        public ActionResult<List<Company>> GetCompaniesByPostalCode(string postalCode)
        {
            // Get companies from database
            var companies = new List<Company>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM companies WHERE postal_code = @postalCode";
                    command.Parameters.AddWithValue("postalCode", postalCode);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var company = new Company
                            {
                                BusinessId = reader.IsDBNull(0) ? null : reader.GetString(0),
                                Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                                CompanyForm = reader.IsDBNull(2) ? null : reader.GetString(2),
                                DetailsUri = reader.IsDBNull(3) ? null : reader.GetString(3),
                                RegistrationDate = reader.GetDateTime(4)
                            };

                            companies.Add(company);
                        }
                    }
                }
            }
            return Ok(companies);
        }
    }
}