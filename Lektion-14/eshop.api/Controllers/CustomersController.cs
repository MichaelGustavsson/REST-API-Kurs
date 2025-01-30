using System.Diagnostics;
using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(DataContext context) : ControllerBase
{
  private readonly DataContext _context = context;

  // 3 stycken metoder
  // En för att lista alla kunder http get
  // En för att hämta en kund på id http get
  // En för att lägga till en ny kund http post

  [HttpGet()]
  public async Task<ActionResult> GetAllCustomers()
  {
    // One-liner...
    var customers = await _context.Customers.ToListAsync();
    return Ok(new { success = true, data = customers });
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetCustomer(int id)
  {
    // Returnernar kundens namn, e-post, telefon samt adresserna...
    var customer = await _context.Customers
      .Where(c => c.Id == id)
      .Include(c => c.CustomerAddresses)
      .Select(customer => new
      {
        customer.Id,
        customer.FirstName,
        customer.LastName,
        customer.Email,
        customer.Phone,
        Addresses = customer.CustomerAddresses
        .Select(address => new
        {
          address.Address.AddressLine,
          address.Address.PostalAddress.PostalCode,
          address.Address.PostalAddress.City,
          address.Address.AddressType.Value
        })
      })
      .SingleOrDefaultAsync();

    if (customer is null) return NotFound(new { success = false, message = "Hittar ingen kund" });

    return Ok(new { success = true, data = customer });
  }

  [HttpPost()]
  public async Task<ActionResult> AddCustomer(CustomerPostViewModel model)
  {
    // Kontrollera om användare redan existerar...
    if (await _context.Customers.FirstOrDefaultAsync(c => c.Email.ToLower().Trim() == model.Email.ToLower().Trim()) != null)
    {
      return BadRequest(new { success = false, message = $"Kund {model.Email} finns redan i systemet." });
    }

    // 1. Lagt kunden, väntat in resultatet

    // 2. Lagt till leveransadress, väntat in resultatet

    // 3. Lagt till fakturaadress om finns, väntat in resultatet

    var DeliveryPostalAddress = await _context.PostalAddresses
      .FirstOrDefaultAsync(c => c.PostalCode.Replace(" ", "").Trim()
        == model.DeliveryPostalCode.Replace(" ", "").Trim());

    var DeliveryAddress = await _context.Addresses.FirstOrDefaultAsync(
      c => c.AddressLine.ToLower().Trim() == model.DeliveryAddress.ToLower().Trim()
      && c.AddressTypeId == 1
    );

    var InvoicePostalAddress = await _context.PostalAddresses
      .FirstOrDefaultAsync(c => c.PostalCode.Replace(" ", "").Trim()
        == model.InvoicePostalCode.Replace(" ", "").Trim());

    var InvoiceAddress = await _context.Addresses.FirstOrDefaultAsync(
      c => c.AddressLine.ToLower().Trim() == model.InvoiceAddress.ToLower().Trim()
      && c.AddressTypeId == 2
    );

    try
    {
      if (DeliveryPostalAddress is null)
      {
        DeliveryPostalAddress = new PostalAddress
        {
          PostalCode = model.DeliveryPostalCode.Replace(" ", "").Trim(),
          City = model.DeliveryCity.Trim()
        };

        await _context.PostalAddresses.AddAsync(DeliveryPostalAddress);
      }

      if (DeliveryAddress is null)
      {
        DeliveryAddress = new Address
        {
          AddressLine = model.DeliveryAddress.Trim(),
          AddressTypeId = 1,
          PostalAddress = DeliveryPostalAddress
        };

        await _context.Addresses.AddAsync(DeliveryAddress);
      }

      if (InvoicePostalAddress is null)
      {
        InvoicePostalAddress = new PostalAddress
        {
          PostalCode = model.InvoicePostalCode.Replace(" ", "").Trim(),
          City = model.InvoiceCity.Trim()
        };

        await _context.PostalAddresses.AddAsync(InvoicePostalAddress);
      }

      if (InvoiceAddress is null)
      {
        InvoiceAddress = new Address
        {
          AddressLine = model.InvoiceAddress.Trim(),
          AddressTypeId = 2,
          PostalAddress = InvoicePostalAddress
        };

        await _context.Addresses.AddAsync(InvoiceAddress);
      }

      var customer = new Customer
      {
        FirstName = model.FirstName.Trim(),
        LastName = model.LastName.Trim(),
        Email = model.Email.ToLower().Trim(),
        Phone = model.Phone.Trim()
      };

      await _context.Customers.AddAsync(customer);

      var customerDeliveryAddress = new CustomerAddress
      {
        Address = DeliveryAddress,
        Customer = customer
      };

      await _context.CustomerAddresses.AddAsync(customerDeliveryAddress);

      var customerInvoiceAddress = new CustomerAddress
      {
        Address = InvoiceAddress,
        Customer = customer
      };

      await _context.CustomerAddresses.AddAsync(customerInvoiceAddress);

      // Nu far det till databasen...
      await _context.SaveChangesAsync();

      return StatusCode(201);
    }
    catch (Exception ex)
    {
      return BadRequest(new { status = false, message = ex.Message });
    }


  }
}
// ' 444 46 ' => '44446'