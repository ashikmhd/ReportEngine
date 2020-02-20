using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for General
/// </summary>
public class General
{
    public General()
    {

    }

    public string Success(string Message)
    {
        string CustomMessage = " <div class='alert alert-success alert-dismissable'>" +
                      "<button type='button' class='close' data-dismiss='alert' aria-hidden=true'>&times;</button>" +
                      "<h4>	<i class='icon fa fa-check'></i> Alert!</h4>" +
                      Message + "</div>";
        return CustomMessage;
    }
    public string Warning(string Message)
    {
        string CustomMessage = "<div class=alert alert-warning alert-dismissable'> " +
                      "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
                      "<h4><i class='icon fa fa-warning'></i> Alert!</h4>" +
                       Message + "</div>";
        return CustomMessage;
    }
    public string Error(string Message)
    {
        string CustomMessage = "<div class='alert alert-danger alert-dismissable>" +
                        " <button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
                        "<h4><i class='icon fa fa-ban'></i> Alert!</h4>" +
                         Message + "</div>";
        return CustomMessage;
    }
    public string Information(string Message)
    {
        string CustomMessage = "<div class='alert alert-info alert-dismissable>" +
                   "<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
                   "<h4><i class='icon fa fa-info'></i> Alert!</h4>" +
                    Message + "</div>";
        return CustomMessage;
    }



    public string SuccessBanner(string Message)
    {
        string CustomMessage = "<div class='callout callout-success'>" +
                    "<h4></h4> <p>" + Message + "</p></div>";
        return CustomMessage;
    }
    public string WarningBanner(string Message)
    {
        string CustomMessage = "<div class='callout callout-warning'>" +
            "<h4></h4> <p>" + Message + "</p></div>";
        return CustomMessage;
    }
    public string ErrorBanner(string Message)
    {
        string CustomMessage = "<div class='callout callout-danger'>" +
            "<h4></h4> <p>" + Message + "</p></div>";
        return CustomMessage;
    }
    public string InformationBanner(string Message)
    {
        string CustomMessage = "<div class='callout callout-info'>" +
             "<h4></h4> <p>" + Message + "</p></div>";
        return CustomMessage;
    }

}