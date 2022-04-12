﻿using System;
using System.Collections.Generic;

namespace Training.EfCore.Infrastructure.Models
{
    public partial class User
    {
        public User()
        {
            Badges = new HashSet<Badge>();
            Comments = new HashSet<Comment>();
            PostLastEditorUsers = new HashSet<Post>();
            PostOwnerUsers = new HashSet<Post>();
            Votes = new HashSet<Vote>();
        }

        public int Id { get; set; }
        public string? AboutMe { get; set; }
        public int? Age { get; set; }
        public DateTime CreationDate { get; set; }
        public string DisplayName { get; set; } = null!;
        public int DownVotes { get; set; }
        public string? EmailHash { get; set; }
        public DateTime LastAccessDate { get; set; }
        public string? Location { get; set; }
        public int Reputation { get; set; }
        public int UpVotes { get; set; }
        public int Views { get; set; }
        public string? WebsiteUrl { get; set; }
        public int? AccountId { get; set; }
        public string? Region { get; set; }

        public virtual ICollection<Badge> Badges { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> PostLastEditorUsers { get; set; }
        public virtual ICollection<Post> PostOwnerUsers { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
