using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Training.EfCore.Infrastructure.Models;
using Xunit;
using Xunit.Abstractions;

namespace Training.EfCore.DemoTests;

public class StreamingVsBufferingTests : TestBase
{
	public StreamingVsBufferingTests(ITestOutputHelper output) : base(output)
	{
	}

	[Fact]
	public async Task buffering_examples()
	{
		Stopwatch.Start();

		var users = Context
			.Users
			//.AsNoTracking()
			.Where(i => i.Location.StartsWith("Poland"))
			.Take(10);

		var result = await users.ToListAsync();

		Stopwatch.Stop();
		Output.WriteLine("All records available after {0} ms", Stopwatch.ElapsedMilliseconds);

		result.Any().Should().BeTrue();
	}

	[Fact]
	public async Task streaming_examples()
	{
		await Task.CompletedTask;

		var users = Context
			.Users
			.AsNoTracking()
			.Where(i => i.Location.StartsWith("Poland"))
			.Take(10);

		var result = new List<User>();
		var index = 1;

		Stopwatch.Start();

		foreach (var user in users)
		{
			Output.WriteLine("{0} record available after {1} ms", index, Stopwatch.ElapsedMilliseconds);

			result.Add(user);

			index++;
		}
		Stopwatch.Stop();
		Output.WriteLine("All records available after {0} ms", Stopwatch.ElapsedMilliseconds);

		result.Any().Should().BeTrue();
	}

	[Fact]
	public async Task streaming_async_examples()
	{
		var users = Context
			.Users
			.AsNoTracking()
			.Where(i => i.Location.StartsWith("Poland"))
			.Take(10);

		var result = new List<User>();
		var index = 1;

		Stopwatch.Start();

		await foreach (var user in users.AsAsyncEnumerable())
		{
			Output.WriteLine("{0} record available after {1} ms", index, Stopwatch.ElapsedMilliseconds);

			result.Add(user);

			index++;
		}
		Stopwatch.Stop();
		Output.WriteLine("All records available after {0} ms", Stopwatch.ElapsedMilliseconds);

		result.Any().Should().BeTrue();
	}
}
