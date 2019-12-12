<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WorldWego &mdash; The World Geography Game | Identify the Country</title>
    <link rel="shortcut icon" type="image/png" href="favicon.png"/>
    <link href="js/jquery-ui.css" rel="stylesheet" />
    <style type="text/css">
        body {
            background-color: #f3f3f3;
            color: #272D33;
            font-size: 16px;
            line-height: 20px;
            font-family: "Bookman Old Style", "Ostrich Sans Black", Arial, sans-serif;
            font-weight: normal;
        }

        .content {
            width: 960px;
            margin: auto;
        }

        #lblLevel, #lblLifelinesUsed, #lblAnswerValue, #lblMaxScore, #lblScore {
            width: 275px;
        }

        #lblScore {
            font-weight: bold;
        }

        #scoreboard {
            background-color: ivory;
            height: 45px;
            padding: 5px 10px 10px;
            border-radius: 5px;
            border: thin solid black;
            margin-bottom: 10px;
            width: 900px;
        }

        #playLeft, #playRight {
            width:440px;
            float:left;
        }
        #playLeft {
        }
        #playRight {
            margin: 10px 0 0 40px;
        }

        #playGame table {
            float: left;
        }
        .answerValue {
            text-align: center;
            padding-right: 30px;
            width: 120px;
        }
        .playerName, .playerScore {
            text-align: center;
            width: 160px;
        }

        .active {
            color: red;
        }

        .flag, .shape, .locator, .map {
            margin: auto;
            display: block;
            clear: both;
        }

        .lifeline .flag, .lifeline .locator, .lifeline .map {
            max-height: 297px;
            max-width: 420px;
        }

        .datum {
            color: crimson;
        }

        .shapeContainer {
            clear: both;
            background-color: white;
        }
        .shapeContainer .shape {
            max-height: 297px;
            max-width: 420px;
            margin: auto;
            display: block;
        }

        .cities th:nth-child(1), .cities td:nth-child(1) {
            width: 70px;
            text-align: center;
        }
        .cities th:nth-child(2), .cities td:nth-child(2) {
            width: 160px;
            text-align:left;
        }

        h3.neighbors {
            margin: 5px 0px;
            line-height: 150%;
        }
        table.neighbors {
            width: 100%;
            margin-left: 2em;
        }
        table.neighbors tr th {
            text-align: left;
        }
        table.neighbors tr th:nth-of-type(1) {
            width: 40%;
        }
        table.neighbors tr th:nth-of-type(2) {
            width: 60%;
        }
        
        table.condense {
            font-size:smaller;
            border-spacing: 0;
            line-height: 18px;
        }

        .hideThis {
            display: none;
        }

        #restart {
            float: right;
        }

        #restart input {
            margin-top: 10px;
            width: 85px;
            height: 30px;
        }

        #playButtons input {
            width: 75px;
            height: 30px;
            margin-bottom: 10px;
        }

        .llicon {
            float: right;
            /*margin: 0px 20px 10px;*/
            margin: -5px 0px 5px;
        }
       #llicon {
            width: 32px;
        }

        .lifeline {
            width: 420px;
            height: 365px;
            float: left;
            padding: 10px;
            border: solid thin #888;
            border-radius: 5px;
            background-color: #DDD;
            margin: 10px 40px 0 0;
        }

            .lifeline h3 {
                line-height: 175%;
            }

            .lifeline h4 {
                line-height: 160%;
            }

        #map {
            width: 480px;
            height: 310px;
            float: left;
        }

        #letterKeys {
            font-family:Arial, Helvetica, sans-serif;
            font-weight: bold;
            margin: 1ex 0;
            color:darkblue;
        }
        #letterKeys span:hover {
            color:orangered;
            font-style:italic;
        }

        #txtCountry {
            width: 40ex;
        }

        .clear {
            clear: both;
        }

        #destDescription {
            line-height: 150%;
        }

        div.destText {
            /*width: 480px;*/
            width: 440px;
            margin-right: 40px;
            margin-bottom: 20px;
            float: left;
        }

        div.destImg {
            /*width: 336px;*/
            float: left;
            /*margin: 0 0 20px 50px;*/
        }

        img.destImg {
            /*width: 336px;
            height: 190px;*/
            width: 440px;
            border-radius: 8px;
        }

        .scoringHistory {
            width: 420px;
            margin: 0 auto;
        }

        .congrats {
            font-weight: bold;
            color: #E74C3C;
        }

        .ui-widget-content {
            font-size: .85em;
            line-height:1.15em;
        }

       #zoom {
            display: none;
        }
        #zoom img {
            max-height: 480px;
            max-width: 640px;
        }
        #zoom img.flag, #zoom img.waterways {
            border: solid thin #dcdcdc;
        }
    </style>
    <script src="js/jquery-3.3.1.min.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script type="text/javascript">
        function zoomBox() {
            $(function () {
                $("#zoom").dialog({
                    modal: true,
                    open: function (event, ui) {
                        $('.ui-widget-overlay').bind('click', function () {
                            $("#zoom").dialog('close');
                        });
                    },
                    width: "670",
                    maxWidth: "670",
                    position: { my: "top", at: "bottom+10", of: "#scoreboard" },
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
    <div class="content">
        <form id="form1" runat="server">
            <div id="getPlayers" runat="server">
                <h1>WorldWego &mdash; The World Geography Game</h1>
                <h3>Enter the names of up to 4 players:</h3>
                <p>
                    Player 1:
                    <asp:TextBox ID="tbPlayer1" runat="server" />
                </p>
                <p>
                    Player 2:
                    <asp:TextBox ID="tbPlayer2" runat="server" />
                </p>
                <p>
                    Player 3:
                    <asp:TextBox ID="tbPlayer3" runat="server" />
                </p>
                <p>
                    Player 4:
                    <asp:TextBox ID="tbPlayer4" runat="server" />
                </p>
                <p>
                    <asp:Button runat="server" ID="btnPlay" OnClick="SubmitButton_Click" Text="Play" />
                </p>
            </div>

            <div id="playGame" runat="server" visible="false">
               <div id="scoreboard">
                        <table>
                            <tr>
                                <th class="answerValue">Answer Value</th>
                                <asp:Literal ID="litPlayerNames" runat="server" Text=""></asp:Literal>
                            </tr>
                            <tr>
                                <td class="answerValue">
                                    <asp:Literal ID="litAnswerValue" runat="server" Text=""></asp:Literal></td>
                                <asp:Literal ID="litPlayerScores" runat="server" Text=""></asp:Literal>
                            </tr>
                        </table>
                        <div id="restart">
                            <asp:Button ID="btnNewGame" Text="New Game" OnClick="NewGame_Click" runat="server" OnClientClick="return confirmation();" />
                        </div>
                    </div>
               <div id="playLeft">
                    <div class="clear"></div>
                    <div class="destText">
                        <h2 class="destination>">
                            <asp:Label ID="destName" runat="server" Text=""></asp:Label>
                        </h2>
                        <h3>
                            <asp:Label ID="destDescription" runat="server" Text=""></asp:Label>
                        </h3>
                    </div>
                    <div class="clear"></div>
                    <div id="selected-state">Is in what country?</div>
                    <div id="letterKeys">
                        <span onclick="$('#txtCountry').autocomplete('search', 'a');">A</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'b');">B</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'c');">C</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'd');">D</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'e');">E</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'f');">F</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'g');">G</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'h');">H</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'i');">I</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'j');">J</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'k');">K</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'l');">L</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'm');">M</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'n');">N</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'o');">O</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'p');">P</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'q');">Q</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'r');">R</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 's');">S</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 't');">T</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'u');">U</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'v');">V</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'w');">W</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'x');">X</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'y');">Y</span> 
                        <span onclick="$('#txtCountry').autocomplete('search', 'z');">Z</span> 
                    </div>
                    Country Name:
                    <asp:TextBox ID="txtCountry" runat="server" OnTextChanged="AnswerButton_Click"></asp:TextBox>
                </div>
                <div id="playRight">
                    <div class="destImg">
                        <asp:Literal ID="destImg" runat="server" Text=""></asp:Literal>
                    </div>
                    <div class="clear"></div>
                    <asp:Panel ID="pnlLifeline" CssClass="lifeline" runat="server">
                    <div id="playButtons">
                        <asp:Button ID="btnLifeline" Text="Lifeline" OnClick="Lifeline_Click" runat="server" />
                        <asp:Button ID="btnNext" Text="Next" OnClick="NextButton_Click" runat="server" />
                        <asp:Button ID="btnAnswer" Text="Answer" OnClick="AnswerButton_Click" runat="server" CssClass="hideThis" />
                        <div class="llicon">
                            <asp:Image runat="server" ID="llicon" />
                        </div>
                    </div>
                    <asp:Literal ID="litLifeline" runat="server" Text="" Visible="false"></asp:Literal>
                    <h4>
                        <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
                    </h4>
                </asp:Panel>
                </div>
            </div>
            <div id="zoom" title="<%=zoomTitle %>">
                <p>
                    <asp:Literal ID="litZoom" Text="" runat="server"></asp:Literal>
                </p>
            </div>
        </form>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtCountry').autocomplete({
                source: 'CountryHandler.ashx',
                change: function (event, ui) {
                    if (!ui.item) {
                        $(this).val('');
                    }
                },
                select: function (event, ui) {
                    $('#txtCountry').val(ui.item.value);
                    $(this).closest('form').submit();
                }
            });
        });

        //force autocomplete search
        //$('#txtCountry').autocomplete("search", "s");

        function confirmation() {
            return confirm("Leave this game?");
        }
    </script>
</body>
</html>
