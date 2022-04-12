using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part2Ef5Diff
{
	public class LinqToEntitiesTest : TestBase
	{
		public LinqToEntitiesTest(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task query_with_to_string_cast_in_where_clause_left()
		{
			var users = await ContextDbFirst
				.Users
				.CountAsync(i => i.UpVotes.ToString() == "777");

			users.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task query_with_to_string_cast_in_where_clause_right()
		{
			var filter = 0;
			var users = await ContextDbFirst
				.Users
				.CountAsync(i => i.DisplayName.StartsWith(filter.ToString()));

			users.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task query_with_to_string_cast_in_select()
		{
			var users = await ContextDbFirst
				.Users
				.OrderBy(i => i.Id)
				.Take(2)
				.Select(i => new UserDto
				{
					Name = i.DisplayName,
					Reputation = i.Reputation.ToString()
				})
				.ToListAsync();

			users.First().Reputation.Should().NotBeNullOrWhiteSpace();
		}

		[Fact]
		public async Task query_with_to_string_cast_in_select_with_concat()
		{
			var users = await ContextDbFirst
				.Users
				.OrderBy(i => i.Id)
				.Take(2)
				.Select(i => new UserDto
				{
					Name = i.DisplayName,
					Url = "api/users/" + i.Id 
				})
				.ToListAsync();

			users.First().Url.Should().NotBeNullOrWhiteSpace();
		}
	}

	public class UserDto
	{
		public string Name { get; set; }
		public string Reputation { get; set; }
		public string Url { get; set; }
	}
}