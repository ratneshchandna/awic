using System;
using System.ComponentModel.DataAnnotations;

namespace AWIC.Models
{
    public class Donations
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Display(Name="Name")]
        public string Donor { get; set; }

        [Display(Name="Email")]
        [EmailAddress(ErrorMessage="This e-mail address is not valid. ")]
        public string DonorEmail { get; set; }

        [Required]
        [Display(Name = "Amount")]
        [RegularExpression("[0-9]+(.([0-9]{1,2})?)?", ErrorMessage="The field Amount must be a valid number with not more than 2 decimal places. ")]
        [Range(0.50, 999999.99)]
        public double AmountInCAD { get; set; }

        public DateTime DonationDateAndTime { get; set; }
    }
}