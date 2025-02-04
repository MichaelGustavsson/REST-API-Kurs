namespace eshop.api;

public interface ISupplierRepository
{
  public Task<IList<SuppliersViewModel>> ListAllSuppliers();
  public Task<SupplierViewModel> GetSupplier(int id);

}
