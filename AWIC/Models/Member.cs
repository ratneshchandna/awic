using System;
using System.ComponentModel.DataAnnotations;

namespace AWIC.Models
{
    public enum MembershipType
    {
        New = 1, 

        Renewal = 2
    }

    public enum FeeOption
    {
        [Display(Name = "$10 CAD - 1 Year Membership")]
        OneYearMembership = 1, 

        [Display(Name = "$40 CAD - 5 Year Membership")]
        FiveYearMembership = 2, 

        [Display(Name = "$200 CAD - Lifetime Membership")]
        LifetimeMembership = 3,

        [Display(Name = "$500 CAD - 1 Year Patronage")]
        OneYearPatronage = 4
    }

    public enum PaymentMethod
    {
        Cash = 1, 

        Cheque = 2, 

        [Display(Name="Credit Card")]
        CreditCard = 3
    }

    public class Member
    {
        public DateTime Date { get; set; }

        [Required]
        [Range(1,2,ErrorMessage="Please select a membership type")]
        [Display(Name="Membership Type")]
        public MembershipType MembershipType { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [Display(Name = "Province or State")]
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

        [Display(Name = "Referred By")]
        public string ReferredBy { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Please select a fee option")]
        [Display(Name = "Fee Option")]
        public FeeOption FeeOption { get; set; }

        [Required]
        [Range(1,3,ErrorMessage="Please select a payment method")]
        [Display(Name="Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }
    }
}