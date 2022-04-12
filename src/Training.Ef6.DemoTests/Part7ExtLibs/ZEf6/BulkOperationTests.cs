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
	public class BulkOperationTests : TestBase
	{
		public BulkOperationTests(ITestOutputHelper output) : base(output)
		{
			ContextDbFirst.Database.Log = null;
		}

		[Fact]
		public async Task bulk_insert()
		{
			var users = GetTestUsers();

			await ContextDbFirst.BulkInsertAsync(users);

			LogTrackedEntities();

			users.All(i => i.Id > 0).Should().BeTrue();

			users.All(user => ContextDbFirst.Entry(user).State == EntityState.Detached).Should().BeTrue();
		}

		[Fact]
		public async Task bulk_delete()
		{
			var users = GetTestUsers(1);

			await ContextDbFirst.BulkInsertAsync(users);

			await ContextDbFirst.BulkDeleteAsync(users);

			LogTrackedEntities();

			users.All(user => ContextDbFirst.Entry(user).State == EntityState.Detached).Should().BeTrue();
		}

		[Fact]
		public async Task bulk_update()
		{
			var users = await ContextDbFirst
				.Users
				.OrderByDescending(i => i.Id)
				.Take(5)
				.ToListAsync();

			users.ForEach(user => user.Reputation += 100);

			await ContextDbFirst.BulkUpdateAsync(users);

			LogTrackedEntities();

			users.All(u => ContextDbFirst.Entry(u).State == EntityState.Modified).Should().BeTrue();
		}

		[Fact]
		public async Task bulk_save()
		{
			var users = await ContextDbFirst
				.Users
				.OrderByDescending(i => i.Id)
				.Take(5)
				.ToListAsync();

			users.ForEach(user => user.Reputation += 100);
			
			var postType = GetTestPostType();
			ContextDbFirst.PostTypes.Add(postType);

			await ContextDbFirst.BulkSaveChangesAsync();

			LogTrackedEntities();

			users.All(u => ContextDbFirst.Entry(u).State == EntityState.Unchanged).Should().BeTrue();
		}
	}
}