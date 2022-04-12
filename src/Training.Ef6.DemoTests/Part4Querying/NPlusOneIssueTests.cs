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
	public class NPlusOneIssueTests : TestBase
	{
		private readonly IMapper _mapper;

		public NPlusOneIssueTests(ITestOutputHelper output) : base(output)
		{
			_mapper = GetTestMapper();
		}

		[Fact]
		// N + 1 issue
		public async Task n_plus_one_issue_1()
		{
			var posts = await ContextDbFirst
				.Posts
				.OrderBy(i => i.Id)
				.Take(10)
				.ToListAsync();

			var topAnswer = posts
				.Where(p => p.AcceptedAnswerId != null)
				.Select(p => p.Post2)
				.OrderByDescending(a => a.Score)
				.FirstOrDefault();

			var topComment = posts
				.SelectMany(p => p.Comments)
				.OrderByDescending(c => c.Score)
				.FirstOrDefault();


			topAnswer?.Score.Should().BeGreaterThan(0);
			topComment?.Score.Should().BeGreaterThan(0);
		}

		[Fact]
		// N + 1 issue
		public async Task n_plus_one_issue_2()
		{
			var data = await ContextDbFirst
				.Posts
				.OrderBy(i => i.Id)
				.Take(1)
				.ToListAsync();

			var result = data.Select(i => _mapper.Map<PostWithAnswer>(i));

			//var result = await ContextDbFirst
			//	.Posts
			//	.OrderBy(i => i.Id)
			//	.Take(1)
			//	.Select(i => new PostWithAnswer
			//	{
			//		Id = i.Id,
			//		Title = i.Title,
			//		Body = i.Body,
			//		Answer = i.Post2.Body
			//	})
			//	.ToListAsync();

			//var result = await _mapper.ProjectTo<PostWithAnswer>(
			//	ContextDbFirst
			//		.Posts
			//		.OrderBy(i => i.Id)
			//		.Take(1))
			//	.ToListAsync();

			result.Any().Should().BeTrue();
		}

		[Fact]
		public async Task n_plus_one_fix()
		{
			ContextDbFirst.Configuration.LazyLoadingEnabled = false;

			var data = await ContextDbFirst
				.Posts
				.OrderBy(i => i.Id)
				.Take(1)
				.ToListAsync();

			var result = data.Select(i => _mapper.Map<PostWithAnswer>(i));

			result.Any().Should().BeTrue();
		}

		private IMapper GetTestMapper()
		{
			var config = new MapperConfiguration(cfg => 
				cfg.CreateMap<Post, PostWithAnswer>()
					.ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Post2.Body)));

			return config.CreateMapper();
		}

		public class PostWithAnswer
		{
			public int Id { get; set; }
			public string Title { get; set; }
			public string Body { get; set; }
			public string Answer { get; set; }

		}
	}
}