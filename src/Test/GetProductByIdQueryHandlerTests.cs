using Application.Dto;
using Application.Handlers;
using Application.Queries;
using AutoMapper;
using Domain.Cache;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IProductDiscountRepository> _mockProductDiscountRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Domain.Cache.MemoryCache _memoryCache;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            var memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
            _mockProductDiscountRepository = new Mock<IProductDiscountRepository>();
            _memoryCache = new Domain.Cache.MemoryCache(memoryCache);
            _handler = new GetProductByIdQueryHandler(_mockRepository.Object, _mockMapper.Object, _memoryCache, _mockProductDiscountRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductDto()
        {
            // Arrange
            var query = new GetProductByIdQuery { Id = 1 };
            var product = new ProductEntity { Id = query.Id, Name = "Test Product", Price = 10.99M };
            var productDiscount = new ProductDiscountEntity { ProductId = query.Id, Discount = 50 }; 
            var productDto = new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price };

            _mockRepository.Setup(r => r.GetProductByIdAsync(query.Id)).ReturnsAsync(product);
            _mockProductDiscountRepository.Setup(r => r.GetDataAsync(query.Id)).ReturnsAsync(productDiscount);
            _mockMapper.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<ProductDto>(result);
            Assert.Equal(productDto.Id, result.Id);
            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Price, result.Price);
        }
    }
}
