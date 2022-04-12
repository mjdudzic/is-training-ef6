using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class LazyVsEagerIssueTests : TestBase
	{
		public LazyVsEagerIssueTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task lazy_loading_case()
		{
			var posts = await ContextDbFirst
				.Posts
				.OrderBy(i => i.Id)
				.Take(5)
				.ToListAsync();

			foreach (var post in posts)
			{
				TestOutput.WriteLine($"Post: {post.Title} - Score: {post.Score} - Type: {post.PostType.Type}");
			}

			posts.Any().Should().BeTrue();
		}

		[Fact]
		public async Task eager_loading_case()
		{
			var testContext = new TestContext(TestOutput);

			var posts = await testContext
				.Posts
				.Include(i => i.PostType)
				.OrderBy(i => i.Id)
				.Take(5)
				.ToListAsync();

			foreach (var post in posts)
			{
				TestOutput.WriteLine($"Post: {post.Title} - Score: {post.Score} - Type: {post.PostType?.Type}");
			}

			posts.Any().Should().BeTrue();
		}

		[Fact]
		public async Task eager_loading_many_levels_case()
		{
			var testContext = new TestContext(TestOutput);

			var posts = await testContext
				.Posts
				.Include(i => i.User.Badges)
				.OrderBy(i => i.Id)
				.Take(5)
				.ToListAsync();

			foreach (var post in posts)
			{
				TestOutput.WriteLine($"Post: {post.Title} - Owner: {post.User.DisplayName} - Badges count: {post.User.Badges.Count}");
			}

			posts.Any().Should().BeTrue();
		}

		public class TestContext : StackOverflow2010Entities
		{
			public TestContext(ITestOutputHelper output)
			{
				Configuration.LazyLoadingEnabled = false;
				Database.Log = output.WriteLine;
			}
		}
	}
}