<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="ATSWeb.AdminDashboard" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard</title>
    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f4f4f4;
        }

        /* Header */
        .form-header {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
            color: #333;
        }

        /* GridView */
        .gridview-style {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
            background-color: #fff;
        }

        .gridview-style th, .gridview-style td {
            padding: 10px;
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

        /* Form styling */
        .form-table {
            width: 100%;
            margin-top: 20px;
        }

        .form-table td {
            padding: 10px;
            vertical-align: top;
        }

        .form-input,
        .form-select {
            width: 100%;
            max-width: 400px;
            padding: 8px;
            box-sizing: border-box;
        }

        /* Buttons */
        .button {
            min-width: 120px;
            padding: 8px;
            margin: 10px;
            background-color: #4CAF50;
            color: white;
            border: none;
            cursor: pointer;
            border-radius: 4px;
            transition: background-color 0.3s ease;
        }

        .button:hover:enabled {
            background-color: #45a049;
        }

        /* Greyed out buttons when disabled */
        .button:disabled {
            background-color: #cccccc;
            color: #666666;
            cursor: not-allowed;
        }

        /* Wide button variant */
        .button-wide {
            min-width: 140px;
        }

        /* Button group layout */
        .button-group {
            display: flex;
            justify-content: space-between;
            flex-wrap: wrap;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblMessage" runat="server" Text="Manage Jobs" CssClass="form-header"></asp:Label>

            <!-- GridView to display job data -->
            <asp:GridView ID="dataGridView1" runat="server" Width="100%" CssClass="gridview-style"
                OnSelectedIndexChanged="dataGridView1_SelectedIndexChanged">
                <Columns>
                    <asp:ButtonField Text="Select" CommandName="Select" />
                </Columns>
            </asp:GridView>

            <!-- Form Inputs -->
            <table class="form-table">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Title:"></asp:Label><br />
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-input"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Status:"></asp:Label><br />
                        <asp:DropDownList ID="cmbStatus" runat="server" CssClass="form-select">
                            <asp:ListItem>Open</asp:ListItem>
                            <asp:ListItem>Closed</asp:ListItem>
                            <asp:ListItem>On Hold</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Department:"></asp:Label><br />
                        <asp:DropDownList ID="cmbDepartment" runat="server" CssClass="form-select">
                            <asp:ListItem>Human Resources (HR)</asp:ListItem>
                            <asp:ListItem>Finance</asp:ListItem>
                            <asp:ListItem>Marketing</asp:ListItem>
                            <asp:ListItem>Sales</asp:ListItem>
                            <asp:ListItem>Operations</asp:ListItem>
                            <asp:ListItem>Information Technology (IT)</asp:ListItem>
                            <asp:ListItem>Product</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Location:"></asp:Label><br />
                        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-input"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label6" runat="server" Text="Description:"></asp:Label><br />
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-input" Height="90px"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <!-- Buttons -->
            <div class="button-group">
                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" CssClass="button" />
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="button" />
                <asp:Button ID="btnRemove" runat="server" OnClick="btnRemove_Click" Text="Remove" CssClass="button" />
                <asp:Button ID="btnReject" runat="server" OnClick="btnReject_Click" Text="Reject" CssClass="button" />
                <asp:Button ID="btnAccept" runat="server" OnClick="btnAccept_Click" Text="Accept" CssClass="button" />
            </div>

            <!-- Additional Buttons for Display Candidates and Back -->
            <div class="button-group">
                <asp:Button ID="btnDisplayCandidates" runat="server" OnClick="btnDisplayCandidates_Click"
                    Text="Display Candidates" CssClass="button button-wide" />
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="button" />
            </div>
        </div>
    </form>
</body>
</html>
