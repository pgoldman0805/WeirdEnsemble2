//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeirdEnsemble2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductProductImage
    {
        public int ProductId { get; set; }
        public int ProductImageId { get; set; }
        public Nullable<System.DateTime> DateLastModified { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual ProductImage ProductImage { get; set; }
    }
}
