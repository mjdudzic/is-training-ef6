using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Training.Ef6.Infrastructure.Services;
using Training.Ef6.UnitTests.EfAsyncMock;
using Xunit;

namespace Training.Ef6.UnitTests
{
	public class UnitTestWithMock
	{
		private IPostService _postService;
		private StackOverflow2010Entities _context;

		[Fact]
		public async Task non_query_with_mock()
		{
			var mockSet = Substitute.For<DbSet<PostType>>();
			
			_context = Substitute.For<StackOverflow2010Entities>();
			_context.PostTypes.Returns(mockSet); 
			_postService = new PostsService(_context);

			await _postService.AddPostType("test");

			mockSet.Received().Add(Arg.Any<PostType>());
			await _context.Received().SaveChangesAsync();
		}

		[Fact]
		public async Task query_async_with_mock()
		{
			_context = GetContextWithAsyncQueryMock();
			_postService = new PostsService(_context);

			var result = await _postService.GetTopXBy(10, "<c#>");

			result.Any().Should().BeTrue();
		}

		private StackOverflow2010Entities GetContextWithAsyncQueryMock()
		{
			var tagSets = new List<string>
			{
				"<c#>",
				"<sql>",
				"<typescript>",
				"<c#><asp.net>",
			};

			var postsFaker = new Faker<Post>()
				.RuleFor(i => i.Score, f => f.Random.Number(0, 100))
				.RuleFor(i => i.Tags, f => f.PickRandom(tagSets));

			var dataSetMock = postsFaker.Generate(100).AsQueryable();

			var postsSetMock = Substitute.For<DbSet<Post>, IQueryable<Post>, IDbAsyncEnumerable<Post>>();
			((IDbAsyncEnumerable<Post>)postsSetMock).GetAsyncEnumerator()
				.Returns(new TestDbAsyncEnumerator<Post>(dataSetMock.GetEnumerator()));

			((IQueryable<Post>)postsSetMock).Provider.Returns(new TestDbAsyncQueryProvider<Post>(dataSetMock.Provider));

			((IQueryable<Post>)postsSetMock).Expression.Returns(dataSetMock.Expression);

			((IQueryable<Post>)postsSetMock).ElementType.Returns(dataSetMock.ElementType);

			((IQueryable<Post>)postsSetMock).GetEnumerator().Returns(dataSetMock.GetEnumerator());

			var contextMock = Substitute.For<StackOverflow2010Entities>();

			contextMock
				.Posts
				.Returns(postsSetMock);

			return contextMock;
		}
	}
}