using System;
using AutoMapper;
using backend_labo02.Models;

namespace backend_labo02.DTO
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<VaccinationRegistration,VaccinationRegistrationDTO>();
        }
    }
}
