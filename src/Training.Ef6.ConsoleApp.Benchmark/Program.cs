using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Training.Ef6.Infrastructure.Services;

namespace Training.Ef6.ConsoleApp.Benchmark
{
	public class Program
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

			//var result = BenchmarkRunner.Run<EfBenchmark>();

		}

		[MemoryDiagnoser]
		[MedianColumn]
		[Config(typeof(Config))]
		public class EfBenchmark
		{
			private readonly IEnumerable<int> _numberList;
			private readonly PostsService _postsService;
			private readonly StackOverflow2010Entities _context;

			public EfBenchmark()
			{
				_numberList = Enumerable.Range(1, 1000);
				_context = new StackOverflow2010Entities();
				_postsService = new PostsService(_context);
			}

			[Benchmark]
			public List<Post> GetPosts()
			{
				return _postsService.GetBy(_numberList);
			}

			[Benchmark]
			public List<User> BufferingUsers()
			{
				var filter = Guid.NewGuid().ToString("N").Substring(0, 10);
				var users = _context
					.Users
					//.AsNoTracking()
					.Where(i => i.Location.StartsWith(filter))
					.Take(10)
					.ToList();

				var result = new List<User>();
				foreach (var user in users)
				{
					result.Add(user);
				}

				return result;
			}

			[Benchmark]
			public List<User> StreamingUsers()
			{
				var filter = Guid.NewGuid().ToString("N").Substring(0, 10);
				var users = _context
					.Users
					//.AsNoTracking()
					.Where(i => i.Location.StartsWith(filter))
					.Take(10);

				var result = new List<User>();
				foreach (var user in users)
				{
					result.Add(user);
				}

				return result;
			}

			private class Config : ManualConfig
			{
				public Config()
				{
					AddJob(Job.MediumRun
						.WithLaunchCount(1)
						.WithToolchain(InProcessEmitToolchain.Instance)
						.WithId("InProcess"));
				}
			}
		}
	}
}
