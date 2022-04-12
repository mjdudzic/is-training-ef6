using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Training.Ef6.Infrastructure.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class QueryCachePlanIssueTests : TestBase
	{
		public QueryCachePlanIssueTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task filter_by_nullable_variables()
		{
			//Bad Parameter Sniffing
			var filterCountry = "Poland";
			int? filterMinUpVotes = 100;
			int? filterMinReputation = 1000;

			var usersFromPlCount = await ContextDbFirst
				.Users
				.Where(i =>
					(filterCountry == null || i.Location.StartsWith(filterCountry)) &&
					(filterMinUpVotes == null || i.UpVotes >= filterMinUpVotes) &&
					(filterMinUpVotes == null || i.Reputation >= filterMinReputation))
				.CountAsync();

			//ContextDbFirst.Users.GetObjectQuery().EnablePlanCaching = false;
			//var query = ContextDbFirst
			//	.Users
			//	.Where(i =>
			//		(filterCountry == null || i.Location.StartsWith(filterCountry)) &&
			//		(filterMinUpVotes == null || i.UpVotes >= filterMinUpVotes) &&
			//		(filterMinUpVotes == null || i.Reputation >= filterMinReputation));

			//var usersFromPlCount = await query.CountAsync();

			usersFromPlCount.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task select_with_random_rows_count()
		{
			var pageIndex = 1;
			var pageSize = 10;

			var skip = (pageIndex - 1) * pageSize;

			var usersPage = await ContextDbFirst
				.Users
				.OrderBy(i => i.Id)
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			await ContextDbFirst
				.Users
				.OrderBy(i => i.Id)
				.Skip((++pageIndex - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			//var usersPage = await ContextDbFirst
			//	.Users
			//	.OrderBy(i => i.Id)
			//	.Skip(() => skip)
			//	.Take(() => pageSize)
			//	.ToListAsync();

			//pageIndex++;
			//skip = (pageIndex - 1) * pageSize;
			//await ContextDbFirst
			//	.Users
			//	.OrderBy(i => i.Id)
			//	.Skip(() => skip)
			//	.Take(() => pageSize)
			//	.ToListAsync();

			usersPage.Any().Should().BeTrue();
		}

		[Fact]
		public async Task filter_by_constant_1()
		{
			await ContextDbFirst
				.Users
				.FirstOrDefaultAsync(i => i.CreationDate > DateTime.UtcNow);

			await ContextDbFirst
				.Users
				.FirstOrDefaultAsync(i => i.CreationDate > DateTime.Now);

			//var date = DateTime.UtcNow;
			//await ContextDbFirst
			//	.Users
			//	.FirstOrDefaultAsync(i => i.CreationDate > date);

			//date = DateTime.Now;
			//await ContextDbFirst
			//	.Users
			//	.FirstOrDefaultAsync(i => i.CreationDate > date);
		}

		[Fact]
		public async Task filter_by_constant_2()
		{
			await ContextDbFirst
				.Posts
				.OrderByDescending(i => i.Id)
				.FirstOrDefaultAsync(i => i.PostTypeId == (int)PostTypeEnum.Question);

			await ContextDbFirst
				.Posts
				.OrderByDescending(i => i.Id)
				.FirstOrDefaultAsync(i => i.PostTypeId == (int)PostTypeEnum.Answer);


			//var typeFilter = (int)PostTypeEnum.Question;
			//await ContextDbFirst
			//	.Posts
			//	.OrderByDescending(i => i.Id)
			//	.FirstOrDefaultAsync(i => i.PostTypeId == typeFilter);

			//typeFilter = (int)PostTypeEnum.Answer;
			//await ContextDbFirst
			//	.Posts
			//	.OrderByDescending(i => i.Id)
			//	.FirstOrDefaultAsync(i => i.PostTypeId == typeFilter);
		}

		[Fact]
		public async Task filter_by_list_contains()
		{
			var locations = new[] { "Poland", "Germany", "France", "UK" };
			var locations2 = new[] { "Poland", "Spain", "France", "UK" };

			var usersFromPlCount = await ContextDbFirst
				.Users
				.Where(i => locations.Contains(i.Location))
				.CountAsync();

			var usersFromPlCount2 = await ContextDbFirst
				.Users
				.Where(i => locations2.Contains(i.Location))
				.CountAsync();

			usersFromPlCount.Should().BeGreaterThan(0);
			usersFromPlCount2.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task filter_by_list_contains_solution1()
		{
			var locations = new[] { "Poland", "Germany", "France", "UK" };
			var locations2 = new[] { "Poland", "Spain", "France", "UK" };

			var usersFromPlCount = await ContextDbFirst
				.Users
				.In(locations, i => i.Location)
				.CountAsync();

			var usersFromPlCount2 = await ContextDbFirst
				.Users
				.In(locations2, i => i.Location)
				.CountAsync();

			usersFromPlCount.Should().BeGreaterThan(0);
			usersFromPlCount2.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task filter_by_list_contains_solution2()
		{
			var locations = new[] { "Poland", "Germany", "France", "UK" };
			var locations2 = new[] { "Poland", "Spain", "France", "UK" };

			var locationsJoined = string.Join(",", locations);
			var locations2Joined = string.Join(",", locations2);

			var usersFromPlCount = await ContextDbFirst
				.Users
				.Where(i => ContextDbFirst.GetDelimitedStringValues(locationsJoined)
					.Any(s => s == i.Location))
				.CountAsync();

			var usersFromPlCount2 = await ContextDbFirst
				.Users
				.Where(i => ContextDbFirst.GetDelimitedStringValues(locations2Joined)
					.Any(s => s == i.Location))
				.CountAsync();

			usersFromPlCount.Should().BeGreaterThan(0);
			usersFromPlCount2.Should().BeGreaterThan(0);
		}
	}

	public enum PostTypeEnum
	{
		Question = 1,
		Answer = 2,
		Wiki = 3,
		TagWikiExerpt = 4,
		TagWiki = 5,
		ModeratorNomination = 6,
		WikiPlaceholder = 7,
		PrivilegeWiki = 8
	}
}