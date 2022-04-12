﻿using System;
using System.Collections.Generic;

namespace Training.EfCore.Infrastructure.Models
{
    public partial class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime Date { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
