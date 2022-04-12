using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Training.Ef6.Infrastructure.Extensions
{
	public static class DbContextExtensions
	{
		public static MultipleResultSetWrapper MultipleResults(this DbContext db, string storedProcedure)
		{
			return new MultipleResultSetWrapper(db, storedProcedure);
		}

		public static MultipleResultSetWrapper MultipleResults(this DbContext db, string storedProcedure, params DbParameter[] parameters)
		{
			return new MultipleResultSetWrapper(db, storedProcedure, parameters);
		}

		public class MultipleResultSetWrapper
		{
			private readonly DbContext _db;
			private readonly string _storedProcedure;
			public List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>> ResultSets;
			public List<DbParameter> Parameters;

			public MultipleResultSetWrapper(DbContext db, string storedProcedure)
			{
				_db = db;
				_storedProcedure = storedProcedure;
				ResultSets = new List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>>();
				Parameters = new List<DbParameter>();
			}

			public MultipleResultSetWrapper(DbContext db, string storedProcedure, params DbParameter[] parameters)
			{
				_db = db;
				_storedProcedure = storedProcedure;
				ResultSets = new List<Func<IObjectContextAdapter, DbDataReader, IEnumerable>>();
				Parameters = parameters.ToList();
			}

			public MultipleResultSetWrapper With<TResult>()
			{
				ResultSets.Add((adapter, reader) => adapter
					.ObjectContext
					.Translate<TResult>(reader)
					.ToList());

				return this;
			}

			public async Task<List<IEnumerable>> Execute()
			{
				var results = new List<IEnumerable>();

				using (var connection = _db.Database.Connection)
				{
					await connection.OpenAsync();
					var command = connection.CreateCommand();
					command.CommandText = _storedProcedure;
					command.CommandType = CommandType.StoredProcedure;

					Parameters.ForEach(p => command.Parameters.Add(p));

					using (var reader = await command.ExecuteReaderAsync())
					{
						var adapter = ((IObjectContextAdapter)_db);
						foreach (var resultSet in ResultSets)
						{
							results.Add(resultSet(adapter, reader));
							await reader.NextResultAsync();
						}
					}

					return results;
				}
			}
		}

		public class StoredProcedureIntParam
		{
			public string Name { get; set; }
			public int Value { get; set; }
		}
    }
}