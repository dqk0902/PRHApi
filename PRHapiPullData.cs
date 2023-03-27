using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace PRHApiClient
{
    class Company
    {
        public string? BusinessId { get; set; }
        public string? Name { get; set; }
        public string? CompanyForm { get; set; }
        public string? DetailsUri { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

    class ApiResponse
    {
        public List<Company> Results { get; set; }
    }

    class PrhApiClient
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
    var companies = JsonConvert.DeserializeObject<List<Company>>(responseContent);

    using (var conn = new NpgsqlConnection(connectionString))
    {
        await conn.OpenAsync();

        foreach (var company in companies)
        {
            using (var cmd = new NpgsqlCommand("INSERT INTO companies (business_id, name, company_form, details_uri, registration_date) VALUES (@BusinessId, @Name, @CompanyForm, @DetailsUri, @RegistrationDate)", conn))
            {
                cmd.Parameters.AddWithValue("BusinessId", company.BusinessId);
                cmd.Parameters.AddWithValue("Name", company.Name);
                cmd.Parameters.AddWithValue("CompanyForm", company.CompanyForm);
                cmd.Parameters.AddWithValue("DetailsUri", company.DetailsUri);
                cmd.Parameters.AddWithValue("RegistrationDate", company.RegistrationDate);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    return companies;
}
}
}