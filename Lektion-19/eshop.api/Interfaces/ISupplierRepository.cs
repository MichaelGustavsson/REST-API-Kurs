using eshop.api.ViewModels.Address;
using eshop.api.ViewModels.Supplier;

namespace eshop.api;

public interface ISupplierRepository
{
  public Task<IList<SuppliersViewModel>> List();
  public Task<SupplierViewModel> Find(int id);
  public Task<bool> Add(SupplierPostViewModel model);
  public Task<bool> Add(int supplierId, AddressPostViewModel model);
  public Task<bool> Update(int supplierId, AddressPostViewModel model);
  public Task<bool> Remove(int supplierId, AddressPostViewModel model);
}
