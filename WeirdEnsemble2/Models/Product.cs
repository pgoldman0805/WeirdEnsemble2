using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WeirdEnsemble2.Models
{
    public class Product
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [StringLength(50)]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required]
        [StringLength(1500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Image")]
        [RegularExpression(@"/\.(gif|jpe?g|tiff|png|svg)$/i")]
        public string ImagePath { get; set; }

        public decimal? Rating { get; set; }

        public string Product_Link { get; set; }
        public string Website_Link { get; set; }
    }

    //public class Image
    //{
    //    public int ID { get; set; }
    //    public int ProductId { get; set; }
    //    public string FullPath { get; set; }
    //}
}