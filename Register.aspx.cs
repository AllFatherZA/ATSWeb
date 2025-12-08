using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATSWeb
{
    public partial class Register : System.Web.UI.Page
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\29422396\Desktop\ATSWeb-master\ATSDb.mdf;Integrated Security=True;Connect Timeout=30";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //Gender
            string gender = "";

            if (RadMale.Checked)
            {
                gender = "Male";
            }
            else if (RadFemale.Checked)
            {
                gender = "Female";
            }
            else
            {
                // Handle the case where neither is selected (optional validation)
                 lblError.Text = "Please select a gender.";
                return;
            }

            // 1. Validate user input
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                lblError.Text = "Please fill in all fields.";
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                lblError.Text = "Passwords do not match.";
                return;
            }

            // 2. Generate a salt and hash the password
            string salt = GenerateSalt();
            string passwordHash = HashPassword(txtPassword.Text);

            // 3. Insert the new user into the database
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "INSERT INTO USERS (first_name, last_name, email, password,role,gender,phone_number) " +
                                   "VALUES (@first_name, @last_name, @email, @password, @role,@gender,@phone_number)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@first_name", txtFirstName.Text.Trim());
                        cmd.Parameters.AddWithValue("@last_name", txtLastName.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", passwordHash);
                        cmd.Parameters.AddWithValue("@role", "APPLICANT");
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@phone_number", txtPhone.Text.Trim());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            lblError.Text = "Registration successful!";
                            ClearForm();
                            lblError.ForeColor = System.Drawing.Color.Green;
                            
                        }
                        else
                        {
                            lblError.Text = "Registration failed. Please try again.";
                            lblError.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "An error occurred: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16]; // 16 bytes = 128 bits
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        private void ClearForm()
        {
            txtFirstName.Text="";
            txtLastName.Text="";
            txtEmail.Text="";
            txtPassword.Text="";
            txtConfirmPassword.Text="";
            txtPhone.Text="";
            RadFemale.Checked = false;
            RadMale.Checked= false;
        }

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