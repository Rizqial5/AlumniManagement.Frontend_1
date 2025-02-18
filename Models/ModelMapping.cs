﻿
using AlumniManagement.Frontend.AlumniService;
using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.JobHistoryService;
using AlumniManagement.Frontend.MajorService;
using AlumniManagement.Frontend.Models;
using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        }
    }
}
