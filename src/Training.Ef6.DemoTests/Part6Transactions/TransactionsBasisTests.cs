using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part6Transactions
{
	public class TransactionsBasisTests : TestBase
	{
		public TransactionsBasisTests(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public async Task implicit_transaction()
		{
			var post = await ContextDbFirst
				.Posts
				.Where(i => i.CommentCount > 0)
				.OrderByDescending(i => i.Id).FirstAsync();

			post.Score += 1;

			post.User.Reputation += 1;

			await ContextDbFirst.SaveChangesAsync();
		}

		[Fact]
		public async Task explicit_transaction()
		{
			using (var transaction = ContextDbFirst.Database.BeginTransaction())
			{
				try
				{
					var post = await ContextDbFirst
						.Posts
						.OrderByDescending(i => i.Id).FirstAsync();

					var user = GetTestUsers(1).First();
					ContextDbFirst.Users.Add(user);

					await ContextDbFirst.SaveChangesAsync();

					post.OwnerUserId = user.Id;
					await ContextDbFirst.SaveChangesAsync();

					//throw new Exception();

					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		[Fact]
		public async Task explicit_transaction_with_isolation_level_defined()
		{
			using (var transaction = ContextDbFirst.Database.BeginTransaction(IsolationLevel.Serializable))
			{
				try
				{
					var post = await ContextDbFirst
						.Posts
						.OrderByDescending(i => i.Id).FirstAsync();

					var user = GetTestUsers(1).First();
					ContextDbFirst.Users.Add(user);

					await ContextDbFirst.SaveChangesAsync();

					post.OwnerUserId = user.Id;
					await ContextDbFirst.SaveChangesAsync();

					throw new Exception();

					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();
					throw;
				}
			}
		}
	}
}