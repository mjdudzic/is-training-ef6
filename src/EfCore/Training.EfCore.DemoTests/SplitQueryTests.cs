using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace Training.EfCore.DemoTests;

public class SplitQueryTests : TestBase
{
	public SplitQueryTests(ITestOutputHelper output) : base(output)
	{
	}

	[Fact]
	public async Task one_query()
	{
		var result = await Context
			.Posts
			.Include(i => i.Comments)
			.OrderBy(i => i.Id)
			.Take(10)
			.ToListAsync();

		result.Any().Should().BeTrue();
	}

	[Fact]
	public async Task split_query()
	{
		var result = await Context
			.Posts
			.Include(i => i.Comments)
			.AsSplitQuery()
			.OrderBy(i => i.Id)
			.Take(10)
			.ToListAsync();

		result.Any().Should().BeTrue();
	}

	[Fact]
	public async Task split_query2()
	{
		var result = await Context
			.Posts
			.Include(i => i.OwnerUser)
			.AsSplitQuery()
			.OrderBy(i => i.Id)
			.Take(10)
			.ToListAsync();

		result.Any().Should().BeTrue();
	}
}
