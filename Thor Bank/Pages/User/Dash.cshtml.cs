using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NBitcoin;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using Thor_Bank.Entity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Thor_Bank.Pages.User
{

    public class DashModel : PageModel
    {

        private readonly string connectionString;
        private readonly IHelper _helper;
        public DashModel(IConfiguration config, IHelper helper)
        {
            connectionString = config.GetConnectionString("Default");
            _helper = helper;
        }
        public int Valuetotal = 0;

        [BindProperty]
        public List<Wallet> Walletvalue { get; set; } = new List<Wallet> { };

        public List<VirtualCard> Virtualcardinfos { get; set; } = new List<VirtualCard> { };

        [BindProperty]
        public List<WalletTransactions> Transactioninfo { get; set; } = new List<WalletTransactions> { };


        

        [BindProperty]
        public List<Wallet> Wallets { get; set; } = new List<Wallet> { };

        [BindProperty]
        public Wallet Walletinfo { get; set; }

        [BindProperty]

        public Client AccountInfo { get; set; }

        public VirtualCard Virtualcardinfo { get; set; }

        public string FalseEdit = "readonly";
        public string StatusMessage { get; set; } = null;


        public static string GenerateRandomAlphanumericString(int length = 36)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }

        public IActionResult OnPost()
        {

            SqlCommand sqlCommand = _helper.SqlExec("CreateWallet");
            sqlCommand.Parameters.AddWithValue("@Ownerid", HttpContext.Session.GetInt32("ID"));
            sqlCommand.Parameters.AddWithValue("@Walletname", Walletinfo.Walletname);
            sqlCommand.Parameters.AddWithValue("@Address", GenerateRandomAlphanumericString());
            sqlCommand.Parameters.AddWithValue("@Type", Walletinfo.Type);
            sqlCommand.Connection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Connection.Close();

            return Redirect("/user/dash");


        }

        public IActionResult OnPostBuy()
        {
            Console.WriteLine("cryptobuy: " + Walletinfo.cryptobuy);
            Console.WriteLine("id: " + HttpContext.Session.GetInt32("ID"));
            Console.WriteLine("name: " + Walletinfo.Walletname);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string Check = String.Format("UPDATE WALLETS SET Value = Value + '{0}' WHERE Ownerid = '{1}' AND Walletname = '{2}'", Walletinfo.cryptobuy, HttpContext.Session.GetInt32("ID"), Walletinfo.Walletname);
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                SqlDataReader reader = inputcommand.ExecuteReader();




            }
            return Redirect("/user/dash");
        }

        public IActionResult OnPostSell()
        {
            Console.WriteLine("sellcrypto: " + Walletinfo.sellcrypto);
            Console.WriteLine("id: " + HttpContext.Session.GetInt32("ID"));
            Console.WriteLine("name: " + Walletinfo.Walletname);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string checkvalue = String.Format("SELECT Value FROM WALLETS WHERE Ownerid = '{0}' AND Walletname = '{1}'", HttpContext.Session.GetInt32("ID"), Walletinfo.Walletname);
                SqlCommand inputcommand = new SqlCommand(checkvalue, connection);
                SqlDataReader reader = inputcommand.ExecuteReader();
                int somevalue = 0;
                while (reader.Read())
                {
                    somevalue = Convert.ToInt32(reader["Value"]);
                }
                reader.Close();

                if (somevalue < Walletinfo.sellcrypto)
                {
                    StatusMessage = "You don't have enough crypto to sell";
                    return Redirect("/user/dash");
                }
                else
                {

                    string valuecheck = String.Format("UPDATE WALLETS SET Value = Value - '{0}' WHERE Ownerid = '{1}' AND Walletname = '{2}'", Walletinfo.sellcrypto, HttpContext.Session.GetInt32("ID"), Walletinfo.Walletname);
                    SqlCommand input = new SqlCommand(valuecheck, connection);
                    input.ExecuteNonQuery();
                    connection.Close();
                }






            }
            return Redirect("/user/dash");
        }

        public IActionResult OnPostSend()
        {
            Console.WriteLine("sendcrypto: " + Walletinfo.sendcrypto);
            Console.WriteLine("id: " + HttpContext.Session.GetInt32("ID"));
            Console.WriteLine("name: " + Walletinfo.Walletname);
            Console.WriteLine("address: " + Walletinfo.Address);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string checkvalue = String.Format("SELECT Value FROM WALLETS WHERE Ownerid = '{0}' AND Address = '{1}'", HttpContext.Session.GetInt32("ID"), Walletinfo.Address);
                SqlCommand inputcommand = new SqlCommand(checkvalue, connection);
                SqlDataReader reader = inputcommand.ExecuteReader();
                int somevalue = 0;
                while (reader.Read())
                {
                    somevalue = Convert.ToInt32(reader["Value"]);
                }
                reader.Close();
                
                if (somevalue < Walletinfo.sendcrypto)
                {
                    StatusMessage = "You don't have enough crypto to send";
                    return Redirect("/user/dash");
                }
                else
                {                    
                    string Check = String.Format("UPDATE WALLETS SET Value = Value - '{0}' WHERE Ownerid = '{1}' AND Address = '{2}'", Walletinfo.sendcrypto, HttpContext.Session.GetInt32("ID"), Walletinfo.Address);
                    SqlCommand commands = new SqlCommand(Check, connection);
                    SqlDataReader read = commands.ExecuteReader();
                    connection.Close();

                    connection.Open();
                    string Checks = String.Format("UPDATE WALLETS SET Value = Value + '{0}' WHERE Address = '{1}'", Walletinfo.sendcrypto, Walletinfo.walletsend);
                    SqlCommand inputcommands = new SqlCommand(Checks, connection);
                    SqlDataReader readers = inputcommands.ExecuteReader();
                    connection.Close();
                    
                    connection.Open();
                    string query = String.Format("INSERT INTO Transactions (Sender, Recipient, value, Type)  VALUES ('{0}', '{1}', '{2}', '{3}')", Walletinfo.Address, Walletinfo.walletsend, Walletinfo.sendcrypto, Walletinfo.Type);
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return Redirect("/user/dash");


        }











        public IActionResult OnPostEdit()
        {
            if (string.IsNullOrWhiteSpace(AccountInfo.NewPassword) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string Check = String.Format("SELECT Password FROM USERS WHERE ID = '{0}'", Convert.ToInt32(Request.Query["id"]));
                    SqlCommand inputcommand = new SqlCommand(Check, connection);
                    SqlDataReader reader = inputcommand.ExecuteReader();

                    while (reader.Read())
                    {
                        if (AccountInfo.OldPassword == reader.GetString(0))
                        {
                            if (AccountInfo.NewPassword == AccountInfo.ConfirmPassword)
                            {
                                reader.Close();
                                string query = String.Format("UPDATE USERS SET Password = '{0}' WHERE ID = '{1}'", AccountInfo.NewPassword, Convert.ToInt32(Request.Query["id"]));
                                SqlCommand command = new SqlCommand(query, connection);
                                command.ExecuteNonQuery();
                                break;
                            }
                            else
                            {
                                StatusMessage = "New Password and Confirm Password do not match";
                                return OnGet();
                            }
                        }
                        else
                        {
                            StatusMessage = "Old password is wrong";
                            return OnGet();
                        }
                    }
                }

            }
            return Redirect("/user/dash");
        }



        public IActionResult OnPostDelete()
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("hello test ");
                Console.WriteLine(Walletinfo.Address);
                Console.WriteLine("OMG FORTNITE");
                connection.Open();
                string Check = String.Format("DELETE FROM WALLETS WHERE Ownerid = '{0}' AND Address = '{1}'", HttpContext.Session.GetInt32("ID"), Walletinfo.Address);
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                inputcommand.ExecuteNonQuery();
            }
            return Redirect("/user/dash");
        }










        public IActionResult OnGet()
        {
            Console.WriteLine(HttpContext.Session.GetInt32("ID"));
            if (HttpContext.Session.GetInt32("ID") == null)
            {
                return Redirect("/login");
            }



            Console.WriteLine("on get");
            string Edit = Request.Query["edit"].ToString().ToLower();
            if (Edit == "true")
            {
                FalseEdit = "";
            }


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string Check = String.Format("SELECT * FROM WALLETS WHERE Ownerid = '{0}'", HttpContext.Session.GetInt32("ID"));
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                SqlDataReader reader = inputcommand.ExecuteReader();
                while (reader.Read())
                {
                    Walletvalue.Add(new Wallet
                    {
                        Ownerid = reader.GetInt32(0),
                        Walletid = reader.GetInt32(1),
                        Walletname = reader.GetString(2),
                        Address = reader.GetString(3),
                        Type = reader.GetString(4),
                        Value = reader.GetInt32(5),


                    });
                    Valuetotal += reader.GetInt32(5);

                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string Check = String.Format("SELECT * FROM VirtualCard WHERE CardOwnerID = '{0}'", HttpContext.Session.GetInt32("ID"));
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                SqlDataReader reader = inputcommand.ExecuteReader();
                while (reader.Read())
                {
                    Virtualcardinfo = new VirtualCard
                    {
                        CardOwnerID = reader.GetInt32(0),
                        Cardnumber = reader.GetString(1),
                        EXP = reader.GetString(2),
                        Nameoncard = reader.GetString(3),
                        CCV = reader.GetInt32(4),
                        createdDate = reader.GetDateTime(5),


                    };
                }
            }


            foreach (var item in Walletvalue)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();
                    string Check = String.Format("SELECT * FROM Transactions WHERE Sender  = '{0}' OR Recipient = '{0}'", item.Address);
                    SqlCommand inputcommand = new SqlCommand(Check, connection);
                    SqlDataReader reader = inputcommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Transactioninfo.Add(new WalletTransactions
                        {
                            Sender = reader.GetString(1),
                            Reciptient = reader.GetString(2),
                            value = reader.GetInt32(3),
                            Type = reader.GetString(4),
                            createdDate = reader.GetDateTime(5),
                        });


                    }
                }
            }

            foreach (var item in Transactioninfo)
            {
                Console.WriteLine(item.value);
            }

            // get user data from Database
            //HttpContext.Session.SetInt32("ID", 1);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();
                string Check = String.Format("SELECT * FROM USERS WHERE ID = '{0}'", HttpContext.Session.GetInt32("ID"));
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                SqlDataReader reader = inputcommand.ExecuteReader();
                while (reader.Read())
                {
                    AccountInfo = new Client
                    {
                        ID = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Email = reader.GetString(3),
                        Firstname = reader.GetString(4),
                        Lastname = reader.GetString(5),
                        Address = reader.GetString(6),
                        City = reader.GetString(7),
                        Zipcode = reader.GetString(8),
                        State = reader.GetString(9),
                        Country = reader.GetString(10),
                        Phone = reader.GetString(11),
                    };
                }
            }
            return Page();




        }


    }
}