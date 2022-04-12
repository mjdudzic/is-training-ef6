using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part5DataModifying
{
	public class UpdatingRelatedDataTests : TestBase
	{
		public UpdatingRelatedDataTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task updating_related_items_using_proxies()
		{
			Stopwatch.Start();
			var post = await ContextDbFirst
				.Posts
				.Where(i => i.CommentCount > 0)
				.OrderByDescending(i => i.Id).FirstAsync();

			post.Score += 1;

			post.User.Reputation += 1;

			post.Comments.OrderByDescending(i => i.Id).First().Score += 1;

			await ContextDbFirst.SaveChangesAsync();

			Stopwatch.Stop();
			TestOutput.WriteLine("Data changes provided in {0} ms", Stopwatch.ElapsedMilliseconds);
		}

		[Fact]
		public async Task updating_related_items_without_proxies()
		{
			Stopwatch.Start();
			var post = await ContextDbFirst
				.Posts
				.Where(i => i.CommentCount > 0)
				.OrderByDescending(i => i.Id)
				.FirstAsync();

			post.Score += 1;

			var user = await ContextDbFirst.Users.FindAsync(post.OwnerUserId);
			user.Reputation += 1;

			var comment = await ContextDbFirst
				.Comments
				.Where(i => i.PostId == post.Id)
				.OrderByDescending(i => i.Id)
				.FirstAsync();

			comment.Score += 1;

			await ContextDbFirst.SaveChangesAsync();
			Stopwatch.Stop();
			TestOutput.WriteLine("Data changes provided in {0} ms", Stopwatch.ElapsedMilliseconds);
		}
	}
}