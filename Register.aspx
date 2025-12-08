<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ATSWeb.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .register-container {
            background-color: white;
            padding: 30px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
            border-radius: 8px;
            text-align: center;
            width: 100%;
            max-width: 900px;
        }

        .form-header {
            font-size: 24px;
            font-weight: bold;
            color: #333;
            margin-bottom: 20px;
        }

        .form-input, .form-select, .form-radio, .form-button {
            width: 100%;
            max-width: 350px;
            padding: 10px;
            box-sizing: border-box;
            font-size: 14px;
            margin: 10px 0;
            border-radius: 4px;
            border: 1px solid #ccc;
        }

        .form-input:focus, .form-select:focus, .form-radio:focus, .form-button:focus {
            outline: none;
            border-color: #4CAF50;
        }

        .radio-group {
            display: flex;
            justify-content: center;
            gap: 20px;
            margin-top: 10px;
        }

        .button-group {
            margin-top: 20px;
            display: flex;
            justify-content: center;
            gap: 20px;
        }

        .button {
            padding: 10px 20px;
            background-color: #4CAF50;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            transition: background-color 0.3s ease;
        }

        .button:hover {
            background-color: #45a049;
        }

        .button:disabled {
            background-color: #cccccc;
            cursor: not-allowed;
        }

        .error-message {
            color: red;
            font-size: 14px;
            margin-top: 10px;
        }

        .input-label {
            text-align: left;
            font-size: 16px;
            font-weight: bold;
            display: block;
            margin-bottom: 5px;
        }

        .form-divider {
            margin: 20px 0;
        }

        .form-divider::after {
            content: "";
            display: block;
            height: 1px;
            background-color: #ddd;
            margin-top: 10px;
        }

        .logo {
            width: 100px;
            height: 100px;
            margin-bottom: 20px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="register-container">
            <!-- Logo -->
            <asp:Image ID="imgRegisterLogo" runat="server" CssClass="logo" ImageUrl="~/twisklogo.png" />

            <!-- Form Header -->
            <asp:Label ID="lblMessage" runat="server" Text="Register" CssClass="form-header"></asp:Label>

            <!-- First Name and Last Name Fields -->
            <div class="form-group">
                <label for="txtFirstName" class="input-label">First Name</label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-input" />

                <label for="txtLastName" class="input-label">Last Name</label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-input" />
            </div>

            <!-- Email Field -->
            <div class="form-group">
                <label for="txtEmail" class="input-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" />
            </div>

            <!-- Gender Selection -->
            <div class="form-group radio-group">
                <asp:RadioButton ID="RadMale" runat="server" GroupName="Gender" Text="Male" CssClass="form-radio" />
                <asp:RadioButton ID="RadFemale" runat="server" GroupName="Gender" Text="Female" CssClass="form-radio" />
            </div>

            <!-- Phone Number Field -->
            <div class="form-group">
                <label for="txtPhone" class="input-label">Phone Number</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-input" />
            </div>

            <!-- Password and Confirm Password Fields -->
            <div class="form-group">
                <label for="txtPassword" class="input-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" />

                <label for="txtConfirmPassword" class="input-label">Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-input" TextMode="Password" />
            </div>

            <!-- Error Message -->
            <asp:Label ID="lblError" runat="server" CssClass="error-message" />

            <!-- Submit Button -->
            <div class="button-group">
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="button" />
            </div>

        </div>
    </form>
</body>
</html>
