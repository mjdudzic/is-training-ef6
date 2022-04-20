using System;
using System.Threading;
using System.Threading.Tasks;

namespace Training.Ef6.Infrastructure.DatabaseFirst
{
	public class StackOverflow2010EntitiesReadOnly : StackOverflow2010Entities
	{
		public StackOverflow2010EntitiesReadOnly()
		{
			Configuration.LazyLoadingEnabled = false;
			Configuration.AutoDetectChangesEnabled = false;
			Configuration.ProxyCreationEnabled = false;
		}

		public override int SaveChanges()
		{
			throw new NotSupportedException();
		}

		public override Task<int> SaveChangesAsync()
		{
			throw new NotSupportedException();
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}
    }
}