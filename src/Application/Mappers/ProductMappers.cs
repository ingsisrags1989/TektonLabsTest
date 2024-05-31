using Application.Commands;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Enum;
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
            CreateMap<CreateProductCommand, ProductEntity>()
                .ForMember(y => y.StatusName, o => o.MapFrom(w => w.Status ? Status.Active : Status.Inactive))
                ;

            CreateMap<UpdateProductCommand, ProductEntity>()
                            .ForMember(y => y.StatusName, o => o.MapFrom(w => w.Status ? Status.Active : Status.Inactive))
                ;

            CreateMap<ProductEntity, ProductDto>();

            CreateMap<ProductDto, ProductEntity>();
        }
    }
}
