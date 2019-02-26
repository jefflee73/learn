<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="worldwego_test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Autocomplete Test</title>
    <script src="js/jquery-3.3.1.min.js"></script>
    <script src="js/jquery-ui.js"></script>
    <link href="js/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtCountry').autocomplete({
                source: 'CountryHandler.ashx'
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Country Name:
            <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
        </div>
    </form>
</body>
</html>
