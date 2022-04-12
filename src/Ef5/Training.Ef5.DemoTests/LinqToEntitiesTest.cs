using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef5.DemoTests
{
	public class LinqToEntitiesTest : TestBase
	{
		public LinqToEntitiesTest(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void query_with_to_string_cast_in_where_clause_left()
		{
			var users = ContextDbFirst
				.Users
				.Count(i => i.UpVotes.ToString() == "777");

			users.Should().BeGreaterThan(0);
		}

		[Fact]
		public void query_with_to_string_cast_in_where_clause_right()
		{
			var filter = 0;
			var users = ContextDbFirst
				.Users
				.Count(i => i.DisplayName.StartsWith(filter.ToString()));

			users.Should().BeGreaterThan(0);
		}

		[Fact]
		public void query_with_to_string_cast_in_select()
		{
			var users = ContextDbFirst
				.Users
				.OrderBy(i => i.Id)
				.Take(2)
				.Select(i => new UserDto
				{
					Name = i.DisplayName,
					Reputation = i.Reputation.ToString()
				})
				.ToList();

			users.First().Reputation.Should().NotBeNullOrWhiteSpace();
		}

		[Fact]
		public void query_with_to_string_cast_in_select_with_concat()
		{
			var users = ContextDbFirst
				.Users
				.OrderBy(i => i.Id)
				.Take(2)
				.Select(i => new UserDto
				{
					Name = i.DisplayName,
					Url = "api/users/" + i.Id
				})
				.ToList();

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