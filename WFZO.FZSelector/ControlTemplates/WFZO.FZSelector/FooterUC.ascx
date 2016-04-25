<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FooterUC.ascx.cs" Inherits="WFZO.FZSelector.ControlTemplates.WFZO.FZSelector.FooterUC" %>

<div class="footer-links">
    <ul>
        <asp:Repeater ID="FooterRP" runat="server" OnItemDataBound="FooterRP_ItemDataBound">
            <ItemTemplate>
                <li>
                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>' Visible="false"></asp:Label>
                    <asp:Literal ID="ltrUrl" runat="server"></asp:Literal>
                    <asp:Label ID="lblInWindow" runat="server" Text='<%# Eval("InWindow") %>' Visible="false"></asp:Label>
                    <asp:Label ID="lblUrl" runat="server" Text='<%# Eval("LinkURL") %>' Visible="false"></asp:Label>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>
