<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsListingWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.OtherWP.NewsListingWP.NewsListingWPUserControl" %>

<asp:HiddenField ID="errorMessage" runat="server" />
<%--OnItemDataBound="NewsRP_ItemDataBound"--%>
<asp:Repeater ID="NewsRP" runat="server">
    <HeaderTemplate>
        <div class="news-head col-xs-12">
            <div class="col-xs-12">
                <h2>Latest News & Articles</h2>
            </div>
        </div>
    </HeaderTemplate>
    <ItemTemplate>
        <div class="news-row col-xs-12">
            <div class="col-sm-3">
                <img src="<%# Eval("Image") %>?renditionid=6" class="news-img" title='<%# Eval("Title") %>' />
            </div>
            <div class="col-sm-9">
                <div class="news-list">
                    <h2>
                        <asp:HyperLink ID="hplTitle" runat="server" NavigateUrl='<%# Eval("URL") %>'><%# Eval("Title") %></asp:HyperLink>
                    </h2>
                    <h5><%# Eval("ArticleStartDate") %></h5>
                    <p><%# Eval("ArticleByLine") %></p>
                </div>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
<br />


<div class="col-md-4">
    <div>
        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
    </div>
    <asp:Repeater ID="rptPages" runat="server">
        <HeaderTemplate>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr class="text">
                    <td>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:LinkButton ID="btnPage" CommandName="Page" CommandArgument="<%# Container.DataItem %>"
                runat="server"><%# Container.DataItem %>
            </asp:LinkButton>&nbsp;
        </ItemTemplate>
        <FooterTemplate>
            </td> </tr> </table>
        </FooterTemplate>
    </asp:Repeater>

    <div class="btn-toolbar" role="toolbar">
        <asp:Button ID="cmdFirst" runat="server" Text="<< First" OnClick="cmdFirst_Click" CssClass="btn btn-default"></asp:Button>
        <asp:Button ID="cmdPrev" runat="server" Text="< Prev" OnClick="cmdPrev_Click" CssClass="btn btn-default"></asp:Button>
        <asp:Button ID="cmdNext" runat="server" Text="Next >" CssClass="btn btn-default " OnClick="cmdNext_Click"></asp:Button>
        <asp:Button ID="cmdLast" runat="server" Text="Last >>" CssClass="btn btn-default " OnClick="cmdLast_Click"></asp:Button>
    </div>
</div>


