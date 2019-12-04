<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>USWego &mdash; The U.S. Geography Game | Identify the State</title>
    <link rel="stylesheet" href="js/jquery-ui.css" />
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
            width: 935px;
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

        .flag, .quarter, .shape, .waterways {
            margin: auto;
            display: block;
            clear: both;
        }

        .lifeline .flag, .lifeline .quarter {
            /*width: 390px;*/
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

        .shapeContainer .shape, .waterwaysContainer .waterways {
            max-height: 297px;
            max-width: 420px;
            margin: auto;
            display: block;
        }

        .waterwaysContainer {
            clear: both;
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
            float: right;
            margin: -20px 0 0 10px;
        }

            .lifeline h3 {
                line-height: 150%;
            }

            .lifeline h4 {
                line-height: 135%;
            }

        #map {
            width: 480px;
            height: 310px;
            float: left;
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
            float: left;
        }

        div.destImg {
            /*width: 336px;*/
            float: left;
            margin: 20px 0 0 40px;
            /*margin-left: 10px;*/
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
    <script src="js/jquery.min.js"></script>
    <script src="js/raphael-min.js"></script>
    <script src="js/jquery.usmap.js"></script>
    <script src="js/color.jquery.js"></script>
    <script src="js/jquery-ui.js"></script>
    <script type="text/javascript">
        function zoomBox() {
            //alert('ztest');
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
                <h1>USWego &mdash; The U.S. Geography Game</h1>
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
                <div>
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
                    <div class="clear"></div>
                    <div class="destText">
                        <h2 class="destination>">
                            <asp:Label ID="destName" runat="server" Text=""></asp:Label>
                        </h2>
                        <h3>
                            <asp:Label ID="destDescription" runat="server" Text=""></asp:Label>
                        </h3>
                    </div>
                    <div class="destImg">
                        <asp:Literal ID="destImg" runat="server" Text=""></asp:Literal>
                    </div>
                    <div class="clear"></div>
                    <div id="selected-state">Is in what state? <span></span></div>
                    <br />
                    <div id="map"></div>
                    <asp:DropDownList ID="ddlStates" runat="server" AutoPostBack="false" CssClass="hideThis">
                        <asp:ListItem Value="">Select the state</asp:ListItem>
                        <asp:ListItem Value="AL">Alabama</asp:ListItem>
                        <asp:ListItem Value="AK">Alaska</asp:ListItem>
                        <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                        <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                        <asp:ListItem Value="CA">California</asp:ListItem>
                        <asp:ListItem Value="CO">Colorado</asp:ListItem>
                        <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                        <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                        <asp:ListItem Value="DE">Delaware</asp:ListItem>
                        <asp:ListItem Value="FL">Florida</asp:ListItem>
                        <asp:ListItem Value="GA">Georgia</asp:ListItem>
                        <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                        <asp:ListItem Value="ID">Idaho</asp:ListItem>
                        <asp:ListItem Value="IL">Illinois</asp:ListItem>
                        <asp:ListItem Value="IN">Indiana</asp:ListItem>
                        <asp:ListItem Value="IA">Iowa</asp:ListItem>
                        <asp:ListItem Value="KS">Kansas</asp:ListItem>
                        <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                        <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                        <asp:ListItem Value="ME">Maine</asp:ListItem>
                        <asp:ListItem Value="MD">Maryland</asp:ListItem>
                        <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                        <asp:ListItem Value="MI">Michigan</asp:ListItem>
                        <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                        <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                        <asp:ListItem Value="MO">Missouri</asp:ListItem>
                        <asp:ListItem Value="MT">Montana</asp:ListItem>
                        <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                        <asp:ListItem Value="NV">Nevada</asp:ListItem>
                        <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                        <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                        <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                        <asp:ListItem Value="NY">New York</asp:ListItem>
                        <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                        <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                        <asp:ListItem Value="OH">Ohio</asp:ListItem>
                        <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                        <asp:ListItem Value="OR">Oregon</asp:ListItem>
                        <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                        <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                        <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                        <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                        <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                        <asp:ListItem Value="TX">Texas</asp:ListItem>
                        <asp:ListItem Value="UT">Utah</asp:ListItem>
                        <asp:ListItem Value="VT">Vermont</asp:ListItem>
                        <asp:ListItem Value="VA">Virginia</asp:ListItem>
                        <asp:ListItem Value="WA">Washington</asp:ListItem>
                        <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                        <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                        <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                    </asp:DropDownList>
                </div>
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
            <div id="zoom" title="<%=zoomTitle %>">
                <p>
                    <asp:Literal ID="litZoom" Text="" runat="server"></asp:Literal>
                </p>
            </div>
        </form>
    </div>
    <script>
        /* 
        Forked from: https://github.com/NewSignature/us-map
        Instructions for use at: https://newsignature.github.io/us-map/ 
        */

        $(document).ready(function () {

            $('#map').usmap({
                stateStyles: { fill: '#CA6F1E' },
                stateHoverStyles: { fill: '#DAF7A6' },
                showLabels: true,
                labelBackingStyles: { fill: '#CA6F1E' },
                labelBackingHoverStyles: { fill: '#DAF7A6' },
                labelWidth: 27,
                labelHeight: 21,
                mouseover: function (event, data) {
                    $('#selected-state > span').text(data.name).css({ "font-weight": "bold", "color": "#FF6E6E" });
                },
                mouseout: function (event, data) {
                    $('#selected-state > span').text(' ').css({ "font-weight": "normal", "color": "#272D33" });
                },
                click: function (event, data) {
                    $("#<%=ddlStates.ClientID%> option:selected").val(data.name);
                    $("#<%=btnAnswer.ClientID %>").click();
                }
            });
        });

        function confirmation() {
            return confirm("Leave this game?");
        }
    </script>
</body>
</html>
