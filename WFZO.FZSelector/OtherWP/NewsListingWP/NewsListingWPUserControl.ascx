<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsListingWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.OtherWP.NewsListingWP.NewsListingWPUserControl" %>

<style type="text/css">
    /*body {
        font-family: Arial;
        font-size: 10pt;
    }*/

    .Repeater, .Repeater td, .Repeater td {
        border: 1px solid #ccc;
    }

        .Repeater td {
            background-color: #eee !important;
        }

        .Repeater th {
            background-color: #6C6C6C !important;
            color: White;
            font-size: 10pt;
            line-height: 200%;
        }

        .Repeater span {
            color: black;
            font-size: 10pt;
            line-height: 200%;
        }

    .page_enabled, .page_disabled {
        display: inline-block;
        height: 20px;
        min-width: 20px;
        line-height: 20px;
        text-align: center;
        text-decoration: none;
        border: 1px solid #ccc;
    }

    .page_enabled {
        background-color: #eee;
        color: #000;
    }

    .page_disabled {
        background-color: #6C6C6C;
        color: #fff !important;
    }
</style>

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
<br />

<div>
    <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
</div>
<div>
    <table>
        <tr>
            <td>
                <asp:Button ID="cmdFirst" runat="server" Text="<< First" OnClick="cmdFirst_Click"></asp:Button>&nbsp;
            </td>
            <td>
                <asp:Button ID="cmdPrev" runat="server" Text="< Prev" OnClick="cmdPrev_Click"></asp:Button>&nbsp;
            </td>
            <td>
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
            </td>
            <td>&nbsp;<asp:Button ID="cmdNext" runat="server" Text="Next >" OnClick="cmdNext_Click"></asp:Button>
            </td>
            <td>&nbsp;<asp:Button ID="cmdLast" runat="server" Text="Last >>" OnClick="cmdLast_Click"></asp:Button>
            </td>
        </tr>
    </table>
</div>
