//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Training.Ef6.Infrastructure.DatabaseFirst
{
    using System;
    using System.Collections.Generic;
    
    public partial class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual User User { get; set; }
    }
}
