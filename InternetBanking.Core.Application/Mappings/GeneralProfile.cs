using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.Payment;
using InternetBanking.Core.Application.ViewModels.Product;
using InternetBanking.Core.Application.ViewModels.User;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region UserProfile
            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.InitialAmount, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Role, opt => opt.MapFrom(src => src.UserType.ToString()));

            CreateMap<AuthenticationResponse, SaveUserViewModel>()
                .ForMember(x => x.UserType, opt => opt.MapFrom(src => src.Roles.FirstOrDefault()))
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Roles, opt => opt.MapFrom(src => new List<string> { src.UserType.ToString() }));

            CreateMap<AuthenticationResponse, UserViewModel>()
                .ForMember(x => x.UserType, opt => opt.MapFrom(src => src.Roles.FirstOrDefault()))
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Roles, opt => opt.MapFrom(src => new List<string> { src.UserType }));
            #endregion

            #region ProductProfile
            CreateMap<Product, SaveProductViewModel>()
                .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Balance))
                .ForMember(x => x.ProductType, opt => opt.MapFrom(src => src.ProductTypeId))
                .ReverseMap()
                .ForMember(x => x.Balance, opt => opt.MapFrom(src => src.Amount))
                .ForMember(x => x.ProductTypeId, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.ProductType, opt => opt.MapFrom(src => src.ProductTypeId))
                .ReverseMap()
                .ForMember(x => x.ProductTypeId, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<SaveProductViewModel, ProductViewModel>()
                .ForMember(x => x.Balance, opt => opt.MapFrom(src => src.Amount))
                .ForMember(x => x.UserName, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Amount, opt => opt.MapFrom(src => src.Balance));

            #endregion

            #region BeneficiaryProfile
            CreateMap<Beneficiary, SaveBeneficiaryViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region PaymentProfile
            CreateMap<Payment, SavePaymentViewModel>()
                .ForMember(x => x.PaymentType, opt => opt.MapFrom(src => src.PaymentTypeId))
                .ForMember(x => x.Products, opt => opt.Ignore())
                .ForMember(x => x.CreditCardsProducts, opt => opt.Ignore())
                .ForMember(x => x.LoanProducts, opt => opt.Ignore())
                .ForMember(x => x.Beneficiaries, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.PaymentTypeId, opt => opt.MapFrom(src => src.PaymentType))
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            #endregion
        }
    }
}
