<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        //Handle view state manipulation
        HttpApplication httpApp = this.Context.ApplicationInstance;
        HttpException httpEx = httpApp.Server.GetLastError() as HttpException;
        if (httpEx != null)
        {
            if (
                httpEx.WebEventCode == System.Web.Management.WebEventCodes.AuditInvalidViewStateFailure
                ||
                httpEx.WebEventCode == System.Web.Management.WebEventCodes.InvalidViewState
                ||
                httpEx.WebEventCode == System.Web.Management.WebEventCodes.InvalidViewStateMac
                ||
                httpEx.WebEventCode == System.Web.Management.WebEventCodes.RuntimeErrorViewStateFailure
                )
            {
                HttpContext.Current.ClearError();
                Response.Write("Error: An invalid viewstate has been detected (WebEventCode: " + httpEx.WebEventCode.ToString() + ").");
            }
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

</script>
