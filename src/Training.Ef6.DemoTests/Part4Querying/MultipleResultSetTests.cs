using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Training.Ef6.Infrastructure.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part4Querying
{
	public class MultipleResultSetTests : TestBase
	{
		public MultipleResultSetTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task result_with_edmx_manual_update()
		{
			var result1 = ContextDbFirst.GetDictionaries();
			var postTypes = result1.Select(i => i.Type).ToList();

			var result2 = result1.GetNextResult<LinkType>();
			var linkTypes = result2.Select(i => i.Type).ToList();

			var result3 = result2.GetNextResult<VoteType>();
			var voteTypes = result3.Select(i => i.Name).ToList();

			postTypes.Any().Should().BeTrue();
			linkTypes.Any().Should().BeTrue();
			voteTypes.Any().Should().BeTrue();

			await Task.CompletedTask;
		}

		[Fact]
		public async Task result_with_raw_command()
		{
			using (var conn = ContextDbFirst.Database.Connection)
			{
				await conn.OpenAsync();

				var cmd = ContextDbFirst.Database.Connection.CreateCommand();
				cmd.CommandText = "[dbo].[GetUserByReputationWithLastPost]";
				cmd.CommandType = CommandType.StoredProcedure;

				var param = cmd.CreateParameter();
				param.ParameterName = "@MinReputation";
				param.Value = 1047863;
				param.DbType = DbType.Int32;
				param.Direction = ParameterDirection.Input;
				
				cmd.Parameters.Add(param);

				using (var reader = await cmd.ExecuteReaderAsync())
				{
					var users = ((IObjectContextAdapter)ContextDbFirst)
						.ObjectContext
						.Translate<User>(reader, "Users", MergeOption.AppendOnly)
						.ToList();

					await reader.NextResultAsync();

					var posts = ((IObjectContextAdapter)ContextDbFirst)
						.ObjectContext
						.Translate<Post>(reader, "Posts", MergeOption.AppendOnly)
						.ToList();
				}
			}
		}

		[Fact]
		public async Task result_with_extension_method()
		{
			var result = await ContextDbFirst
				.MultipleResults(
					"[dbo].[GetUserByReputationWithLastPost]",
					new SqlParameter
					{
						ParameterName = "@MinReputation",
						Value = 1047863,
						DbType = DbType.Int32,
						Direction = ParameterDirection.Input
					})
				.With<User>()
				.With<Post>()
				.Execute();

			result.Count.Should().BeGreaterThan(0);
		}
	}
}