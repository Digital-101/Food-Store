using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodStore.Models
{
    //Address is used more than once for Users n Orders
    //Also used for EF composition whereby object is composed of another object
    public class Address
    {
        [Required]
        [Display(Name ="Address Line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }

    }
}