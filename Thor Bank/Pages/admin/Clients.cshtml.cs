using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Principal;
using Thor_Bank.Entity;

namespace Thor_Bank.Pages.admin
{
    public class ClientsModel : PageModel
    {
        [BindProperty]
        public Client AccountInfo { get; set; }

        private readonly string connectionString;
        public ClientsModel(IConfiguration config)
        { connectionString = config.GetConnectionString("Default"); }
        [BindProperty]
        public List<Client> Userlist { get; set; } = new List<Client> { };

        public string FalseEdit = "readonly";

        public string StatusMessage { get; set; } = null;

        public IActionResult OnGet()
        {





            Console.WriteLine("on get");
            string Edit = Request.Query["edit"].ToString().ToLower();
            if (Edit == "true")
            {
                FalseEdit = "";
            }


            //GET ALL USERS FROM THE DATABASE IN A BIG LIST
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string Check = String.Format("SELECT * FROM USERS");
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                SqlDataReader reader = inputcommand.ExecuteReader();
                while (reader.Read())
                {
                    Userlist.Add(new Client
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
                        createdDate = reader.GetDateTime(13),
                    });
                }
            }
            return Page();
        }
        public IActionResult OnPost()
        {

            //DELTE USER FROM DATABASE USING WITH BUTTON
            string action = Request.Query["action"].ToString().ToLower();
            if (action == "delete")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string Check = String.Format("DELETE FROM USERS WHERE ID = '{0}'", Convert.ToInt32(Request.Query["id"]));
                    SqlCommand inputcommand = new SqlCommand(Check, connection);
                    inputcommand.ExecuteNonQuery();

                }
            }

            //DELTE USER FROM DATABASE USING WITH BUTTON
            string actions = Request.Query["action"].ToString().ToLower();
            if (actions == "delete")
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string Check = String.Format("DELETE FROM WALLETS WHERE Ownerid = '{0}'", Convert.ToInt32(Request.Query["id"]));
                    SqlCommand inputcommand = new SqlCommand(Check, connection);
                    inputcommand.ExecuteNonQuery();

                }


            }
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
                }
            }
            return Redirect("/admin/Clients");
        }

    }


}









