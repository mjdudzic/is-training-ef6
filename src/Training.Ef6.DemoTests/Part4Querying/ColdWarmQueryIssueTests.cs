using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InteractivePreGeneratedViews;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class ColdWarmQueryIssueTests : TestBase, IDisposable
	{
		public ColdWarmQueryIssueTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task cold_and_warm_query()
		{
			Stopwatch.Start();

			var result = await ContextDbFirst
				.Users
				.Where(i =>
					i.Reputation > 1000 &&
					i.UpVotes > 1000 &&
					i.Location.Contains("Poland"))
				.Select(i => new
				{
					i.DisplayName,
					PostsCount = ContextDbFirst.Posts.Count(p => p.OwnerUserId == i.Id)
				})
				.ToListAsync();

			Stopwatch.Stop();

			TestOutput.WriteLine("First query: {0}", Stopwatch.ElapsedMilliseconds);

			Stopwatch.Restart();

			await ContextDbFirst
				.Users
				.Where(i =>
					i.Reputation > 1000 &&
					i.UpVotes > 1000 &&
					i.Location.Contains("Poland"))
				.Select(i => new
				{
					i.DisplayName,
					PostsCount = ContextDbFirst.Posts.Count(p => p.OwnerUserId == i.Id)
				})
				.ToListAsync();

			Stopwatch.Stop();

			TestOutput.WriteLine("Second query: {0}", Stopwatch.ElapsedMilliseconds);

			result.Any().Should().BeTrue();
		}

		[Fact]
		public async Task query_without_or_with_pregenerated_views()
		{
			// To test pre-generated views include DataModel.Views.cs class
			Stopwatch.Start();

			var result = await ContextDbFirst
				.Users
				.FirstOrDefaultAsync();
			
			Stopwatch.Stop();

			TestOutput.WriteLine("First query: {0}", Stopwatch.ElapsedMilliseconds);

			Stopwatch.Restart();

			await ContextDbFirst
				.Users
				.FirstOrDefaultAsync();

			Stopwatch.Stop();

			TestOutput.WriteLine("Second query: {0}", Stopwatch.ElapsedMilliseconds);

			result.Should().NotBeNull();
		}

		[Fact]
		public async Task query_with_interactive_pregenerated_views()
		{
			// Disable profiler and static pre-generated views
			InteractiveViews
				.SetViewCacheFactory(
					ContextDbFirst,
					new SqlServerViewCacheFactory(ContextDbFirst.Database.Connection.ConnectionString));

			Stopwatch.Start();

			var result = await ContextDbFirst
				.Users
				.FirstOrDefaultAsync();

			Stopwatch.Stop();

			TestOutput.WriteLine("First query: {0}", Stopwatch.ElapsedMilliseconds);

			Stopwatch.Restart();

			await ContextDbFirst
				.Users
				.FirstOrDefaultAsync();

			Stopwatch.Stop();

			TestOutput.WriteLine("Second query: {0}", Stopwatch.ElapsedMilliseconds);

			result.Should().NotBeNull();
		}

		public void Dispose()
		{
			ContextDbFirst?.Dispose();
		}
	}
}