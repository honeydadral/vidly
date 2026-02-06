using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore;
using Vidly.Data;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers;

public class CustomersController: Controller
{
    // private ApplicationDbContext _context;
    // public CustomersController()
    // {
    //     _context = new ApplicationDbContext();
    // }
    private readonly ApplicationDbContext _context;

    // Dependency Injection: Gets the properly configured DbContext from Program.
    //construct
    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }
    protected override void Dispose(bool disposing)
    {
        _context.Dispose();
    }
    public ViewResult Index()
    {
        // var customers = GetCustomers();
        // var customers = _context.Customers.ToList();
        // var customers = _context.Customers
        //         .Include(c => c.MembershipType) // Eager load the related MembershipType
        //         .ToList();
        return View();
    }

    public ActionResult Details(int Id)
    {
        // var customers = GetCustomers().SingleOrDefault(c => c.Id == Id);
        // var customers = _context.Customers.SingleOrDefault(c => c.Id == Id);
        var customer = _context.Customers
              .Include(c => c.MembershipType)
                .SingleOrDefault(c => c.Id == Id);

        if( customer == null)
        {
            return NotFound();
        }
        return View(customer);
    }
    public ActionResult New()
    {
        var membershipTypes = _context.MembershipTypes.ToList();
        var viewModel = new CustomerFormViewModel
        {
            MembershipTypes = membershipTypes,
            Customer = new Customer() // Initialize an empty customer for the form
        };
        return View("CustomerForm", viewModel);
    }

   
    public ActionResult Edit(int id)
    {
        var customer = _context.Customers.SingleOrDefault(c => c.Id ==id);
        if(customer == null)
        {
            return NotFound();
        }
        var viewModel = new CustomerFormViewModel
        {
            MembershipTypes = _context.MembershipTypes.ToList(),
            Customer = customer
        };
        return View("CustomerForm", viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Save(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };
            return View("CustomerForm", viewModel);
        }
      
       
        if(customer.Id == 0)
        {
            _context.Customers.Add(customer);
        }
        else
        {
            var customerInDb = _context.Customers.Find(customer.Id);
            if (customerInDb == null)
                return NotFound();

            //Mapper.Map(customer, customerInDb); // Using AutoMapper to map properties
            customerInDb.Name = customer.Name;
            customerInDb.Birthdate = customer.Birthdate;
            customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            customerInDb.MembershipTypeId = customer.MembershipTypeId;
        }
        
        

        _context.SaveChanges();

        return RedirectToAction("Index", "Customers");
    }
    
}