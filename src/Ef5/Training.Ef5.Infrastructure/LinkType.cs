//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Training.Ef5.Infrastructure
{
    using System;
    using System.Collections.Generic;
    
    public partial class LinkType
    {
        public LinkType()
        {
            this.PostLinks = new HashSet<PostLink>();
        }
    
        public int Id { get; set; }
        public string Type { get; set; }
    
        public virtual ICollection<PostLink> PostLinks { get; set; }
    }
}
