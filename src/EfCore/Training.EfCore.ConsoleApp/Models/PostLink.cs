namespace Training.EfCore.ConsoleApp.Models
{
	public partial class PostLink
	{
		public int Id { get; set; }
		public DateTime CreationDate { get; set; }
		public int PostId { get; set; }
		public int RelatedPostId { get; set; }
		public int LinkTypeId { get; set; }
		public bool IsDeleted { get; set; }

		public virtual LinkType LinkType { get; set; } = null!;
		public virtual Post Post { get; set; } = null!;
		public virtual Post RelatedPost { get; set; } = null!;
	}
}
