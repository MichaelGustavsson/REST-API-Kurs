
using eshop.api.Data;
using Microsoft.EntityFrameworkCore;

namespace eshop.api;

public class SupplierRepository : ISupplierRepository
{
  private readonly DataContext _context;
  public SupplierRepository(DataContext context)
  {
    _context = context;

  }
  public async Task<SupplierViewModel> GetSupplier(int id)
  {
    try
    {
      var supplier = await _context.Suppliers
      .Where(s => s.Id == id)
      .Include(s => s.SupplierAddresses)
        .ThenInclude(s => s.Address)
        .ThenInclude(s => s.PostalAddress)
      .Include(s => s.SupplierAddresses)
        .ThenInclude(s => s.Address)
        .ThenInclude(s => s.AddressType)
      .SingleOrDefaultAsync();

      // Om inte leverantören finns så är det kört här...
      if (supplier is null)
      {
        throw new EShopException($"Finns ingen leverantör med id {id}");
      }

      var view = new SupplierViewModel
      {
        Id = supplier.Id,
        Name = supplier.Name,
        Email = supplier.Email,
        Phone = supplier.Phone
      };

      IList<AddressViewModel> addresses = [];

      foreach (var a in supplier.SupplierAddresses)
      {
        var addressView = new AddressViewModel
        {
          AddressLine = a.Address.AddressLine,
          PostalCode = a.Address.PostalAddress.PostalCode,
          City = a.Address.PostalAddress.City,
          AddressType = a.Address.AddressType.Value
        };

        addresses.Add(addressView);
      }

      view.Addresses = addresses;

      return view;
    }
    catch (EShopException ex)
    {
      throw new Exception(ex.Message);
    }
    catch (Exception ex)
    {
      throw new Exception($"Hoppsan det gick fel {ex.Message}");
    }
  }

  public async Task<IList<SuppliersViewModel>> ListAllSuppliers()
  {
    try
    {
      var suppliers = await _context.Suppliers.ToListAsync();

      IList<SuppliersViewModel> response = [];

      foreach (var supplier in suppliers)
      {
        var view = new SuppliersViewModel
        {
          Id = supplier.Id,
          Name = supplier.Name,
          Email = supplier.Email,
          Phone = supplier.Phone
        };

        response.Add(view);
      }

      return response;
    }
    catch (Exception ex)
    {
      throw new Exception($"Ett fel inträffade i ListAllSupplier(SupplierRepository) {ex.Message}");
    }

  }

}
