using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Google.Protobuf.Reflection;
using Microsoft.EntityFrameworkCore;
using Training.EfCore.Infrastructure.Models;
using Xunit;
using Xunit.Abstractions;

namespace Training.EfCore.DemoTests;

public class CompiledQueryTests : TestBase
{
	private static readonly Func<StackOverflow2010Context, string, int, Task<int>> CompiledQuery
		= EF.CompileAsyncQuery(
			(StackOverflow2010Context context, string location, int minReputation) => 
			context.Users.Count(u => u.Location != null && u.Location.StartsWith(location) && u.Reputation >= minReputation));

	public CompiledQueryTests(ITestOutputHelper output) : base(output)
	{
	}

	[Fact]
	public async Task compiled_qery()
	{
		Stopwatch.Start();
		
		var result = await CompiledQuery(Context, "Poland", 10000);
		await CompiledQuery(Context, "US", 20000);

		Stopwatch.Stop();
		Output.WriteLine("Compiled query: {0} ms", Stopwatch.ElapsedMilliseconds);
		
		result.Should().BeGreaterThan(0);
	}

	[Fact]
	public async Task linq_query()
	{
		Stopwatch.Start();

		var result = await Context.Users.CountAsync(u => u.Location != null && u.Location.StartsWith("Poland") && u.Reputation >= 10000);
		await Context.Users.CountAsync(u => u.Location != null && u.Location.StartsWith("US") && u.Reputation >= 20000);

		Stopwatch.Stop();
		Output.WriteLine("Linq query: {0} ms", Stopwatch.ElapsedMilliseconds);

		result.Should().BeGreaterThan(0);
	}
}
