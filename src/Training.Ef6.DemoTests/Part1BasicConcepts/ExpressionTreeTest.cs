using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part1BasicConcepts
{
	public class ExpressionTreeTest : TestBase
	{
		public ExpressionTreeTest(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void expression_tree_example()
		{
			var postType1 = ContextDbFirst.PostTypes
				.Where(i => i.Type == "Question")
				.OrderBy(i => i.Id)
				.FirstOrDefault();

			var postType2 = ContextDbFirst.PostTypes
				.Where(i => i.Type == "Answer")
				.OrderBy(i => i.Id)
				.FirstOrDefault();

			// Refactor to simple method call
			var postType3 = GetPostType(i => i.Type == "Question");
			var postType4 = GetPostType(i => i.Type == "Answer");

			postType1.Should().NotBeNull();
			postType2.Should().NotBeNull();
			postType3?.Id.Should().Be(postType1?.Id);
			postType4?.Id.Should().Be(postType2?.Id);
		}

		[Fact]
		public async Task expression_tree_low_level_usage()
		{
			var usersCount1A = await GetUsersCountFilteredByA(nameof(User.UpVotes), 1000);
			var usersCount2A = await GetUsersCountFilteredByA(nameof(User.DownVotes), 1000);

			var usersCount1B = await GetUsersCountFilteredByB(nameof(User.UpVotes), 1000);
			var usersCount2B = await GetUsersCountFilteredByB(nameof(User.DownVotes), 1000);

			usersCount1A.Should().Be(usersCount1B);
			usersCount2A.Should().Be(usersCount2B);
		}

		public PostType GetPostType(Func<PostType, bool> filter)
		{
			return ContextDbFirst.PostTypes
				.Where(filter)
				.OrderBy(i => i.Id)
				.FirstOrDefault();
		}

		public Task<int> GetUsersCountFilteredByA(string propertyToFilter, int filterValue)
		{
			switch (propertyToFilter)
			{
				case nameof(User.UpVotes):
					return ContextDbFirst.Users.CountAsync(i => i.UpVotes == filterValue);

				case nameof(User.DownVotes):
					return ContextDbFirst.Users.CountAsync(i => i.DownVotes == filterValue);
				
				default:
					throw new NotSupportedException();
			}
		}

		public Task<int> GetUsersCountFilteredByB(string propertyToFilter, int filterValue)
		{
			var user = Expression.Parameter(typeof(User));
			var memberAccess = Expression.PropertyOrField(user, propertyToFilter);
			var exprRight = Expression.Constant(filterValue);
			var equalExpr = Expression.Equal(memberAccess, exprRight);
			Expression<Func<User, bool>> lambda = Expression.Lambda<Func<User, bool>>(equalExpr, user);

			return ContextDbFirst.Users.CountAsync(lambda);
		}
	}
}