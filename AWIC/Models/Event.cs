﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AWIC.Models
{
    public class CalendarData
    {
        public string currentDay { get; set; }
        public string currentMonth { get; set; }
        public string currentYear { get; set; }
    }

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