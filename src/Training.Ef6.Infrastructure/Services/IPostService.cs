using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Training.Ef6.Infrastructure.DatabaseFirst;

namespace Training.Ef6.Infrastructure.Services
{
	public interface IPostService
	{
		List<Post> GetBy(IEnumerable<int> idList);

		Task AddPostType(string postType);

		Task AddScore(int postId, int score);

		Task<List<Post>> GetTopXBy(
			int topThreshold,
			string tag);

		Task<List<Post>> GetTopXBy(
			int topThreshold,
			string tag,
			CancellationToken cancellationToken);
	}
}