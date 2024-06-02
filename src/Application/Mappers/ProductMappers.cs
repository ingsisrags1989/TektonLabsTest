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

            CreateMap<Tuple<ProductEntity, int, decimal>, ProductDto>()
                .ForMember(y => y.Id, o => o.MapFrom(x => x.Item1.Id))
                .ForMember(y => y.Name, o => o.MapFrom(x => x.Item1.Name))
                .ForMember(y => y.Stock, o => o.MapFrom(x => x.Item1.Stock))
                .ForMember(y => y.Price, o => o.MapFrom(x => x.Item1.Price))
                .ForMember(y => y.Description, o => o.MapFrom(x => x.Item1.Description))
                .ForMember(y => y.StatusName, o => o.MapFrom(x => x.Item1.StatusName))
                .ForMember(y => y.Discount, o => o.MapFrom(x => x.Item2))
                .ForMember(y => y.FinalPrice, o => o.MapFrom(x => x.Item3));

            CreateMap<ProductDto, ProductEntity>();
        }
    }
}
