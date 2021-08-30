using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodStore.Models
{
    public class Order
    {
        [Display(Name ="Order Id")]
        public int OrderID { get; set; }
        [Display(Name = "User")]
        public string UserID { get; set; }
        [Display(Name = "Deliver To:")]
        public string DeliveryName { get; set; }
        [Display(Name = "Delivery Address")]
        public Address DeliveryAddress { get; set; }
        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalPrice { get; set; }
        [Display(Name = "Time of Order")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }
}