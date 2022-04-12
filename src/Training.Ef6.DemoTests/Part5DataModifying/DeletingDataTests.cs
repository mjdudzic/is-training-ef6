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
	public class DeletingDataTests : TestBase
	{
		public DeletingDataTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task delete_entity_round_trip_issue()
		{
			var newPostType = GetTestPostType();
			ContextDbFirst.PostTypes.Add(newPostType);
			await ContextDbFirst.SaveChangesAsync();

			var postTypeToDelete = await ContextDbFirst.PostTypes.FirstAsync(i => i.Id == newPostType.Id);
			ContextDbFirst.PostTypes.Remove(postTypeToDelete);
			await ContextDbFirst.SaveChangesAsync();

			ContextDbFirst.Entry(postTypeToDelete).State.Should().Be(EntityState.Detached);
		}

		[Fact]
		public async Task delete_entity_round_trip_issue_fix()
		{
			int newPostTypeId;
			using (var context = new StackOverflow2010Entities())
			{
				var newPostType = GetTestPostType();
				context.PostTypes.Add(newPostType);
				await context.SaveChangesAsync();

				newPostTypeId = newPostType.Id;
			}
			
			var postTypeToDelete = new PostType { Id = newPostTypeId };
			ContextDbFirst.PostTypes.Attach(postTypeToDelete);
			ContextDbFirst.PostTypes.Remove(postTypeToDelete);
			//ContextDbFirst.Entry(postTypeToDelete).State = System.Data.Entity.EntityState.Deleted;

			await ContextDbFirst.SaveChangesAsync();

			ContextDbFirst.Entry(postTypeToDelete).State.Should().Be(EntityState.Detached);
		}
	}
}