using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;
using Newtonsoft.Json;

namespace Training.Ef6.LoadTests
{
	internal class Program
	{
		public const string BaseUrl = "http://localhost/InfoShare.Training.Ef6.WebApp/api/v2/posts";
		//public const string BaseUrl = "https://mjd-is-ef6-appplan.azurewebsites.net/api/v1/posts";
		static void Main(string[] args)
		{
			QueryLoadTest();
			//CommandLoadTest();
		}

		public static void QueryLoadTest()
		{
			var feedPostIds = Feed.CreateRandom("postIds", Enumerable.Range(1, 100000));

			var httpFactory = HttpClientFactory.Create();

			var postRequestStep = Step.Create(
				"get post",
				feed: feedPostIds,
				timeout: TimeSpan.FromSeconds(20),
				clientFactory: httpFactory,
				execute: async context =>
				{
					var postId = context.FeedItem;

					var request =
						Http.CreateRequest("GET", $"{BaseUrl}/{postId}")
							.WithCheck(res =>
								res.IsSuccessStatusCode
									? Task.FromResult(Response.Ok())
									: Task.FromResult(Response.Fail())
							);

					var response = await Http.Send(request, context);
					return response;
				});

			var scenario = ScenarioBuilder
				.CreateScenario(BaseUrl, postRequestStep)
				.WithWarmUpDuration(TimeSpan.FromSeconds(5))
				.WithLoadSimulations(Simulation.InjectPerSec(100, TimeSpan.FromSeconds(30)));

			NBomberRunner
				.RegisterScenarios(scenario)
				.Run();
		}

		public static void CommandLoadTest()
		{
			var postsFaker = new Faker<PostDto>()
				.RuleFor(i => i.Id, f => f.Random.Number(4, 1000000))
				.RuleFor(i => i.Score, f => f.Random.Number(0, 100))
				.RuleFor(i => i.Body, f => f.Lorem.Sentence(20))
				.RuleFor(i => i.Title, f => f.Random.Words(5));

			var dataSetMock = postsFaker.Generate(10000);

			var feedPosts = Feed.CreateRandom("posts", dataSetMock);

			var httpFactory = HttpClientFactory.Create();

			var postRequestStep = Step.Create(
				"add post",
				feed: feedPosts,
				timeout: TimeSpan.FromSeconds(10),
				clientFactory: httpFactory,
				execute: async context =>
				{
					var post = context.FeedItem;

					var request =
						Http.CreateRequest("PUT", $"{BaseUrl}/{post.Id}")
							.WithBody(new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json"))
							.WithCheck(res =>
								res.IsSuccessStatusCode
									? Task.FromResult(Response.Ok())
									: Task.FromResult(Response.Fail())
							);

					var response = await Http.Send(request, context);
					return response;
				});

			var scenario = ScenarioBuilder
				.CreateScenario(BaseUrl, postRequestStep)
				.WithWarmUpDuration(TimeSpan.FromSeconds(5))
				.WithLoadSimulations(Simulation.InjectPerSec(100, TimeSpan.FromSeconds(10)));

			NBomberRunner
				.RegisterScenarios(scenario)
				.Run();
		}

		public class PostDto
		{
			public int Id { get; set; }
			public string Title { get; set; }
			public string Body { get; set; }
			public int Score { get; set; }
		}
	}
}
