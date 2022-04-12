using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Training.Ef6.DemoTests.Common;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class RedundantTrackingIssueTests : TestBase
	{
		private readonly IMapper _mapper;

		public RedundantTrackingIssueTests(ITestOutputHelper output) : base(output)
		{
			_mapper = GetUserToViewModelMapper();
		}

		[Fact]
		public async Task tracked_entities_example()
		{
			var pageSize = 10;

			var result = await ContextDbFirst
				.Users
				.OrderBy(i => i.Id)
				.Take(pageSize)
				.ToListAsync();

			LogTrackedEntities();

			result.Any().Should().BeTrue();
			ContextDbFirst.Users.Local.Count.Should().Be(pageSize);
		}

		[Fact]
		public async Task explicit_tracking_disabling()
		{
			var pageSize = 10;

			var result = await ContextDbFirst
				.Users
				.AsNoTracking()
				.OrderBy(i => i.Id)
				.Take(pageSize)
				.ToListAsync();

			LogTrackedEntities();

			result.Any().Should().BeTrue();
			ContextDbFirst.Users.Local.Count.Should().Be(0);
		}

		[Fact]
		public async Task implicit_tracking_disabling()
		{
			var pageSize = 10;

			var result = await ContextDbFirst
				.Users
				.OrderBy(i => i.Id)
				.Take(pageSize)
				.Select(i => new UserViewModel
				{
					DisplayName = i.DisplayName,
					Region = i.Region
				})
				.ToListAsync();

			var result2 = await _mapper.ProjectTo<UserViewModel>(
					ContextDbFirst
						.Users
						.OrderBy(i => i.Id)
						.Take(pageSize))
				.ToListAsync();

			LogTrackedEntities();

			result.Any().Should().BeTrue();
			result2.Any().Should().BeTrue();
			ContextDbFirst.Users.Local.Count.Should().Be(0);
		}

		[Fact]
		public async Task tracking_disabled_by_context_splitting()
		{
			var pageSize = 10;

			var result = await ContextDbFirstNoProxy
				.Users
				.OrderBy(i => i.Id)
				.Take(pageSize)
				.ToListAsync();

			LogTrackedEntities();

			result.Any().Should().BeTrue();
			ContextDbFirst.Users.Local.Count.Should().Be(0);
		}
	}
}