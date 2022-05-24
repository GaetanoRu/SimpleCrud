using SimpleCrud.Shared.Models.Requests;
using SimpleCrud.Shared.Models.Responses;
using AutoMapper;
using SimpleCrud.DataAccessLayer;
using Entities = SimpleCrud.DataAccessLayer.Entities;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace SimpleCrud.BusinessLayer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDataContext dataContext;
        private readonly IMapper mapper;

        public CustomerService(IDataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            var dbCustomer = await dataContext.GetAsync<Entities.Customer>(id);

            if (dbCustomer != null)
            {
                var customer = mapper.Map<Customer>(dbCustomer);
                return customer;
            }
            return null;

        }

        public async Task<ListResultPagination<Customer>> GetAsync(string searchText, int pageIndex, int itemsPerPage)
        {
            var dbCustomer = dataContext.GetData<Entities.Customer>()
                .Where(a => searchText == null || a.FirstName.Contains(searchText) || a.LastName.Contains(searchText));

            var totalCount = dbCustomer.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
            var currentPage = (pageIndex <= 0) ? 1 : pageIndex;

            var customers = await dbCustomer
                .OrderBy(c => c.City)
                .Skip(pageIndex * itemsPerPage)
                .Take(itemsPerPage + 1)
                .ProjectTo<Customer>(mapper.ConfigurationProvider)
                .ToListAsync();

            return new ListResultPagination<Customer>(customers.Take(itemsPerPage), totalCount, totalPages, currentPage, customers.Count > itemsPerPage);
        }

        public async Task<Customer> NewCustomerAsync(SaveCustomerRequest customer)
        {
            var newCustomer = mapper.Map<Entities.Customer>(customer);

            dataContext.Insert(newCustomer);
            await dataContext.SaveAsync();

            var result = mapper.Map<Customer>(newCustomer);
            return result;
        }

        public async Task UpdateCustomerAsync(Guid id, SaveCustomerRequest customer)
        {

            var edit = mapper.Map<Entities.Customer>(customer);
            edit.Id = id;

            dataContext.Edit(edit);
            await dataContext.SaveAsync();
            mapper.Map<Customer>(edit);

        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            var customer = await dataContext.GetAsync<Entities.Customer>(id);
            if (customer != null)
            {
                dataContext.Delete(customer);
                await dataContext.SaveAsync();
            }
        }

        public async Task<Customer> GetCustomerForEdit(Guid id)
        {
            var dbCustomer = await dataContext.GetData<Entities.Customer>().FirstOrDefaultAsync(a => a.Id == id);

            if (dbCustomer != null)
            {
                var result = mapper.Map<Customer>(dbCustomer);
                return result;
            }
            return null;
        }
    }
}