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
	public class RawSqlCommandTests : TestBase
	{
		public RawSqlCommandTests(ITestOutputHelper output) : base(output)
		{
			ContextDbFirst.Database.Log = null;
		}

		[Fact]
		public async Task updating_many_with_the_same_rule_with_tracking()
		{
			var ids = await ContextDbFirst.Posts.OrderBy(i => i.Id).Take(10).Select(i => i.Id).ToListAsync();

			Stopwatch.Start();
			
			var posts = await ContextDbFirst.Posts.Where(i => ids.Contains(i.Id)).ToListAsync();
			posts.ForEach(post => post.Score++);

			await ContextDbFirst.SaveChangesAsync();

			Stopwatch.Stop();
			TestOutput.WriteLine("Entities updated in {0} ms", Stopwatch.ElapsedMilliseconds);
		}

		[Fact]
		public async Task updating_many_with_the_same_rule_with_sql_command()
		{
			var ids = await ContextDbFirst.Posts.OrderBy(i => i.Id).Take(10).Select(i => i.Id).ToListAsync();

			Stopwatch.Start();
			
			var command = $"update dbo.Posts set Score = Score + 1 where Id in ({string.Join(",", ids)})";
			await ContextDbFirst.Database.ExecuteSqlCommandAsync(command);

			Stopwatch.Stop();
			TestOutput.WriteLine("Entities updated in {0} ms", Stopwatch.ElapsedMilliseconds);
		}
	}
}