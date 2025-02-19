namespace eshop.api;

public interface IUnitOfWork
{
  ISupplierRepository SupplierRepository { get; }
  IAddressRepository AddressRepository { get; }

  Task<bool> Complete();
  bool HasChanges();
}
