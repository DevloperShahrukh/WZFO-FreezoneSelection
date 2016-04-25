<%@ Assembly Name="WFZO.FZSelector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3db5563f26eeb2fa" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FAQWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.OtherWP.FAQWP.FAQWPUserControl" %>


<asp:Repeater ID="rpFAQ" runat="server">
    <HeaderTemplate>
        <dl id="faqs">
    </HeaderTemplate>
    <ItemTemplate>
        <dt><%# Eval("Title") %></dt>
        <dd><%# Eval("Answer") %></dd>
    </ItemTemplate>
    <FooterTemplate>
        </dl>
    </FooterTemplate>
</asp:Repeater>

<script type="text/javascript">
    $("#faqs dd").hide();
    $("#faqs dt").click(function () {
        $(this).next("#faqs dd").slideToggle(500);
        $(this).toggleClass("expanded");
    });
</script>
<style>
    #faqs dt, #faqs dd {
        padding: 0 0 0 50px;
    }

    #faqs dt {
        font-size: 14px;
        color: #006281;
        cursor: pointer;
        height: 37px;
        line-height: 37px;
        margin: 0 0 15px 25px;
        font-family: Univers (45 Light);
    }

    #faqs dd {
        font-size: 14px;
        color: #8E7630;
        margin: 0 0 20px 25px;
        font-family: Univers (45 Light);
    }

    #faqs dt {
        background: url("../../Style%20Library/Images/toggle-expand-alt_blue.png") no-repeat left;
    }

    #faqs .expanded {
        background: url("../../Style%20Library/Images/toggle-collapse-alt_blue.png") no-repeat left;
    }
</style>
