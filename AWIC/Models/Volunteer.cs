using System;
using System.ComponentModel.DataAnnotations;

namespace AWIC.Models
{
    public class Volunteer
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [Display(Name="Province or State")]
        public string ProvinceOrState { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail Address")]
        public string EmailAddress { get; set; }
        
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
        
        [Display(Name = "Languages spoken fluently")]
        public string LanguagesSpokenFluently { get; set; }

        [Required]
        public string Qualifications { get; set; }


        public bool Monday { get; set; }
        public string MondayTimes { get; set; }
        public bool Tuesday { get; set; }
        public string TuesdayTimes { get; set; }
        public bool Wednesday { get; set; }
        public string WednesdayTimes { get; set; }
        public bool Thursday { get; set; }
        public string ThursdayTimes { get; set; }
        public bool Friday { get; set; }
        public string FridayTimes { get; set; }
        public bool Saturday { get; set; }
        public string SaturdayTimes { get; set; }
        public bool Sunday { get; set; }
        public string SundayTimes { get; set; }
    }
}