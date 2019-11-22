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
    protected int lifelineCount = 16;
    public string zoomTitle;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["wwPlayers"] == null || Session["wwPlayerScores"] == null || Session["wwPlayerTurn"] == null)
            {
                getPlayers.Visible = true;
                playGame.Visible = false;
            }
            else
            {
                getPlayers.Visible = false;
                playGame.Visible = true;

                string[] wwPlayers = Session["wwPlayers"].ToString().TrimEnd('|').Split('|');
                string[] wwPlayerScores = Session["wwPlayerScores"].ToString().TrimEnd('|').Split('|');
                int playerCount = wwPlayers.Length;
                int playerTurn = (int)Session["wwPlayerTurn"];
                int gameScore = (Session["wwscore"] != null) ? (int)Session["wwscore"] : 0;
                Session["wwscore"] = gameScore;

                int gameLevel = (Session["wwlevel"] != null) ? 1 + Convert.ToInt32(Session["wwlevel"]) : 1;
                Session["wwlevel"] = gameLevel;

                if (gameLevel > 48)
                {
                    lblResult.Text = "Congratulations...  You have reached the highest level!";
                    litAnswerValue.Text = Session["wwanswerValue"].ToString();
                    for (int i = 0; i < playerCount; i++)
                    {
                        litPlayerNames.Text += "<th class='playerName'>" + wwPlayers[i] + "</th>";
                        litPlayerScores.Text += "<td class='playerScore'>" + wwPlayerScores[i] + "</td>";
                    }
                    Session.Abandon();
                    return;
                }

                int answerValue = answerBaseScore + (gameLevel - 1) * answerIncrement;
                Session["wwanswerValue"] = answerValue;
                litAnswerValue.Text = answerValue.ToString();
                for (int i = 0; i < playerCount; i++)
                {
                    string activeClass = ((i == playerTurn) ? " active" : "");
                    litPlayerNames.Text += "<th class='playerName" + activeClass + "'>" + wwPlayers[i] + "</th>";
                    litPlayerScores.Text += "<td class='playerScore'>" + wwPlayerScores[i] + "</td>";
                }

                litLifeline.Visible = false;
                Geography geography = new Geography();
                int effectiveLevel = (int)Math.Ceiling((decimal)gameLevel / 4);
                List<int> prevDestList = (List<int>)Session["wwPrevDestList"];
                DataRow drDestination = geography.GetRandomWWDestination(effectiveLevel);
                string strId = drDestination["id"].ToString();
                int intId = Convert.ToInt32(strId);
                prevDestList.Add(intId);
                string strA2 = drDestination["a2"].ToString();
                string strCountry = drDestination["country"].ToString();
                string strLevel = drDestination["level"].ToString();
                string strName = drDestination["destination"].ToString();
                string strDescription = drDestination["description"].ToString();
                int[] intArrLifeline = RandomIntegerArray(lifelineCount);
                string strLifelines = string.Join("|", Array.ConvertAll<int, String>(intArrLifeline, Convert.ToString));

                Session["wwid"] = strId;
                Session["wwPrevDestList"] = prevDestList;
                Session["wwa2"] = strA2;
                Session["wwcountry"] = strCountry;
                //Session["wwlevel"] = strLevel;
                Session["wwname"] = strName;
                Session["wwdescription"] = strDescription;
                Session["wwlifelines"] = strLifelines;
                Session["wwllIndex"] = 0;
                destName.Text = Session["wwlevel"].ToString() + ". " + strName;
                destDescription.Text = strDescription;
                //destImg.Text = "<img src=\"" + "images/destimages/" + strA2 + "/" + strLevel + "-" + strName.Replace(" ", "-").Replace(".", "") + ".jpg\" class=\"destImg\">";
                destImg.Text = "<img src=\"" + "images/destimages/" + strA2 + "/" + drDestination["imagefilename"].ToString() + "\" class=\"destImg\">";
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
        Session["wwPlayers"] = players;
        Session["wwPlayerScores"] = playerScores;
        Session["wwPlayerTurn"] = 0;
        Session["wwPrevDestList"] = new List<int>();
        Response.Redirect("Default.aspx");
    }

    protected void Lifeline_Click(object sender, EventArgs e)
    {
        llicon.Visible = false;
        if (Session["wwllIndex"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        litLifeline.Visible = false;
        //Random r = new Random();
        //int lifelineType = r.Next(1, 10);
        int llIndex = (int)Session["wwllIndex"];
        string[] strArrLifelines = (Session["wwlifelines"].ToString()).Split('|');
        int lifelineType;
        if (llIndex >= strArrLifelines.Length)
        {
            lifelineType = 999;
        }
        else
        {
            lifelineType = 1 + Convert.ToInt32(strArrLifelines[llIndex]);
            Session["wwllIndex"] = 1 + llIndex;
        }
        int answerValue = (int)Session["wwanswerValue"];
        if (answerValue < lifelineCost)
        {
            lifelineType = 999;
        }
        else
        {
            answerValue -= lifelineCost;
            litAnswerValue.Text = answerValue.ToString();
            Session["wwanswerValue"] = answerValue;
        }

        int intId = Convert.ToInt32(Session["wwid"]);
        Geography geography = new Geography();
        DataRow drData = geography.GetWWDestination(intId);
        string strImage, strA2;
        DataTable dt;
        switch (lifelineType)
        {
            case 1:     //latlng
                //litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a country located at (" + drData["lat"].ToString() + ", " + drData["lng"].ToString() + ")</h3>";
                litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a country located at <span class='datum'>" + DDToDMS(Convert.ToDouble(drData["lat"].ToString()), Convert.ToDouble(drData["lng"].ToString())) + "</span></h3>";
                llicon.ImageUrl = "images/icons/latlng.png";
                llicon.AlternateText = "Latitude and longitude";
                llicon.ToolTip = "Latitude and longitude";
                break;
            case 2:     //flag
                strImage = "<img class=\"flag\" src=\"" + drData["flagurl"].ToString() + "\" alt=\"Country Flag\" title=\"Country Flag\" onclick=\"zoomBox()\" >";
                litLifeline.Text = strImage;
                llicon.ImageUrl = "images/icons/flag.png";
                llicon.AlternateText = "Flag";
                llicon.ToolTip = "Flag";
                zoomTitle = "Country Flag";
                litZoom.Text = "<img class=\"flag\" src=\"" + drData["flagurl"].ToString() + "\" alt =\"Country Flag\" title=\"Country Flag\">";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                break;
            case 3:     //locator
                strImage = "<img class=\"locator\" src=\"" + drData["locurl"].ToString() + "\" alt=\"Country Locator\" title=\"Country Locator\" onclick=\"zoomBox()\" >";
                litLifeline.Text = strImage;
                llicon.ImageUrl = "images/icons/locator.png";
                llicon.AlternateText = "Locator map";
                llicon.ToolTip = "Locator map";
                zoomTitle = "Country Locator Map";
                litZoom.Text = "<img class=\"locator\" src=\"" + drData["locurl"].ToString() + "\" alt=\"Country Locator\" title=\"Country Locator\" >";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                break;
            case 4:     //map
                strImage = "<img class=\"map\" src=\"" + drData["mapurl"].ToString() + "\" alt=\"Country Map\" title=\"Country Map\" onclick=\"zoomBox()\" >";
                litLifeline.Text = strImage;
                llicon.ImageUrl = "images/icons/map.png";
                llicon.AlternateText = "Country map";
                llicon.ToolTip = "Country map";
                zoomTitle = "Country Map";
                litZoom.Text = "<img class=\"map\" src=\"" + drData["mapurl"].ToString() + "\" alt=\"Country Map\" title=\"Country Map\" >";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                break;
            case 5:     //population and area
                //litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a country whose population is " + ((int)drData["population"]).ToString("#,##0") + " (" + drData["poprank"].ToString() + ")";
                //litLifeline.Text += " and whose area is " + ((int)drData["area"]).ToString("#,##0") + " sq. mi. (" + drData["arearank"].ToString() + ")</h3>";
                litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a country whose population is <span class='datum'>" + ((int)drData["population"]).ToString("#,##0") + " (" + stringToOrdinal(drData["poprank"].ToString()) + ")</span>";
                litLifeline.Text += " and whose area is <span class='datum'>" + Convert.ToInt32(drData["area"]).ToString("#,##0") + " sq. mi. (" + stringToOrdinal(drData["arearank"].ToString()) + ")</span></h3>";
                llicon.ImageUrl = "images/icons/population.png";
                llicon.AlternateText = "Population and area";
                llicon.ToolTip = "Population and area";
                break;
            case 6:     //elevation extremes
                //litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a country whose low point is " + drData["lowpoint"].ToString() + " (" + drData["lowelevation"].ToString() + " m.)";
                //litLifeline.Text += " and whose high point is " + drData["highpoint"].ToString() + " (" + drData["highelevation"].ToString() + " m.)</h3>";
                litLifeline.Text = "<h3>" + drData["destination"].ToString() + " is in a country whose low point is <span class='datum'>" + drData["lowpoint"].ToString() + " (" + drData["lowelevation"].ToString() + " m.)</span>";
                litLifeline.Text += " and whose high point is <span class='datum'>" + drData["highpoint"].ToString() + " (" + drData["highelevation"].ToString() + " m.)</span></h3>";
                llicon.ImageUrl = "images/icons/elevation.png";
                llicon.AlternateText = "Elevation extremes";
                llicon.ToolTip = "Elevation extremes";
                break;
            case 7:     //terrain and climate
                string terrain = drData["terrain"].ToString();
                terrain = terrain.Substring(0, 1).ToLower() + terrain.Substring(1, terrain.Length - 1); //uncapitalize first letter
                string climate = drData["climate"].ToString();
                climate = climate.Substring(0, 1).ToLower() + climate.Substring(1, climate.Length - 1); //uncapitalize first letter
                litLifeline.Text = "<h4>The country's terrain is <span class='datum'>" + terrain + "</span></h4>";
                litLifeline.Text += "<h4>The climate is <span class='datum'>" + climate + "</span></h4>";
                llicon.ImageUrl = "images/icons/terrain.png";
                llicon.AlternateText = "Terrain and climate";
                llicon.ToolTip = "Terrain and climate";
                break;
            case 8:     //religions
                litLifeline.Text = "<h3>The country's religions are <span class='datum'>" + drData["religions"].ToString() + "</span></h3>";
                llicon.ImageUrl = "images/icons/religion.png";
                llicon.AlternateText = "Religions";
                llicon.ToolTip = "Religions";
                break;
            case 9:     //languages
                litLifeline.Text = "<h3>The country's main languages include <span class='datum'>" + drData["languages"].ToString() + "</span></h3>";
                llicon.ImageUrl = "images/icons/language.png";
                llicon.AlternateText = "Main languages";
                llicon.ToolTip = "Main languages";
                break;
            case 10:     //currency
                litLifeline.Text = "<h3>The country's monetary unit is the <span class='datum'>" + drData["currency"].ToString() + "</span></h3>";
                llicon.ImageUrl = "images/icons/currency.png";
                llicon.AlternateText = "Currency";
                llicon.ToolTip = "Currency";
                break;
            case 11:     //notes
                litLifeline.Text = "<h4>" + drData["note"].ToString() + "</h4>";
                llicon.ImageUrl = "images/icons/note.png";
                llicon.AlternateText = "Note";
                llicon.ToolTip = "Note";
                break;
            case 12:     //shape
                strImage = "<div class=\"shapeContainer\"><img class=\"shape\" src=\"images/shape/" + drData["a2"].ToString() + ".jpg\" alt=\"Country Shape\" title=\"Country Shape\" onclick=\"zoomBox()\"></div>";
                litLifeline.Text = strImage;
                llicon.ImageUrl = "images/icons/shape.png";
                llicon.AlternateText = "Country Shape";
                llicon.ToolTip = "Country Shape";
                zoomTitle = "Country Locator Map";
                litZoom.Text = "<img class=\"shape\" src=\"images/shape/" + drData["a2"].ToString() + ".jpg\" alt=\"Country Shape\" title=\"Country Shape\" >";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                break;
            case 13:     //5 Largest Cities
                strA2 = Session["wwa2"].ToString();
                dt = geography.GetWWCities(strA2);
                litLifeline.Text = "<h3>The Country's 5 Largest Cities:</h3>";
                litLifeline.Text += "<table class='cities'><th>Rank</th><th>City</th><th>Population</th></tr>";
                foreach (DataRow dr in dt.Rows)
                {
                    litLifeline.Text += "<tr><td>" + dr["rank"].ToString() + "</td><td>" + dr["city"].ToString() + "</td><td>" + ((int)dr["population"]).ToString("N0") + "</td></tr>";
                }
                litLifeline.Text += "</table>";
                llicon.ImageUrl = "images/icons/city.png";
                llicon.AlternateText = "Five largest cities";
                llicon.ToolTip = "Five largest cities";
                break;
            case 14:     //resources
                litLifeline.Text = "<h4>The country's natural resources include <span class='datum'>" + drData["resources"].ToString() + "</span></h4>";
                litLifeline.Text += "<h4>Major crops are <span class='datum'>" + drData["crops"].ToString() + "</span></h4>";
                llicon.ImageUrl = "images/icons/resources.png";
                llicon.AlternateText = "Natural resources and crops";
                llicon.ToolTip = "Natural resources and crops";
                break;
            case 15:     //National capital
                litLifeline.Text = "<h3>The national capital is <span class='datum'>" + drData["capital"].ToString() + "</span></h3>";
                llicon.ImageUrl = "images/icons/capital.png";
                llicon.AlternateText = "National capital";
                llicon.ToolTip = "National capital";
                break;
            case 16:     //Neighboring countries
                dt = geography.GetWWNeighbors(drData["a2"].ToString());
                string tblClass = (dt.Rows.Count > 10) ? "neighbors condense" : "neighbors";
                litLifeline.Text = "<h3 class='neighbors'>Neighboring countries are</h3>";
                litLifeline.Text += "<table class='" + tblClass + "'><th>Country</th><th>Border Length</th></tr>";
                foreach (DataRow dr in dt.Rows)
                {
                    litLifeline.Text += "<tr><td>" + dr["neighbor"].ToString() + "</td><td>" + dr["neighborborder"].ToString() + " km</td></tr>";
                }
                litLifeline.Text += "</table>";
                llicon.ImageUrl = "images/icons/neighbors.png";
                llicon.AlternateText = "Neighboring countries";
                llicon.ToolTip = "Neighboring countries";
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
        DataTable dt = geography.GetWWScoringHistory(name, level);
        foreach (DataRow row in dt.Rows)
        {
            int scoreId = geography.GetWWScoreId(Session["wwPlayers"].ToString().TrimEnd('|').Split('|')[0], Convert.ToInt32(Session["wwlevel"]));
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
        Session["wwshowhistory"] = "";
        if (Session["wwa2"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (txtCountry.Text == "")
        {
            lblResult.Text = "Please select a country";
            return;
        }
        //string strA2 = Session["wwa2"].ToString();
        string strCountry = Session["wwcountry"].ToString();
        int intLevel = Convert.ToInt32((Session["wwlevel"].ToString()));
        int gameScore;
        //int gameMaxScore = Session["wwmaxScore"] != null ? (int)Session["wwmaxScore"] : 0;
        litLifeline.Text = "";
        string[] wwPlayers = Session["wwPlayers"].ToString().TrimEnd('|').Split('|');
        string[] wwPlayerScores = Session["wwPlayerScores"].ToString().TrimEnd('|').Split('|');
        int playerTurn = (int)Session["wwPlayerTurn"];
        int newScore;

        //Correct Answer
        if (strCountry == txtCountry.Text)
        {
            llicon.ImageUrl = "images/icons/correct.png";
            llicon.AlternateText = "Correct";
            llicon.ToolTip = "Correct";
            lblResult.Text = "Correct!<br /><br />" + Session["wwname"].ToString().ToUpper() + " is in " + txtCountry.Text;
            int answerValue = (int)Session["wwanswerValue"];
            gameScore = answerValue + (int)Session["wwscore"];
            newScore = Convert.ToInt32(wwPlayerScores[playerTurn]) + answerValue;
            wwPlayerScores[playerTurn] = newScore.ToString();
            Session["wwPlayerScores"] = String.Join("|", wwPlayerScores);

            //Record and display scores for single players
            int gameLevel = Convert.ToInt32(Session["wwlevel"]);
            if ((gameLevel == 12 || gameLevel == 24 || gameLevel == 36 || gameLevel == 48) && (wwPlayers.Length == 1))
            {
                Geography geography = new Geography();
                geography.PutWWScore(Session["wwPlayers"].ToString().TrimEnd('|').Split('|')[0], DateTime.Now, gameLevel, gameScore);
                Session["wwshowhistory"] = Session["wwPlayers"].ToString().TrimEnd('|').Split('|')[0];
            }
        }
        //Incorrect Answer
        else
        {
            llicon.ImageUrl = "images/icons/incorrect.png";
            llicon.AlternateText = "Inorrect";
            llicon.ToolTip = "Inorrect";
            lblResult.Text = "I'm sorry, that is Incorrect!<br /><br />" + Session["wwname"].ToString() + " is in " + Session["wwcountry"].ToString();
            int gameLevel = -1 + Convert.ToInt32(Session["wwlevel"]); // Don't advance to next level
            Session["wwlevel"] = gameLevel;
            //int originalAnswerValue = answerBaseScore + (gameLevel * answerIncrement);
            //Penalize wrong answer if single player, otherwise no penalty
            int originalAnswerValue = (wwPlayers.Length == 1) ? answerBaseScore + (gameLevel * answerIncrement) : 0;
            gameScore = (int)Session["wwscore"] - originalAnswerValue; // Deduct original answer value
            gameScore = Math.Max(gameScore, 0); // Don't allow score to go negative
            newScore = Convert.ToInt32(wwPlayerScores[playerTurn]) - originalAnswerValue; // Deduct original answer value;
            newScore = Math.Max(newScore, 0); // Don't allow score to go negative
            wwPlayerScores[playerTurn] = newScore.ToString();
        }
        llicon.Visible = true;
        string wikiUrl = @"https://en.wikipedia.org/wiki/" + Session["wwname"].ToString() + ",_" + Session["wwcountry"].ToString();
        string wikiLink = "<a href=\"" + wikiUrl + "\" target=\"_blank\">wikipedia</a>";
        string gmapUrl = @"https://www.google.com/maps/place/" + Session["wwname"].ToString() + ",+" + Session["wwcountry"].ToString();
        string gmapLink = "<a href=\"" + gmapUrl + "\" target=\"_blank\">google map</a>";
        lblResult.Text += "<br /><br /><br />Learn more about " + Session["wwname"].ToString() + ":<br /><br />&nbsp;&nbsp;&nbsp;" + wikiLink + " | " + gmapLink;
        Session["wwscore"] = gameScore;
        Session["wwPlayerScores"] = String.Join("|", wwPlayerScores);
        litPlayerScores.Text = "";
        for (int i = 0; i < wwPlayerScores.Length; i++)
        {
            //litPlayerNames.Text += "<th class='playerName'>" + wwPlayers[i] + "</th>";
            litPlayerScores.Text += "<td class='playerScore'>" + wwPlayerScores[i] + "</th>";
        }

        int[] intArrLifeline = RandomIntegerArray(lifelineCount);
        string strLifelines = string.Join("|", Array.ConvertAll<int, String>(intArrLifeline, Convert.ToString));
        Session["wwlifelines"] = strLifelines;
        Session["wwllIndex"] = 0;
        playerTurn = (playerTurn + 1) % wwPlayers.Length;
        Session["wwPlayerTurn"] = playerTurn;

        btnLifeline.Visible = false;
        btnNext.Visible = true;
    }

    protected void NextButton_Click(object sender, EventArgs e)
    {
        if (Session["wwshowhistory"] != null && Session["wwshowhistory"].ToString() != "")
        {
            btnLifeline.Visible = false;
            btnNext.Visible = true;
            lblResult.Text = "";
            litLifeline.Text = scoringHistory(Session["wwshowhistory"].ToString(), Convert.ToInt32(Session["wwlevel"]));
            litLifeline.Visible = true;
            Session["wwshowhistory"] = (Session["wwshowhistory"].ToString() == "%" ? "" : "%");
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
        string strDMS = Math.Abs(latDegrees) + "°" + latMinutes + "'" + (latDegrees >= 0 ? "N" : "S") + ", ";
        strDMS += Math.Abs(longDegrees) + "°" + longMinutes + "'" + (longDegrees >= 0 ? "E" : "W");
        return strDMS;
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