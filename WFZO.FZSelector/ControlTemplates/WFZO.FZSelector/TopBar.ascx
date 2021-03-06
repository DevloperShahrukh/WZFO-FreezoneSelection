﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopBar.ascx.cs" Inherits="WFZO.FZSelector.ControlTemplates.WFZO.FZSelector.TopBar" %>


<script src="../_layouts/15/WFZO.FZSelector/js/CustomScript.js"></script>
<script>

    $(document).ready(function () {
        
        var EncryptedTokenString = '<%= SPContext.Current.Web.CurrentUser != null ? WFZO.FZSelector.Classes.Secure.getencryptedToken(SPContext.Current.Web.CurrentUser.LoginName.Replace("i:0#.f|fbamembershipprovider|", "").Trim()) : string.Empty %>';


        if (EncryptedTokenString != '') {
            
            var Link = $('.logo-cont > a').attr('href');

            if (Link != undefined) {

                $('.logo-cont > a').attr('href', Link + '?wToken=' + EncryptedTokenString);
            }
        }

    });


</script>

<asp:HiddenField ID="errorMessage" runat="server" />

<div class="logout-box">
    <ul>
        <li class="welcome">
            <asp:Literal ID="ltrMembershipValidity" runat="server" Visible="false" /></li>
        <li>
            <asp:HyperLink ID="hypHome" runat="server" ToolTip="Home" NavigateUrl="/"><span class="glyphicon glyphicon-home"></span></asp:HyperLink></li>
        <%-- <li><a href="http://wwww.worldfzo.org" title="WorldFZO Home"  target="_blank">WorldFZO<span class="glyphicon glyphicon-home"></span></a></li>--%>
        <li>
            <asp:HyperLink ID="hypDashboard" ToolTip="Dashboard" runat="server" Enabled="false" NavigateUrl="/pages/dashboard.aspx"><i class="fa fa-server" aria-hidden="true"></i></asp:HyperLink></li>
        <li class="welcome">
            <asp:Literal ID="ltrWelcome" runat="server" /></li>
        <li>
            
            <asp:LinkButton ID="lnkLogout" ToolTip="Logout" runat="server"  OnClick="lnkLogout_Click"><span class="glyphicon glyphicon-log-out"></span> Logout</asp:LinkButton></li>
    </ul>
</div>
