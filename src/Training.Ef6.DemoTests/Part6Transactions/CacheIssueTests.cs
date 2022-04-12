using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part6Transactions
{
	public class CacheIssueTests : TestBase
	{
		public CacheIssueTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task query_with_cache_and_concurrent_update_issue()
		{
			var entities = await ContextDbFirst
				.Posts
				.OrderByDescending(i => i.Id)
				.Take(2)
				.ToListAsync();

			var entity = entities.First();
			var newScore = entity.Score + 1;
			await ContextDbFirst.Database.ExecuteSqlCommandAsync(
				$"update dbo.Posts set Score={newScore} where Id={entity.Id}");

			var entities2 = await ContextDbFirst
				.Posts
				.OrderByDescending(i => i.Id)
				.Take(2)
				.ToListAsync();

			entities2.First(i => i.Id == entity.Id).Score.Should().NotBe(newScore);
		}

		[Fact]
		public async Task query_with_cache_and_concurrent_update_issue_fix1()
		{
			var entities = await ContextDbFirst
				.Posts
				.OrderByDescending(i => i.Id)
				.Take(2)
				.ToListAsync();

			var entity = entities.First();
			var newScore = entity.Score + 1;
			await ContextDbFirst.Database.ExecuteSqlCommandAsync(
				$"update dbo.Posts set Score={newScore} where Id={entity.Id}");

			using (var context = new StackOverflow2010Entities())
			{
				var entities2 = await context
					.Posts
					.OrderByDescending(i => i.Id)
					.Take(2)
					.ToListAsync();

				entities2.First(i => i.Id == entity.Id).Score.Should().Be(newScore);
			}
		}

		[Fact]
		public async Task query_with_cache_and_concurrent_update_issue_fix2()
		{
			var entities = await ContextDbFirst
				.Posts
				.AsNoTracking()
				.OrderByDescending(i => i.Id)
				.Take(2)
				.ToListAsync();

			var entity = entities.First();
			var newScore = entity.Score + 1;
			await ContextDbFirst.Database.ExecuteSqlCommandAsync(
				$"update dbo.Posts set Score={newScore} where Id={entity.Id}");

			var entities2 = await ContextDbFirst
				.Posts
				.OrderByDescending(i => i.Id)
				.Take(2)
				.ToListAsync();

			entities2.First(i => i.Id == entity.Id).Score.Should().Be(newScore);
		}

		[Fact]
		public async Task query_with_cache_and_concurrent_update_issue_fix3()
		{
			var entities = await ContextDbFirst
				.Posts
				.OrderByDescending(i => i.Id)
				.Take(2)
				.ToListAsync();

			var entity = entities.First();
			var newScore = entity.Score + 1;
			await ContextDbFirst.Database.ExecuteSqlCommandAsync(
				$"update dbo.Posts set Score={newScore} where Id={entity.Id}");

			foreach (var post in entities)
			{
				await ContextDbFirst.Entry(post).ReloadAsync();
			}

			var entities2 = await ContextDbFirst
				.Posts
				.OrderByDescending(i => i.Id)
				.Take(2)
				.ToListAsync();

			entities2.First(i => i.Id == entity.Id).Score.Should().Be(newScore);
		}
	}
}