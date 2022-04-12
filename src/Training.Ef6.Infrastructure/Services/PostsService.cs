using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Training.Ef6.Infrastructure.DatabaseFirst;

namespace Training.Ef6.Infrastructure.Services
{
	public class PostsService : IPostService
	{
		private readonly StackOverflow2010Entities _context;

		public PostsService(StackOverflow2010Entities context)
		{
			_context = context;
		}

		public List<Post> GetBy(IEnumerable<int> idList)
		{
			return _context.Posts.Where(i => idList.Contains(i.Id)).ToList();
		}

		public async Task AddPostType(string postType)
		{
			_context.PostTypes.Add(new PostType { Type = postType });

			await _context.SaveChangesAsync();
		}

		public async Task AddScore(int postId, int score)
		{
			var post = await _context.Posts.FindAsync(postId);

			if (post == null) 
				return;

			post.Score += score;
			
			await _context.SaveChangesAsync();
		}

		public Task<List<Post>> GetTopXBy(
			int topThreshold,
			string tag)
		{
			return _context
				.Posts
				.Where(i => i.Tags.Contains(tag))
				.OrderByDescending(i => i.Score)
				.Take(topThreshold)
				.ToListAsync();
		}

		public Task<List<Post>> GetTopXBy(
			int topThreshold,
			string tag,
			CancellationToken cancellationToken)
		{
			return _context
				.Posts
				.Where(i => i.Tags.Contains(tag))
				.OrderByDescending(i => i.Score)
				.Take(topThreshold)
				.ToListAsync(cancellationToken);
		}
	}
}