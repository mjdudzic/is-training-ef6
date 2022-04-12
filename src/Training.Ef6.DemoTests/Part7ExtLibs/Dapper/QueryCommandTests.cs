using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part7ExtLibs.Dapper
{
	public class QueryCommandTests : TestBase
	{
		public QueryCommandTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task query_example()
		{
			using (var connection =
			       new SqlConnection(ConfigurationManager.ConnectionStrings["StackOverflow2010Context"].ToString()))
			{
				var users = (await connection.QueryAsync<UserDto>(
					"select top(5) * from dbo.Users order by Id desc")).ToList();

				users.Any().Should().BeTrue();
			}
		}

		[Fact]
		public async Task command_example()
		{
			using (var connection =
			       new SqlConnection(ConfigurationManager.ConnectionStrings["StackOverflow2010Context"].ToString()))
			{
				var affected = await connection.ExecuteAsync("update dbo.Users set Reputation=@Reputation where Id=@Id", new { Reputation = 0, Id = 0 });

				affected.Should().Be(1);
			}
		}

	}

	public class UserDto
	{
		public int Id { get; set; }
		public string DisplayName { get; set; }
	}
}