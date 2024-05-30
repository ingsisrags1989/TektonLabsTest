using Application.Commands;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class ProductMappers : Profile
    {
        public ProductMappers()
        {
            CreateMap<CreateProductCommand, ProductEntity>();

            CreateMap<ProductEntity, ProductDto>();
           
            CreateMap<ProductDto, ProductEntity>();
        }
    }
}
