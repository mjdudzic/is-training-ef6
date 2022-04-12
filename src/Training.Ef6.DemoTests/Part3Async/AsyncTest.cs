using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part3Async
{
	public class AsyncTest : TestBase
	{
		public AsyncTest(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task async_querying()
		{
			var firstPost = await ContextDbFirst.Posts.OrderBy(i => i.Id).FirstOrDefaultAsync();

			var postList = await ContextDbFirst.Posts
				.Where(i => i.Score > 10)
				.OrderBy(i => i.Id)
				.Take(10)
				.ToListAsync();

			firstPost?.Body.Should().NotBeNullOrWhiteSpace();
			postList.Any().Should().BeTrue();
		}

		[Fact]
		public async Task async_command()
		{
			var post = await ContextDbFirst.Posts.OrderBy(i => i.Id).FirstAsync();

			post.Score += 10;

			await ContextDbFirst.SaveChangesAsync();
		}

		[Fact]
		public async Task async_in_parallel()
		{
			var task1 = ContextDbFirst.Posts.OrderBy(i => i.Id).FirstAsync();
			var task2 = ContextDbFirst.PostTypes.OrderBy(i => i.Id).FirstAsync();

			Func<Task> act = () => Task.WhenAll(task1, task2);

			await act.Should().ThrowAsync<NotSupportedException>();
		}

		[Fact]
		public async Task async_cancellation()
		{
			var tokenSource = new CancellationTokenSource();

			var task1 = ContextDbFirst.Posts
				.Where(i => i.Score > 10)
				.OrderBy(i => i.Id)
				.Take(10)
				.ToListAsync(tokenSource.Token);

			var task2 = Task.Run(() =>
			{
				tokenSource.CancelAfter(100);
			});

			Func<Task> act = () => Task.WhenAll(task1, task2);

			await act.Should().ThrowAsync<Exception>();

			var taskAborted = task1.IsCanceled || task1.IsFaulted;
			taskAborted.Should().BeTrue();
		}
	}
}