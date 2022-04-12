using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EFCache;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part7ExtLibs.EfCache
{
	public class SecondLevelCacheTests : TestBase
	{
		public SecondLevelCacheTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task cache_example()
		{
			var postTypes = await ContextDbFirst
				.PostTypes
				.OrderBy(i => i.Id)
				.Take(8)
				.ToListAsync();

			//TestOutput.WriteLine("Items in cache: {0}", Cache.Count);

			postTypes = await ContextDbFirst
				.PostTypes
				.OrderBy(i => i.Id)
				.Take(8)
				.ToListAsync();

			//TestOutput.WriteLine("Items in cache: {0}", Cache.Count);
		}
	}
}