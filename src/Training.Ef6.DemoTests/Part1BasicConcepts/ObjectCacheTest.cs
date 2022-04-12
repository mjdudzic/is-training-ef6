using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part1BasicConcepts
{
	public class ObjectCacheTest : TestBase
	{
		public ObjectCacheTest(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task query_without_first_level_cache()
		{
			var posts = await ContextDbFirst.Posts.Take(100).ToListAsync();
			var testPostId = posts[Random.Next(1, 100)].Id;

			TestOutput.WriteLine("testPostId = {0}", testPostId);

			Stopwatch.Start();

			var post1 = await ContextDbFirst.Posts.FirstOrDefaultAsync(i => i.Id == testPostId);

			TestOutput.WriteLine("First query time is {0}", Stopwatch.Elapsed.Milliseconds);

			Stopwatch.Restart();

			var post2 = await ContextDbFirst.Posts.FirstOrDefaultAsync(i => i.Id == testPostId);

			Stopwatch.Stop();
			TestOutput.WriteLine("Second query time is {0}", Stopwatch.Elapsed.Milliseconds);

			post2?.Id.Should().Be(post1?.Id);
		}

		[Fact]
		public async Task query_with_first_level_cache()
		{
			var posts = await ContextDbFirst.Posts.Take(100).ToListAsync();
			var testPostId = posts[Random.Next(1, 100)].Id;

			TestOutput.WriteLine("testPostId = {0}", testPostId);

			Stopwatch.Start();

			var post1 = await ContextDbFirst.Posts.FindAsync(testPostId);

			TestOutput.WriteLine("First query time is {0}", Stopwatch.Elapsed.Milliseconds);

			Stopwatch.Restart();

			var post2 = await ContextDbFirst.Posts.FindAsync(testPostId);

			Stopwatch.Stop();
			TestOutput.WriteLine("Second query time is {0}", Stopwatch.Elapsed.Milliseconds);

			post2?.Id.Should().Be(post1?.Id);
		}
	}
}