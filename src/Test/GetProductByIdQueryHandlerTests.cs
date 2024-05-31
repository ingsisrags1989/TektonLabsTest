using Application.Dto;
using Application.Handlers;
using Application.Queries;
using AutoMapper;
using Domain.Cache;
using Domain.Entities;
using Domain.Repositories;
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
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<MemoryCache> _mockMemoryCache;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetProductByIdQueryHandler(_mockRepository.Object, _mockMapper.Object, _mockMemoryCache.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductDto()
        {
            // Arrange
            var query = new GetProductByIdQuery { Id = Guid.NewGuid() };
            var product = new ProductEntity { Id = query.Id, Name = "Test Product", Price = 10.99M };
            var productDto = new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price };

            _mockRepository.Setup(r => r.GetProductByIdAsync(query.Id)).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<ProductDto>(result);
            Assert.Equal(productDto.Id, result.Id);
            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Price, result.Price);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyProductDto_WhenProductNotFound()
        {
            // Arrange
            var Id = Guid.NewGuid();
            var query = new GetProductByIdQuery { Id =Id};



            _mockRepository.Setup(r => r.GetProductByIdAsync(query.Id)).Returns(Task.FromResult(new ProductEntity(){ }));
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
