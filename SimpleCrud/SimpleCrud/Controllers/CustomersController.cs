using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCrud.BusinessLayer.Services;
using SimpleCrud.Shared.Models.Requests;
using SimpleCrud.Shared.Models.Responses;
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


        /// <summary>
        /// Get the paginated customers list
        /// </summary>
        /// <param name="search">Part of the first name or of the last name of the customers to retrieve</param>
        /// <param name="pageIndex">The index of the page to get</param>
        /// <param name="itemPerPage">The number of elements to get</param>
        /// <response code="200">The customers list</response>
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


        /// <summary>
        /// Get a specific customer
        /// </summary>
        /// <param name="id">Id of the customer to retrive</param>
        /// <response code="200">The desired customer</response>
        /// <response code="404">Customer not found</response>
        [HttpGet("{id:guid}", Name = nameof(GetCustomerById))]
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

        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <response code="201">Customer created successfully</response>
        /// <response code="400">Unable to create the customer due to validation error</response>
        [HttpPost]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> NewCustomerAsync([FromBody] SaveCustomerRequest customer)
        {
            var newCustomer = await service.NewCustomerAsync(customer);

            return CreatedAtRoute(routeName: nameof(GetCustomerById),
                                  routeValues: new { id = newCustomer.Id },
                                  value: newCustomer);

        }

        /// <summary>
        /// Delete a customer with a specific Id
        /// </summary>
        /// <param name="id">Id of the customer to delete</param>
        /// <response code="204">The customer was successfully deleted</response>
        /// <response code="404">Customer not found</response>
        /// <response code="400">The client’s request is not valid based on the server’s input validations.</response>
        [HttpDelete("{id:guid}")]
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

        /// <summary>
        ///  Update a customer with a specific Id
        /// </summary>
        /// <param name="id">Id of Customer to edit</param>
        /// <param name="customer"></param>
        /// <response code="204">The customer updated successfully</response>
        /// <response code="404">The customer to update was not found</response>
        /// <response code="400">Unable to edit the customer due to validation error</response>
        [HttpPut("{id:guid}")]
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
