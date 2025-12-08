using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATSWeb
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\29422396\Desktop\ATSWeb-master\ATSDb.mdf;Integrated Security=True;Connect Timeout=30";
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

        private int _applicantUserId
        {
            get
            {
                // Retrieve the value from ViewState or default to 0
                return ViewState["ApplicantUserId"] != null ? (int)ViewState["ApplicantUserId"] : 0;
            }
            set
            {
                // Store the value in ViewState
                ViewState["ApplicantUserId"] = value;
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
                // Retrieve the value from ViewState or default to false
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
            if (!IsPostBack)
            {
                LoadApplicationsData();
                ToggleButtonsForCandidateView(false);
                

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // 1. Basic Input Validation
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtLocation.Text) ||
                string.IsNullOrWhiteSpace(cmbStatus.SelectedValue))
            {
                lblMessage.Text = "Please fill in all required fields.";
                return;
            }

            try
            {
                // 2. Connect to the database
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    // 3. Define the SQL query
                    string query = "INSERT INTO JOB (title, description, department, location, status, hiring_manager_id, date_posted)" +
                                   "VALUES (@title, @description, @department, @location, @status, @hiring_manager_id, @date_posted)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // 4. Add parameters from form controls
                        cmd.Parameters.AddWithValue("@title", txtTitle.Text.Trim());
                        cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                        cmd.Parameters.AddWithValue("@department", cmbDepartment.SelectedValue);
                        cmd.Parameters.AddWithValue("@location", txtLocation.Text.Trim());
                        cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedValue); // Default status for a new posting
                        cmd.Parameters.AddWithValue("@hiring_manager_id", GetHiringManagerId()); // You need a method to get the current user's ID
                        cmd.Parameters.AddWithValue("@date_posted", DateTime.Now);

                        // 5. Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            lblMessage.Text = "Job posting added successfully!";
                            ClearForm();
                            LoadApplicationsData();
                        }
                        else
                        {
                            lblMessage.Text = "Failed to add job posting. Please try again.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"An error occurred: {ex.Message}";
            }
        }

        protected void btnDisplayCandidates_Click(object sender, EventArgs e)
        {




            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Converted Job ID: {lblMessage.Text}');", true);

            try
            {

                // Call the method to load candidates for the selected job
                LoadCandidatesForJob(_jobId);


                // Enable/Disable relevant buttons
                ToggleButtonsForCandidateView(true);


            }
            catch (Exception ex)
            {
                lblMessage.Text = $"An unexpected error occurred: {ex.Message}";
            }
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            UpdateApplicationStatus("Accepted");
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            UpdateApplicationStatus("Rejected");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (_applicationsview==false)
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                LoadApplicationsData();
                ToggleButtonsForCandidateView(false);

            }
        }

        private void ToggleButtonsForCandidateView(bool enable)
        {
           
            btnAdd.Enabled = !enable;
            btnRemove.Enabled = !enable;
            btnUpdate.Enabled = !enable;
            btnDisplayCandidates.Enabled = !enable;
            btnAccept.Enabled = enable;
            btnReject.Enabled = enable;
            _applicationsview=enable;
    
            

        }

        private void LoadCandidatesForJob(int jobId)
        {
            
            string query = @"
                SELECT A.user_id, U.email, A.date_applied, A.application_status
                FROM APPLICATION AS A
                INNER JOIN Users AS U ON A.user_id = U.user_id
                WHERE A.job_id = @jobId
                ORDER BY A.date_applied DESC";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@jobId", jobId);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.DataBind();
                }
            }
           
        }

        protected void UpdateApplicationStatus(string newStatus)
        {

            try
            {


                // SQL query to update the application status
                string query = @"
                UPDATE APPLICATION 
                SET application_status = @status 
                WHERE user_id = @user_id AND job_id = @job_id";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Add parameters safely
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@user_id", _applicantUserId);
                        cmd.Parameters.AddWithValue("@job_id", _jobId);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            lblMessage.Text = $"Application status updated to '{newStatus}' successfully.";
                            LoadCandidatesForJob(_jobId); // use parsed jobId here
                        }
                        else
                        {
                            lblMessage.Text = "No matching application found to update.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }

        }


        private void LoadApplicationsData()
        {
            string query = "SELECT job_id, title, description, location,department, status FROM JOB";
            SqlDataAdapter da = new SqlDataAdapter(query, connectionString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.DataBind();
            cmbDepartment.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            
        }

        private void ClearForm()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtLocation.Text = "";
            cmbDepartment.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
        }

        private int GetHiringManagerId()
        {
            // Get the current hiring manager's ID (dummy example for now)
            return 8; // You need to implement this according to your session or login system
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {

            // Ensure a row is selected
            if (dataGridView1.SelectedIndex != -1)  // Check if a row is selected
            {
                
 
                // Queries: delete applications first, then the job
                string deleteApplicationsQuery = "DELETE FROM APPLICATION WHERE job_id=@job_id";
                string deleteJobQuery = "DELETE FROM JOB WHERE job_id=@job_id";

                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Converted Job ID: {_jobId}');", true);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand deleteApplicationsCmd = new SqlCommand(deleteApplicationsQuery, connection);
                    SqlCommand deleteJobCmd = new SqlCommand(deleteJobQuery, connection);

                    // Add the parameter to both commands
                    deleteApplicationsCmd.Parameters.AddWithValue("@job_id", _jobId);
                    deleteJobCmd.Parameters.AddWithValue("@job_id", _jobId);

                    try
                    {
                        connection.Open();

                        // Delete applications first
                        deleteApplicationsCmd.ExecuteNonQuery();

                        // Then delete the job
                        int rowsAffected = deleteJobCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            lblMessage.Text = "Job and related applications deleted successfully.";
                            lblMessage.ForeColor = System.Drawing.Color.Green;

                            // Refresh GridView
                            LoadApplicationsData();
                        }
                        else
                        {
                            lblMessage.Text = "No matching job found to delete.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error: " + ex.Message;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            else
            {
                lblMessage.Text = "Please select a job to delete.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedIndex != -1)  // Check if a row is selected
            {


                try
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Converted Job ID: {_jobId}');", true);

                    // Construct SQL query for update
                    string query = "UPDATE JOB SET title=@Title, description=@Description, department=@Department, location=@Location, status=@Status, hiring_manager_id=@HiringManagerId WHERE job_id=@_jobid";

                    // Open connection to the database
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);

                        // Add parameters for updated data
                        command.Parameters.AddWithValue("@_jobid", _jobId);
                        command.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
                        command.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                        command.Parameters.AddWithValue("@Department", cmbDepartment.SelectedValue);
                        command.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());
                        command.Parameters.AddWithValue("@Status", cmbStatus.SelectedValue);
                        command.Parameters.AddWithValue("@HiringManagerId", GetHiringManagerId());  // Ensure this function is implemented

                        // Open connection and execute the command
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            lblMessage.Text = "Job updated successfully.";
                            lblMessage.ForeColor = System.Drawing.Color.Green;

                            // Reload GridView data to show updated job details
                            LoadApplicationsData();
                        }
                        else
                        {
                            lblMessage.Text = "No matching job found to update. Please try again.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Catch any exceptions and display the error
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblMessage.Text = "Please select a job to update.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }


        }

        protected void dataGridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int userId = 0;
            // Ensure there is at least one row selected
            if (dataGridView1.SelectedIndex >= 0)
            {
                HttpCookie userJobCookie = Request.Cookies["UserJobInfo"];

                if (userJobCookie != null && userJobCookie["UserId"] != null)
                {


                    if (int.TryParse(userJobCookie["UserId"], out userId))
                    {

                        _userId = userId;
                    }
                    else
                    {
                        lblMessage.Text = "Error converting coookie User Id";

                    }
                }
                else
                {
                    lblMessage.Text = "Cookie not found or UserId missing.";
                }

                GridViewRow selectedRow = dataGridView1.SelectedRow;
                // When viewing applications, the DataKeys collection (which includes application_id) is reliable
                if (_applicationsview==true)
                {
                    if (int.TryParse(selectedRow.Cells[1].Text, out int applicantUserId))
                    {
                        _applicantUserId = applicantUserId;
                        lblMessage.Text = $"Selected Applicant ID: {_applicantUserId}";


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

                    lblMessage.Text = $"Selected Job ID: {_jobId}";

                    txtTitle.Text = selectedRow.Cells[2].Text;  // Assuming 'Title' is in the second column
                    txtDescription.Text = selectedRow.Cells[3].Text;  // Assuming 'Description' is in the third column
                    txtLocation.Text = selectedRow.Cells[4].Text;  // Assuming 'Location' is in the fourth column
                                                                   // Get the department value (assuming it's in the 5th cell of the GridView row)
                    string departmentValue = selectedRow.Cells[5].Text;

                    // Try to set the SelectedValue in the DropDownList
                    ListItem item = cmbDepartment.Items.FindByValue(departmentValue);
                    if (item != null)
                    {
                        cmbDepartment.SelectedValue = departmentValue;
                    }
                    else
                    {
                        lblMessage.Text = "Department not found!";
                    }

                    string status = selectedRow.Cells[6].Text;

                    // Try to set the SelectedValue in the DropDownList
                    ListItem listItem = cmbStatus.Items.FindByValue(status);
                    if (listItem != null)
                    {
                        cmbStatus.SelectedValue = status;
                    }
                    else
                    {
                        lblMessage.Text = "status not found!";
                    }



                }


            }
        }
    }
}
