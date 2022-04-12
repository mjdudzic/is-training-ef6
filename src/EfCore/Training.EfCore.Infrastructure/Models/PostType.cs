using System;
using System.Collections.Generic;

namespace Training.EfCore.Infrastructure.Models
{
    public partial class PostType
    {
        public PostType()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;

        public virtual ICollection<Post> Posts { get; set; }
    }
}
