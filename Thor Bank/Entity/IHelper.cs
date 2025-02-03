using System.Data.SqlClient;

namespace Thor_Bank.Entity
{
    public interface IHelper
    {
        public SqlCommand SqlExec(string sp);
    }
}