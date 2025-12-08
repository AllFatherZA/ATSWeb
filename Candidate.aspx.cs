using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATSWeb
{
    public partial class Candidate : System.Web.UI.Page
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\29422396\Desktop\ATSWeb-master\ATSDb.mdf;Integrated Security=True;Connect Timeout=30";

        // Use a property to wrap ViewState access
        private int _jobId
        {
            get
            {
                // Retrieve the value from ViewState or default to 0
                return ViewState["JobId"] != null ? (int)ViewState["JobId"] : 0;
            }
            set
            {
                // Store the value in ViewState
                ViewState["JobId"] = value;
            }
        }

        private int _applicationId
        {
            get
            {
                // Retrieve the value from ViewState or default to 0
                return ViewState["ApplicationId"] != null ? (int)ViewState["ApplicationId"] : 0;
            }
            set
            {
                // Store the value in ViewState
                ViewState["ApplicationId"] = value;
            }
        }

        private int _userId
        {
            get
            {
                // Retrieve the value from ViewState or default to 0
                return ViewState["userId"] != null ? (int)ViewState["userId"] : 0;
            }
            set
            {
                // Store the value in ViewState
                ViewState["userId"] = value;
            }
        }



        private bool _applicationsview
        {
            get
            {
                // Retrieve the value from ViewState or default to 0
                return ViewState["ApplicationView"] != null ? (bool)ViewState["ApplicationView"] : false;
            }
            set
            {
                // Store the value in ViewState
                ViewState["ApplicationView"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (_applicationsview)
            {
                LoadAppliedJobsData(_userId);
                
                
            }
            else
            {
                LoadApplicationsData();
            }
            
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                // Get data from form controls
                int userId=GetLoggedInUserId();


                DateTime dateApplied = DateTime.Now;

                // Validate input
                if (string.IsNullOrWhiteSpace(lblMessage.Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('NO JOB SELECTED');", true);
                    return;
                }
               
                // SQL query to insert a new application
                string query = "INSERT INTO APPLICATION (user_id, job_id, date_applied, application_status, notes) " +
                               "VALUES (@userId, @jobId, @dateApplied, @status, @notes)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@jobId", _jobId);
                        command.Parameters.AddWithValue("@dateApplied", dateApplied);
                        command.Parameters.AddWithValue("@status", "pending");
                        command.Parameters.AddWithValue("@notes", "pending");

                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            lblMessage.Text = $"Succesfully applied for the job {_jobId}";


                        }
                        else
                        {
                            
                        }
                            
                    }
                }
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"Succesfully applied for the jobid:{_jobId}", true);
            }
            catch (SqlException ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"{ex.ToString()}", true);
            }
           
        }

        private void LoadApplicationsData()
        {
            string query = "SELECT job_id, title, description, location, status FROM JOB";
            SqlDataAdapter da = new SqlDataAdapter(query, connectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.DataBind();
            btnRemove.Enabled = false;
            lblMessage.Text = "Available Job positions";
        }

        protected void dataGridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ensure there is at least one row selected
            if (dataGridView1.SelectedIndex >= 0)
            {

                if (!IsPostBack&&_applicationsview)
                {
                    LoadAppliedJobsData(_userId);
                }
                GridViewRow selectedRow = dataGridView1.SelectedRow;
                // When viewing applications, the DataKeys collection (which includes application_id) is reliable
                if (_applicationsview)
                {
                    if (int.TryParse(selectedRow.Cells[1].Text, out int applicationId))
                    {
                        _applicationId = applicationId;
                        lblMessage.Text = $"Selected Application ID: {_applicationId}";
                       

                    }
                    


                }
                else
                {
                    if (int.TryParse(selectedRow.Cells[1].Text, out int indexnumber))
                    {
                        _jobId = indexnumber;
                        lblMessage.Text = indexnumber.ToString();
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Converted Job id:{_jobId}');", true);


                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Couldnt convert job id');", true);
                    }

                    lblMessage.Text = _jobId.ToString();
                }

            }
        }

        protected void btnApplications_Click(object sender, EventArgs e)
        {
            int userId = 0;
            HttpCookie userJobCookie = Request.Cookies["UserJobInfo"];

            if (userJobCookie != null && userJobCookie["UserId"] != null && int.TryParse(userJobCookie["UserId"], out userId))
            {
                // Call the new method to load the user's applied jobs
                _applicationsview = true;
                _userId = userId;
                LoadAppliedJobsData(userId);
                btnBack.Enabled = true;
                btnRemove.Enabled = true;
                btnApplications.Enabled=false;
                btnApply.Enabled = true;
            }
            else
            {
                // Handle case where user is not logged in or cookie is missing
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('User information missing. Cannot load applications.');", true);
                // Optionally reload the available jobs view
                LoadApplicationsData();
            }
        }
        private void LoadAppliedJobsData(int userId)
        {
            lblMessage.Text = "Your Submitted Applications";

            // Join APPLICATION and JOB tables to get job details and application status
            string query = @"
                    SELECT 
                        A.application_id, 
                        J.job_id,        -- <<< THE CRITICAL ADDITION: Ensures 'job_id' is in the DataTable
                        J.title, 
                        J.location, 
                        A.date_applied, 
                        A.application_status 
                    FROM 
                        APPLICATION A
                    INNER JOIN 
                        JOB J ON A.job_id = J.job_id
                    WHERE 
                        A.user_id = @userId";

            SqlDataAdapter da = new SqlDataAdapter(query, connectionString);
            da.SelectCommand.Parameters.AddWithValue("@userId", userId);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.DataBind();
            

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



        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (!_applicationsview) {
                Response.Redirect("default.aspx");
            }
            else
            {
                LoadApplicationsData();
                btnBack.Enabled = true;
                btnRemove.Enabled = false;
                _applicationsview = false;
                btnApply.Enabled = true;
                btnApplications.Enabled = true;
            }
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            // Define the DELETE query to remove the application
            string query = @"
            DELETE FROM APPLICATION
            WHERE application_id = @applicationId";
            if (_applicationId!=0)
            {
                // Execute the query to remove the application
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@applicationId", _applicationId);
                        cmd.ExecuteNonQuery();
                    }
                }
                LoadAppliedJobsData(_userId);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "Select an application", true);
            }
        }

        protected void btnProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("Profile.aspx");
        }
    }
}