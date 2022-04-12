using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Training.EfCore.DemoTests;

public class QueryFilterTests : TestBase
{
	public QueryFilterTests(ITestOutputHelper output) : base(output)
	{
	}

	[Fact]
	public async Task query_filter_example()
	{
		var result = await Context
			.PostLinks
			.OrderBy(i => i.Id)
			.Take(10)
			.ToListAsync();

		result.Any().Should().BeTrue();
	}
}
