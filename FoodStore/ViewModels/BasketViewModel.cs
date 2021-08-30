using FoodStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodStore.ViewModels
{
    public class BasketViewModel
    {
        public List<BasketLine> BasketLines { get; set; }
        [Display(Name ="Basket Total:")]
        [DisplayFormat(DataFormatString ="{0:c}")]
        public decimal TotalCost { get; set; }
    }
}