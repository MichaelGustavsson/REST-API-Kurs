
using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.ViewModels.Address;
using eshop.api.ViewModels.Supplier;
using Microsoft.EntityFrameworkCore;

namespace eshop.api;

public class SupplierRepository(DataContext context, IAddressRepository repo) : ISupplierRepository
{
  private readonly DataContext _context = context;
  private readonly IAddressRepository _repo = repo;

  public async Task<bool> Add(SupplierPostViewModel model)
  {
    try
    {
      if (await _context.Suppliers.FirstOrDefaultAsync(c => c.Email.ToLower().Trim() ==
       model.Email.ToLower().Trim()) != null)
      {
        throw new EShopException("Leverantören finns redan");
      }

      var supplier = new Supplier
      {
        Name = model.Name,
        Email = model.Email,
        Phone = model.Phone
      };

      await _context.Suppliers.AddAsync(supplier);

      foreach (var a in model.Addresses)
      {
        var address = await _repo.Add(a);
        await _context.SupplierAddresses.AddAsync(new SupplierAddress { Address = address, Supplier = supplier });
      }

      return true;
    }
    catch (EShopException ex)
    {
      throw new Exception(ex.Message);
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }

  }

  public async Task<bool> Add(int supplierId, AddressPostViewModel model)
  {
    try
    {
      var supplier = await _context.Suppliers.FindAsync(supplierId) ?? throw new EShopException($"Hittar ingen leverantör med id {supplierId}");

      var addressTypeExists = await _context.SupplierAddresses
        .Include(s => s.Address)
        .ThenInclude(s => s.AddressType)
        .Where(s => s.Address.AddressTypeId == (int)model.AddressType)
        .FirstOrDefaultAsync(c => c.SupplierId == supplierId);

      if (addressTypeExists is not null) throw new EShopException($"Leverantören har redan en adress med typen {model.AddressType}, gör en uppdatering istället.");

      var address = await _repo.Add(model);
      await _context.SupplierAddresses.AddAsync(new SupplierAddress { Address = address, Supplier = supplier });

      return true;
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }

  }

  public async Task<SupplierViewModel> Find(int id)
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

  public async Task<IList<SuppliersViewModel>> List()
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

  public async Task<bool> Remove(int supplierId, AddressPostViewModel model)
  {
    try
    {
      var address = await _context.SupplierAddresses
        .Include(c => c.Address)
          .ThenInclude(c => c.AddressType)
        .Where(s => s.Address.AddressTypeId == (int)model.AddressType)
        .FirstOrDefaultAsync(c => c.SupplierId == supplierId);

      var result = _repo.Delete(address.Address);
      return true;
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }
  }

  public async Task<bool> Update(int supplierId, AddressPostViewModel model)
  {
    try
    {
      var result = await _context.SupplierAddresses
        .Include(c => c.Address)
          .ThenInclude(c => c.AddressType)
        .Include(c => c.Address.PostalAddress)
        .Where(s => s.Address.AddressTypeId == (int)model.AddressType)
        .FirstOrDefaultAsync(c => c.SupplierId == supplierId);

      result.Address.AddressLine = model.AddressLine;
      result.Address.AddressTypeId = (int)model.AddressType;
      result.Address.PostalAddress.PostalCode = model.PostalCode;
      result.Address.PostalAddress.City = model.City;
      return true;
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }
  }
}
