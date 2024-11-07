using AutoMapper;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Common.Services
{
    public class ObjectUtils
    {
        private static readonly IMapper _mapper;

        static ObjectUtils()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GameInput, GameInput>();
                cfg.CreateMap<CustomerLoanRequestProposal, CustomerLoanRequestProposal>();
                cfg.CreateMap<CustomerActionIteration, CustomerActionIteration>()
                    .ForMember(dest => dest.CustomerActions, opt => opt.MapFrom(src => new Dictionary<string, CustomerAction>(src.CustomerActions)));
                cfg.CreateMap<CustomerAction, CustomerAction>();
            });

            _mapper = config.CreateMapper();
        }

        public static T DeepCopyWithAutoMapper<T>(T record)
        {
            return _mapper.Map<T>(record);
        }


        public static T? DeepCopyWithJson<T>(T record)
        {
            var json = JsonSerializer.Serialize(record);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}