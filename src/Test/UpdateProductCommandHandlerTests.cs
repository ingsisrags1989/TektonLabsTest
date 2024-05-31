using Application.Commands;
using Application.Dto;
using Application.Handlers;
using AutoMapper;
using Domain.Common.MiddlewareException;
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
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateProductCommandHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductDto()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product",
                Price = 15.99M,
            };

            var product = new ProductEntity { Id = command.Id, Name = "Old Product", Price = 10.99M };
            var updatedProduct = new ProductEntity { Id = command.Id, Name = command.Name, Price = command.Price };
            var productDto = new ProductDto { Id = command.Id, Name = command.Name, Price = command.Price };

            _mockRepository.Setup(r => r.GetProductByIdAsync(command.Id)).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map(command, product)).Returns(updatedProduct);
            _mockRepository.Setup(r => r.UpdateProductAsync(updatedProduct)).Returns(Task.FromResult(product));
            _mockMapper.Setup(m => m.Map<ProductDto>(updatedProduct)).Returns(productDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ProductDto>(result);
            Assert.Equal(productDto.Id, result.Id);
            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Price, result.Price);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product",
                Price = 15.99M,
            };

            ProductEntity productEntity = null;

            _mockRepository.Setup(r => r.GetProductByIdAsync(command.Id)).Returns(Task.FromResult(productEntity));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
