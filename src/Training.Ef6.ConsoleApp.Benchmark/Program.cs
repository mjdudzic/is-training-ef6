using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Training.Ef6.Infrastructure.DatabaseFirst;

namespace Training.Ef6.ConsoleApp.Benchmark
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var context = new StackOverflow2010Entities();

			var post = await context
				.Posts
				.Where(i => i.OwnerUserId != null)
				.FirstOrDefaultAsync();

			var user = post?.User;

			if (user == null)
				throw new Exception("Database contains no data");

			Console.WriteLine("Database connection works OK");
			
		}
	}
}
