using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodStore.Models
{
    public class BasketLine
    {
        public int ID { get; set; }
        public string BasketID { get; set; }//(a GUID or userID)
        public int ProductID { get; set; }//navigation prop
        [Range(0, 50, ErrorMessage ="Please enter a quantity 0 and 50")]
        public int Quantity { get; set; }//prevent negatives
        public DateTime DateCreated { get; set; }
        public virtual Product Product { get; set; }

    }
}