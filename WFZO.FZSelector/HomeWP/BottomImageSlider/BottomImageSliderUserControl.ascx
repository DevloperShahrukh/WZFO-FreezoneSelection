<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BottomImageSliderUserControl.ascx.cs" Inherits="WFZO.FZSelector.HomeWP.BottomImageSlider.BottomImageSliderUserControl" %>


		<div class="row">
    <asp:Repeater ID="Repeater1" runat="server">
        <HeaderTemplate>
            <div class="owl-carousel m-logos">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="item">
                <img src='<%# GetSrcFromImgTag(Eval("ImageColumn").ToString())%>' alt="logo" />
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
            </div>
      
 
<script src="../../Style%20Library/WFZO/js/jquery.min.js"></script>
<script src="../../Style%20Library/WFZO/js/bootstrap.js"></script>
<script src="../../Style%20Library/WFZO/js/owl.carousel.js"></script>

<script>
    var Timeinter = <%=this.Timeintervalb%>;
    var Time = <%=this.Timeb%>;

    if(Time == 0)
    {
        Time = Timeinter;
    }

    var owl = $('.owl-carousel');
    owl.owlCarousel({
        loop: true,
        nav: true,
        autoplay: true,
        autoplayTimeout: Time,
        autoplayHoverPause: true,
        margin: 10,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 3
            },
            960: {
                items: 4
            },
            1200: {
                items: 5
            }
        }
    });
    owl.on('mousewheel', '.owl-stage', function (e) {
        if (e.deltaY > 0) {
            owl.trigger('next.owl');
        } else {
            owl.trigger('prev.owl');
        }
        e.preventDefault();
    });
</script>


