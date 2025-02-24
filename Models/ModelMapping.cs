
using AlumniManagement.Frontend.AlumniImageService;
using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.JobHistoryService;
using AlumniManagement.Frontend.PostingJobService;
using AlumniManagement.Frontend.MajorService;
using AlumniManagement.Frontend.Models;
using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlumniDTO = AlumniManagement.Frontend.AlumniService.AlumniDTO;

namespace AlumniManagement.Frontend
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<ModelMapping>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {

            CreateMap<MajorModel, MajorDTO>().ReverseMap();
            CreateMap<JobModel, JobDTO>().ReverseMap();
            CreateMap<AlumniModel, AlumniDTO>().ReverseMap();
            CreateMap<FacultyModel, FacultyDTO>().ReverseMap();
            CreateMap<AlumniImageModel, ImageDTO>().ReverseMap();
            CreateMap<HobbyModel, HobbyDTO>().ReverseMap();
            CreateMap<JobPostingModel, JobPostingDTO>().ReverseMap();
            CreateMap<AttachmentTypeModel, AttachmentTypeDTO>().ReverseMap();
            CreateMap<SkillModel, SkillDTO>().ReverseMap();
            CreateMap<EmploymentTypeModel, EmploymentTypeDTO>().ReverseMap();
            CreateMap<JobAttachmentModel, JobAttachmentDTO>().ReverseMap();
            CreateMap<ShowCandidateModel, JobCandidateDTO>().ReverseMap();

        }
    }
}
