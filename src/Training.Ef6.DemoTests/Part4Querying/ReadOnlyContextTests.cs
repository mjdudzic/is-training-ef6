using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class ReadOnlyContextTests : TestBase
	{
		public ReadOnlyContextTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task query_succeeds()
		{
			var users = await ContextDbFirstReadOnly
				.Users
				.AsNoTracking()
				.OrderBy(i => i.Id)
				.Take(5)
				.ToListAsync();

			users.Any().Should().BeTrue();
		}

		[Fact]
		public async Task command_fails()
		{
			var user = GetTestUsers().First();

			ContextDbFirstReadOnly
				.Users
				.Add(user);

			Func<Task<int>> act = ContextDbFirstReadOnly.SaveChangesAsync;

			await act.Should().ThrowAsync<NotSupportedException>();
		}
	}
}