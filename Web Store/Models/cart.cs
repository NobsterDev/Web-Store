//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_Store.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class cart
    {
        public cart()
        {
            this.Quantity = 0;
        }
        public int Product_idItems { get; set; }
        public int User_idUsers { get; set; }
        public int Quantity { get; set; }
    
        public virtual product product { get; set; }
        public virtual user user { get; set; }
    }
}
