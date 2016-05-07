<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopImageSliderUserControl.ascx.cs" Inherits="WFZO.FZSelector.HomeWP.TopImageSlider.TopImageSliderUserControl" %>
<asp:HiddenField ID="errorMessage" runat="server" />


<asp:Repeater ID="Repeater2" runat="server" OnItemDataBound="Repeater2_ItemDataBound">
    <HeaderTemplate>

        <div id="carousel-example" class="carousel slide mb20" data-ride="carousel">
            <div class="carousel-inner">
    </HeaderTemplate>
    <ItemTemplate>
        <div id="panelItem" runat="server">
            <%# !string.IsNullOrEmpty(Convert.ToString(Eval("ImageLink"))) ? "<a href='" + Convert.ToString(Eval("ImageLink")).Split(',')[0]   + "'>" : string.Empty %> 

            <img src='<%# GetSrcFromImgTag(Eval("ImageColumn").ToString())%>?renditionId=5' alt="slide1" class="img-responsive" />

            <%# !string.IsNullOrEmpty(Convert.ToString(Eval("ImageLink"))) ? "</a>" : string.Empty %> 
            <div class="carousel-caption2" runat="server" id="dvCaption" >
                <h2>
                    <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("Title") %>'></asp:Label></h2>
            </div>
        </div>
    </ItemTemplate>
    <FooterTemplate>
        </div>
                    <!--PREVIUS-NEXT BUTTONS-->
        <a class="left carousel-control" href="#carousel-example" data-slide="prev">
            <span class="glyphicon glyphicon-chevron-left"></span>
        </a>
        <a class="right carousel-control" href="#carousel-example" data-slide="next">
            <span class="glyphicon glyphicon-chevron-right"></span>
        </a>
        </div>    
    </FooterTemplate>
</asp:Repeater>

<script>

    var Timeinter = <%=this.Timeinterval%>;
    var Time = <%=this.Time%>;

    if(Time == 0)
    {
        Time = Timeinter;
    }

    $('#carousel-example').carousel({
        interval: Time  //TIME IN MILLI SECONDS
    });
</script>
