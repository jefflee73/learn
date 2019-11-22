<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Explore.aspx.cs" Inherits="worldwego_Explore" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WorldWego Explorer</title>
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
            width: 120px;
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
            max-height: 400px;
            max-width: 620px;
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
        <form id="form1" runat="server">
            <div id="playGame" runat="server" visible="true">
                <div>
                    <div id="scoreboard">
                        <h3 class="title">
                            <asp:Literal ID="litTitle" runat="server" Text="WorldWego Explorer"></asp:Literal>
                        </h3>
                    </div>
                    <div class="clear"></div>
                    <br />
                    <asp:DropDownList ID="ddlStates" runat="server" AutoPostBack="false">
                        <asp:ListItem Value="AF" Selected="True">Afghanistan</asp:ListItem>
                        <asp:ListItem Value="AL">Albania</asp:ListItem>
                        <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                        <asp:ListItem Value="AD">Andorra</asp:ListItem>
                        <asp:ListItem Value="AO">Angola</asp:ListItem>
                        <asp:ListItem Value="AG">Antigua and Barbuda</asp:ListItem>
                        <asp:ListItem Value="AR">Argentina</asp:ListItem>
                        <asp:ListItem Value="AM">Armenia</asp:ListItem>
                        <asp:ListItem Value="AU">Australia</asp:ListItem>
                        <asp:ListItem Value="AT">Austria</asp:ListItem>
                        <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                        <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                        <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                        <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                        <asp:ListItem Value="BB">Barbados</asp:ListItem>
                        <asp:ListItem Value="BY">Belarus</asp:ListItem>
                        <asp:ListItem Value="BE">Belgium</asp:ListItem>
                        <asp:ListItem Value="BZ">Belize</asp:ListItem>
                        <asp:ListItem Value="BJ">Benin</asp:ListItem>
                        <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                        <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                        <asp:ListItem Value="BA">Bosnia and Herzegovina</asp:ListItem>
                        <asp:ListItem Value="BW">Botswana</asp:ListItem>
                        <asp:ListItem Value="BR">Brazil</asp:ListItem>
                        <asp:ListItem Value="BN">Brunei</asp:ListItem>
                        <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                        <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                        <asp:ListItem Value="BI">Burundi</asp:ListItem>
                        <asp:ListItem Value="CV">Cabo Verde</asp:ListItem>
                        <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                        <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                        <asp:ListItem Value="CA">Canada</asp:ListItem>
                        <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                        <asp:ListItem Value="TD">Chad</asp:ListItem>
                        <asp:ListItem Value="CL">Chile</asp:ListItem>
                        <asp:ListItem Value="CN">China</asp:ListItem>
                        <asp:ListItem Value="CO">Colombia</asp:ListItem>
                        <asp:ListItem Value="KM">Comoros</asp:ListItem>
                        <asp:ListItem Value="CD">Democratic Republic of the Congo</asp:ListItem>
                        <asp:ListItem Value="CG">Republic of the Congo</asp:ListItem>
                        <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                        <asp:ListItem Value="CI">Cote d'Ivoire</asp:ListItem>
                        <asp:ListItem Value="HR">Croatia</asp:ListItem>
                        <asp:ListItem Value="CU">Cuba</asp:ListItem>
                        <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                        <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>
                        <asp:ListItem Value="DK">Denmark</asp:ListItem>
                        <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                        <asp:ListItem Value="DM">Dominica</asp:ListItem>
                        <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                        <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                        <asp:ListItem Value="EG">Egypt</asp:ListItem>
                        <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                        <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                        <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                        <asp:ListItem Value="EE">Estonia</asp:ListItem>
                        <asp:ListItem Value="SZ">Eswatini</asp:ListItem>
                        <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                        <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                        <asp:ListItem Value="FI">Finland</asp:ListItem>
                        <asp:ListItem Value="FR">France</asp:ListItem>
                        <asp:ListItem Value="GA">Gabon</asp:ListItem>
                        <asp:ListItem Value="GM">Gambia</asp:ListItem>
                        <asp:ListItem Value="GE">Georgia</asp:ListItem>
                        <asp:ListItem Value="DE">Germany</asp:ListItem>
                        <asp:ListItem Value="GH">Ghana</asp:ListItem>
                        <asp:ListItem Value="GR">Greece</asp:ListItem>
                        <asp:ListItem Value="GD">Grenada</asp:ListItem>
                        <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                        <asp:ListItem Value="GN">Guinea</asp:ListItem>
                        <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>
                        <asp:ListItem Value="GY">Guyana</asp:ListItem>
                        <asp:ListItem Value="HT">Haiti</asp:ListItem>
                        <asp:ListItem Value="HN">Honduras</asp:ListItem>
                        <asp:ListItem Value="HU">Hungary</asp:ListItem>
                        <asp:ListItem Value="IS">Iceland</asp:ListItem>
                        <asp:ListItem Value="IN">India</asp:ListItem>
                        <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                        <asp:ListItem Value="IR">Iran</asp:ListItem>
                        <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                        <asp:ListItem Value="IE">Ireland</asp:ListItem>
                        <asp:ListItem Value="IL">Israel</asp:ListItem>
                        <asp:ListItem Value="IT">Italy</asp:ListItem>
                        <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                        <asp:ListItem Value="JP">Japan</asp:ListItem>
                        <asp:ListItem Value="JO">Jordan</asp:ListItem>
                        <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                        <asp:ListItem Value="KE">Kenya</asp:ListItem>
                        <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                        <asp:ListItem Value="XK">Kosovo</asp:ListItem>
                        <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                        <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                        <asp:ListItem Value="LA">Laos</asp:ListItem>
                        <asp:ListItem Value="LV">Latvia</asp:ListItem>
                        <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                        <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                        <asp:ListItem Value="LR">Liberia</asp:ListItem>
                        <asp:ListItem Value="LY">Libya</asp:ListItem>
                        <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                        <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                        <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                        <asp:ListItem Value="MK">Macedonia</asp:ListItem>
                        <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                        <asp:ListItem Value="MW">Malawi</asp:ListItem>
                        <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                        <asp:ListItem Value="MV">Maldives</asp:ListItem>
                        <asp:ListItem Value="ML">Mali</asp:ListItem>
                        <asp:ListItem Value="MT">Malta</asp:ListItem>
                        <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                        <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                        <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                        <asp:ListItem Value="MX">Mexico</asp:ListItem>
                        <asp:ListItem Value="FM">Micronesia</asp:ListItem>
                        <asp:ListItem Value="MD">Moldova</asp:ListItem>
                        <asp:ListItem Value="MC">Monaco</asp:ListItem>
                        <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                        <asp:ListItem Value="ME">Montenegro</asp:ListItem>
                        <asp:ListItem Value="MA">Morocco</asp:ListItem>
                        <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                        <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                        <asp:ListItem Value="NA">Namibia</asp:ListItem>
                        <asp:ListItem Value="NR">Nauru</asp:ListItem>
                        <asp:ListItem Value="NP">Nepal</asp:ListItem>
                        <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                        <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                        <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                        <asp:ListItem Value="NE">Niger</asp:ListItem>
                        <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                        <asp:ListItem Value="KP">North Korea</asp:ListItem>
                        <asp:ListItem Value="NO">Norway</asp:ListItem>
                        <asp:ListItem Value="OM">Oman</asp:ListItem>
                        <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                        <asp:ListItem Value="PW">Palau</asp:ListItem>
                        <asp:ListItem Value="PS">Palestine</asp:ListItem>
                        <asp:ListItem Value="PA">Panama</asp:ListItem>
                        <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                        <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                        <asp:ListItem Value="PE">Peru</asp:ListItem>
                        <asp:ListItem Value="PH">Philippines</asp:ListItem>
                        <asp:ListItem Value="PL">Poland</asp:ListItem>
                        <asp:ListItem Value="PT">Portugal</asp:ListItem>
                        <asp:ListItem Value="QA">Qatar</asp:ListItem>
                        <asp:ListItem Value="RO">Romania</asp:ListItem>
                        <asp:ListItem Value="RU">Russia</asp:ListItem>
                        <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                        <asp:ListItem Value="KN">Saint Kitts and Nevis</asp:ListItem>
                        <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                        <asp:ListItem Value="VC">Saint Vincent and the Grenadines</asp:ListItem>
                        <asp:ListItem Value="WS">Samoa</asp:ListItem>
                        <asp:ListItem Value="SM">San Marino</asp:ListItem>
                        <asp:ListItem Value="ST">Sao Tome and Principe</asp:ListItem>
                        <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                        <asp:ListItem Value="SN">Senegal</asp:ListItem>
                        <asp:ListItem Value="RS">Serbia</asp:ListItem>
                        <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                        <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                        <asp:ListItem Value="SG">Singapore</asp:ListItem>
                        <asp:ListItem Value="SK">Slovakia</asp:ListItem>
                        <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                        <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                        <asp:ListItem Value="SO">Somalia</asp:ListItem>
                        <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                        <asp:ListItem Value="KR">South Korea</asp:ListItem>
                        <asp:ListItem Value="SS">South Sudan</asp:ListItem>
                        <asp:ListItem Value="ES">Spain</asp:ListItem>
                        <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                        <asp:ListItem Value="SD">Sudan</asp:ListItem>
                        <asp:ListItem Value="SR">Suriname</asp:ListItem>
                        <asp:ListItem Value="SE">Sweden</asp:ListItem>
                        <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                        <asp:ListItem Value="SY">Syria</asp:ListItem>
                        <asp:ListItem Value="TW">Taiwan</asp:ListItem>
                        <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                        <asp:ListItem Value="TZ">Tanzania</asp:ListItem>
                        <asp:ListItem Value="TH">Thailand</asp:ListItem>
                        <asp:ListItem Value="TL">Timor-Leste</asp:ListItem>
                        <asp:ListItem Value="TG">Togo</asp:ListItem>
                        <asp:ListItem Value="TO">Tonga</asp:ListItem>
                        <asp:ListItem Value="TT">Trinidad and Tobago</asp:ListItem>
                        <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                        <asp:ListItem Value="TR">Turkey</asp:ListItem>
                        <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                        <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                        <asp:ListItem Value="UG">Uganda</asp:ListItem>
                        <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                        <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                        <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                        <asp:ListItem Value="US">United States</asp:ListItem>
                        <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                        <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                        <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                        <asp:ListItem Value="VA">Vatican City</asp:ListItem>
                        <asp:ListItem Value="VE">Venezuela</asp:ListItem>
                        <asp:ListItem Value="VN">Vietnam</asp:ListItem>
                        <asp:ListItem Value="YE">Yemen</asp:ListItem>
                        <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                        <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>
                    </asp:DropDownList>

                    <asp:DropDownList ID="ddlLifelines" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="1" Selected="True">Latitude and Longitude</asp:ListItem>
                        <asp:ListItem Value="2">Country flag</asp:ListItem>
                        <asp:ListItem Value="3">Country locator</asp:ListItem>
                        <asp:ListItem Value="4">Country map</asp:ListItem>
                        <asp:ListItem Value="5">Population and area</asp:ListItem>
                        <asp:ListItem Value="6">Elevation extremes</asp:ListItem>
                        <asp:ListItem Value="7">Terrain and climate</asp:ListItem>
                        <asp:ListItem Value="8">Religions</asp:ListItem>
                        <asp:ListItem Value="9">Languages</asp:ListItem>
                        <asp:ListItem Value="10">Currency</asp:ListItem>
                        <asp:ListItem Value="11">Notes</asp:ListItem>
                        <asp:ListItem Value="12">Shape</asp:ListItem>
                        <asp:ListItem Value="13">Ten largest cities</asp:ListItem>
                        <asp:ListItem Value="14">Resources</asp:ListItem>
                        <asp:ListItem Value="15">National capital</asp:ListItem>
                        <asp:ListItem Value="16">Neighboring countries</asp:ListItem>
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

                    </asp:DropDownList>

                </div>
                <asp:Panel ID="pnlLifeline" CssClass="lifeline" runat="server">
                    <div id="playButtons">
                        <asp:Button ID="btnLifeline" Text="Lifeline" OnClick="Lifeline_Click" runat="server" />
                        <asp:Button ID="btnPrevLL" Text="Prev Lifeline" OnClick="PrevLL_Click" runat="server" />
                        <asp:Button ID="btnNextLL" Text="Next Lifeline" OnClick="NextLL_Click" runat="server" />
                        <asp:Button ID="btnPrevST" Text="Prev Country" OnClick="PrevST_Click" runat="server" />
                        <asp:Button ID="btnNextST" Text="Next Country" OnClick="NextST_Click" runat="server" />
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
        function confirmation() {
            return confirm("Leave this game?");
        }
    </script>
</body>
</html>
