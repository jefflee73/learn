using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected int answerBaseScore = 100;
    protected int answerIncrement = 5;
    protected int lifelineCost = 20;
    protected int lifelineCount = 13;
    public string zoomTitle;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["Players"] == null || Session["PlayerScores"] == null || Session["PlayerTurn"] == null)
            {
                getPlayers.Visible = true;
                playGame.Visible = false;
            }
            else
            {
                getPlayers.Visible = false;
                playGame.Visible = true;

                string[] players = Session["Players"].ToString().TrimEnd('|').Split('|');
                string[] playerScores = Session["PlayerScores"].ToString().TrimEnd('|').Split('|');
                int playerCount = players.Length;
                int playerTurn = (int)Session["PlayerTurn"];
                int gameScore = (Session["score"] != null) ? (int)Session["score"] : 0;
                Session["score"] = gameScore;

                int gameLevel = (Session["level"] != null) ? 1 + Convert.ToInt32(Session["level"]) : 1;
                Session["level"] = gameLevel;

                if (gameLevel > 48)
                {
                    lblResult.Text = "Congratulations...  You have reached the highest level!";
                    litAnswerValue.Text = Session["answerValue"].ToString();
                    for (int i = 0; i < playerCount; i++)
                    {
                        litPlayerNames.Text += "<th class='playerName'>" + players[i] + "</th>";
                        litPlayerScores.Text += "<td class='playerScore'>" + playerScores[i] + "</td>";
                    }
                    Session.Abandon();
                    return;
                }

                int answerValue = answerBaseScore + (gameLevel - 1) * answerIncrement;
                Session["answerValue"] = answerValue;
                litAnswerValue.Text = answerValue.ToString();
                for (int i = 0; i < playerCount; i++)
                {
                    string activeClass = ((i == playerTurn) ? " active" : "");
                    litPlayerNames.Text += "<th class='playerName" + activeClass + "'>" + players[i] + "</th>";
                    litPlayerScores.Text += "<td class='playerScore'>" + playerScores[i] + "</td>";
                }

                litLifeline.Visible = false;
                Geography geography = new Geography();
                int effectiveLevel = (int)Math.Ceiling((decimal)gameLevel / 2); //2 destinations at each level
                List<int> prevDestList = (List<int>)Session["PrevDestList"];
                DataRow drDestination = geography.GetRandomDestination(effectiveLevel, prevDestList);
                string strId = drDestination["id"].ToString();
                int intId = Convert.ToInt32(strId);
                prevDestList.Add(intId);
                string strRank = drDestination["rankinstate"].ToString();
                string strName = drDestination["destination"].ToString();
                string strDescription = drDestination["description"].ToString();
                string strAbbr = drDestination["abbr"].ToString();
                int[] intArrLifeline = RandomIntegerArray(lifelineCount);
                string strLifelines = string.Join("|", Array.ConvertAll<int, String>(intArrLifeline, Convert.ToString));

                Session["id"] = strId;
                Session["PrevDestList"] = prevDestList;
                Session["rank"] = strRank;
                Session["name"] = strName;
                Session["description"] = strDescription;
                Session["abbr"] = strAbbr;
                Session["lifelines"] = strLifelines;
                Session["llIndex"] = 0;
                destName.Text = gameLevel.ToString() + ". " + strName;
                destDescription.Text = strDescription;
                destImg.Text = "<img src=\"" + "images/destimages/" + strAbbr + "/" + strRank + "-" + strName.Replace(" ", "-").Replace(".", "") + ".jpg\" class=\"destImg\">";
            }
        }
        btnLifeline.Visible = true;
        btnNext.Visible = false;
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        int playerCount = 0;
        string players = "";
        string playerScores = "";
        if (!String.IsNullOrEmpty(tbPlayer1.Text))
        {
            playerCount++;
            players += tbPlayer1.Text + "|";
            playerScores += "0|";
        }
        if (!String.IsNullOrEmpty(tbPlayer2.Text))
        {
            playerCount++;
            players += tbPlayer2.Text + "|";
            playerScores += "0|";
        }
        if (!String.IsNullOrEmpty(tbPlayer3.Text))
        {
            playerCount++;
            players += tbPlayer3.Text + "|";
            playerScores += "0|";
        }
        if (!String.IsNullOrEmpty(tbPlayer4.Text))
        {
            playerCount++;
            players += tbPlayer4.Text + "|";
            playerScores += "0|";
        }
        if (String.IsNullOrEmpty(players))
        {
            players = "Guest|";
            playerScores = "0|";
        }
        Session["Players"] = players;
        Session["PlayerScores"] = playerScores;
        Session["PlayerTurn"] = 0;
        Session["PrevDestList"] = new List<int>();
        Response.Redirect("Default.aspx");
    }

    protected void Lifeline_Click(object sender, EventArgs e)
    {
        llicon.Visible = false;
        if (Session["llIndex"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        litLifeline.Visible = false;
        //Random r = new Random();
        //int lifelineType = r.Next(1, 10);
        int llIndex = (int)Session["llIndex"];
        string[] strArrLifelines = (Session["lifelines"].ToString()).Split('|');
        int lifelineType;
        if (llIndex >= strArrLifelines.Length)
        {
            lifelineType = 999; //ran out of lifelines
        }
        else
        {
            lifelineType = 1 + Convert.ToInt32(strArrLifelines[llIndex]);
            Session["llIndex"] = 1 + llIndex;
        }
        int answerValue = (int)Session["answerValue"];
        if (answerValue < lifelineCost)
        {
            lifelineType = 999; //don;t allow negative score
        }
        else
        {
            answerValue -= lifelineCost;
            litAnswerValue.Text = answerValue.ToString();
            Session["answerValue"] = answerValue;
        }

        int intId = Convert.ToInt32(Session["id"]);
        Geography geography = new Geography();
        DataRow drData = geography.GetDestination(intId);
        string strImage;
        string strAbbr;
        DataTable dt;
        switch (lifelineType)
        {
            case 1:     //latlng
                //litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is located at (" + drData["lat"].ToString() + ", " + drData["lng"].ToString() + ")</h3>";
                litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is located at <span class='datum'>" + DDToDMS(Convert.ToDouble(drData["lat"].ToString()), Convert.ToDouble(drData["lng"].ToString())) + "</span></h3>";
                llicon.ImageUrl = "images/icons/latlng.png";
                llicon.AlternateText = "Latitude and longitude";
                llicon.ToolTip = "Latitude and longitude";
                break;
            case 2:     //flag
                strImage = "<img class=\"flag\" src=\"images/zzz.png\" alt=\"State Flag\" title=\"State Flag\" onclick=\"zoomBox()\" >";
                strImage = strImage.Replace("zzz", "x" + drData["abbr"].ToString().ToLower());
                litLifeline.Text = strImage;
                llicon.ImageUrl = "images/icons/flag.png";
                llicon.AlternateText = "Flag";
                llicon.ToolTip = "Flag";
                zoomTitle = "State Flag";
                litZoom.Text = "<img class=\"flag\" src=\"images/zzz.png\" alt=\"State Flag\" title=\"State Flag\">";
                litZoom.Text = litZoom.Text.Replace("zzz", "x" + drData["abbr"].ToString().ToLower());
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                break;
            case 3:     //quarter
                strImage = "<img class=\"quarter\" src=\"images/zzz.png\" alt=\"State Quarter\" title=\"State Quarter\" onclick=\"zoomBox()\" >";
                strImage = strImage.Replace("zzz", "y" + drData["abbr"].ToString().ToLower());
                litLifeline.Text = strImage;
                llicon.ImageUrl = "images/icons/quarter.png";
                llicon.AlternateText = "Quarter";
                llicon.ToolTip = "Quarter";
                zoomTitle = "State Quarter";
                litZoom.Text = "<img class=\"quarter\" src=\"images/zzz.png\" alt=\"State Quarter\" title=\"State Quarter\">";
                litZoom.Text = litZoom.Text.Replace("zzz", "y" + drData["abbr"].ToString().ToLower());
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                break;
            case 4:     //statehood
                litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a state that gained statehood on <span class='datum'>" + drData["statehood"].ToString();
                litLifeline.Text += " (" + stringToOrdinal(drData["stateorder"].ToString()) + ")</span></h3>";
                llicon.ImageUrl = "images/icons/statehood.png";
                llicon.AlternateText = "Statehood";
                llicon.ToolTip = "Statehood";
                break;
            case 5:     //population and area
                //litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a state whose population is " + ((int)drData["population"]).ToString("#,##0");
                //litLifeline.Text += " and whose area is " + ((decimal)drData["area"]).ToString("#,##0.00") + " sq. mi.</h3>";
                litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a state whose population is <span class='datum'>" + ((int)drData["population"]).ToString("#,##0") + " (" + stringToOrdinal(drData["poprank"].ToString()) + ")</span>";
                litLifeline.Text += " and whose area is <span class='datum'>" + Convert.ToInt32(drData["area"]).ToString("#,##0") + " sq. mi. (" + stringToOrdinal(drData["arearank"].ToString()) + ")</span></h3>";
                llicon.ImageUrl = "images/icons/population.png";
                llicon.AlternateText = "Population and area";
                llicon.ToolTip = "Population and area";
                break;
            case 6:     //elevation extremes
                //litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a state whose highest point is " + drData["highpoint"].ToString();
                //litLifeline.Text += " at " + drData["highelevation"].ToString() + " feet</h3>";
                litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a state whose low point is <span class='datum'>" + drData["lowpoint"].ToString() + " (" + drData["lowelevation"].ToString() + " ft.)</span>";
                litLifeline.Text += " and whose high point is <span class='datum'>" + drData["highpoint"].ToString() + " (" + drData["highelevation"].ToString() + " ft.)</span></h3>";
                llicon.ImageUrl = "images/icons/elevation.png";
                llicon.AlternateText = "Elevation extremes";
                llicon.ToolTip = "Elevation extremes";
                break;
            case 7:     //state symbols
                litLifeline.Text = "<h3>State Symbols</h3><ul>";
                litLifeline.Text += "<li>Bird: " + drData["bird"].ToString() + "</li>";
                litLifeline.Text += "<li>Flower: " + drData["flower"].ToString() + "</li>";
                litLifeline.Text += "<li>Insect: " + drData["insect"].ToString() + "</li>";
                litLifeline.Text += "<li>Tree: " + drData["tree"].ToString() + "</li></ul>";
                llicon.ImageUrl = "images/icons/symbol.png";
                llicon.AlternateText = "State symbols";
                llicon.ToolTip = "State symbols";
                break;
            case 8:     //nickname
                litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a state whose nickname is <span class='datum'>" + drData["nickname"].ToString() + "</span></h3>";
                llicon.ImageUrl = "images/icons/nickname.png";
                llicon.AlternateText = "State nickname";
                llicon.ToolTip = "State nickname"; break;
            case 9:     //5 largest cities
                litLifeline.Text = "<h3>State's 5 Largest Cities</h3><ol>";
                litLifeline.Text += "<li>" + drData["city1"].ToString() + "</li>";
                litLifeline.Text += "<li>" + drData["city2"].ToString() + "</li>";
                litLifeline.Text += "<li>" + drData["city3"].ToString() + "</li>";
                litLifeline.Text += "<li>" + drData["city4"].ToString() + "</li>";
                litLifeline.Text += "<li>" + drData["city5"].ToString() + "</li></ol>";
                llicon.ImageUrl = "images/icons/nickname.png";
                llicon.AlternateText = "Five largest cities";
                llicon.ToolTip = "Five largest cities";
                break;
            case 10:     //Federal lands
                strAbbr = Session["abbr"].ToString();
                dt = geography.GetUSParks(strAbbr);
                litLifeline.Text = "<h3>The state is home to these federal lands</h3><ul>";
                foreach (DataRow dr in dt.Rows)
                {
                    litLifeline.Text += "<li>" + dr["park"].ToString() + "</li>";
                }
                litLifeline.Text += "</ul>";
                llicon.ImageUrl = "images/icons/federalland.png";
                llicon.AlternateText = "Federal lands";
                llicon.ToolTip = "Federal lands";
                break;
            case 11:     //Physical features
                strAbbr = Session["abbr"].ToString();
                dt = geography.GetUSFeatures(strAbbr);
                litLifeline.Text = "<h3>These physical features are found in the state</h3><ul>";
                foreach (DataRow dr in dt.Rows)
                {
                    litLifeline.Text += "<li>" + dr["feature"].ToString() + "</li>";
                }
                litLifeline.Text += "</ul>";
                llicon.ImageUrl = "images/icons/physicalfeatures.png";
                llicon.AlternateText = "Physical features";
                llicon.ToolTip = "Physical features";
                break;
            case 12:     //shape
                strImage = "<div class=\"shapeContainer\"><img class=\"shape\" src=\"images/shape/" + drData["abbr"].ToString().ToLower() + ".jpg\" alt=\"State Shape\" title=\"State Shape\" onclick=\"zoomBox()\"></div>";
                litLifeline.Text = strImage;
                llicon.ImageUrl = "images/icons/shape.png";
                llicon.AlternateText = "State Shape";
                llicon.ToolTip = "State Shape";
                zoomTitle = "State Shape";
                litZoom.Text = "<img class=\"shape\" src=\"images/shape/zzz.jpg\" alt=\"State Shape\" title=\"State Shape\">";
                litZoom.Text = litZoom.Text.Replace("zzz", drData["abbr"].ToString().ToLower());
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                break;
            case 13:     //waterway map snippet
                strImage = "<div class=\"waterwaysContainer\"><img class=\"waterways\" src=\"images/waterways/" + drData["abbr"].ToString().ToLower() + ".gif\" alt=\"Waterways Map Snippet\" title=\"Waterways Map Snippet\" onclick=\"zoomBox()\"></div>";
                litLifeline.Text = strImage;
                llicon.ImageUrl = "images/icons/waterways.png";
                llicon.AlternateText = "Waterways Map Snippet";
                llicon.ToolTip = "Waterways Map Snippet";
                zoomTitle = "Waterways Map Snippet";
                litZoom.Text = "<img class=\"waterways\" src=\"images/waterways/zzz.gif\" alt=\"Waterways Map Snippet\" title=\"Waterways Map Snippet\">";
                litZoom.Text = litZoom.Text.Replace("zzz", drData["abbr"].ToString().ToLower());
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                break;
            case 999: //Out of lifelines
                litLifeline.Text = "<h3>You are out of lifelines for this destination.</h3>";
                llicon.ImageUrl = "images/icons/sadface.png";
                llicon.AlternateText = "Out of lifelines";
                llicon.ToolTip = "Out of lifelines";
                break;
        }
        llicon.Visible = true;
        litLifeline.Visible = true;
    }

    protected string scoringHistory(string name, int level)
    {
        string player = (name != "%") ? name + "'s" : "All-time";
        string scoringHistory = "<h3>" + player + " top level " + level.ToString() + " scores</h3>";
        scoringHistory += "<table class='scoringHistory'><tr><th>Player</th><th>Score</th><th>Date</th></tr>";
        Geography geography = new Geography();
        DataTable dt = geography.GetScoringHistory(name, level);
        foreach (DataRow row in dt.Rows)
        {
            int scoreId = geography.GetWWScoreId(Session["Players"].ToString().TrimEnd('|').Split('|')[0], Convert.ToInt32(Session["level"]));
            string congrats = (scoreId == Convert.ToInt32(row["id"])) ? " Congratulations!" : "";
            string rowClass = (scoreId == Convert.ToInt32(row["id"])) ? " class='congrats'" : "";
            scoringHistory += "<tr" + rowClass + "><td>" + row["name"] + "</td><td>" + row["score"] + "</td><td>" + ((DateTime)row["playdate"]).ToString("dd-MMM-yy") + congrats + "</td></tr>";
        }
        scoringHistory += "</table>";
        return scoringHistory;
    }

    protected void NewGame_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Default.aspx");
    }
    protected void AnswerButton_Click(object sender, EventArgs e)
    {
        llicon.Visible = false;
        Session["showhistory"] = "";
        if (Session["abbr"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (ddlStates.SelectedItem.Value == "")
        {
            lblResult.Text = "Please select a state";
            return;
        }
        string strAbbr = Session["abbr"].ToString();
        int intRank = Convert.ToInt32((Session["rank"].ToString()));
        int gameScore;
        litLifeline.Text = "";
        string[] players = Session["Players"].ToString().TrimEnd('|').Split('|');
        string[] playerScores = Session["PlayerScores"].ToString().TrimEnd('|').Split('|');
        int playerTurn = (int)Session["PlayerTurn"];
        int newScore;

        //Correct Answer
        if (strAbbr == ddlStates.SelectedItem.Value)
        {
            llicon.ImageUrl = "images/icons/correct.png";
            llicon.AlternateText = "Correct";
            llicon.ToolTip = "Correct";
            lblResult.Text = "Correct!<br /><br />" + Session["name"].ToString().ToUpper() + " is in " + stateFromAbbr(strAbbr).ToUpper();
            int answerValue = (int)Session["answerValue"];
            gameScore = answerValue + (int)Session["score"];
            newScore = Convert.ToInt32(playerScores[playerTurn]) + answerValue;
            playerScores[playerTurn] = newScore.ToString();
            Session["PlayerScores"] = String.Join("|", playerScores);

            //Record and display scores for single players
            int gameLevel = Convert.ToInt32(Session["level"]);
            if ((gameLevel == 12 || gameLevel == 24 || gameLevel == 36 || gameLevel == 48) && (players.Length == 1))
            {
                Geography geography = new Geography();
                geography.PutScore(Session["Players"].ToString().TrimEnd('|').Split('|')[0], DateTime.Now, gameLevel, newScore);
                Session["showhistory"] = Session["Players"].ToString().TrimEnd('|').Split('|')[0];
            }
        }
        //Incorrect Answer
        else
        {
            llicon.ImageUrl = "images/icons/incorrect.png";
            llicon.AlternateText = "Incorrect";
            llicon.ToolTip = "Incorrect";
            lblResult.Text = "I'm sorry, that is Incorrect!<br /><br />" + Session["name"].ToString().ToUpper() + " is in " + stateFromAbbr(strAbbr).ToUpper();
            int gameLevel = -1 + Convert.ToInt32(Session["level"]); // Don't advance to next level
            Session["level"] = gameLevel;
            //int originalAnswerValue = answerBaseScore + (gameLevel * answerIncrement);
            //Penalize wrong answer if single player, otherwise no penalty
            int originalAnswerValue = (players.Length == 1) ? answerBaseScore + (gameLevel * answerIncrement) : 0;
            gameScore = (int)Session["score"] - originalAnswerValue; // Deduct original answer value
            gameScore = Math.Max(gameScore, 0); // Don't allow score to go negative
            newScore = Convert.ToInt32(playerScores[playerTurn]) - originalAnswerValue; // Deduct original answer value;
            newScore = Math.Max(newScore, 0); // Don't allow score to go negative
            playerScores[playerTurn] = newScore.ToString();
        }
        llicon.Visible = true;
        string wikiUrl = @"https://en.wikipedia.org/wiki/" + Session["name"].ToString() + ",_" + stateFromAbbr(strAbbr);
        string wikiLink = "<a href=\"" + wikiUrl + "\" target=\"_blank\">wikipedia</a>";
        string gmapUrl = @"https://www.google.com/maps/place/" + Session["name"].ToString() + ",+" + strAbbr;
        string gmapLink = "<a href=\"" + gmapUrl + "\" target=\"_blank\">google map</a>";
        lblResult.Text += "<br /><br /><br />Learn more about " + Session["name"].ToString() + ":<br /><br />&nbsp;&nbsp;&nbsp;" + wikiLink + " | " + gmapLink;
        Session["score"] = gameScore;

        Session["PlayerScores"] = String.Join("|", playerScores);
        litPlayerScores.Text = "";
        for (int i = 0; i < playerScores.Length; i++)
        {
            litPlayerScores.Text += "<td class='playerScore'>" + playerScores[i] + "</th>";
        }
        int[] intArrLifeline = RandomIntegerArray(lifelineCount);
        string strLifelines = string.Join("|", Array.ConvertAll<int, String>(intArrLifeline, Convert.ToString));
        Session["lifelines"] = strLifelines;
        Session["llIndex"] = 0;
        playerTurn = (playerTurn + 1) % players.Length;
        Session["PlayerTurn"] = playerTurn;

        btnLifeline.Visible = false;
        btnNext.Visible = true;
    }

    protected void NextButton_Click(object sender, EventArgs e)
    {
        if (Session["showhistory"] != null && Session["showhistory"].ToString() != "")
        {
            btnLifeline.Visible = false;
            btnNext.Visible = true;
            lblResult.Text = "";
            litLifeline.Text = scoringHistory(Session["showhistory"].ToString(), Convert.ToInt32(Session["level"]));
            litLifeline.Visible = true;
            Session["showhistory"] = (Session["showhistory"].ToString() == "%" ? "" : "%");
        }
        else
        {
            Response.Redirect("Default.aspx");
        }
    }

    public static string stringToOrdinal(string strInt)
    {
        int num = Convert.ToInt32(strInt);
        if (num <= 0) return num.ToString();

        switch (num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num + "th";
        }

        switch (num % 10)
        {
            case 1:
                return num + "st";
            case 2:
                return num + "nd";
            case 3:
                return num + "rd";
            default:
                return num + "th";
        }
    }

    public string DDToDMS(double latitude, double longitude)
    {
        int latSeconds = (int)Math.Round(latitude * 3600);
        int latDegrees = latSeconds / 3600;
        latSeconds = Math.Abs(latSeconds % 3600);
        int latMinutes = latSeconds / 60;
        latSeconds %= 60;

        int longSeconds = (int)Math.Round(longitude * 3600);
        int longDegrees = longSeconds / 3600;
        longSeconds = Math.Abs(longSeconds % 3600);
        int longMinutes = longSeconds / 60;
        longSeconds %= 60;

        //string strDMS = Math.Abs(latDegrees) + "° " + latMinutes + "' " + latSeconds + "\" " + (latDegrees >= 0 ? "N" : "S") + ", ";
        //strDMS += Math.Abs(longDegrees) + "° " + longMinutes + "' " + longSeconds + "\" " + (longDegrees >= 0 ? "E" : "W");
        string strDMS = Math.Abs(latDegrees) + "°" +latMinutes + "'" + (latDegrees >= 0 ? "N" : "S") + ", ";
        strDMS += Math.Abs(longDegrees) + "°" + longMinutes + "'" + (longDegrees >= 0 ? "E" : "W");
        return strDMS;
    }

    public string stateFromAbbr(string abbr)
    {
        Dictionary<string, string> states = new Dictionary<string, string>();

        states.Add("AL", "Alabama");
        states.Add("AK", "Alaska");
        states.Add("AZ", "Arizona");
        states.Add("AR", "Arkansas");
        states.Add("CA", "California");
        states.Add("CO", "Colorado");
        states.Add("CT", "Connecticut");
        states.Add("DE", "Delaware");
        states.Add("DC", "District of Columbia");
        states.Add("FL", "Florida");
        states.Add("GA", "Georgia");
        states.Add("HI", "Hawaii");
        states.Add("ID", "Idaho");
        states.Add("IL", "Illinois");
        states.Add("IN", "Indiana");
        states.Add("IA", "Iowa");
        states.Add("KS", "Kansas");
        states.Add("KY", "Kentucky");
        states.Add("LA", "Louisiana");
        states.Add("ME", "Maine");
        states.Add("MD", "Maryland");
        states.Add("MA", "Massachusetts");
        states.Add("MI", "Michigan");
        states.Add("MN", "Minnesota");
        states.Add("MS", "Mississippi");
        states.Add("MO", "Missouri");
        states.Add("MT", "Montana");
        states.Add("NE", "Nebraska");
        states.Add("NV", "Nevada");
        states.Add("NH", "New Hampshire");
        states.Add("NJ", "New Jersey");
        states.Add("NM", "New Mexico");
        states.Add("NY", "New York");
        states.Add("NC", "North Carolina");
        states.Add("ND", "North Dakota");
        states.Add("OH", "Ohio");
        states.Add("OK", "Oklahoma");
        states.Add("OR", "Oregon");
        states.Add("PA", "Pennsylvania");
        states.Add("RI", "Rhode Island");
        states.Add("SC", "South Carolina");
        states.Add("SD", "South Dakota");
        states.Add("TN", "Tennessee");
        states.Add("TX", "Texas");
        states.Add("UT", "Utah");
        states.Add("VT", "Vermont");
        states.Add("VA", "Virginia");
        states.Add("WA", "Washington");
        states.Add("WV", "West Virginia");
        states.Add("WI", "Wisconsin");
        states.Add("WY", "Wyoming");
        if (states.ContainsKey(abbr))
            return (states[abbr]);
        /* error handler is to return an empty string rather than throwing an exception */
        return "";
    }

    public int[] RandomIntegerArray(int size)  //starts with 0
    {
        int[] array = new int[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = i;
        }
        Shuffle(array);
        return array;
    }

    /// <summary>
    /// Knuth shuffle
    /// </summary>        
    public void Shuffle(int[] array)
    {
        Random random = new Random();
        int n = array.Count();
        while (n > 1)
        {
            n--;
            int i = random.Next(n + 1);
            int temp = array[i];
            array[i] = array[n];
            array[n] = temp;
        }
    }
}