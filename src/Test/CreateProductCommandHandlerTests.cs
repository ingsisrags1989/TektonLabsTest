using Application.Commands;
using Application.Dto;
using Application.Handlers;
using AutoMapper;
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
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new CreateProductCommandHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductDto()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Price = 10.99M,
            };

            var product = new ProductEntity { Id = Guid.NewGuid(), Name = command.Name, Price = command.Price };
            var productDto = new ProductDto { Id = product.Id, Name = product.Name, Price = product.Price};

            _mockMapper.Setup(m => m.Map<ProductEntity>(command)).Returns(product);
            _mockRepository.Setup(r => r.CreateProductAsync(product)).Returns(Task.FromResult(product));
            _mockMapper.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ProductDto>(result);
            Assert.Equal(productDto.Id, result.Id);
            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Price, result.Price);
        }
    }
}
