﻿using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Repositories.Generic;
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
        public ProductRepository(IGenericRepository<ProductEntity> genericRepository)
        {

            _productGenericRepository = genericRepository;
        }
        public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
        {
            var productResult = await _productGenericRepository.InsertAsync(product);
            return productResult;
        }

        public IQueryable<ProductEntity> GetAll()
        {
            return _productGenericRepository.GetAll();
        }

        public async Task<ProductEntity> GetProductByIdAsync(int id)
        {
            return await _productGenericRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id) ?? null;
        }

        public async Task<ProductEntity> UpdateProductAsync(ProductEntity product)
        {
            var productResult = await _productGenericRepository.UpdateAsync(product);
            return productResult;
        }
    }
}
