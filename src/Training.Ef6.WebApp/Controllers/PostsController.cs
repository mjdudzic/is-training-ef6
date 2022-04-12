using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Training.Ef6.Infrastructure.Services;

namespace Training.Ef6.WebApp.Controllers
{
	public class PostsController : ApiController
	{
		readonly StackOverflow2010Entities _context;
		private readonly IPostService _postService;

		public PostsController()
		{
			_context = new StackOverflow2010Entities();
			_postService = new PostsService(_context);
		}

		[HttpGet]
		[Route("api/v1/posts/{id}")]
		public PostDto GetV1(int id)
		{
			var postDb = _context.Posts.FirstOrDefault(i => i.Id == id);

			if (postDb == null)
				return null;

			return new PostDto
			{
				Id = postDb.Id,
				Body = postDb.Body,
				Score = postDb.Score
			};
		}

		[HttpGet]
		[Route("api/v2/posts/{id}")]
		public async Task<PostDto> GetV2(int id)
		{
			var postDb = await _context.Posts.FirstOrDefaultAsync(i => i.Id == id);

			if (postDb == null)
				return null;

			return new PostDto
			{
				Id = postDb.Id,
				Title = postDb.Title,
				Body = postDb.Body,
				Score = postDb.Score
			};
		}


		[HttpPut]
		[Route("api/v1/posts/{id}")]
		public void PutV1(int id, [FromBody] PostDto postDto)
		{
			var postDb = _context.Posts.FirstOrDefault(i => i.Id == id);
			if (postDb == null)
				return;

			postDb.LastActivityDate = DateTime.UtcNow;
			postDb.Score = postDb.Score;
			postDb.Tags += "<load_test>";

			_context.SaveChanges();
		}

		[HttpPut]
		[Route("api/v2/posts/{id}")]
		public async Task PutV2(int id, [FromBody] PostDto postDto)
		{
			var postDb = await _context.Posts.FirstOrDefaultAsync(i => i.Id == id);
			if (postDb == null)
				return;

			postDb.LastActivityDate = DateTime.UtcNow;
			postDb.Score = postDb.Score;
			postDb.Tags += "<load_test>";

			await _context.SaveChangesAsync();
		}

		[HttpGet]
		[Route("api/v1/posts/top10/{tag}")]
		public async Task<IEnumerable<PostDto>> GetTopTenByTag(string tag)
		{
			var tagFormatted = $"<{tag}>";
			var result = (await _postService.GetTopXBy(10, tagFormatted))
				.Select(i => new PostDto
				{
					Id = i.Id,
					Body = i.Body,
					Title = i.Title,
					Score = i.Score
				});

			return result;
		}

		[HttpGet]
		[Route("api/v2/posts/top10/{tag}")]
		public async Task<IEnumerable<PostDto>> GetTopTenByTag(string tag, CancellationToken cancellationToken)
		{
			var tagFormatted = $"<{tag}>";
			var result = (await _postService.GetTopXBy(10, tagFormatted, cancellationToken))
				.Select(i => new PostDto
				{
					Id = i.Id,
					Body = i.Body,
					Title = i.Title,
					Score = i.Score
				});

			return result;
		}
	}

	public class PostDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public int Score { get; set; }
	}
}