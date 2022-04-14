using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class ModifyingUntrackedIssueTests : TestBase
	{
		private readonly IMapper _mapper;

		public ModifyingUntrackedIssueTests(ITestOutputHelper output) : base(output)
		{
			_mapper = GetUserToViewModelMapper();
		}

		[Fact]
		public async Task untracked_and_modified_issue()
		{
			var user = await ContextDbFirst
				.Users
				.AsNoTracking()
				.OrderBy(i => i.Id)
				.FirstAsync();

			var originUserReputation = user.Reputation;

			user.Reputation += 100;

			LogTrackedEntities();

			await ContextDbFirst.SaveChangesAsync();

			user = await ContextDbFirst
				.Users
				.AsNoTracking()
				.OrderBy(i => i.Id)
				.FirstAsync();

			user.Reputation.Should().Be(originUserReputation);
		}

		[Fact]
		public async Task untracked_implicitly_and_modified_issue()
		{
			var post = await GetPost();

			var originUserReputation = post.Owner.Reputation;

			post.Answer.Score += 100;
			post.Owner.Reputation += 100;

			await ContextDbFirst.SaveChangesAsync();

			post = await GetPost();
			post.Owner.Reputation.Should().Be(originUserReputation);
		}

		[Fact]
		public async Task untracked_implicitly_and_modified_issue_fix()
		{
			var post = await GetPost();

			var originUserReputation = post.Owner.Reputation;

			// Attach must occur before changes are provided
			ContextDbFirst.Users.Attach(post.Owner);
			ContextDbFirst.Posts.Attach(post.Answer);

			post.Answer.Score += 100;
			post.Owner.Reputation += 100;

			LogTrackedEntities();

			await ContextDbFirst.SaveChangesAsync();

			post = await GetPost();
			post.Owner.Reputation.Should().BeGreaterThan(originUserReputation);
		}

		private async Task<PostData> GetPost()
		{
			return await ContextDbFirst
				.Posts
				.AsNoTracking()
				.Where(i => i.OwnerUserId != null && i.AcceptedAnswerId != null)
				.OrderByDescending(i => i.Id)
				.Select(i => new PostData
				{
					Id = i.Id,
					Title = i.Title,
					Body = i.Body,
					Owner = i.User,
					Answer = i.Post2
				})
				.FirstAsync();
		}

		public class PostData
		{
			public int Id { get; set; }
			public string Title { get; set; }
			public string Body { get; set; }
			public User Owner { get; set; }
			public Post Answer { get; set; }
		}
	}
}