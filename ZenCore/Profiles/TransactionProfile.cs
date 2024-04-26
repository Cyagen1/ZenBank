using AutoMapper;

namespace ZenCore.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Entities.Transaction, Models.TransactionDto>();
            CreateMap<Entities.Transaction, Models.TransactionForUpdateDto>();
            CreateMap<Models.TransactionDto, Entities.Transaction>();
        }
    }
}
