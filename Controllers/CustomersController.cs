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
        var customers = _context.Customers
                .Include(c => c.MembershipType) // Eager load the related MembershipType
                .ToList();
        return View(customers);
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
        var MembershipType = _context.MembershipTypes.ToList();
        var ViewModels= new CustomerFormViewModel
        {
            MembershipTypes = MembershipType
        };
        return View("CustomerForm",ViewModels);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]    
    public ActionResult Create(CustomerFormViewModel viewModel)
    {
        // if (!ModelState.IsValid)
        // {
        //     viewModel.MembershipTypes = _context.MembershipTypes.ToList();
        //     return View("CustomerForm", viewModel);    
        // }

        _context.Customers.Add(viewModel.Customer);
        _context.SaveChanges();
        return RedirectToAction("Index","Customers");
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
    public ActionResult Edit(CustomerFormViewModel viewModel)
    {
        // if (!ModelState.IsValid)
        // {
        //     viewModel.MembershipTypes = _context.MembershipTypes.ToList();
        //     return View("CustomerForm", viewModel);
        // }
        if (viewModel.Customer.Id == 0)
        {
            _context.Customers.Add(viewModel.Customer);
        }
        else
        {
            var customerInDb = _context.Customers.Find(viewModel.Customer.Id);
            if (customerInDb == null)
                return NotFound();

            customerInDb.Name = viewModel.Customer.Name;
            customerInDb.Birthdate = viewModel.Customer.Birthdate;
            customerInDb.IsSubscribedToNewsletter = viewModel.Customer.IsSubscribedToNewsletter;
            customerInDb.MembershipTypeId = viewModel.Customer.MembershipTypeId;
        }
        

        _context.SaveChanges();

        return RedirectToAction("Index");
    }
    
}