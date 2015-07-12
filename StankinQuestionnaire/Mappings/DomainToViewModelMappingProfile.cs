using AutoMapper;
using StankinQuestionnaire.Areas.Admin.Models;
using StankinQuestionnaire.Areas.User.Models;
using StankinQuestionnaire.Core.Enums;
using StankinQuestionnaire.Data.Models;
using StankinQuestionnaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StankinQuestionnaire.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "DomainToViewModel";
            }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<CalculationType, CalculationTypeFormModel>();

            Mapper.CreateMap<Indicator, IndicatorFormModel>()
                .ForMember(indicatorForm => indicatorForm.CalculationTypes,
                indicator => indicator.Ignore());
            Mapper.CreateMap<CalculationType, CalculationTypeSelect>();

            Mapper.CreateMap<IndicatorGroup, IndicatorGroupFormModel>()
                .ForMember(indicatorGroupForm => indicatorGroupForm.Indicators,
                indicatoGroup => indicatoGroup.Ignore());
            Mapper.CreateMap<Indicator, IndicatorSelect>();

            Mapper.CreateMap<DocumentType, DocumentTypeFormModel>()
    .ForMember(documentTypeForm => documentTypeForm.IndicatorGroups,
    documentType => documentType.Ignore());
            Mapper.CreateMap<IndicatorGroup, IndicatorGroupSelect>();

            Mapper.CreateMap<Document, DocumentJSON>()
                .ForMember(docJson => docJson.IndicatorGroups,
                document => document.MapFrom(s => Mapper.Map<IEnumerable<IndicatorGroup>,
                    IEnumerable<IndicatorGroupJSON>>(s.DocumentType.IndicatorsGroups)))
                    .ForMember(vm => vm.MaxPoint,
                    m => m.MapFrom(s => s.DocumentType.MaxPoint))
                    .ForMember(vm => vm.Weight,
                    m => m.MapFrom(s => s.DocumentType.Weight));

            Mapper.CreateMap<Calculation, CalculationJSON>();

            Mapper.CreateMap<CalculationType, CalculationTypeJSON>()
                .ForMember(calcTypeJSON => calcTypeJSON.Calculations,
                calcType => calcType.Ignore());

            Mapper.CreateMap<Indicator, IndicatorJSON>();
            Mapper.CreateMap<IndicatorGroup, IndicatorGroupJSON>();

            Mapper.CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(userVM => userVM.SubvisionName,
                user => user.ResolveUsing((ApplicationUser u) => u.Subdivision == null ? "Кафедра не назначена" : u.Subdivision.Name))
                // user => user.MapFrom(userVM => userVM.Subdivision != null ? userVM.Subdivision.Name : userVM.Subdivision.Name))
                .ForMember(userVM => userVM.UserID,
                user => user.MapFrom(userVM => userVM.Id));
            Mapper.CreateMap<ApplicationUser, UserEditModel>();

            Mapper.CreateMap<SubdivisionWithUsersCount, SubdivisionViewModel>();
            Mapper.CreateMap<Subdivision, SubdivisionEditModel>()
                .ForMember(subdivisionVM => subdivisionVM.UsersID,
                subdivision => subdivision.MapFrom(s => s.Directors.Select(d => d.Id)));

            Mapper.CreateMap<ApplicationUser, ReviewerViewModel>()
                .ForMember(vm => vm.Name,
                m => m.MapFrom(s => string.Format("{0} {1} {2}", s.SecondName, s.FirstName, s.MiddleName)))
                .ForMember(vm => vm.IndicatorGroupName,
                m => m.MapFrom(s => s.AllowIndicatorGroups.Select(aig => aig.Name)));

            Mapper.CreateMap<DocumentType, DocumentTypeSelect>();
            Mapper.CreateMap<DocumentType, IndicatorGroups>()
                .ForMember(vm => vm.DocumentTypeID,
                m => m.MapFrom(s => s.DocumentTypeID))
                .ForMember(vm => vm.SelectList,
                m => m.MapFrom(s => s.IndicatorsGroups));

            Mapper.CreateMap<DocumentsWithCheckedCountDTO, DocumentElement>()
                .ForMember(vm => vm.DocumentID,
                m => m.MapFrom(s => s.Document.DocumentID))
                .ForMember(vm => vm.Year,
                m => m.MapFrom(s => s.Document.DateCreated.Year));
            Mapper.CreateMap<DocumentsWithMaxCheckedCountDTO, GroupDocuments>();

            Mapper.CreateMap<RatingGroup, RatingGroupViewModel>();
            Mapper.CreateMap<AcademicRank, AcademicRankViewModel>()
                .ForMember(vm => vm.Groups,
                m => m.MapFrom(s => s.RatingGroups))
                .ForMember(vm => vm.ID,
                m => m.MapFrom(s => s.AcademicRankID));

            Mapper.CreateMap<AcademicRank, AcademicRankCatalogViewModel>()
                  .ForMember(vm => vm.ID,
                m => m.MapFrom(s => s.AcademicRankID))
                .ForMember(vm => vm.ParentName,
                m => m.MapFrom(s => s.Parent.Title));

            Mapper.CreateMap<AcademicRank, AcademicRankCatalogEditModel>()
                .ForMember(vm => vm.ID,
              m => m.MapFrom(s => s.AcademicRankID));

            Mapper.CreateMap<DocumentLogDTO, DocumentLogViewModel>()
                .ForMember(vm => vm.ActionName,
                m => m.MapFrom(s => s.DocumentAction.Action))
                .ForMember(vm => vm.Action,
                m => m.MapFrom(s => (DocumentActionEnum)s.DocumentActionId));
        }
    }
}