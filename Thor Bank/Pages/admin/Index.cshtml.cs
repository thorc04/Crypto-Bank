using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Thor_Bank.Entity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Thor_Bank.Pages.admin
{
    public class DashboardModel : PageModel
    {

        private readonly string connectionString;
        private readonly IHelper _helper;
        public DashboardModel(IConfiguration config, IHelper helper)
        {
            connectionString = config.GetConnectionString("Default");
            _helper = helper;
        }
        public int Valuetotal = 0;

        public int userstotal = 0;

        public int Walletstotal = 0;

        public int Transactionstotal = 0;

        public int transfertotal = 0;


        [BindProperty]
        public List<WalletTransactions> Transactioninfo { get; set; } = new List<WalletTransactions> { };

        [BindProperty]
        public Wallet Walletinfo { get; set; }

        [BindProperty]
        public List<Wallet> Walletvalue { get; set; } = new List<Wallet> { };

        [BindProperty]
        public List<Wallet> Wallets { get; set; } = new List<Wallet> { };



        [BindProperty]
        public List<Client> uservalue { get; set; } = new List<Client> { };

        public void OnGet()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string Check = String.Format("SELECT COUNT(*) FROM USERS");
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                userstotal = (int)inputcommand.ExecuteScalar();

            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string Check = String.Format("SELECT Value FROM WALLETS");
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                SqlDataReader reader = inputcommand.ExecuteReader();
                while (reader.Read())
                {
                    Walletvalue.Add(new Wallet
                    {
                        Value = reader.GetInt32(0),
                    });
                    Valuetotal += reader.GetInt32(0);
                }

            }
            
            // CALCULATE TOTAL USERS
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string Check = String.Format("SELECT * FROM WALLETS");
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                SqlDataReader reader = inputcommand.ExecuteReader();
                while (reader.Read())
                {
                    Wallets.Add(new Wallet
                    {
                        Ownerid = reader.GetInt32(0),
                    });
                    Walletstotal += 1;
                }

            }
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string Check = String.Format("SELECT * FROM Transactions");
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                SqlDataReader reader = inputcommand.ExecuteReader();
                while (reader.Read())
                {
                    Transactioninfo.Add(new WalletTransactions
                    {
                        TransactionID = reader.GetInt32(0),
                        Sender = reader.GetString(1),
                        Reciptient = reader.GetString(2),
                        value = reader.GetInt32(3),
                        Type = reader.GetString(4),
                        createdDate = reader.GetDateTime(5),
                    });
                    Transactionstotal += 1;
                    transfertotal += reader.GetInt32(3);

                }

            }
        }
    }
}




