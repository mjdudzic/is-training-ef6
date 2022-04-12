using System.Collections.Generic;
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
	public class SelectAllIssueTests : TestBase
	{
		private readonly IMapper _mapper;

		public SelectAllIssueTests(ITestOutputHelper output) : base(output)
		{
			_mapper = GetUserToViewModelMapper();
		}

		[Fact]
		public async Task limited_result_issue()
		{
			var users = await ContextDbFirst
				.Users
				.OrderBy(i => i.Id)
				.Take(5)
				.ToListAsync();

			var result = new List<UserViewModel>();

			foreach (var user in users)
			{
				result.Add(new UserViewModel
				{
					DisplayName = user.DisplayName,
					Region = user.Region
				});
			}

			//var result = await ContextDbFirst
			//	.Users
			//	.OrderBy(i => i.Id)
			//	.Take(5)
			//	.Select(i => new UserViewModel
			//	{
			//		DisplayName = i.DisplayName,
			//		Region = i.Region
			//	})
			//	.ToListAsync(); ;

			//var result = await _mapper.ProjectTo<UserViewModel>(
			//	ContextDbFirst
			//		.Users
			//		.OrderBy(i => i.Id)
			//		.Take(5))
			//	.ToListAsync();

			//var user0 = await ContextDbFirst.Users.FindAsync(1);

			result.Any().Should().BeTrue();
		}
	}
}