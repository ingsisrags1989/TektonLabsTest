using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Repositories.Generic;
using Infrastructure.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly IGenericRepository<ProductEntity> _productGenericRepository;
        private readonly IUnitOfWorkAsync _unitOfWorkAync;
        public ProductRepository(IGenericRepository<ProductEntity> genericRepository, IUnitOfWorkAsync unitOfWorkAsync)
        {

            _productGenericRepository = genericRepository;
            _unitOfWorkAync = unitOfWorkAsync;
        }
        public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
        {
            var productResult = await _productGenericRepository.InsertAsync(product);
            await _unitOfWorkAync.SaveChangesAsync();
            return productResult;
        }

        public async Task<ProductEntity> GetProductByIdAsync(Guid id)
        {
            return await _productGenericRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id) ?? new ProductEntity();
        }

        public async Task<ProductEntity> UpdateProductAsync(ProductEntity product)
        {
            var productResult = _productGenericRepository.UpdateAsync(product);
            await _unitOfWorkAync.SaveChangesAsync();
            return productResult;
        }
    }
}
