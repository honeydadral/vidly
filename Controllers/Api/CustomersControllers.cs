using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vidly.Data;
using Vidly.Models;
using AutoMapper;
using Vidly.Dtos; // Add this if CustomerDto is in the Dtos folder/namespace

namespace Vidly.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CustomersController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: /api/customers
    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers()
    {
        var customers = _context.Customers.Include(c=> c.MembershipType).ToList();
        // var customerDtos = customers.Select(c => _mapper.Map<CustomerDto>(c));
        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
        return Ok(customerDtos);
    }

    // GET: /api/customers/1
    [HttpGet("{id}")]
    public ActionResult<CustomerDto> GetCustomer(int id)
    {
        var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
        if (customer == null)
            return NotFound();

        return Ok(_mapper.Map<Customer, CustomerDto>(customer));
    }

    // POST: /api/customers
    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerDto customerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = _mapper.Map<Customer>(customerDto);
        _context.Customers.Add(customer);
        _context.SaveChanges();

        customerDto.Id = customer.Id; // update the dto with the generated id

        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.Id },
            customerDto
        );
    }

    // PUT: /api/customers/1
    [HttpPut("{id}")]
    public IActionResult UpdateCustomer(int id, CustomerDto customerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
        if (customerInDb == null)
            return NotFound();

        _mapper.Map(customerDto, customerInDb);

        // customerInDb.Name = customerDto.Name;
        // customerInDb.IsSubscribedToNewsletter = customerDto.IsSubscribedToNewsletter;
        // customerInDb.MembershipTypeId = customerDto.MembershipTypeId;
        // customerInDb.Birthdate = customerDto.Birthdate;

        _context.SaveChanges();

        return NoContent();
    }

    // DELETE: /api/customers/1
    [HttpDelete("{id}")]
    public IActionResult DeleteCustomer(int id)
    {
        var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
        if (customerInDb == null)
            return NotFound();

        _context.Customers.Remove(customerInDb);
        _context.SaveChanges();

        return NoContent();
    }
}
