using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part5DataModifying
{
	public class UpdatingDataTests : TestBase
	{
		public UpdatingDataTests(ITestOutputHelper output) : base(output)
		{
		}


		[Fact]
		public async Task updating_entity_most_common_flow()
		{
			var user = await ContextDbFirst.Users.OrderByDescending(i => i.Id).FirstAsync();

			user.Reputation += 1;

			await ContextDbFirst.SaveChangesAsync();

			ContextDbFirst.Entry(user).State.Should().Be(EntityState.Unchanged);
		}

		[Fact]
		public async Task updating_entity_without_querying_first()
		{
			int userId;
			using (var context = new StackOverflow2010Entities())
			{
				userId = (await context.Users.OrderByDescending(i => i.Id).FirstAsync()).Id;
			}

			var user = new User
			{
				Id = userId,
				Reputation = 0,
				DisplayName = string.Empty,
				CreationDate = DateTime.UtcNow,
				LastAccessDate = DateTime.UtcNow
			};

			ContextDbFirst.Users.Attach(user);

			var entry = ContextDbFirst.Entry(user);
			entry.Property(i => i.Reputation).IsModified = true;

			LogTrackedEntities();

			await ContextDbFirst.SaveChangesAsync();

			entry.State.Should().Be(EntityState.Unchanged);
		}

		[Fact]
		public async Task update_record_with_implicit_detect_changes()
		{
			var existingPost1 = await ContextDbFirst.Posts.OrderByDescending(i => i.Id).FirstAsync();
			var originPostScore = existingPost1.Score;
			existingPost1.Score += 1;

			LogTrackedEntities();

			await ContextDbFirst.SaveChangesAsync();

			existingPost1 = await ContextDbFirst.Posts.OrderByDescending(i => i.Id).FirstAsync();

			existingPost1.Score.Should().BeGreaterThan(originPostScore);
		}

		[Fact]
		public async Task update_record_with_explicit_detect_changes()
		{
			ContextDbFirst.Configuration.AutoDetectChangesEnabled = false;

			var existingPost1 = await ContextDbFirst.Posts.OrderByDescending(i => i.Id).FirstAsync();
			var originPostScore = existingPost1.Score;
			existingPost1.Score += 1;

			LogTrackedEntities();

			// Without .DetectChanges() test succeeds anyway :) - why?
			//ContextDbFirst.ChangeTracker.DetectChanges();

			await ContextDbFirst.SaveChangesAsync();

			existingPost1 = await ContextDbFirst.Posts.OrderByDescending(i => i.Id).FirstAsync();

			//await ContextDbFirst.Entry(existingPost1).ReloadAsync();

			existingPost1.Score.Should().BeGreaterThan(originPostScore);
		}
	}
}