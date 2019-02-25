<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="uswego_dev_Default" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>jQuery UI Dialog - Modal message</title>
    <link rel="stylesheet" href="../css/style.css">
    <link rel="stylesheet" href="/geobee/js/jquery-ui-1.12.1/jquery-ui.css">
    <style type="text/css">
        #dialog { display:none;}
        #dialog img {
            max-height: 640px;
            max-width: 640px;
            display: block;
            margin: auto;
        }
        #dialog img.cia {
            /*width: 500px;*/
        }

    </style>
    <script src="/geobee/js/jquery-ui-1.12.1/external/jquery/jquery.js"></script>
    <script src="/geobee/js/jquery-ui-1.12.1/jquery-ui.js"></script>
    <script>
        function showModal() {
            $(function () {
                $("#dialog").dialog({
                    modal: true,
                    open: function (event, ui) {
                        $('.ui-widget-overlay').bind('click', function () {
                            $("#dialog").dialog('close');
                        });
                    },
                    width: "670",
                    maxWidth: "670",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="Button1" runat="server" Text="Show Modal" OnClick="Button1_Click" />
            <div id="dialog" title="<%=dialogTitle %>">
<%--                <p>
                    <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
                    Your files have downloaded successfully into the My Downloads folder.
                </p>--%>
                <p>
                    <asp:Literal ID="litZoom" Text="" runat="server"></asp:Literal>
                </p>
            </div>
            <p>Sed vel diam id libero <a href="http://example.com">rutrum convallis</a>. Donec aliquet leo vel magna. Phasellus rhoncus faucibus ante. Etiam bibendum, enim faucibus aliquet rhoncus, arcu felis ultricies neque, sit amet auctor elit eros a lectus.</p>
        </div>
    </form>
</body>
</html>
