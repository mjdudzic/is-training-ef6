using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part1BasicConcepts
{
	public class UnitOfWorkPatternTest : TestBase
	{
		public UnitOfWorkPatternTest(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task unit_of_work_pattern()
		{
			var testPostType = Guid.NewGuid().ToString();

			// unit of work start 
			AddNewPostType(ContextDbFirst, testPostType);
			await DoSomeJob();
			await ContextDbFirst.SaveChangesAsync();
			// unit of work end with commit

			// unit of work start 
			await DeletePostType(ContextDbFirst, testPostType);
			await ContextDbFirst.SaveChangesAsync();
			// unit of work end with commit

			ContextDbFirst.PostTypes.Any(i => i.Type == testPostType).Should().BeFalse();
		}

		[Fact]
		public async Task unit_of_work_pattern_issue()
		{
			var testPostType = Guid.NewGuid().ToString();

			// unit of work start 
			AddNewPostType(ContextDbFirst, testPostType);
			await DoSomeJob();
			await DeletePostType(ContextDbFirst, testPostType);
			await ContextDbFirst.SaveChangesAsync();
			// unit of work end with commit

			await DeletePostType(ContextDbFirst, testPostType);
			await ContextDbFirst.SaveChangesAsync();

			ContextDbFirst.PostTypes.Any(i => i.Type == testPostType).Should().BeFalse();
		}

		[Fact]
		public async Task navigation_property_issue()
		{
			var postsList = new Faker<Post>()
				.RuleFor(i => i.Tags, f => "test")
				.RuleFor(i => i.PostTypeId, f => 1)
				.RuleFor(i => i.Body, f => f.Lorem.Paragraph())
				.RuleFor(i => i.CreationDate, f => DateTime.UtcNow)
				.RuleFor(i => i.LastActivityDate, f => DateTime.UtcNow)
				.Generate(2);

			var postChild = postsList[0];
			var postParent = postsList[1];
			postChild.Post1 = postParent;

			ContextDbFirst.Posts.Add(postChild);
			await ContextDbFirst.SaveChangesAsync();

			postParent.Posts1.Remove(postChild);
			//ContextDbFirst.Posts.Remove(postParent);
			await ContextDbFirst.SaveChangesAsync();
			//await CustomSaveChanges();

			(await ContextDbFirst.Posts.CountAsync(i => i.Id == postParent.Id)).Should().Be(1);
			(await ContextDbFirst.Posts.CountAsync(i => i.Id == postChild.Id)).Should().Be(1);
		}

		public void AddNewPostType(StackOverflow2010Entities context, string postType)
		{
			var newPostType = new PostType
			{
				Type = postType
			};

			context.PostTypes.Add(newPostType);
		}

		public async Task DeletePostType(StackOverflow2010Entities context, string postType)
		{
			var postTypeEntity = await context.PostTypes.FirstOrDefaultAsync(i => i.Type == postType);

			if (postTypeEntity == null)
				return;

			context.PostTypes.Remove(postTypeEntity);
		}

		public async Task CustomSaveChanges()
		{
			foreach (var post in ContextDbFirst.Posts.Local.ToList())
			{
				if (post.ParentId == null)
				{
					ContextDbFirst.Posts.Remove(post);
				}
			}

			await ContextDbFirst.SaveChangesAsync();
		}

		public Task DoSomeJob()
		{
			return Task.CompletedTask;
		}
	}
}