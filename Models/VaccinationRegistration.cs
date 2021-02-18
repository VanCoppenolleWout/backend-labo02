using System;
using System.ComponentModel.DataAnnotations;

namespace backend_labo02.Models
{
    public class VaccinationRegistration
    {
        public Guid VaccinationRegistrationId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Firstname { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Range(18, 120)]
        public int Age { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public Guid VaccinationTypeId { get; set; }
        public Guid VaccinationLocationId { get; set; }
    }
}
