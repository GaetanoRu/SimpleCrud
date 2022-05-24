namespace SimpleCrud.Shared.Models.Responses
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}