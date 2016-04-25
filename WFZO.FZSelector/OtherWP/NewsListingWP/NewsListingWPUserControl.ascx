<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsListingWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.OtherWP.NewsListingWP.NewsListingWPUserControl" %>


<asp:Repeater ID="NewsRP" runat="server" OnItemDataBound="NewsRP_ItemDataBound">
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
                <img src="<%# Eval("Image") %>" class="news-img" />
            </div>
            <div class="col-sm-9">
                <div class="news-list">
                    <h2>
                        <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>' Visible="false"></asp:Label>
                        <asp:Literal ID="ltrUrl" runat="server"></asp:Literal>
                        <asp:Label ID="lblUrl" runat="server" Text='<%# Eval("URL") %>' Visible="false"></asp:Label>
                    </h2>
                    <h5><%# Eval("ArticleStartDate") %></h5>
                    <p><%# Eval("Comments") %></p>
                </div>
            </div>
        </div>
    </ItemTemplate>

</asp:Repeater>
