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

            CreateMap<Order, OrderResponse>()
                // Из поля Quantity в БД делаем Amount для ответа
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Quantity))

                // Достаем Email и Телефон из связанной таблицы Client
                .ForMember(dest => dest.EmailClient, opt => opt.MapFrom(src => src.Client.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Client.PhoneNumber))

                // Достаем Цену из связанной таблицы Product
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));
        }
    }
}