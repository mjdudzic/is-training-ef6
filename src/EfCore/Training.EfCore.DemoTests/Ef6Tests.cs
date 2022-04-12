using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.EfCore.DemoTests;

public class Ef6Tests : TestBase
{
	public Ef6Tests(ITestOutputHelper output) : base(output)
	{
	}

	[Fact]
	public async Task query_example()
	{
		var result = await Ef6Context
			.PostTypes
			.OrderBy(i => i.Id)
			.Take(10)
			.ToListAsync();

		result.Any().Should().BeTrue();
	}
}
