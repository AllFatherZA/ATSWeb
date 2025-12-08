<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Candidate.aspx.cs" Inherits="ATSWeb.Candidate" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Apply for Jobs</title>
    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
        }

        .candidate-container {
            background-color: white;
            padding: 30px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
            border-radius: 8px;
            text-align: center;
            width: 100%;
            max-width: 900px;
            margin: 20px auto;
        }

        .form-header {
            font-size: 24px;
            font-weight: bold;
            color: #333;
            margin-bottom: 20px;
        }

        .gridview-style {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
            background-color: #fff;
        }

        .gridview-style th, .gridview-style td {
            padding: 12px;
            text-align: center;
            border: 1px solid #ddd;
            font-size: 14px;
        }

        .gridview-style th {
            background-color: #4CAF50;
            color: white;
            font-weight: bold;
        }

        .gridview-style tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .gridview-style tr:hover {
            background-color: #eaeaea;
        }

        .button-group {
            display: flex;
            justify-content: space-between;
            gap: 10px;
            margin-top: 20px;
        }

        .button {
            width: 100%;
            padding: 12px;
            background-color: #4CAF50;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            text-align: center;
            transition: background-color 0.3s ease;
        }

        .button:hover {
            background-color: #45a049;
        }

        .button:disabled {
            background-color: #cccccc;
            cursor: not-allowed;
        }

        .spacer {
            margin-top: 20px;
        }

        /* Styling for file upload input */
        .file-upload {
            margin-top: 20px;
            display: inline-block;
        }

        .file-upload input {
            padding: 8px;
            font-size: 16px;
        }

        .file-upload label {
            font-size: 16px;
            color: #333;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="candidate-container">
            <asp:Label ID="lblMessage" runat="server" Text="Apply for jobs" CssClass="form-header"></asp:Label>

            <!-- GridView for displaying job listings -->
            <asp:GridView ID="dataGridView1" runat="server" Width="100%" CssClass="gridview-style"
                AutoGenerateColumns="true" 
                OnSelectedIndexChanged="dataGridView1_SelectedIndexChanged"
                AllowPaging="true"
                DataKeyNames="job_id">
                <Columns>
                    <asp:ButtonField Text="Select" CommandName="Select" />
                </Columns>
            </asp:GridView>


                

                <!-- Display Message after File Upload -->
                <asp:Label ID="lblFileMessage" runat="server" ForeColor="Green" CssClass="form-header"></asp:Label>
            </div>

            <!-- Action Buttons -->
            <div class="button-group">
                <asp:Button ID="btnApply" runat="server" OnClick="btnApply_Click" Text="Apply" CssClass="button" />
                <asp:Button ID="btnApplications" runat="server" Text="Applications" OnClick="btnApplications_Click" CssClass="button" />
                <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="button" OnClick="btnRemove_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="button" OnClick="btnBack_Click" />
                <asp:Button ID="btnProfile" runat="server" Text="Go to Profile" CssClass="button" OnClick="btnProfile_Click" />
            </div>

            <div class="spacer"></div>
        
    </form>
</body>
</html>
