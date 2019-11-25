using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;

public partial class uswego_Explore : System.Web.UI.Page
{
    protected int stateCount = 50;
    protected int lifelineCount = 16;
    protected int destinationCount = 48;
    public string zoomTitle;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
            if (Session["stateindex"] == null || Session["lifelineindex"] == null)
            {
                //ddlStates.SelectedIndex = randomInt(0, stateCount);
                //ddlLifelines.SelectedIndex = randomInt(0, lifelineCount);
                Session["stateindex"] = ddlStates.SelectedIndex;
                Session["lifelineindex"] = ddlLifelines.SelectedIndex;
            }
        //}
        btnLifeline.Visible = true;
    }

    protected void Lifeline_Click(object sender, EventArgs e)
    {
        string strAbbr = ddlStates.SelectedValue;
        int lifelineType = Convert.ToInt32(ddlLifelines.SelectedValue);
        Session["stateindex"] = ddlStates.SelectedIndex;
        Session["lifelineindex"] = ddlLifelines.SelectedIndex;
        showLifeline(strAbbr, lifelineType);
    }

    protected void PrevLL_Click(object sender, EventArgs e)
    {
        ddlStates.SelectedIndex = (int)Session["stateindex"] ;
        ddlLifelines.SelectedIndex = mod((-1 + (int)Session["lifelineindex"]), (lifelineCount + destinationCount));
        string strAbbr = ddlStates.SelectedValue;
        int lifelineType = Convert.ToInt32(ddlLifelines.SelectedValue);
        Session["stateindex"] = ddlStates.SelectedIndex;
        Session["lifelineindex"] = ddlLifelines.SelectedIndex;
        showLifeline(strAbbr, lifelineType);
    }

    protected void NextLL_Click(object sender, EventArgs e)
    {
        ddlStates.SelectedIndex = (int)Session["stateindex"];
        ddlLifelines.SelectedIndex = (1 + (int)Session["lifelineindex"]) % (lifelineCount + destinationCount);
        string strAbbr = ddlStates.SelectedValue;
        int lifelineType = Convert.ToInt32(ddlLifelines.SelectedValue);
        Session["stateindex"] = ddlStates.SelectedIndex;
        Session["lifelineindex"] = ddlLifelines.SelectedIndex;
        showLifeline(strAbbr, lifelineType);
    }

    protected void PrevST_Click(object sender, EventArgs e)
    {
        ddlStates.SelectedIndex = mod((-1 + (int)Session["stateindex"]), stateCount);
        ddlLifelines.SelectedIndex = (int)Session["lifelineindex"];
        string strAbbr = ddlStates.SelectedValue;
        int lifelineType = Convert.ToInt32(ddlLifelines.SelectedValue);
        Session["stateindex"] = ddlStates.SelectedIndex;
        Session["lifelineindex"] = ddlLifelines.SelectedIndex;
        showLifeline(strAbbr, lifelineType);
    }

    protected void NextST_Click(object sender, EventArgs e)
    {
        ddlStates.SelectedIndex = (1 + (int)Session["stateindex"]) % stateCount;
        ddlLifelines.SelectedIndex = (int)Session["lifelineindex"];
        string strAbbr = ddlStates.SelectedValue;
        int lifelineType = Convert.ToInt32(ddlLifelines.SelectedValue);
        Session["stateindex"] = ddlStates.SelectedIndex;
        Session["lifelineindex"] = ddlLifelines.SelectedIndex;
        showLifeline(strAbbr, lifelineType);
    }

    protected void RandLL_Click(object sender, EventArgs e)
    {
        ddlStates.SelectedIndex = randomInt(0, stateCount - 1);
        ddlLifelines.SelectedIndex = randomInt(0, lifelineCount + destinationCount - 1);
        string strAbbr = ddlStates.SelectedValue;
        int lifelineType = Convert.ToInt32(ddlLifelines.SelectedValue);
        Session["stateindex"] = ddlStates.SelectedIndex;
        Session["lifelineindex"] = ddlLifelines.SelectedIndex;
        showLifeline(strAbbr, lifelineType);
    }

    protected void showLifeline(string strAbbr, int lifelineType)
    {
        Geography geography = new Geography();
        DataRow drData = geography.GetState(strAbbr);
        string strImage, strText;
        DataTable dt;
        DataRow dd;
        switch (lifelineType)
        {
            case 1:     //latlng
                litLifeline.Text = "<h3>" + drData["name"].ToString() + " is located at (" + drData["statelat"].ToString() + ", " + drData["statelng"].ToString() + ")</h3>";
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
                litLifeline.Text = "<h3>" + drData["name"].ToString() + " gained statehood on <span class='datum'>" + drData["statehood"].ToString();
                litLifeline.Text += " (" + stringToOrdinal(drData["stateorder"].ToString()) + ")</span></h3>";
                llicon.ImageUrl = "images/icons/statehood.png";
                llicon.AlternateText = "Statehood";
                llicon.ToolTip = "Statehood";
                break;
            case 5:     //population and area
                litLifeline.Text = "<h3>" + drData["name"].ToString() + " has a population of <span class='datum'>" + ((int)drData["population"]).ToString("#,##0") + " (" + stringToOrdinal(drData["poprank"].ToString()) + ")</span>";
                litLifeline.Text += " and an area of <span class='datum'>" + Convert.ToInt32(drData["area"]).ToString("#,##0") + " sq. mi. (" + stringToOrdinal(drData["arearank"].ToString()) + ")</span></h3>";
                llicon.ImageUrl = "images/icons/population.png";
                llicon.AlternateText = "Population and area";
                llicon.ToolTip = "Population and area";
                break;
            case 6:     //elevation extremes
                litLifeline.Text = "<h3>" + drData["name"].ToString() + " has a low point of <span class='datum'>" + drData["lowpoint"].ToString() + " (" + drData["lowelevation"].ToString() + " ft.)</span>";
                litLifeline.Text += " and a high point of <span class='datum'>" + drData["highpoint"].ToString() + " (" + drData["highelevation"].ToString() + " ft.)</span></h3>";
                llicon.ImageUrl = "images/icons/elevation.png";
                llicon.AlternateText = "Elevation extremes";
                llicon.ToolTip = "Elevation extremes";
                break;
            case 7:     //state symbols
                litLifeline.Text = "<h3>" + drData["name"].ToString() + " State Symbols</h3><ul>";
                litLifeline.Text += "<li>Bird: " + drData["bird"].ToString() + "</li>";
                litLifeline.Text += "<li>Flower: " + drData["flower"].ToString() + "</li>";
                litLifeline.Text += "<li>Insect: " + drData["insect"].ToString() + "</li>";
                litLifeline.Text += "<li>Tree: " + drData["tree"].ToString() + "</li></ul>";
                llicon.ImageUrl = "images/icons/symbol.png";
                llicon.AlternateText = "State symbols";
                llicon.ToolTip = "State symbols";
                break;
            case 8:     //nickname
                litLifeline.Text = "<h3>" + drData["name"].ToString() + " is known as <span class='datum'>" + drData["nickname"].ToString() + "</span></h3>";
                llicon.ImageUrl = "images/icons/nickname.png";
                llicon.AlternateText = "State nickname";
                llicon.ToolTip = "State nickname"; break;
            case 9:     //10 largest cities
                dt = geography.GetUSCities(drData["abbr"].ToString(), 10);
                litLifeline.Text = "<h3>" + drData["name"].ToString() + "'s 10 Largest Cities:</h3>";
                litLifeline.Text += "<table class='cities'><th>Rank</th><th>City</th><th>Population</th></tr>";
                foreach (DataRow dr in dt.Rows)
                {
                    Boolean isCapital = Convert.ToBoolean(dr["iscapital"]);
                    string pretag = isCapital ? "<strong>" : "";
                    string posttag = isCapital ? "</strong>" : "";
                    litLifeline.Text += "<tr><td>" + dr["rank"].ToString() + "</td><td>" + pretag + dr["city"].ToString() + posttag + "</td><td>" + ((int)dr["population"]).ToString("N0") + "</td></tr>";
                }
                litLifeline.Text += "</table>";
                llicon.ImageUrl = "images/icons/city.png";
                llicon.AlternateText = "Ten largest cities";
                llicon.ToolTip = "Tene largest cities";
                break;
            case 10:     //Federal lands
                //strAbbr = Session["abbr"].ToString();
                dt = geography.GetUSParks(strAbbr);
                litLifeline.Text = "<h3>" + drData["name"].ToString() + " is home to these federal lands</h3><ul>";
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
                //strAbbr = Session["abbr"].ToString();
                dt = geography.GetUSFeatures(strAbbr);
                litLifeline.Text = "<h3>These physical features are found in " + drData["name"].ToString() + "</h3><ul>";
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
            case 14:     //biggest company
                litLifeline.Text = "<h3>The largest company headquartered in " + drData["name"].ToString() + " is <span class='datum'>" + drData["bigco"].ToString() + " (" + drData["bigcosize"].ToString() + ")</span></h3>";
                llicon.ImageUrl = "images/icons/bigco.png";
                llicon.AlternateText = "Largest company";
                llicon.ToolTip = "Largest company";
                break;
            case 15:     //capital
                litLifeline.Text = "<h3>The state capital is <span class='datum'>" + drData["capital"].ToString() + "</span></h3>";
                llicon.ImageUrl = "images/icons/capital.png";
                llicon.AlternateText = "State capital";
                llicon.ToolTip = "State capital";
                break;
            case 16:     //Neighboring states
                dt = geography.GetUSNeighbors(strAbbr);
                litLifeline.Text = "<h3>These states border " + drData["name"].ToString() + "</h3><ul>";
                foreach (DataRow dr in dt.Rows)
                {
                    litLifeline.Text += "<li>" + dr["neighbor"].ToString() + "</li>";
                }
                litLifeline.Text += "</ul>";
                llicon.ImageUrl = "images/icons/neighbors.png";
                llicon.AlternateText = "Neighboring states";
                llicon.ToolTip = "Neighboring states";
                break;
            default:
                if (lifelineType > 100 && lifelineType <= 100 + destinationCount)
                {
                    int intRank = lifelineType - 100;
                    dd = geography.GetDestination(strAbbr, intRank);
                    strText = "<p><span class='dest'>" + intRank.ToString() + ". <a target=\"_blank\" href=\"https://en.wikipedia.org/wiki/" + (dd["destination"].ToString() + ",_" + drData["name"].ToString()).Replace(" ","_") + "\">" + dd["destination"].ToString() + "</a></span> - <span class='desc'>" + dd["description"].ToString() + "</span></p>";
                    strImage = "<img class='destImg' src=\"" + "images/destimages/" + strAbbr + "/" + intRank.ToString() + "-" + dd["destination"].ToString().Replace(" ", "-").Replace(".", "") + ".jpg\" class=\"destImg\">";
                    litLifeline.Text = strText + strImage;
                    llicon.ImageUrl = "images/icons/destination.png";
                    llicon.AlternateText = "Destination";
                    llicon.ToolTip = "Destination";
                    zoomTitle = "Destination #" + intRank.ToString();
                    litZoom.Text = strImage;
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                }
                break;
        }
        litTitle.Text = ddlStates.SelectedItem + " &mdash; " + ddlLifelines.SelectedItem;
        llicon.Visible = true;
        litLifeline.Visible = true;
        string wikiUrl = @"https://en.wikipedia.org/wiki/" + drData["name"].ToString();
        string wikiLink = "<a href=\"" + wikiUrl + "\" target=\"_blank\">wikipedia</a>";
        string gmapUrl = @"https://www.google.com/maps/place/" + drData["name"].ToString();
        string gmapLink = "<a href=\"" + gmapUrl + "\" target=\"_blank\">google map</a>";
        string articlesUrl = @"http://filbert.com/geobee/countries/usstates.htm#" + drData["abbr"].ToString();
        string articlesLink = "<a href=\"" + articlesUrl + "\" target=\"_blank\">state articles</a>";
        string mapsUrl = @"http://filbert.com/geobee/countries/usmaps.htm#" + drData["abbr"].ToString();
        string mapsLink = "<a href=\"" + mapsUrl + "\" target=\"_blank\">state maps</a>";
        string atlasUrl = @"https://www.worldatlas.com/webimage/countrys/namerica/usstates/lgcolor/zzzcolor.gif".Replace("zzz", drData["abbr"].ToString().ToLower());
        lblResult.Text = "<br />Learn more about <a target=_blank href=\"" + atlasUrl + "\">" + drData["name"].ToString() + "</a>:<br /><br />&nbsp;&nbsp;&nbsp;" + wikiLink + " | " + gmapLink + " | " + articlesLink + " | " + mapsLink;
    }

    protected void NewGame_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Default.aspx");
    }

    protected void NextButton_Click(object sender, EventArgs e)
    {

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

    int randomInt(int a, int b)
    {
        Random rnd = new Random();
        return rnd.Next(a, b+1);
    }

    int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}