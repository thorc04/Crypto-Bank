using Microsoft.AspNetCore.Http.Extensions;
using System.Data.SqlClient;
using System.Data;

namespace Thor_Bank.Entity
{
    public class Helper : IHelper
    {
        private readonly string connectionString;
        public Helper(IConfiguration config)
        {
            connectionString = config.GetConnectionString("Default");
        }


        public SqlCommand SqlExec(string sp)
        {
            SqlCommand sqlCommand = new SqlCommand(sp, new SqlConnection(connectionString));
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return sqlCommand;
        }
    }
}
