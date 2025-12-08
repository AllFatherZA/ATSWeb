using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATSWeb
{
    public partial class Profile : System.Web.UI.Page
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\29422396\Desktop\ATSWeb-master\ATSDb.mdf;Integrated Security=True;Connect Timeout=30";
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadUserProfile();
        }

        private int GetLoggedInUserId()
        {
            // Get data from form controls
            int userId = 0;
            // Try to get the cookie
            HttpCookie userJobCookie = Request.Cookies["UserJobInfo"];

            if (userJobCookie != null && userJobCookie["UserId"] != null)
            {


                if (int.TryParse(userJobCookie["UserId"], out userId))
                {


                }
                else
                {


                }
            }
            else
            {
                lblMessage.Text = "Cookie not found or UserId missing.";
            }
            return userId;  // Replace with actual logic to retrieve the logged-in user's ID.
        }

        private void LoadUserProfile()
        {
            string query = "SELECT email, phone_number FROM Users WHERE user_id = @userId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userId", GetLoggedInUserId());
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        
                        txtEmail.Text = reader["email"].ToString();
                        txtPhone.Text = reader["phone_number"].ToString();
                    }
                }
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // Check if the resume file is uploaded
            if (fileUpload.HasFile)
            {
                // Check if the uploaded file is a PDF
                if (fileUpload.PostedFile.ContentType == "application/pdf")
                {
                    try
                    {
                        // Convert the uploaded resume to binary data
                        byte[] resumeBytes = fileUpload.FileBytes;

                        // Save the binary data to the database for the logged-in user
                        SaveFileToDatabase(resumeBytes);

                        lblMessage.Text = "Resume uploaded successfully.";
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error uploading resume: " + ex.Message;
                    }
                }
                else
                {
                    lblMessage.Text = "Please upload a PDF resume.";
                }
            }
            else
            {
                lblMessage.Text = "Please select a resume to upload.";
            }
        }

        private void SaveFileToDatabase(byte[] resumeData)
        {
            // Assuming the user is logged in and we have their user_id
            int userId = GetLoggedInUserId();  // You should implement this method to get the logged-in user's ID
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("UPDATE USERS SET resume = @Resume WHERE user_id = @UserId", con))
                {
                    cmd.Parameters.AddWithValue("@Resume", resumeData);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string newEmail = txtEmail.Text.Trim();
            string newPhone = txtPhone.Text.Trim();
            string newPassword = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            // Validation for empty fields
            if (string.IsNullOrWhiteSpace(newEmail) || string.IsNullOrWhiteSpace(newPhone) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                lblError.Text = "Please fill in all fields.";
                return;
            }

            // Check if passwords match
            if (newPassword != confirmPassword)
            {
                lblError.Text = "Passwords do not match.";
                return;
            }

            // Update the user profile in the database
            string query = "UPDATE Users SET email = @newEmail, phone_number = @newPhone, password = @newPassword WHERE user_id = @userId";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@userId", GetLoggedInUserId());
                    cmd.Parameters.AddWithValue("@newEmail", newEmail);
                    cmd.Parameters.AddWithValue("@newPhone", newPhone);
                    cmd.Parameters.AddWithValue("@newPassword", newPassword); // You should hash the password before storing it

                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        lblError.Text = "Profile updated successfully!";
                        lblError.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblError.Text = "Error updating profile. Please try again.";
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("candidate.aspx"); // Redirect to the homepage or previous page
        }


    }
}
