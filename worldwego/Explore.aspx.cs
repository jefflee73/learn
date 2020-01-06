using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;

public partial class worldwego_Explore : System.Web.UI.Page
{
    protected int stateCount = 197;
    protected int lifelineCount = 18;
    protected int destinationCount = 12;
    public string zoomTitle;

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        //{
            if (Session["stateindex"] == null || Session["lifelineindex"] == null)
            {
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
        DataRow drData = geography.GetCountry(strAbbr);
        string strImage, strText;
        DataTable dt;
        DataRow dd;
        switch (lifelineType)
        {
            case 1:     //latlng
                litLifeline.Text = "<h3>" + drData["country"].ToString() + " is located at (" + drData["lat"].ToString() + ", " + drData["lng"].ToString() + ")</h3>";
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
                litLifeline.Text = "<h3>" + drData["country"].ToString() + " has a population of <span class='datum'>" + ((int)drData["population"]).ToString("#,##0") + " (" + stringToOrdinal(drData["poprank"].ToString()) + ")</span>";
                litLifeline.Text += " and an area of <span class='datum'>" + Convert.ToInt32(drData["area"]).ToString("#,##0") + " sq. mi. (" + stringToOrdinal(drData["arearank"].ToString()) + ")</span></h3>";
                llicon.ImageUrl = "images/icons/population.png";
                llicon.AlternateText = "Population and area";
                llicon.ToolTip = "Population and area";
                break;
            case 6:     //elevation extremes
                litLifeline.Text = "<h3>" + drData["country"].ToString() + " has a low point of <span class='datum'>" + drData["lowpoint"].ToString() + " (" + drData["lowelevation"].ToString() + " m.)</span>";
                litLifeline.Text += " and a high point of <span class='datum'>" + drData["highpoint"].ToString() + " (" + drData["highelevation"].ToString() + " m.)</span></h3>";
                llicon.ImageUrl = "images/icons/elevation.png";
                llicon.AlternateText = "Elevation extremes";
                llicon.ToolTip = "Elevation extremes";
                break;
            case 7:     //terrain and climate
                string terrain = drData["terrain"].ToString();
                terrain = terrain.Substring(0, 1).ToLower() + terrain.Substring(1, terrain.Length - 1); //uncapitalize first letter
                string climate = drData["climate"].ToString();
                climate = climate.Substring(0, 1).ToLower() + climate.Substring(1, climate.Length - 1); //uncapitalize first letter
                litLifeline.Text = "<h4>" + drData["country"].ToString() + "'s terrain is <span class='datum'>" + terrain + "</span></h4>";
                litLifeline.Text += "<h4>The climate is <span class='datum'>" + climate + "</span></h4>";
                llicon.ImageUrl = "images/icons/terrain.png";
                llicon.AlternateText = "Terrain and climate";
                llicon.ToolTip = "Terrain and climate";
                break;
            case 8:     //religions
                litLifeline.Text = "<h3>" + drData["country"].ToString() + "'s religions are <span class='datum'>" + drData["religions"].ToString() + "</span></h3>";
                llicon.ImageUrl = "images/icons/religion.png";
                llicon.AlternateText = "Religions";
                llicon.ToolTip = "Religions";
                break;
            case 9:     //languages
                litLifeline.Text = "<h3>" + drData["country"].ToString() + "'s main languages include <span class='datum'>" + drData["languages"].ToString() + "</span></h3>";
                llicon.ImageUrl = "images/icons/language.png";
                llicon.AlternateText = "Main languages";
                llicon.ToolTip = "Main languages";
                break;
            case 10:     //currency
                litLifeline.Text = "<h3>" + drData["country"].ToString() + "'s monetary unit is the <span class='datum'>" + drData["currency"].ToString() + "</span></h3>";
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
                zoomTitle = "Country Shape";
                litZoom.Text = "<img class=\"shape\" src=\"images/shape/" + drData["a2"].ToString() + ".jpg\" alt=\"Country Shape\" title=\"Country Shape\" >";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                break;
            case 13:     //10 largest cities
                dt = geography.GetWWCities(drData["a2"].ToString(), 10);
                litLifeline.Text = "<h3>" + drData["country"].ToString() + "'s 10 Largest Cities:</h3>";
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
                llicon.ToolTip = "Ten largest cities";
                break;
            case 14:     //resources
                litLifeline.Text = "<h4>" + drData["country"].ToString() + "'s natural resources include <span class='datum'>" + drData["resources"].ToString() + "</span></h4>";
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
                litLifeline.Text = "<h3 class='neighbors'>These countries border " + drData["country"].ToString() + "</h3>";
                litLifeline.Text += "<table class='neighbors'><th>Country</th><th>Border Length</th></tr>";
                foreach (DataRow dr in dt.Rows)
                {
                    litLifeline.Text += "<tr><td>" + dr["neighbor"].ToString() + "</td><td>" + dr["neighborborder"].ToString() + " km</td></tr>";
                }
                litLifeline.Text += "</table>";
                llicon.ImageUrl = "images/icons/neighbors.png";
                llicon.AlternateText = "Neighboring countries";
                llicon.ToolTip = "Neighboring countries";
                break;
            case 17:     //stamp
                strImage = "<div class=\"stampContainer\"><img class=\"stamp\" src=\"images/stamp/" + drData["a2"].ToString() + ".jpg\" alt=\"Country Stamp\" title=\"Country Stamp\" onclick=\"zoomBox()\"></div>";
                litLifeline.Text = strImage;
                llicon.ImageUrl = "images/icons/stamp.png";
                llicon.AlternateText = "Country Stamp";
                llicon.ToolTip = "Country Stamp";
                zoomTitle = "Country Stamp";
                litZoom.Text = "<img class=\"stamp\" src=\"images/stamp/" + drData["a2"].ToString() + ".jpg\" alt=\"Country Stamp\" title=\"Country Stamp\" >";
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "zoomBox", "zoomBox()", true);
                break;
            case 18:     //initial
                litLifeline.Text = "<h3 class='neighbors'>The first letter of the country name is</h3>";
                litLifeline.Text += "<span class=\"initial\">" + char.ToUpper(drData["country"].ToString()[0]) + "</span>";
                llicon.ImageUrl = "images/icons/initial.png";
                llicon.AlternateText = "Country initial";
                llicon.ToolTip = "Country initial";
                break;
            default:
                if (lifelineType > 100 && lifelineType <= 100 + destinationCount)
                {
                    int intRank = lifelineType - 100;
                    dd = geography.GetWWDestination(strAbbr, intRank);
                    strText = "<p><span class='dest'>" + intRank.ToString() + ". <a target=\"_blank\" href=\"https://en.wikipedia.org/wiki/" + dd["destination"].ToString().Replace(" ", "_") + "\">" + dd["destination"].ToString() + "</a></span> - <span class='desc'>" + dd["description"].ToString() + "</span></p>";
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
        string wikiUrl = @"https://en.wikipedia.org/wiki/" + drData["country"].ToString();
        string wikiLink = "<a href=\"" + wikiUrl + "\" target=\"_blank\">wikipedia</a>";
        string gmapUrl = @"https://www.google.com/maps/place/" + drData["country"].ToString();
        string gmapLink = "<a href=\"" + gmapUrl + "\" target=\"_blank\">google map</a>";
        string articlesUrl = @"http://filbert.com/geobee/countries/default.htm#" + drData["a2"].ToString();
        string articlesLink = "<a href=\"" + articlesUrl + "\" target=\"_blank\">country articles</a>";
        string mapsUrl = @"http://filbert.com/geobee/countries/maps.htm#" + drData["a2"].ToString().ToLower();
        string mapsLink = "<a href=\"" + mapsUrl + "\" target=\"_blank\">country maps</a>";
        string atlasUrl = @"https://www.worldatlas.com/webimage/countrys/" + drData["continent"].ToString() + "/lgcolor/" + drData["a2"].ToString().ToLower() + "color.gif";
        lblResult.Text = "<br />Learn more about <a target=_blank href=\"" + atlasUrl + "\">" + drData["country"].ToString() + "</a>:<br /><br />&nbsp;&nbsp;&nbsp;" + wikiLink + " | " + gmapLink + " | " + articlesLink + " | " + mapsLink;
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

    int randomInt(int a, int b) //Return random integer between a and b inclusive
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