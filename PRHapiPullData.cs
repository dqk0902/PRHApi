using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Npgsql;
using Models;

namespace PRHApiClient
{
    class ApiResponse
    {
        public List<Company> Results { get; set; }
        
        public IEnumerator<Company> GetEnumerator()
        {
            return Results.GetEnumerator();
        }
    }

    public class PrhApiClient
    {
        private const string ApiBaseUrl = "https://avoindata.prh.fi/bis/v1";
        private readonly HttpClient _httpClient;

        public PrhApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Company>> GetCompaniesByPostalCode(string postalCode, string connectionString)
        {
            var requestUrl = $"{ApiBaseUrl}?totalResults=false&maxResults=20&resultsFrom=0&streetAddressPostCode={postalCode}&companyRegistrationFrom=2014-02-28";

            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to retrieve companies for postal code {postalCode}. Status code: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var companies = JsonConvert.DeserializeObject<ApiResponse>(responseContent);

            using (var conn = new NpgsqlConnection(connectionString))
            {
                await conn.OpenAsync();

                foreach (var company in companies)
                {
                    using (var cmd = new NpgsqlCommand("INSERT INTO companies (business_id, name, company_form, details_uri, registration_date, postal_code) VALUES (@BusinessId, @Name, @CompanyForm, @DetailsUri, @RegistrationDate, @PostalCode)", conn))
                    {
                        cmd.Parameters.AddWithValue("BusinessId", company.BusinessId);
                        cmd.Parameters.AddWithValue("Name", company.Name);
                        cmd.Parameters.AddWithValue("CompanyForm", company.CompanyForm);
                        cmd.Parameters.AddWithValue("DetailsUri", company.DetailsUri);
                        cmd.Parameters.AddWithValue("RegistrationDate", company.RegistrationDate);
                        cmd.Parameters.AddWithValue("PostalCode", postalCode);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }

            return companies.Results;
        }
    }
}