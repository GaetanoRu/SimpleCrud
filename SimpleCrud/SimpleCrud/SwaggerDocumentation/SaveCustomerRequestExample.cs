using Swashbuckle.AspNetCore.Filters;
using SimpleCrud.Shared.Models.Requests;

namespace SimpleCrud.SwaggerDocumentation
{
    public class SaveCustomerRequestExample : IExamplesProvider<SaveCustomerRequest>
    {
        public SaveCustomerRequest GetExamples()
        {
            return new SaveCustomerRequest
            {
                FirstName = "Mario",
                LastName = "Rossi",
                StreetAddress = "Via Roma 1",
                City = "Roma",
                Country = "Italy",
                PostalCode = "00118",
                Phone = "333 1234567",
                Email = "mario.rossi@example.com"
            };
        }
    }
}