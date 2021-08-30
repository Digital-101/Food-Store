using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoodStore.Models
{
    public class ProductImage
    {
        public int ID { get; set; }
        [Display(Name="File")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string FileName { get; set; }

        //property containing a collection of ProductImageMappings
        public virtual ICollection<ProductImageMapping> ProductImageMappings { get; set; }

    }
}