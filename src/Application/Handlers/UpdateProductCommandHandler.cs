using Application.Commands;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IProductRepository repository, IMapper mapper)
        {
            _productRepository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            var product = await _productRepository.GetProductByIdAsync(request.Id);

            if (product is null) throw new Exception("No existe el producto");

            product = _mapper.Map(request, product);

            await _productRepository.UpdateProductAsync(product);

            return _mapper.Map<ProductDto>(product);

        }
    }
}
