using AutoMapper;
using StankinQuestionnaire.Areas.Admin.Models;
using StankinQuestionnaire.Areas.User.Models;
using StankinQuestionnaire.Data.Models;
using StankinQuestionnaire.Model;
using StankinQuestionnaire.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace StankinQuestionnaire.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "ViewModelToDomain";
            }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<CalculationTypeAddModel, CalculationType>();
            Mapper.CreateMap<CalculationTypeFormModel, CalculationType>();

            Mapper.CreateMap<IndicatorAddModel, Indicator>();
            Mapper.CreateMap<IndicatorFormModel, Indicator>();
            Mapper.CreateMap<IndicatorEditModel, Indicator>()
                .ForMember(indicator => indicator.CalculationTypes,
                indicatoreEit => indicatoreEit.Ignore());

            Mapper.CreateMap<IndicatorGroupAddModel, IndicatorGroup>();
            Mapper.CreateMap<IndicatorGroupFormModel, IndicatorGroup>();
            Mapper.CreateMap<IndicatorGroupEditModel, IndicatorGroup>()
                .ForMember(indicatorGroup => indicatorGroup.Indicators,
                indicatoreGroupEdit => indicatoreGroupEdit.Ignore());

            Mapper.CreateMap<DocumentTypeAddModel, DocumentType>();
            Mapper.CreateMap<DocumentTypeFormModel, DocumentType>();
            Mapper.CreateMap<DocumentTypeEditModel, DocumentType>()
                .ForMember(documentType => documentType.IndicatorsGroups,
                documentTypeEdit => documentTypeEdit.Ignore());

            Mapper.CreateMap<CalculationJSON, Calculation>();

            Mapper.CreateMap<UserEditModel, ApplicationUser>()
                .AfterMap((vm, m) => m.EmailConfirmed = false)
                .AfterMap((vm, m) => m.SecurityStamp = Guid.NewGuid().ToString())
                .AfterMap((vm, m) => m.PhoneNumberConfirmed = false)
                .AfterMap((vm, m) => m.TwoFactorEnabled = false)
                .AfterMap((vm, m) => m.LockoutEnabled = false)
                .AfterMap((vm, m) => m.AccessFailedCount = 0)
                .AfterMap((vm, m) => m.PasswordHash = Crypto.HashPassword(vm.Password));

            Mapper.CreateMap<SubdivisionEditModel, Subdivision>();

            Mapper.CreateMap<AcademicRankViewModel, AcademicRank>()
                .ForMember(m => m.AcademicRankID,
                vm => vm.MapFrom(s => s.ID))
                .ForMember(m => m.RatingGroups,
                vm => vm.MapFrom(s => s.Groups));
            Mapper.CreateMap<RatingGroupViewModel, RatingGroup>();
            Mapper.CreateMap<AcademicRankCatalogEditModel, AcademicRank>()
                .ForMember(m => m.AcademicRankID,
                vm => vm.MapFrom(s => s.ID));
        }
    }
}