<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DynamicReportDisplay.aspx.cs" Inherits="ReportEngine.Reports.DynamicReportDisplay" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            
            <div>
            <asp:ScriptManager ID="script1" runat="server" />
            <div runat="server" id="div_report" style="display: none; overflow: auto;">
                <div style="overflow: auto;">
                     <rsweb:ReportViewer ID="ReportViewer1" runat="server" BorderStyle="None" Height="800px"
                        Width="100%" ShowParameterPrompts="false" Font-Names="Verdana" Font-Size="8pt"
                        ShowPrintButton="true" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                    </rsweb:ReportViewer>
                    <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%"></rsweb:ReportViewer>--%>
                </div>
            </div>
            <div runat="server" id="div_noreport" style="display: none; text-align: center; padding-top: 40px; border: solid 0px;">
                <span style="font-weight: bold; color: Red;">
                    <asp:Label ID="lblMessage" runat="server" Text="No Report Data Found"></asp:Label></span>
            </div>
        </div>
        </div>
    </form>
    <script src="../bower_components/jquery/dist/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $("[id$=ReportViewer1]").find("div[id*='ReportContent']").each(function (index, element) {
            $(element).attr("align", "center");
        });
    </script>
</body>
</html>
