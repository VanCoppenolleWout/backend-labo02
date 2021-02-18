using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using backend_labo02.Models;
using System.Linq;
using backend_labo02.Configuration;
using Microsoft.Extensions.Options;
using System.Globalization;
using CsvHelper.Configuration;
using System.IO;
using CsvHelper;
using AutoMapper;
using backend_labo02.DTO;

namespace backend_labo02.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class VaccinationController : ControllerBase
    {
        private CSVSettings _settings;
        private static List<VaccinationType> _vaccinTypes;
        private static List<VaccinationLocation> _vaccinLocation;
        private static List<VaccinationRegistration> _vaccinRegistration;
        private IMapper _mapper;
 
        // constructor is belangrijk voor als we dependency injection zullen gaan toepassen
        public VaccinationController(IOptions<CSVSettings> settings, IMapper mapper)
        {
            _mapper = mapper;
            _settings = settings.Value;

            if (_vaccinTypes == null) {
                _vaccinTypes = ReadCSVVaccins();
            }

            if (_vaccinLocation == null) {
                _vaccinLocation = ReadCSVLocation();
            }

            if (_vaccinRegistration == null) {
                _vaccinRegistration = ReadRegistrations();
            }
        }

        private void SaveRegistrations() {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false, Delimiter = ";"
            };

            using (var writer = new StreamWriter(_settings.CSVRegistrations))
            {
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(_vaccinRegistration);
                }
            }
        }

        private List<VaccinationType> ReadCSVVaccins() {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false, Delimiter = ";"
            };

            using (var reader = new StreamReader(_settings.CSVVaccins))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    var records = csv.GetRecords<VaccinationType>();
                    return records.ToList<VaccinationType>();
                }
            }
        }

        private List<VaccinationLocation> ReadCSVLocation() {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false, Delimiter = ";"
            };

            using (var reader = new StreamReader(_settings.CSVLocations))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    var records = csv.GetRecords<VaccinationLocation>();
                    return records.ToList<VaccinationLocation>();
                }
            }
        }

         private List<VaccinationRegistration> ReadRegistrations() {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false, Delimiter = ";"
            };

            using (var reader = new StreamReader(_settings.CSVRegistrations))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    var records = csv.GetRecords<VaccinationRegistration>();
                    return records.ToList<VaccinationRegistration>();
                }
            }
        }
 
        [Route("/registrations")]
        [HttpGet]
        public ActionResult<List<VaccinationRegistration>> GetRegistrations(string date = "")
        {
            if (string.IsNullOrEmpty(date)){
                return new OkObjectResult(_vaccinRegistration);
            }
            else {
                return _vaccinRegistration.Where(r => r.Date == DateTime.Parse(date)).ToList<VaccinationRegistration>();
            }
        }

        [Route("/registrations")]
        [MapToApiVersion("2.0")]
        [HttpGet]
        public ActionResult<List<VaccinationRegistrationDTO>> GetRegistrationsSmall()
        {
            return _mapper.Map<List<VaccinationRegistrationDTO>>(_vaccinRegistration);
        }
 
        [Route("/registration")]
        [HttpPost]
        public ActionResult<VaccinationRegistration> AddRegistration(VaccinationRegistration newRegistration)
        {
            if (newRegistration == null)
                return new BadRequestResult();
            if(_vaccinTypes.Where(vt => vt.VaccinationTypeId == newRegistration.VaccinationTypeId).Count() == 0)
            {
                return new BadRequestResult();
            }
            if(_vaccinLocation.Where(vt => vt.VaccinationLocationId == newRegistration.VaccinationLocationId).Count() == 0)
            {
                return new BadRequestResult();
            }
 
            newRegistration.VaccinationRegistrationId = Guid.NewGuid();
            _vaccinRegistration.Add(newRegistration);
            return newRegistration;
        }
 
        [Route("/vaccins")]
        [HttpGet]
        public ActionResult<List<VaccinationType>> GetVaccins()
        {
            return new OkObjectResult(_vaccinTypes);
        }
 
        [HttpGet]
        [Route("/locations")]
        public ActionResult<List<VaccinationLocation>> GetVaccinLocation()
        {
            return new OkObjectResult(_vaccinLocation);
        }
    }
}