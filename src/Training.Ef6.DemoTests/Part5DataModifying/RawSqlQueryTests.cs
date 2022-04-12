using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part5DataModifying
{
	public class RawSqlQueryTests : TestBase
	{
		public RawSqlQueryTests(ITestOutputHelper output) : base(output)
		{
			//ContextDbFirst.Database.Log = null;
		}

		[Fact]
		public async Task query_with_linq()
		{
			Stopwatch.Start();

			var posts = await ContextDbFirst
				.Posts
				.Where(i => i.ClosedDate != null && DbFunctions.DiffDays(i.ClosedDate.Value, i.CreationDate) < 7)
				.OrderBy(i => i.Id)
				.Take(10)
				.ToListAsync();
			
			Stopwatch.Stop();
			TestOutput.WriteLine("Entities loaded in {0} ms", Stopwatch.ElapsedMilliseconds);

			LogTrackedEntities();

			ContextDbFirst.Posts.Local.Any().Should().BeTrue();
		}

		[Fact]
		public async Task query_with_sql()
		{
			var query =
				"select top(10) * from dbo.Posts where ClosedDate is not null and datediff(day, ClosedDate, CreationDate) < 7 order by Id";
			Stopwatch.Start();

			var posts = await ContextDbFirst
				.Posts
				.SqlQuery(query)
				.ToListAsync();

			Stopwatch.Stop();
			TestOutput.WriteLine("Entities loaded in {0} ms", Stopwatch.ElapsedMilliseconds);

			LogTrackedEntities();

			ContextDbFirst.Posts.Local.Any().Should().BeTrue();
		}
	}
}