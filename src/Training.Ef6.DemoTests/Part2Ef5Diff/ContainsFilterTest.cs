using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part2Ef5Diff
{
	public class ContainsFilterTest : TestBase
	{
		public ContainsFilterTest(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void query_with_contains_filter()
		{
			ContextDbFirst.Database.Log = null;

			//warmup
			//ContextDbFirst.Posts.OrderBy(i => i.Id).Take(10).ToList();

			Stopwatch.Start();

			var idsSearch = Enumerable.Range(1, 10000);
			var postsCount = ContextDbFirst.Posts.Count(i => idsSearch.Contains(i.Id));

			Stopwatch.Stop();

			TestOutput.WriteLine("Query time is {0} ms", Stopwatch.Elapsed.TotalSeconds);

			postsCount.Should().BeGreaterThan(0);
		}
	}
}