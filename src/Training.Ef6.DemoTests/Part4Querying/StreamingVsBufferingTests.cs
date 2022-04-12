using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class StreamingVsBufferingTests : TestBase
	{
		public StreamingVsBufferingTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task buffering_examples()
		{
			await Task.CompletedTask;

			Stopwatch.Start();

			var users = ContextDbFirst
				.Users
				.AsNoTracking()
				.Where(i => i.Location.StartsWith("Poland"))
				.Take(10);

			var result = users.ToList();

			Stopwatch.Stop();
			TestOutput.WriteLine("All records available after {0} ms", Stopwatch.ElapsedMilliseconds);

			result.Any().Should().BeTrue();
		}

		[Fact]
		public async Task streaming_examples()
		{
			await Task.CompletedTask;

			var users = ContextDbFirst
				.Users
				.AsNoTracking()
				.Where(i => i.Location.StartsWith("Poland"))
				.Take(10);

			var result = new List<User>();
			var index = 1;
			
			Stopwatch.Start();

			foreach (var user in users)
			{
				TestOutput.WriteLine("{0} record available after {1} ms", index, Stopwatch.ElapsedMilliseconds);
				
				result.Add(user);
				
				index++;
			}
			Stopwatch.Stop();
			TestOutput.WriteLine("All records available after {0} ms", Stopwatch.ElapsedMilliseconds);

			result.Any().Should().BeTrue();
		}
	}
}