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
        private readonly IProductDiscountRepository _productDiscountRepository;
        public GetProductByIdQueryHandler(IProductRepository repository, IMapper mapper, MemoryCache memoryCache,
            IProductDiscountRepository productDiscountRepository
            )
        {
            _productRepository = repository;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _productDiscountRepository = productDiscountRepository;
        }


        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProductByIdAsync(request.Id);

            if (product is null) return new ProductDto();

            var statusFromCache = _memoryCache.GetCache<Status?>(request.Id.ToString());
            if (statusFromCache == null)
            {
                _memoryCache.SetCache(request.Id.ToString(), product.StatusName);
            }
            else
            {
                product.StatusName = statusFromCache.HasValue ? statusFromCache.Value : product.StatusName;
            }

            var discount = await _productDiscountRepository.GetDataAsync(product.Id);
            var finalPrice = product.Price - (product.Price * discount.Discount/ 100);

            return _mapper.Map<ProductDto>( new Tuple<ProductEntity, int, decimal>(product, discount.Discount, finalPrice));
        }
    }
}
