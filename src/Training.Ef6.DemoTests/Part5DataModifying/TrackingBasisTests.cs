using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part5DataModifying
{
	public class TrackingBasisTests : TestBase
	{
		public TrackingBasisTests(ITestOutputHelper output) : base(output)
		{
			ContextDbFirst.Database.Log = null;
		}

		[Fact]
		public async Task entity_states()
		{
			var newPost = GetTestPost();

			var existingPost1 = await ContextDbFirst.Posts.OrderByDescending(i => i.Id).FirstAsync();
			var existingPost2 = await ContextDbFirst.Posts.OrderByDescending(i => i.Id).Skip(1).FirstAsync();
			var existingPost3 = await ContextDbFirst.Posts.OrderByDescending(i => i.Id).Skip(2).FirstAsync();
			var existingPost4 = await ContextDbFirst.Posts.AsNoTracking().OrderByDescending(i => i.Id).Skip(3).FirstAsync();

			ContextDbFirst.Posts.Add(newPost);
			existingPost2.LastActivityDate = DateTime.UtcNow;
			ContextDbFirst.Posts.Remove(existingPost3);
			ContextDbFirst.Posts.Attach(existingPost4);

			LogTrackedEntities();

			Post existingPost5;
			using (var context = new StackOverflow2010Entities())
			{
				existingPost5 = await context.Posts.OrderByDescending(i => i.Id).Skip(4).FirstAsync();
			}

			using (var context = new StackOverflow2010Entities())
			{
				var entry = context.Entry(existingPost5);
				TestOutput.WriteLine($"{entry.Entity}, {entry.State}");
			}

			ContextDbFirst.Posts.Local.Any().Should().BeTrue();
		}

		[Fact]
		public async Task entity_changes_review()
		{
			var post = await ContextDbFirst.Posts.OrderByDescending(i => i.Id).FirstAsync();
			post.Body = Guid.NewGuid().ToString();

			var postBodyCurrent = ContextDbFirst.Entry(post).Property(p => p.Body).CurrentValue;
			var postBodyOriginal = ContextDbFirst.Entry(post).Property(p => p.Body).OriginalValue;
			var postBodyDbValue = (await ContextDbFirst.Entry(post).GetDatabaseValuesAsync()).GetValue<string>(nameof(Post.Body));
			var postBodyDbValue2 = ((await ContextDbFirst.Entry(post).GetDatabaseValuesAsync()).ToObject() as Post)?.Body;

			postBodyCurrent.Should().NotBe(postBodyOriginal);
			postBodyOriginal.Should().Be(postBodyDbValue);
			postBodyOriginal.Should().Be(postBodyDbValue2);
		}
	}
}