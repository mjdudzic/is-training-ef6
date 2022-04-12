using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class ClientServerEvalIssueTests : TestBase
	{
		public ClientServerEvalIssueTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void client_eval_issue_1()
		{
			var postTypes = ContextDbFirst
				.PostTypes
				.AsEnumerable()
				.Where(i => i.Type.ToLower().Contains("wiki"));

			postTypes.Any().Should().BeTrue();
		}

		[Fact]
		public void client_eval_issue_2()
		{
			var postTypeStats = new Dictionary<string, int>();

			ContextDbFirst
				.PostTypes
				.ToList()
				.ForEach(i =>
				{
					var typeUsage = ContextDbFirst.Posts.Count(p => p.PostTypeId == i.Id);
					postTypeStats.Add(i.Type, typeUsage);
				});

			postTypeStats.Any().Should().BeTrue();

			//var fix = ContextDbFirst
			//	.PostTypes
			//	.Select(t => new
			//	{
			//		t.Type,
			//		Usage = ContextDbFirst.Posts.Count(p => p.PostTypeId == t.Id)
			//	})
			//	.ToDictionary(i => i.Type, i => i.Usage);

			//fix.Any().Should().BeTrue();
		}

		[Fact]
		public void client_eval_issue_3()
		{
			var service = new TestService(ContextDbFirst);

			var postTypes = service
				.GetPostTypes()
				.Where(i => i.Type.ToLower().Contains("wiki"));

			postTypes.Any().Should().BeTrue();
		}

		public class TestService
		{
			private readonly StackOverflow2010Entities _context;

			public TestService(StackOverflow2010Entities context)
			{
				_context = context;
			}

			public IEnumerable<PostType> GetPostTypes()
			{
				return _context.PostTypes;
			}
		}
	}
}