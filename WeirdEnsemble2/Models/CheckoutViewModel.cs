using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WeirdEnsemble2.Models
{
    public class CheckoutViewModel
    {

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Recipient")]
        public string ShippingRecipient { get; set; }

        [Required]
        [Display(Name = "Ship to Street Address")]
        public string ShippingAddressLine1 { get; set; }

        [Display(Name = "Ship to Apartment/Unit #")]
        public string ShippingAddressLine2 { get; set; }

        [Required]
        [Display(Name = "Ship to City")]
        public string ShippingCity { get; set; }

        [Required]
        [Display(Name = "Ship to State")]
        public string ShippingState { get; set; }

        [Required]
        [Display(Name = "Ship to Zip Code")]
        public string ShippingPostalCode { get; set; }

        [Required]
        [Display(Name = "Credit Card Number")]
        public string CreditCardNumber { get; set; }

        [Required]
        [Display(Name = "CVV")]
        public string CreditCardVerificationValue { get; set; }

        [Required]
        [Display(Name = "Expiration")]
        public int CreditCardExpirationMonth { get; set; }

        [Required]
        public int CreditCardExpirationYear { get; set; }

        [Required]
        [Display(Name = "Name on Card")]
        public string CreditCardHolder { get; set; }

        public Cart CurrentCart { get; set; }
    }
}