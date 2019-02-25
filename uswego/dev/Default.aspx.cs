using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class uswego_dev_Default : System.Web.UI.Page
{
    public string dialogTitle;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        dialogTitle = "Waterway Map Snippet";
        //litZoom.Text = "<img class=\"shape\" src=\"../images/waterways/nd.gif\" alt=\"Waterways Map Snippet\" title=\"Waterways Map Snippet\">";
        //litZoom.Text = "<img class=\"flag\" src=\"../images/xsd.png\" alt=\"State Flag\" title=\"State Flag\">";
        //litZoom.Text = "<img class=\"flag\" src=\"../images/ysd.png\" alt=\"State Quarter\" title=\"State Quarter\">";
        //litZoom.Text = "<img class=\"shape\" src=\"../images/shape/al.jpg\" alt=\"State Shape\" title=\"State Shape\">";

        //litZoom.Text = "<img class=\"flag\" src=\"../../worldwego/images/shape/bz.jpg\" alt=\"Country Shape\" title=\"Country Shape\">";
        //litZoom.Text = "<img class=\"flag\" src=\"../../worldwego/images/stamp/af.jpg\" alt=\"Country Shape\" title=\"Country Shape\">";
        litZoom.Text = "<img class=\"cia\" src=\"https://www.cia.gov/library/publications/resources/the-world-factbook/attachments/maps/AF-map.gif\" alt=\"Country Map\" title=\"Country Map\">";
        //litZoom.Text = "<img class=\"cia\" src=\"https://www.cia.gov/library/publications/the-world-factbook/attachments/locator-maps/CI-locator-map.gif\" alt=\"Country Map\" title=\"Country Map\">";

        ScriptManager.RegisterClientScriptBlock(this, GetType(), "showModal", "showModal()", true);
    }
}