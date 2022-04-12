using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Training.Ef6.Infrastructure.Services;
using Training.Ef6.IntegrationTests.TestsCommon;
using Xunit;

namespace Training.Ef6.IntegrationTests
{
	public class PostServiceTest : IClassFixture<DatabaseFixture>
	{
		private readonly StackOverflow2010Entities _context;
		private readonly PostsService _postService;

		public PostServiceTest(DatabaseFixture fixture)
		{
			_context = fixture.Context;
			_postService = new PostsService(_context);
		}

		[Fact]
		public async Task add_post_type_test()
		{
			const string testPostType = "test";
			await _postService.AddPostType(testPostType);

			(await _context.PostTypes.CountAsync(i => i.Type == testPostType)).Should().Be(1);
		}
	}
}