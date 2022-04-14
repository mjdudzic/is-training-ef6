using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Training.EfCore.DemoTests;

public class QueryTagTests : TestBase
{
	public QueryTagTests(ITestOutputHelper output) : base(output)
	{
	}

	[Fact]
	public async Task query_with_tag()
	{
		var result = await Context
			.Posts
			.TagWith(nameof(query_with_tag))
			.OrderBy(i => i.Id)
			.Take(10)
			.ToListAsync();

		result.Any().Should().BeTrue();
	}
}
