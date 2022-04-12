using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
using Bogus;
using EFCache;
using HibernatingRhinos.Profiler.Appender.EntityFramework;
using Training.Ef6.DemoTests.Common;
using Training.Ef6.Infrastructure.CodeFirst;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit.Abstractions;
using Post = Training.Ef6.Infrastructure.DatabaseFirst.Post;
using PostType = Training.Ef6.Infrastructure.DatabaseFirst.PostType;
using User = Training.Ef6.Infrastructure.DatabaseFirst.User;

namespace Training.Ef6.DemoTests
{
	public abstract class TestBase
	{
		//internal static readonly InMemoryCache Cache = new InMemoryCache();

		protected ITestOutputHelper TestOutput { get; }
		protected StackOverflow2010Entities ContextDbFirst { get; }
		protected StackOverflow2010Context ContextCodeFirst { get; }
		protected StackOverflow2010EntitiesNoProxy ContextDbFirstNoProxy { get; }
		protected Stopwatch Stopwatch { get; }
		protected Random Random { get; }

		protected TestBase(ITestOutputHelper testOutput)
		{
			EntityFrameworkProfiler.Initialize();
			//EntityFrameworkCache.Initialize(Cache);

			TestOutput = testOutput;

			ContextDbFirst = new StackOverflow2010Entities();
			ContextCodeFirst = new StackOverflow2010Context();
			ContextDbFirstNoProxy = new StackOverflow2010EntitiesNoProxy();

			Stopwatch = new Stopwatch();
			Random = new Random();

			ContextDbFirst.Database.CommandTimeout = 180;
			ContextDbFirst.Database.Log = TestOutput.WriteLine;
			ContextDbFirstNoProxy.Database.Log = TestOutput.WriteLine;
		}

		protected void LogTrackedEntities()
		{
			var tracker = ContextDbFirst.ChangeTracker.Entries();
			foreach (var t in tracker)
			{
				TestOutput.WriteLine($"{t.Entity}, {t.State}");
			}
		}

		protected IMapper GetUserToViewModelMapper()
		{
			var config = new MapperConfiguration(cfg =>
				cfg.CreateMap<Infrastructure.DatabaseFirst.User, UserViewModel>());

			return config.CreateMapper();
		}

		protected Post GetTestPost()
		{
			var postFaker = new Faker<Post>()
				.RuleFor(p => p.Title, f => f.Hacker.Phrase())
				.RuleFor(p => p.Body, f => f.Lorem.Sentence())
				.RuleFor(p => p.CreationDate, f => f.Date.Recent())
				.RuleFor(p => p.LastActivityDate, f => f.Date.Recent())
				.RuleFor(p => p.PostTypeId, f => f.Random.Int(1, 8))
				.RuleFor(p => p.Tags, f => "test")
				.RuleFor(p => p.Score, f => f.Random.Int(100, 10000))
				.RuleFor(p => p.ViewCount, f => f.Random.Int(100, 10000));

			return postFaker.Generate();
		}

		protected List<User> GetTestUsers(int count = 10)
		{
			var faker = new Faker<User>()
				.RuleFor(p => p.DisplayName, f => f.Name.FullName())
				.RuleFor(p => p.CreationDate, f => f.Date.Recent())
				.RuleFor(p => p.LastAccessDate, f => f.Date.Recent())
				.RuleFor(p => p.Views, f => f.Random.Int(100, 10000))
				.RuleFor(p => p.UpVotes, f => f.Random.Int(100, 10000))
				.RuleFor(p => p.DownVotes, f => f.Random.Int(100, 10000))
				.RuleFor(p => p.Reputation, f => f.Random.Int(100, 10000));

			return faker.Generate(count);
		}

		protected PostType GetTestPostType()
		{
			var faker = new Faker<PostType>()
				.RuleFor(p => p.Type, f => f.Lorem.Word());

			return faker.Generate();
		}

		protected Badge GetTestBadge()
		{
			var faker = new Faker<Badge>()
				.RuleFor(p => p.Name, f => f.Lorem.Word())
				.RuleFor(i => i.Date, f => f.Date.Recent())
				.RuleFor(i => i.UserId, f => f.Random.Number(1, 5));

			return faker.Generate();
		}
	}
}