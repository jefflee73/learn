<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Explore.aspx.cs" Inherits="uswego_Explore" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>USWego Explorer</title>
    <link rel="shortcut icon" type="image/png" href="favicon.png"/>
    <link rel="stylesheet" href="/geobee/js/jquery-ui-1.12.1/jquery-ui.css" />
    <style type="text/css">
        body {
            background-color: #f3f3f3;
            color: #272D33;
            font-size: 16px;
            line-height: 20px;
            font-family: "Bookman Old Style", "Ostrich Sans Black", Arial, sans-serif;
            font-weight: normal;
        }

        h4 {
            clear: both;
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
            margin-bottom: 15px;
            width: 935px;
        }

        .title {
            margin: 1.5ex auto;
            text-align: center;
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

        #ddlStates {
            margin-right: 1em;
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

            .shapeContainer .shape {
                max-height: 297px;
                max-width: 420px;
                margin: auto;
                display: block;
            }

        .waterwaysContainer .waterways {
            max-height: 400px;
            max-width: 600px;
            margin: auto;
            display: block;
        }

        .waterwaysContainer {
            clear: both;
        }

        table.cities {
            width: 100%;
        }

        table.cities tr th {
            text-align: left;
        }

        table.cities tr th:nth-of-type(1) {
            width: 20%;
        }

        table.cities tr th:nth-of-type(2) {
            width: 40%;
        }

        table.cities tr th:nth-of-type(3) {
            width: 40%;
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
            width: 90px;
            height: 30px;
            margin-bottom: 10px;
        }

        #btnLifeline, #btnNextLL {
            margin-right: 1em;
        }


        #btnRand {
            width: 35px !important;
            margin-left: 1em;
        }

        .llicon {
            float: right;
            margin: -5px 0px 5px;
        }

        #llicon {
            width: 32px;
        }

        .lifeline {
            width: 620px;
            height: 540px;
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
                line-height: 105%;
                margin-top: 10px;
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

        span.dest {
            font-weight: bold;
            font-size: larger;
        }

        img.destImg {
            /*width: 336px;
            height: 190px;*/
            width: 580px;
            margin-left: 20px;
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
    <script src="/geobee/js/jquery-ui-1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        function zoomBox() {
            //alert('ztest');
            $(function () {
                return; //zoomBox disabled
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
        <form id="form2" runat="server">
            <div id="playGame" runat="server" visible="true">
                <div>
                    <div id="scoreboard">
                        <h3 class="title">
                            <asp:Literal ID="litTitle" runat="server" Text="USWego Explorer"></asp:Literal>
                        </h3>
                    </div>
                    <div class="clear"></div>
                    <br />
                    <asp:DropDownList ID="ddlStates" runat="server" AutoPostBack="false">
                        <asp:ListItem Value="AL" Selected="True">Alabama</asp:ListItem>
                        <asp:ListItem Value="AK">Alaska</asp:ListItem>
                        <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                        <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                        <asp:ListItem Value="CA">California</asp:ListItem>
                        <asp:ListItem Value="CO">Colorado</asp:ListItem>
                        <asp:ListItem Value="CT">Connecticut</asp:ListItem>
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

                    <asp:DropDownList ID="ddlLifelines" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="1" Selected="True">Latitude and Longitude</asp:ListItem>
                        <asp:ListItem Value="2">State flag</asp:ListItem>
                        <asp:ListItem Value="3">State quarter</asp:ListItem>
                        <asp:ListItem Value="4">Statehood date</asp:ListItem>
                        <asp:ListItem Value="5">Population and area</asp:ListItem>
                        <asp:ListItem Value="6">Elevation extremes</asp:ListItem>
                        <asp:ListItem Value="7">State symbols</asp:ListItem>
                        <asp:ListItem Value="8">State nickname</asp:ListItem>
                        <asp:ListItem Value="9">Ten largest cities</asp:ListItem>
                        <asp:ListItem Value="10">Federal lands</asp:ListItem>
                        <asp:ListItem Value="11">Physical features</asp:ListItem>
                        <asp:ListItem Value="12">Shape</asp:ListItem>
                        <asp:ListItem Value="13">Waterways</asp:ListItem>
                        <asp:ListItem Value="14">Biggest company</asp:ListItem>
                        <asp:ListItem Value="15">State capital</asp:ListItem>
                        <asp:ListItem Value="16">Neighboring states</asp:ListItem>
                        <asp:ListItem Value="101">Destination 1</asp:ListItem>
                        <asp:ListItem Value="102">Destination 2</asp:ListItem>
                        <asp:ListItem Value="103">Destination 3</asp:ListItem>
                        <asp:ListItem Value="104">Destination 4</asp:ListItem>
                        <asp:ListItem Value="105">Destination 5</asp:ListItem>
                        <asp:ListItem Value="106">Destination 6</asp:ListItem>
                        <asp:ListItem Value="107">Destination 7</asp:ListItem>
                        <asp:ListItem Value="108">Destination 8</asp:ListItem>
                        <asp:ListItem Value="109">Destination 9</asp:ListItem>
                        <asp:ListItem Value="110">Destination 10</asp:ListItem>
                        <asp:ListItem Value="111">Destination 11</asp:ListItem>
                        <asp:ListItem Value="112">Destination 12</asp:ListItem>
                        <asp:ListItem Value="113">Destination 13</asp:ListItem>
                        <asp:ListItem Value="114">Destination 14</asp:ListItem>
                        <asp:ListItem Value="115">Destination 15</asp:ListItem>
                        <asp:ListItem Value="116">Destination 16</asp:ListItem>
                        <asp:ListItem Value="117">Destination 17</asp:ListItem>
                        <asp:ListItem Value="118">Destination 18</asp:ListItem>
                        <asp:ListItem Value="119">Destination 19</asp:ListItem>
                        <asp:ListItem Value="120">Destination 20</asp:ListItem>
                        <asp:ListItem Value="121">Destination 21</asp:ListItem>
                        <asp:ListItem Value="122">Destination 22</asp:ListItem>
                        <asp:ListItem Value="123">Destination 23</asp:ListItem>
                        <asp:ListItem Value="124">Destination 24</asp:ListItem>
                        <asp:ListItem Value="125">Destination 25</asp:ListItem>
                        <asp:ListItem Value="126">Destination 26</asp:ListItem>
                        <asp:ListItem Value="127">Destination 27</asp:ListItem>
                        <asp:ListItem Value="128">Destination 28</asp:ListItem>
                        <asp:ListItem Value="129">Destination 29</asp:ListItem>
                        <asp:ListItem Value="130">Destination 30</asp:ListItem>
                        <asp:ListItem Value="131">Destination 31</asp:ListItem>
                        <asp:ListItem Value="132">Destination 32</asp:ListItem>
                        <asp:ListItem Value="133">Destination 33</asp:ListItem>
                        <asp:ListItem Value="134">Destination 34</asp:ListItem>
                        <asp:ListItem Value="135">Destination 35</asp:ListItem>
                        <asp:ListItem Value="136">Destination 36</asp:ListItem>
                        <asp:ListItem Value="137">Destination 37</asp:ListItem>
                        <asp:ListItem Value="138">Destination 38</asp:ListItem>
                        <asp:ListItem Value="139">Destination 39</asp:ListItem>
                        <asp:ListItem Value="140">Destination 40</asp:ListItem>
                        <asp:ListItem Value="141">Destination 41</asp:ListItem>
                        <asp:ListItem Value="142">Destination 42</asp:ListItem>
                        <asp:ListItem Value="143">Destination 43</asp:ListItem>
                        <asp:ListItem Value="144">Destination 44</asp:ListItem>
                        <asp:ListItem Value="145">Destination 45</asp:ListItem>
                        <asp:ListItem Value="146">Destination 46</asp:ListItem>
                        <asp:ListItem Value="147">Destination 47</asp:ListItem>
                        <asp:ListItem Value="148">Destination 48</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Panel ID="pnlLifeline" CssClass="lifeline" runat="server">
                    <div id="playButtons">
                        <asp:Button ID="btnLifeline" Text="Lifeline" OnClick="Lifeline_Click" runat="server" />
                        <asp:Button ID="btnPrevLL" Text="Prev Lifeline" OnClick="PrevLL_Click" runat="server" />
                        <asp:Button ID="btnNextLL" Text="Next Lifeline" OnClick="NextLL_Click" runat="server" />
                        <asp:Button ID="btnPrevST" Text="Prev State" OnClick="PrevST_Click" runat="server" />
                        <asp:Button ID="btnNextST" Text="Next State" OnClick="NextST_Click" runat="server" />
                        <asp:Button ID="btnRand" Text="?" OnClick="RandLL_Click" runat="server" />
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

<%--        $(document).ready(function () {

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
        });--%>

        function confirmation() {
            return confirm("Leave this game?");
        }
    </script>
</body>
</html>
