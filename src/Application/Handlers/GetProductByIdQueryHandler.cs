using Application.Commands;
using Application.Dto;
using Application.Queries;
using AutoMapper;
using Domain.Cache;
using Domain.Entities;
using Domain.Enum;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly MemoryCache _memoryCache;

        public GetProductByIdQueryHandler(IProductRepository repository, IMapper mapper, MemoryCache memoryCache)
        {
            _productRepository = repository;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }


        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.Id);

            var statusFromCache = _memoryCache.GetCache<Status?>(request.Id.ToString());
            if (statusFromCache == null)
            {
                _memoryCache.SetCache(request.Id.ToString(), product.StatusName);
            }
            else
            {
                product.StatusName = statusFromCache.HasValue ? statusFromCache.Value : product.StatusName;
            }


            if (product is null) return new ProductDto();

            return _mapper.Map<ProductDto>(product);
        }
    }
}
