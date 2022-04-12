using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Training.Ef6.Infrastructure.DatabaseFirst;
using Xunit;
using Xunit.Abstractions;

namespace Training.Ef6.DemoTests.Part5DataModifying
{
	public class ModifyWithStoredProceduresTests : TestBase
	{
		public ModifyWithStoredProceduresTests(ITestOutputHelper output) : base(output)
		{
			// EF Core road map - https://github.com/dotnet/efcore/issues/245
		}


		[Fact]
		public async Task insert_with_sp()
		{
			var badge = GetTestBadge();
			ContextDbFirst.Badges.Add(badge);

			await ContextDbFirst.SaveChangesAsync();

			ContextDbFirst.Badges.Local.Any().Should().BeTrue();
		}

		[Fact]
		public async Task update_with_sp()
		{
			var badge = await ContextDbFirst.Badges.OrderBy(i => i.Id).FirstAsync();
			badge.Date = DateTime.UtcNow;

			await ContextDbFirst.SaveChangesAsync();

			ContextDbFirst.Badges.Local.Any().Should().BeTrue();
		}

		[Fact]
		public async Task delete_with_sp()
		{
			var badge = GetTestBadge();
			ContextDbFirst.Badges.Add(badge);

			await ContextDbFirst.SaveChangesAsync();

			ContextDbFirst.Badges.Remove(badge);
			await ContextDbFirst.SaveChangesAsync();

			ContextDbFirst.Badges.Local.Any().Should().BeFalse();
		}
	}
}