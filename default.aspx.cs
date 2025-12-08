using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ATSWeb
{
    public partial class defaullt : System.Web.UI.Page
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\29422396\Desktop\ATSWeb-master\ATSDb.mdf;Integrated Security=True;Connect Timeout=30";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // 1. Validate that the user entered a username and password.
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                lblError.Text = "Please enter both email and password.";
                return;
            }

            try
            {
                // 2. Connect to the database to retrieve the stored user data.
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT user_id, password,  role FROM USERS WHERE email = @email";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@email", email);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 3. Retrieve the stored password hash,and user role.
                                string storedHash = reader["password"].ToString();
                                string userRole = reader["role"].ToString();
                                int userId = (int)reader["user_id"];

                                string enteredPasswordHash;

                                //check for super user-->leave unhashed password
                                if (email == "admin@gmail.com")
                                {
                                    enteredPasswordHash = password;
                                }
                                else
                                {
                                    // 4. Hash the user-entered password .
                                    enteredPasswordHash = HashPassword(password);
                                }


                                // 5. Compare the newly created hash 
                                if (enteredPasswordHash == storedHash)
                                {
                                    HttpCookie userJobCookie = new HttpCookie("UserJobInfo");
                                    userJobCookie["UserId"] = userId.ToString();
                                    userJobCookie.Expires = DateTime.Now.AddMinutes(10);
                                    Response.Cookies.Add(userJobCookie);

                                    // Redirect the user based on their role.
                                    if (userRole.ToLower() == "administrator")
                                    {
                                        Response.Redirect("AdminDashboard.aspx");
                                    }
                                    else if (userRole.ToLower() == "applicant")
                                    {

                                        Response.Redirect("Candidate.aspx");
                                    }
                                    else
                                    {
                                        // Handle other roles or a default dashboard.
                                        string script = "alert('Unassigned User');";
                                        ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
                                    }

                                    
                                }
                                else
                                {
                                    // Password mismatch.
                                    lblError.Text = "Invalid email or password.";
                                }
                            }
                            else
                            {
                                // User not found.
                                lblError.Text = "Invalid email or password.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = $"An error occurred. Please try again later. +{ex}";
                
            }
        }

        //future update--omitted for demo simplicity
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

    }
}