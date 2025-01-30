
namespace eshop.api;

public class SuppliersMockRepository : ISupplierRepository
{
  public Task<SupplierViewModel> GetSupplier(int id)
  {
    throw new NotImplementedException();
  }

  public Task<IList<SuppliersViewModel>> ListAllSuppliers()
  {
    throw new NotImplementedException();
  }
}
