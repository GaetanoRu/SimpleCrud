using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCrud.BusinessLayer.Services;
using SimpleCrud.Shared.Models.Responses;
using SimpleCrud.Shared.Models.Requests;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace SimpleCrud.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService service;

        public CustomersController(ICustomerService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        public async Task<ActionResult<ListResultPagination<Customer>>> GetCustomerList([FromQuery(Name = "search")] string search = null,
                                                                           [FromQuery(Name = "page")] int pageIndex = 0,
                                                                           [FromQuery(Name = "size")] int itemPerPage = 20)
        {
            var list = await service.GetAsync(search, pageIndex, itemPerPage);
            return list;
        }



        [HttpGet("{id:guid}", Name = nameof(GetCustomerById))]
        [ProducesDefaultResponseType]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> GetCustomerById(Guid id)
        {
            var customer = await service.GetCustomerAsync(id);
            if (customer != null)
            {
                return customer;
            }
            return NotFound();
        }


        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> NewCustomerAsync([FromBody] SaveCustomerRequest customer)
        {
            var newCustomer = await service.NewCustomerAsync(customer);

            return CreatedAtRoute(routeName: nameof(GetCustomerById),
                                  routeValues: new { id = newCustomer.Id },
                                  value: newCustomer);

        }


        [HttpDelete("{id:guid}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> DeleteCustomer(Guid id)
        {
            var customer = await service.GetCustomerAsync(id);
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            if (customer == null)
            {
                return NotFound();
            }

            await service.DeleteCustomerAsync(id);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> UpdateCustomerAsync(Guid id, [FromBody] SaveCustomerRequest customer)
        {

            var search = await service.GetCustomerForEdit(id);
            if (search == null)
            {
                return NotFound();
            }
            await service.UpdateCustomerAsync(id, customer);
            return NoContent();
        }

    }
}
