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
    
    public partial class Vote
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> BountyAmount { get; set; }
        public int VoteTypeId { get; set; }
        public System.DateTime CreationDate { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual User User { get; set; }
        public virtual VoteType VoteType { get; set; }
    }
}
