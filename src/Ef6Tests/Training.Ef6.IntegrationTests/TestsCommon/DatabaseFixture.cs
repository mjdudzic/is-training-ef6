using System;
using System.Configuration;
using System.IO;
using DbUp;
using Training.Ef6.Infrastructure.DatabaseFirst;

namespace Training.Ef6.IntegrationTests.TestsCommon
{
	public class DatabaseFixture : IDisposable
	{
		public StackOverflow2010Entities Context { get; private set; }
		
		public DatabaseFixture()
		{
			InitTestDatabase();

			Context = new StackOverflow2010Entities();
			Context.Database.BeginTransaction();
		}
		
		public void Dispose()
		{
			Context.Database.CurrentTransaction?.Rollback();
			Context?.Dispose();
			Context = null;
		}

		private static void InitTestDatabase()
		{
			var connectionString = ConfigurationManager.ConnectionStrings["StackOverflow2010Simple"].ConnectionString;

			EnsureDatabase.For.SqlDatabase(connectionString);

			var result =
				DeployChanges.To
					.SqlDatabase(connectionString)
					.WithScriptsFromFileSystem(Path.Combine(Directory.GetCurrentDirectory(), "DbScripts"))
					.LogToConsole()
					.Build()
					.PerformUpgrade();

			if (!result.Successful)
				throw result.Error;
		}
	}
}