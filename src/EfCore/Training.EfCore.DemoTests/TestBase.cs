using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Training.EfCore.Infrastructure.Models;
using Xunit;
using Xunit.Abstractions;
using EF6 = System.Data.Entity;

namespace Training.EfCore.DemoTests
{
	public abstract class TestBase
	{
		protected ITestOutputHelper Output { get; }
		protected Stopwatch Stopwatch { get; }

		protected IConfiguration TestConfiguration;
		protected StackOverflow2010Context Context;
		protected Training.EfCore.Infrastructure.EfModels.StackOverflow2010Context Ef6Context;

		protected TestBase(ITestOutputHelper outputHelper)
		{
			Output = outputHelper;
			Stopwatch = new Stopwatch();
			TestConfiguration = TestConfig.Config;
			Context = CreateContext();
			Ef6Context = new Infrastructure.EfModels.StackOverflow2010Context(TestConfiguration["ConnectionStrings:StackOverflow2010Context"]);
			Ef6Context.Database.Log = Output.WriteLine;
		}

		private StackOverflow2010Context CreateContext()
		{
			var options = new DbContextOptionsBuilder<StackOverflow2010Context>()
				.LogTo(Output.WriteLine, LogLevel.Information)
				.UseSqlServer(TestConfiguration["ConnectionStrings:DefaultConnection"], opt => opt.CommandTimeout(30))
				.Options;

			return new StackOverflow2010Context(options);
		}
	}
}