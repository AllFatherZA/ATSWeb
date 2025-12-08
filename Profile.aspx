<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="ATSWeb.Profile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Profile</title>
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

        .form-input,
        .form-select {
            width: 100%;
            padding: 12px;
            margin: 8px 0;
            border-radius: 4px;
            border: 1px solid #ccc;
            font-size: 16px;
            box-sizing: border-box;
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

        .error {
            color: red;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="candidate-container">
            <asp:Label ID="lblMessage" runat="server" Text="Update Your Profile" CssClass="form-header"></asp:Label>

            <!-- User Information Form -->
           <div>

                <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input"></asp:TextBox>

                <asp:Label ID="lblPhone" runat="server" Text="Phone:"></asp:Label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-input"></asp:TextBox>

                <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-input"></asp:TextBox>

                <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password:"></asp:Label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-input"></asp:TextBox>

                            <!-- File Upload Section -->
                <div class="file-upload">
                    <asp:Label ID="lblFileUpload" runat="server" Text="Upload Resume (PDF only):" CssClass="form-header"></asp:Label>
                    <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-header" />
                <asp:Button ID="btnUpload" runat="server" Text="Upload PDF" OnClick="btnUpload_Click" CssClass="button" />
                </div>
            </div>

            <!-- Error Message -->
            <asp:Label ID="lblError" runat="server" CssClass="error"></asp:Label>

            <!-- Action Buttons -->
            <div class="button-group">
                <asp:Button ID="btnSave" runat="server" Text="Save Changes" OnClick="btnSave_Click" CssClass="button" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="button" />
            </div>

        </div>
    </form>
</body>
</html>
