using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System;
using System.Web;
using Thor_Bank.Entity;

namespace Thor_Bank.Pages
{
    public class LoginModel : PageModel
    {
        private readonly string connectionString;
        public LoginModel(IConfiguration config)
        { connectionString = config.GetConnectionString("Default"); }


        [BindProperty]
        public Client Client { get; set; }
        public string Check { get; set; }


        public void OnGet()
        {


        }

        public string statusMessage { get; set; }


        public IActionResult OnPost()
        {
            int ID;
            string Role;
            string Password;


            Client AccountInfo;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string Check = String.Format("SELECT ID, Password, Role FROM USERS WHERE Username = '{0}'", Client.Username);
                SqlCommand inputcommand = new SqlCommand(Check, connection);
                using (SqlDataReader reader = inputcommand.ExecuteReader())
                {
                    if (reader.HasRows == false)
                    {
                        this.statusMessage = "WRONG USERNAME OR PASSWORD";

                    }
                    else
                    {

                        while (reader.Read())
                        {
                            ID = reader.GetInt32(0);
                            Password = reader.GetString(1);
                            Role = reader.GetString(2);
                            if (Password == Client.Password)
                            {
                                HttpContext.Session.SetString("Role", Role);
                                HttpContext.Session.SetInt32("ID", ID);
                                return RedirectToPage("/Index");

                            }
                            else
                            {
                                this.statusMessage = "WRONG USERNAME OR PASSWORD";
                            }
                        }
                    }

                    if (true)
                    {

                    }
                }
            }

            return Page();

        }
    }

    public class Accounts
    {
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }


    }
}
