using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part7ExtLibs.ZEf6
{
	public class BatchQueryTests : TestBase
	{
		public BatchQueryTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task delete_from_query()
		{
			var lastId = (await ContextDbFirst
				.PostTypes
				.OrderByDescending(i => i.Id)
				.FirstAsync())
				.Id;

			var newPostType = GetTestPostType();
			ContextDbFirst.PostTypes.Add(newPostType);
			await ContextDbFirst.SaveChangesAsync();

			var affected = await ContextDbFirst
				.PostTypes
				.Where(i => i.Id > lastId)
				.DeleteFromQueryAsync();

			affected.Should().BeGreaterThan(0);

			(await ContextDbFirst.PostTypes
				.CountAsync(i => i.Id > lastId)).Should().Be(0);
		}

		[Fact]
		public async Task update_from_query()
		{
			var lastId = (await ContextDbFirst
					.Users
					.OrderByDescending(i => i.Id)
					.FirstAsync())
				.Id;

			var newUser = GetTestUsers().First();
			ContextDbFirst.Users.Add(newUser);
			await ContextDbFirst.SaveChangesAsync();

			var affected = await ContextDbFirst
				.Users
				.Where(i => i.Id > lastId)
				.UpdateFromQueryAsync(i => new User { Reputation = 1000 } );

			affected.Should().BeGreaterThan(0);
		}
	}
}