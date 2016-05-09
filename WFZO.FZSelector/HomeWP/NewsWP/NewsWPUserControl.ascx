<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.NewsWP.NewsWPUserControl" %>
<asp:HiddenField ID="errorMessage" runat="server" />
<div class="news-section">
    <div class="news-section-heading">
        <h3>LATEST NEWS</h3>
    </div>
    <ul>
        <asp:Repeater ID="NewsRP" runat="server">
            <ItemTemplate>
                <li>
                    <div class="trunc">
                        <asp:HyperLink ID="hplNews" runat="server" NavigateUrl='<%# Eval("URL") %>'><%# Eval("Title") %></asp:HyperLink>
                    </div>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                    <div class="trunc" style="float:right; color:#004375;">
                        <asp:HyperLink ID="hplViewAll" runat="server" NavigateUrl="/News" Visible="false">View All</asp:HyperLink>
                    </div>
            </FooterTemplate>
        </asp:Repeater>
    </ul>
</div>
