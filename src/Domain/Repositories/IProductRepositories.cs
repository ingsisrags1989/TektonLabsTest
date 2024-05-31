using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<ProductEntity> GetProductByIdAsync(Guid id);
        Task<ProductEntity> CreateProductAsync(ProductEntity product);
        Task<ProductEntity> UpdateProductAsync(ProductEntity product);
        IQueryable<ProductEntity> GetAll();
    }
}
