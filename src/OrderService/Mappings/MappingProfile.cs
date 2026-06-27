using AutoMapper;
using OrderService.Models;
using OrderService.UseCases.Commands;
using OrderService.UseCases.Queries;

namespace OrderService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateOrderCommand, Order>();
            CreateMap<Order, OrderResponse>();
        }
    }
}