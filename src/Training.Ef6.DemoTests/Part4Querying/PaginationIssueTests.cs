using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class PaginationIssueTests : TestBase
	{
		public PaginationIssueTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task pagination_examples()
		{
			// First approach - offset
			var pageIndex = 1;
			var pageSize = 10;

			var skip = (pageIndex - 1) * pageSize;

			var usersFirstPage = await ContextDbFirst
				.Users
				.OrderByDescending(i => i.Reputation)
				.ThenBy(i => i.Id)
				.Skip(() => skip)
				.Take(() => pageSize)
				.ToListAsync();

			pageIndex++;
			skip = (pageIndex - 1) * pageSize;
			var usersSecondPage = await ContextDbFirst
				.Users
				.OrderByDescending(i => i.Reputation)
				.ThenBy(i => i.Id)
				.Skip(() => skip)
				.Take(() => pageSize)
				.ToListAsync();

			// Second approach - keyset
			var lastId = usersFirstPage.Last().Id;
			var lastReputation = usersFirstPage.Last().Reputation;

			var usersSecondPageB = await ContextDbFirst
				.Users
				.OrderByDescending(i => i.Reputation)
				.ThenBy(i => i.Id)
				.Where(i => i.Reputation < lastReputation || (i.Reputation == lastReputation && i.Id > lastId))
				.Take(pageSize)
				.ToListAsync();

			usersSecondPage.First().Id.Should().Be(usersSecondPageB.First().Id);
			usersSecondPage.Last().Id.Should().Be(usersSecondPageB.Last().Id);
		}
	}
}