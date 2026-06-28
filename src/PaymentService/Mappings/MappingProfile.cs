using AutoMapper;
using PaymentService.Events;
using PaymentService.Models;
using PaymentService.UseCases.Commands;
using PaymentService.UseCases.Queries;

namespace PaymentService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Маппинг для создания платежа: Команда -> Модель БД
            CreateMap<CreatePaymentCommand, Payment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => false));

            // Маппинг для ответа: Модель БД -> DTO ответа
            CreateMap<Payment, PaymentResponse>();

            // Маппинг для события, которое отправляется в Kafka: Модель БД -> Событие
            CreateMap<Payment, PaymentCompletedEvent>()
                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id));
        }
    }
}