using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleCrud.Shared;
using SimpleCrud.Shared.Models.Responses;
using SimpleCrud.Shared.Models.Requests;
using Entities = SimpleCrud.DataAccessLayer.Entities;

namespace SimpleCrud.BusinessLayer.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerAsync(Guid id);
        Task<ListResultPagination<Customer>> GetAsync(string searchText, int pageIndex, int itemsPerPage);
        Task<Customer> NewCustomerAsync(SaveCustomerRequest customer);
        Task DeleteCustomerAsync(Guid id);
        Task UpdateCustomerAsync(Guid id, SaveCustomerRequest customer);
        Task<Customer> GetCustomerForEdit(Guid id);
    }
}
