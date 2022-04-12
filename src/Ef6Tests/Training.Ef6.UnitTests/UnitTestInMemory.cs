using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Effort;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Training.Ef6.Infrastructure.Services;
using Xunit;

namespace Training.Ef6.UnitTests
{
	public class UnitTestInMemory
	{
		private readonly IPostService _postService;
		private readonly StackOverflow2010Entities _context;

		public UnitTestInMemory()
		{
			/*
			 * Requires to edit .tt file to generate additional constructor
			 */
			var connection = EntityConnectionFactory.CreateTransient("name=Test");

			_context = new StackOverflow2010Entities(connection, false);
			_postService = new PostsService(_context);
		}

		[Fact]
		public async Task non_query_test()
		{
			await _postService.AddPostType("test");

			_context.PostTypes.AsEnumerable().Any(i => i.Type == "test").Should().BeTrue();
		}

		[Fact]
		public async Task query_test()
		{
			await AddTestPosts();

			var result = await _postService.GetTopXBy(10, "<c#>");

			result.Any().Should().BeTrue();
		}

		private async Task AddTestPosts()
		{
			var tagSets = new List<string>
			{
				"<c#>",
				"<sql>",
				"<typescript>",
				"<c#><asp.net>",
			};

			_context.PostTypes.Add(new PostType { Type = "test" });
			await _context.SaveChangesAsync();

			var postTypeTest = _context.PostTypes.First();
			var postsFaker = new Faker<Post>()
				.RuleFor(i => i.Score, f => f.Random.Number(0, 100))
				.RuleFor(i => i.Tags, f => f.PickRandom(tagSets))
				.RuleFor(i => i.Body, f => f.Lorem.Sentence(20))
				.RuleFor(i => i.Title, f => f.Random.Words(5))
				.RuleFor(i => i.PostTypeId, f => postTypeTest.Id);

			var dataSetMock = postsFaker.Generate(100);

			_context.Posts.AddRange(dataSetMock);

			await _context.SaveChangesAsync();
		}
	}
}