<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ATSWeb.defaullt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
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

        .login-container {
            background-color: white;
            width: 100%;
            max-width: 400px;
            padding: 30px;
            box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
            border-radius: 8px;
            text-align: center;
        }

        .login-header img {
            width: 100px;
            height: 120px;
            margin-bottom: 15px;
        }

        .login-header h2 {
            margin: 0;
            font-size: 24px;
            font-weight: bold;
            color: #333;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-group label {
            font-size: 14px;
            color: #555;
            display: block;
            margin-bottom: 5px;
        }

        .form-group input {
            width: 100%;
            padding: 10px;
            font-size: 14px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }

        .form-group input:focus {
            border-color: #4CAF50;
            outline: none;
        }

        .error-message {
            color: red;
            font-size: 14px;
            margin-top: 10px;
            text-align: center;
        }

        .button-group {
            display: flex;
            justify-content: space-between;
            gap: 10px;
        }

        .button {
            width: 48%;
            padding: 10px;
            background-color: #4CAF50;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            text-align: center;
        }

        .button:hover {
            background-color: #45a049;
        }

        .button:disabled {
            background-color: #cccccc;
            cursor: not-allowed;
        }

        .forgot-password {
            text-align: center;
            margin-top: 15px;
        }

        .forgot-password a {
            color: #4CAF50;
            text-decoration: none;
        }

        .forgot-password a:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="login-header">
                <asp:Image ID="imgLogo" runat="server" Height="120px" ImageUrl="~/twisklogo.png" Width="100px" />
            </div>

            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Email:"></asp:Label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" />
            </div>

            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Password:"></asp:Label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" />
            </div>

            <div class="error-message">
                <asp:Label ID="lblError" runat="server" Text="Error"></asp:Label>
            </div>

            <div class="button-group">
                <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" CssClass="button" />
                <asp:Button ID="btnRegister" runat="server" OnClick="btnRegister_Click" Text="Register" CssClass="button" />
            </div>

        </div>
    </form>
</body>
</html>
