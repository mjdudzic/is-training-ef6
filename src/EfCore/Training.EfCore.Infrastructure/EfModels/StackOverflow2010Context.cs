using System.Data.Entity;

namespace Training.EfCore.Infrastructure.EfModels
{
	public class StackOverflow2010Context : DbContext
	{
		public StackOverflow2010Context()
			: base("name=StackOverflow2010Context")
		{
			Database.SetInitializer<StackOverflow2010Context>(null);
		}

		public StackOverflow2010Context(string connectionString)
			: base(connectionString)
		{
			Database.SetInitializer<StackOverflow2010Context>(null);
		}

		public DbSet<Comment> Comments { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<PostType> PostTypes { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Post>()
				.HasMany(e => e.Comments)
				.WithRequired(e => e.Post)
				.WillCascadeOnDelete(false);
		}
	}
}
