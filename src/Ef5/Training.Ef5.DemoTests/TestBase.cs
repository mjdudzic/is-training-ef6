using System;
using System.Diagnostics;
using Training.Ef5.Infrastructure;
using Xunit.Abstractions;

namespace Training.Ef5.DemoTests
{
	public abstract class TestBase
	{
		protected ITestOutputHelper TestOutput { get; }
		protected StackOverflow2010Entities ContextDbFirst { get; }
		protected Stopwatch Stopwatch { get; }
		protected Random Random { get; }

		protected TestBase(ITestOutputHelper testOutput)
		{
			TestOutput = testOutput;

			ContextDbFirst = new StackOverflow2010Entities();
			Stopwatch = new Stopwatch();
			Random = new Random();
		}

		
	}
}