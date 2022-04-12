using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Training.EfCore.Infrastructure.Models
{
	public partial class StackOverflow2010Context : DbContext
	{
		public StackOverflow2010Context()
		{
		}

		public StackOverflow2010Context(DbContextOptions<StackOverflow2010Context> options)
			: base(options)
		{
		}

		public virtual DbSet<Badge> Badges { get; set; } = null!;
		public virtual DbSet<Comment> Comments { get; set; } = null!;
		public virtual DbSet<LinkType> LinkTypes { get; set; } = null!;
		public virtual DbSet<Post> Posts { get; set; } = null!;
		public virtual DbSet<PostLink> PostLinks { get; set; } = null!;
		public virtual DbSet<PostType> PostTypes { get; set; } = null!;
		public virtual DbSet<User> Users { get; set; } = null!;
		public virtual DbSet<Vote> Votes { get; set; } = null!;
		public virtual DbSet<VoteType> VoteTypes { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//if (!optionsBuilder.IsConfigured)
			//{
			//	optionsBuilder.UseSqlServer("Server=.;Database=StackOverflow2010;Trusted_Connection=True;");
			//}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

			modelBuilder.Entity<Badge>(entity =>
			{
				entity.Property(e => e.Date).HasColumnType("datetime");

				entity.Property(e => e.Name).HasMaxLength(40);

				entity.HasOne(d => d.User)
					.WithMany(p => p.Badges)
					.HasForeignKey(d => d.UserId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Badges_Users");
			});

			modelBuilder.Entity<Comment>(entity =>
			{
				entity.Property(e => e.CreationDate).HasColumnType("datetime");

				entity.Property(e => e.Text).HasMaxLength(700);

				entity.HasOne(d => d.Post)
					.WithMany(p => p.Comments)
					.HasForeignKey(d => d.PostId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Comments_Posts");

				entity.HasOne(d => d.User)
					.WithMany(p => p.Comments)
					.HasForeignKey(d => d.UserId)
					.HasConstraintName("FK_Comments_Users");
			});

			modelBuilder.Entity<LinkType>(entity =>
			{
				entity.Property(e => e.Type)
					.HasMaxLength(50)
					.IsUnicode(false);
			});

			modelBuilder.Entity<Post>(entity =>
			{
				entity.HasIndex(e => e.CreationDate, "IX_Posts_CreationDate");

				entity.Property(e => e.ClosedDate).HasColumnType("datetime");

				entity.Property(e => e.CommunityOwnedDate).HasColumnType("datetime");

				entity.Property(e => e.CreationDate).HasColumnType("datetime");

				entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

				entity.Property(e => e.LastEditDate).HasColumnType("datetime");

				entity.Property(e => e.LastEditorDisplayName).HasMaxLength(40);

				entity.Property(e => e.Tags).HasMaxLength(150);

				entity.Property(e => e.Title).HasMaxLength(250);

				entity.HasOne(d => d.AcceptedAnswer)
					.WithMany(p => p.InverseAcceptedAnswer)
					.HasForeignKey(d => d.AcceptedAnswerId)
					.HasConstraintName("FK_Posts_Posts2");

				entity.HasOne(d => d.LastEditorUser)
					.WithMany(p => p.PostLastEditorUsers)
					.HasForeignKey(d => d.LastEditorUserId)
					.HasConstraintName("FK_Posts_Users2");

				entity.HasOne(d => d.OwnerUser)
					.WithMany(p => p.PostOwnerUsers)
					.HasForeignKey(d => d.OwnerUserId)
					.HasConstraintName("FK_Posts_Users1");

				entity.HasOne(d => d.Parent)
					.WithMany(p => p.InverseParent)
					.HasForeignKey(d => d.ParentId)
					.HasConstraintName("FK_Posts_Posts1");

				entity.HasOne(d => d.PostType)
					.WithMany(p => p.Posts)
					.HasForeignKey(d => d.PostTypeId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Posts_PostTypes");
			});

			modelBuilder.Entity<PostLink>(entity =>
			{
				entity.HasQueryFilter(i => i.IsDeleted == false);

				entity.Property(e => e.CreationDate).HasColumnType("datetime");

				entity.HasOne(d => d.LinkType)
					.WithMany(p => p.PostLinks)
					.HasForeignKey(d => d.LinkTypeId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_PostLinks_LinkTypes");

				entity.HasOne(d => d.Post)
					.WithMany(p => p.PostLinkPosts)
					.HasForeignKey(d => d.PostId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_PostLinks_Posts");

				entity.HasOne(d => d.RelatedPost)
					.WithMany(p => p.PostLinkRelatedPosts)
					.HasForeignKey(d => d.RelatedPostId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_PostLinks_Posts2");
			});

			modelBuilder.Entity<PostType>(entity =>
			{
				entity.Property(e => e.Type).HasMaxLength(50);
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity.HasIndex(e => e.Location, "IX_Users_Location");

				entity.HasIndex(e => e.Region, "IX_Users_Region");

				entity.HasIndex(e => e.Reputation, "IX_Users_Reputation");

				entity.HasIndex(e => e.UpVotes, "IX_Users_UpVotes");

				entity.Property(e => e.CreationDate).HasColumnType("datetime");

				entity.Property(e => e.DisplayName).HasMaxLength(40);

				entity.Property(e => e.EmailHash).HasMaxLength(40);

				entity.Property(e => e.LastAccessDate).HasColumnType("datetime");

				entity.Property(e => e.Location).HasMaxLength(100);

				entity.Property(e => e.Region)
					.HasMaxLength(30);
					//.IsUnicode(false);

				entity.Property(e => e.WebsiteUrl).HasMaxLength(200);
			});

			modelBuilder.Entity<Vote>(entity =>
			{
				entity.Property(e => e.CreationDate).HasColumnType("datetime");

				entity.Property(e => e.RowVersion)
					.IsRowVersion()
					.IsConcurrencyToken();

				entity.HasOne(d => d.User)
					.WithMany(p => p.Votes)
					.HasForeignKey(d => d.UserId)
					.HasConstraintName("FK_Votes_Users");

				entity.HasOne(d => d.VoteType)
					.WithMany(p => p.Votes)
					.HasForeignKey(d => d.VoteTypeId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("FK_Votes_VoteTypes");
			});

			modelBuilder.Entity<VoteType>(entity =>
			{
				entity.Property(e => e.Name)
					.HasMaxLength(50)
					.IsUnicode(false);
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
