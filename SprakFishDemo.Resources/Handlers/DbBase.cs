using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SparkFishDemo.Resources.DB;
using System.Data;

namespace SparkFishDemo.Resources.Handlers
{
  public class DbBase
  {
    public SparkFishDemoContext _db = new SparkFishDemoContext();

    /// <summary>
    /// Generic wrapper for executing query params
    /// </summary>
    /// <param name="paramaters"></param>
    /// <param name="sql"></param>
    /// <param name="commandType"></param>
    /// <returns></returns>
    public async Task<DataTable> ExecuteSqlQuery(SqlParameter[] paramaters, string sql, CommandType commandType = CommandType.StoredProcedure)
    {
      var recordsDt = new DataTable();

      using (var command = _db.Database.GetDbConnection().CreateCommand())
      {
        command.CommandType = commandType;

        if (paramaters != null)
        {
          command.Parameters.AddRange(paramaters);
        }

        command.CommandText = sql;
        _db.Database.OpenConnection();

        var getRows = await command.ExecuteReaderAsync();
        recordsDt.Load(getRows);
        return recordsDt;
      }
    }
  }
}
