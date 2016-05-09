<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginUC.ascx.cs" Inherits="WFZO.FZSelector.ControlTemplates.WFZO.FZSelector.LoginUC" %>

<asp:Panel ID="PlLogin" runat="server">
<%--    <ul id="liNotLoggedin" class="nav navbar-right">
        <li id="liHome"><a href="/"><span class="glyphicon glyphicon-home"></span></a></li>
        <li><a href="http://http://www.worldfzo.org/" target="_blank"><span class="wfzohome-icon"></span></a></li>
    </ul>--%>
    <ul class="nav navbar-right user-login-area">
        <li class="dropdown" runat="server" id="liLogin">
            <a href="#" class="btn btn-login dropdown-toggle" onclick="showPopup();">MEMBER LOGIN</a>
            <div class="members dropdown-menu">
                <ul id="login-dp" class="">
                    <div class="arrow-up"></div>
                    <li>
                        <div id="check" class="row">
                            <div class="col-md-12">
                                <div class="form" id="login-nav">
                                    <div class="inner-addon left-addon">
                                        <i class="glyphicon glyphicon-user"></i>
                                        <%--<input id="email" type="email" class="form-control email" placeholder="Login ID" name="email" />--%>
                                        <asp:TextBox ID="txtUserID" runat="server" class="form-control email" placeholder="Login ID"></asp:TextBox>
                                    </div>
                                    <div class="inner-addon left-addon">
                                        <i class="fa fa-lock pw"></i>
                                        <%--<input id="password" type="password" class="form-control password" placeholder="......." name="password" />--%>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="form-control password" placeholder="******"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <%--<button class="btn user-login">LOG IN</button>--%>
                                        <asp:Button ID="btnUserLogin" runat="server" Text="LOG IN" class="btn user-login" OnClick="btnUserLogin_Click" />
                                    </div>
                                    <div class="controls">
                                        <asp:Label ID="lblInvalidUser" runat="server" Text="" ForeColor="DarkRed" />


                                        <asp:Button ID="btnInformAdmin" runat="server" Text="Inform Admin" Visible="false" OnClick="btnInformAdmin_Click" />
                                    </div>

                                    <div class="controls">
                                        <label class="checkbox-inline remember-check">
                                            <asp:CheckBox ID="chkStaySignedIn" runat="server" CssClass="checkbox-custom" Checked="true" />
                                            <asp:Label ID="Label2" runat="server" Text="Stay signed in" CssClass="checkbox-custom-label"></asp:Label>

                                        </label>
                                        <%--<input id="checkbox-1" class="checkbox-custom" name="checkbox-1" type="checkbox" checked>--%>
                                        <%--<label for="checkbox-1" class="checkbox-custom-label">Remember me</label>--%>
                                        <a href="http://www.worldfzo.org/Pages/PasswordRecovery.aspx" target="_blank" class="text-right forget">Forget?</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </li>
    </ul>
</asp:Panel>
<asp:Panel ID="Pllogout" runat="server" Visible="false">
    <div class="logout-box">
        <ul>
            <li><a href="#"><span class="glyphicon glyphicon-home"></span></a></li>
            <li>
                <asp:LinkButton ID="lblogout" runat="server" OnClick="lblogout_Click"><span class="glyphicon glyphicon-log-out"></span></asp:LinkButton>
            </li>
            <li><a href="#"><span class="wfzohome-icon"></span></a></li>
        </ul>
    </div>
</asp:Panel>

<script>

    //function showPopup() {
    //    $("li.dropdown").addClass('open');
    //}

    <%--
    $('#check').click(function () {
        var divid = this.id;
        alert($('.item_' + divid).html());
    });
        //$("body").click(function (e) {
        //    if (e.target.id == "login-dp" || $(e.target).parents("#login-dp").size()) {
        //        alert("Inside div");
        //    } else {
        //        alert("Outside div");
        //    }
        //});--%>
    
    var myVar;
    function showPopup() {
        $("li.dropdown").addClass('open');
        myVar = setTimeout(ClosePopup, 3000);
    }

    $('.form-control').blur(function () {
        //alert('sdf');
        myStopFunction();
        myVar = setTimeout(ClosePopup, 5000);
    });
    $('.form-control').focus(function () {
        //alert('sdf');
        myStopFunction();
        //myVar = setTimeout(ClosePopup, 10000);
    });


    function ClosePopup() {
        $("li.dropdown").removeClass('open');
    }
    function myStopFunction() {
        clearTimeout(myVar);
    }


</script>
