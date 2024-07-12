using AutoMapper;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreditCard, CreditCardModel>();
            CreateMap<CreditCardModel, CreditCard>();
            // Add other mappings as needed

            CreateMap<UserIncome, UserIncomeModel>().ReverseMap();
            CreateMap<UserBudget, UserBudgetModel>().ReverseMap();
            CreateMap<FamilyMemberRequest, FamilyMemberRequestModel>().ReverseMap();
        }
    }

}
