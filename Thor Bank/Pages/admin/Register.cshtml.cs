using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Thor_Bank.Pages.admin
{
    public class RegisterModel : PageModel
    {
        


        private readonly string connectionString;
        public RegisterModel(IConfiguration config)
        { connectionString = config.GetConnectionString("Default"); }


        [BindProperty]
        public Register RegisterInfo { get; set; }



        public void OnGet()
        {


        }

        public string statusMessage { get; set; }
        public IActionResult OnPost()
        {


            // INSERT DATA TO DATABASE 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string query = String.Format("INSERT INTO USERS (Username, Password, Email, Firstname, Lastname, Address, City, Zipcode, State, Country, Phone) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')", RegisterInfo.Username, RegisterInfo.Password, RegisterInfo.Email, RegisterInfo.Firstname, RegisterInfo.Lastname, RegisterInfo.Address, RegisterInfo.City, RegisterInfo.Zipcode, RegisterInfo.State, RegisterInfo.Country, RegisterInfo.Phone);
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                this.statusMessage = "You are now registered";


            }
            return Redirect("/Login");
        }


        public class Register
        {
            [BindProperty]
            public int ID { get; set; }

            [BindProperty]
            public string Username { get; set; }
            [BindProperty]
            public string Password { get; set; }

            [BindProperty]
            public string Email { get; set; }

            [BindProperty]
            public string Firstname { get; set; }

            [BindProperty]
            public string Lastname { get; set; }

            [BindProperty]
            public string Address { get; set; }

            [BindProperty]
            public string City { get; set; }

            [BindProperty]
            public string Zipcode { get; set; }

            [BindProperty]
            public string State { get; set; }


            [BindProperty]
            public string Country { get; set; }

            [BindProperty]
            public string Phone { get; set; }




        }
    }
}