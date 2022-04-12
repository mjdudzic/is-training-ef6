using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part1BasicConcepts
{
	public class ProxyTest : TestBase
	{
		public ProxyTest(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task entity_proxy()
		{
			var postTypeFromDb = await ContextDbFirst.PostTypes.FirstAsync();
			var postTypeFromCode = new PostType();

			var postTypeFromDbType = postTypeFromDb.GetType().Name;
			var postTypeFromCodeType = postTypeFromCode.GetType().Name;
			
			TestOutput.WriteLine(postTypeFromDbType);
			TestOutput.WriteLine(postTypeFromCodeType);

			//TestOutput.WriteLine(ObjectContext.GetObjectType(postTypeFromDb.GetType()).Name);

			postTypeFromDbType.Should().NotBe(postTypeFromCodeType);
		}

		[Fact]
		public async Task entity_without_proxy_with_code_first_approach()
		{
			var postFromDb = await ContextCodeFirst.Posts.Where(i => i.Comments.Any()).FirstAsync();
			var comments = postFromDb.Comments;

			TestOutput.WriteLine(postFromDb.GetType().Name);

			comments.Any().Should().BeFalse();
		}

		[Fact]
		public async Task entity_without_proxy_with_db_first_approach()
		{
			var postFromDb = await ContextDbFirstNoProxy.Posts.Where(i => i.Comments.Any()).FirstAsync();
			var comments = postFromDb.Comments;

			TestOutput.WriteLine(postFromDb.GetType().Name);

			comments.Any().Should().BeFalse();
		}

		[Fact]
		public async Task create_entity_with_or_without_proxy()
		{
			var user1 = await ContextDbFirstNoProxy.Users.FirstAsync();
			var user2 = await ContextDbFirstNoProxy.Users.FirstAsync(i => i.Id > user1.Id);

			var postNoProxy = new Post();
			var postWithProxy = ContextDbFirst.Posts.Create();

			TestOutput.WriteLine(postNoProxy.GetType().Name);
			TestOutput.WriteLine(postWithProxy.GetType().Name);

			postNoProxy.OwnerUserId = user1.Id;
			postWithProxy.OwnerUserId = user2.Id;

			ContextDbFirst.Posts.Add(postNoProxy);
			ContextDbFirst.Posts.Add(postWithProxy);

			var userName1 = postWithProxy.User?.DisplayName;
			var userName2 = postNoProxy.User?.DisplayName;

			userName1.Should().NotBeNullOrWhiteSpace();
			userName2.Should().BeNullOrWhiteSpace();
		}

		[Fact]
		public async Task entity_proxy_issue()
		{
			var postFromDb = await ContextDbFirst.Posts.FirstAsync();
			var postFromCode = await ContextCodeFirst.Posts.FirstAsync();

			var postFromCodeSerialized = JsonConvert.SerializeObject(postFromCode);
			postFromCodeSerialized.Should().NotBeNullOrWhiteSpace();

			FluentActions.Invoking(() => JsonConvert.SerializeObject(postFromDb))
				.Should()
				.Throw<JsonSerializationException>();
		}
	}
}