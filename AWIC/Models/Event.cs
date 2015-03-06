using System;
using System.ComponentModel.DataAnnotations;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

namespace AWIC.Models
{
    public class Event
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Display(Name = "All Day")]
        public bool AllDay { get; set; }

        [Required]
        [Display(Name="Event Date and Time")]
        public DateTime EventDateAndTime { get; set; }

        [Required]
        [Display(Name = "Event Description")]
        public string EventDescription { get; set; }
    }
}