using System;

namespace Training.Ef6.Infrastructure.CodeFirst
{
	public class Comment
	{
		public int Id { get; set; }

		public DateTime CreationDate { get; set; }

		public int PostId { get; set; }

		public int? Score { get; set; }

		public string Text { get; set; }

		public int? UserId { get; set; }

		public Post Post { get; set; }
	}
}
