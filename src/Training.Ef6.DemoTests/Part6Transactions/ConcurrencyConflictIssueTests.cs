using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part6Transactions
{
	public class ConcurrencyConflictIssueTests : TestBase
	{
		public ConcurrencyConflictIssueTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task concurrency_conflict_missed()
		{
			var entity = await ContextDbFirst
				.Posts
				.OrderByDescending(i => i.Id)
				.FirstAsync();

			entity.Score += 1;

			await ContextDbFirst.Database.ExecuteSqlCommandAsync(
				$"update dbo.Posts set Score={entity.Score + 1} where Id={entity.Id}");

			//await ContextDbFirst.SaveChangesAsync();

			Func<Task> act = () => ContextDbFirst.SaveChangesAsync();
			await act.Should().NotThrowAsync<DbUpdateConcurrencyException>();
		}

		[Fact]
		public async Task concurrency_conflict_catch()
		{
			var entity = await ContextDbFirst
				.Votes
				.OrderByDescending(i => i.Id)
				.FirstAsync();

			entity.BountyAmount = 100;

			await ContextDbFirst.Database.ExecuteSqlCommandAsync(
				$"update dbo.Votes set BountyAmount={entity.BountyAmount.Value + 1} where Id={entity.Id}");

			//await ContextDbFirst.SaveChangesAsync();

			Func<Task> act = () => ContextDbFirst.SaveChangesAsync();
			await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
		}

		[Fact]
		public async Task concurrency_conflict_fix_by_db_wins()
		{
			var entity = await ContextDbFirst
				.Votes
				.OrderByDescending(i => i.Id)
				.FirstAsync();

			entity.BountyAmount = 100;

			await ContextDbFirst.Database.ExecuteSqlCommandAsync(
				$"update dbo.Votes set BountyAmount={entity.BountyAmount.Value + 1} where Id={entity.Id}");

			try
			{
				await ContextDbFirst.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException e)
			{
				await e.Entries.Single().ReloadAsync();
			}

			Func<Task> act = () => ContextDbFirst.SaveChangesAsync();
			await act.Should().NotThrowAsync<DbUpdateConcurrencyException>();
		}

		[Fact]
		public async Task concurrency_conflict_fix_by_client_wins()
		{
			var entity = await ContextDbFirst
				.Votes
				.OrderByDescending(i => i.Id)
				.FirstAsync();

			entity.BountyAmount = 100;

			await ContextDbFirst.Database.ExecuteSqlCommandAsync(
				$"update dbo.Votes set BountyAmount={entity.BountyAmount.Value + 1} where Id={entity.Id}");

			try
			{
				await ContextDbFirst.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException e)
			{
				var entry = e.Entries.Single();
				entry.OriginalValues.SetValues(await entry.GetDatabaseValuesAsync());
			}

			Func<Task> act = () => ContextDbFirst.SaveChangesAsync();
			await act.Should().NotThrowAsync<DbUpdateConcurrencyException>();
		}
	}
}