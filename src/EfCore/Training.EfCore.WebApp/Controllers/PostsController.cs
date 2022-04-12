using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.EfCore.Infrastructure.Models;

namespace InfoShare.Training.EfCore.WebApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PostsController : ControllerBase
	{
		private readonly StackOverflow2010Context _context;

		public PostsController(StackOverflow2010Context context)
		{
			_context = context;
		}

		[HttpGet("v1/{id}")]
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

		[HttpGet("v2/{id}")]
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


		[HttpPut("v1/{id}")]
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

		[HttpPut("v2/{id}")]
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
	}

	public class PostDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public int Score { get; set; }
	}
}
