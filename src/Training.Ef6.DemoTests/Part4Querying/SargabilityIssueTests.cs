using System;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class SargabilityIssueTests : TestBase
	{
		public SargabilityIssueTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task filter_by_text()
		{
			var usersFromPlCount = await ContextDbFirst
				.Users
				.Where(i => i.Location.Contains("Poland"))
				.CountAsync();

			usersFromPlCount.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task filter_by_function()
		{
			var importantDate = new DateTime(2010, 11, 13);

			var posts = await ContextDbFirst
				.Posts
				.Where(i => DbFunctions.TruncateTime(i.CreationDate) == importantDate)
				.CountAsync();

			//var posts = await ContextDbFirst
			//	.Posts
			//	.Where(i =>
			//		SqlFunctions.DatePart("year", i.CreationDate) == importantDate.Year &&
			//		SqlFunctions.DatePart("month", i.CreationDate) == importantDate.Month &&
			//		SqlFunctions.DatePart("day", i.CreationDate) == importantDate.Day)
			//	.CountAsync();

			//var endDate = importantDate.AddDays(1).AddMilliseconds(-1);
			//var posts = await ContextDbFirst
			//	.Posts
			//	.Where(i => i.CreationDate >= importantDate && i.CreationDate <= endDate)
			//	.CountAsync();

			//var sql = $"select count(1) from dbo.Posts where cast(CreationDate as date) = '{importantDate.Date:yyyy-MM-dd}'";
			//var posts = await ContextDbFirst.Database.SqlQuery<int>(sql).FirstOrDefaultAsync();

			posts.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task filter_by_nullable_text()
		{
			var filter = "Poland";

			var usersFromPlCount = await ContextDbFirst
				.Users
				.Where(i => filter == null || i.Location.StartsWith(filter))
				.CountAsync();

			usersFromPlCount.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task filter_by_text2()
		{
			var filter = "Europe";

			//var result = await ContextDbFirst
			//	.Users
			//	.Where(i => i.Region == filter)
			//	.CountAsync();

			var result = await ContextCodeFirst
				.Users
				.Where(i => i.Region == filter)
				.CountAsync();

			result.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task filter_by_computation()
		{
			var result = await ContextDbFirst
				.Users
				//.Where(i => i.Reputation > 1000)
				.Where(i => i.Reputation / 1000 > 1)
				.CountAsync();

			result.Should().BeGreaterThan(0);
		}
	}
}