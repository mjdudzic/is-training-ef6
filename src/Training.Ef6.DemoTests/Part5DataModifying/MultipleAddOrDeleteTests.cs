using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part5DataModifying
{
	public class MultipleAddOrDeleteTests : TestBase
	{
		private List<User> _testUsers;

		public MultipleAddOrDeleteTests(ITestOutputHelper output) : base(output)
		{
			ContextDbFirst.Database.Log = null;
		}

		[Fact]
		public async Task add_many_records()
		{
			_testUsers = GetTestUsers();

			Stopwatch.Start();

			_testUsers.ForEach(p => ContextDbFirst.Users.Add(p));

			TestOutput.WriteLine("Adding new entries in {0} ms", Stopwatch.ElapsedMilliseconds);

			LogTrackedEntities();

			Stopwatch.Restart();

			await ContextDbFirst.SaveChangesAsync();

			TestOutput.WriteLine("Saving new entries in {0} ms", Stopwatch.ElapsedMilliseconds);

			_testUsers.Any().Should().BeTrue();
		}

		[Fact]
		public async Task add_many_records_as_range()
		{
			_testUsers = GetTestUsers();

			Stopwatch.Start();

			ContextDbFirst.Users.AddRange(_testUsers);

			TestOutput.WriteLine("Adding new entries in {0} ms", Stopwatch.ElapsedMilliseconds);

			LogTrackedEntities();

			Stopwatch.Restart();

			await ContextDbFirst.SaveChangesAsync();

			TestOutput.WriteLine("Saving new entries in {0} ms", Stopwatch.ElapsedMilliseconds);

			_testUsers.Any().Should().BeTrue();
		}

		[Fact]
		public async Task add_many_records_with_explicit_detect_changes()
		{
			ContextDbFirst.Configuration.AutoDetectChangesEnabled = false;

			_testUsers = GetTestUsers();

			Stopwatch.Start();

			_testUsers.ForEach(p => ContextDbFirst.Users.Add(p));

			TestOutput.WriteLine("Adding new entries in {0} ms", Stopwatch.ElapsedMilliseconds);

			LogTrackedEntities();

			Stopwatch.Restart();

			//ContextDbFirst.ChangeTracker.DetectChanges();
			await ContextDbFirst.SaveChangesAsync();

			TestOutput.WriteLine("Saving new entries in {0} ms", Stopwatch.ElapsedMilliseconds);

			_testUsers.Any().Should().BeTrue();
		}

		[Fact]
		public async Task delete_many_records_with_explicit_detect_changes()
		{
			ContextDbFirst.Configuration.AutoDetectChangesEnabled = false;

			_testUsers = GetTestUsers(1);

			Stopwatch.Start();

			_testUsers.ForEach(p => ContextDbFirst.Users.Add(p));

			TestOutput.WriteLine("Adding new entries in {0} ms", Stopwatch.ElapsedMilliseconds);

			LogTrackedEntities();

			Stopwatch.Restart();

			//ContextDbFirst.ChangeTracker.DetectChanges();
			await ContextDbFirst.SaveChangesAsync();

			ContextDbFirst.Users.RemoveRange(_testUsers);

			LogTrackedEntities();

			await ContextDbFirst.SaveChangesAsync();

			TestOutput.WriteLine("Saving new entries in {0} ms", Stopwatch.ElapsedMilliseconds);

			_testUsers.Any().Should().BeTrue();
		}

		[Fact]
		public async Task delete_many_at_once_issue()
		{
			ContextDbFirst.Database.Log = null;

			var postTypes = new List<PostType>();
			for (var i = 0; i < 10; i++)
			{
				var newPostType = GetTestPostType();
				ContextDbFirst.PostTypes.Add(newPostType);

				postTypes.Add(newPostType);
			}
			await ContextDbFirst.SaveChangesAsync();

			Stopwatch.Start();
			postTypes.ForEach(postType => ContextDbFirst.PostTypes.Remove(postType));

			await ContextDbFirst.SaveChangesAsync();

			Stopwatch.Stop();
			TestOutput.WriteLine("Entities deleted at {0}", Stopwatch.ElapsedMilliseconds);

			postTypes.Any().Should().BeTrue();
		}

		[Fact]
		public async Task delete_many_at_once_issue_fix()
		{
			ContextDbFirst.Database.Log = null;

			var postTypes = new List<PostType>();
			for (var i = 0; i < 10; i++)
			{
				var newPostType = GetTestPostType();
				ContextDbFirst.PostTypes.Add(newPostType);

				postTypes.Add(newPostType);
			}
			await ContextDbFirst.SaveChangesAsync();

			Stopwatch.Start();
			ContextDbFirst.PostTypes.RemoveRange(postTypes);

			await ContextDbFirst.SaveChangesAsync();

			Stopwatch.Stop();
			TestOutput.WriteLine("Entities deleted at {0}", Stopwatch.ElapsedMilliseconds);

			postTypes.Any().Should().BeTrue();
		}
	}
}