using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Training.EfCore.Infrastructure.Models;
using Xunit;
using Xunit.Abstractions;

namespace Training.EfCore.DemoTests;

public class FilterTests : TestBase
{
	public FilterTests(ITestOutputHelper output) : base(output)
	{
	}

	[Fact]
	public void query_with_contains_filter()
	{
		Stopwatch.Start();

		var idsSearch = Enumerable.Range(1, 10000);
		var postsCount = Context.Posts.Count(i => idsSearch.Contains(i.Id));

		Stopwatch.Stop();

		Output.WriteLine("Query time is {0} ms", Stopwatch.Elapsed.TotalSeconds);

		postsCount.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task filter_by_text_and_cast_issue()
	{
		var filter = "Europe";

		var result = await Context
			.Users
			.Where(i => i.Region == filter)
			.CountAsync();

		result.Should().BeGreaterThan(0);
	}
}
