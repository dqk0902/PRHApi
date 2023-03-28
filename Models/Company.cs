namespace Models
{
    public class Company
    {
        public string? BusinessId { get; set; }
        public string? Name { get; set; }
        public string? CompanyForm { get; set; }
        public string? DetailsUri { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? PostalCode {get; set;}
    }
}