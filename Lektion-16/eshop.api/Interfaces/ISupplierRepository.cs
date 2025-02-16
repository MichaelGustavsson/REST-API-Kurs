﻿using eshop.api.ViewModels.Supplier;

namespace eshop.api;

public interface ISupplierRepository
{
  public Task<IList<SuppliersViewModel>> ListAllSuppliers();
  public Task<SupplierViewModel> GetSupplier(int id);
  public Task<bool> Add(SupplierPostViewModel model);
}
