﻿using System;
using System.Collections.Generic;

namespace Training.EfCore.Infrastructure.Models
{
    public partial class VoteType
    {
        public VoteType()
        {
            Votes = new HashSet<Vote>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
