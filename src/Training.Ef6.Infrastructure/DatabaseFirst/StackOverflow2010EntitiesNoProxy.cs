namespace Training.Ef6.Infrastructure.DatabaseFirst
{
	public class StackOverflow2010EntitiesNoProxy : StackOverflow2010Entities
	{
		public StackOverflow2010EntitiesNoProxy()
		{
			Configuration.ProxyCreationEnabled = false;
		}
	}
}