<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeedbackFormWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.OtherWP.FeedbackFormWP.FeedbackFormWPUserControl" %>


<asp:HiddenField ID="errorMessage" runat="server" />


<asp:Panel ID="pnlStatus" runat="server" Visible="false">
    <p>Your <strong><asp:Label ID="lblFeedbackType" runat="server" Text=""></asp:Label></strong> have been submitted.</p>
</asp:Panel>

<asp:Panel ID="pnlFeedback" runat="server">
    <div class="inner-container">
        <div class="">
            <div class="col-md-8">
                <div role="form" class="form-horizontal">
                    <div class="form-group">
                        <label class="col-md-2" for="inputEmail1">From <span class="error">*</span></label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtFrom" TextMode="Email" runat="server" class="form-control txt-box" />
                            <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Name Required" ControlToValidate="txtFrom" Display="Dynamic" />

                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-2" for="inputEmail1">Name <span class="error">*</span></label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtName" runat="server" class="form-control txt-box" />
                            <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required" ControlToValidate="txtName" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-2" for="inputEmail1">Type <span class="error">*</span></label>
                        <div class="col-md-8">
                            <div class="select-style">
                                <asp:DropDownList ID="drpDownType" runat="server" />
                            </div>
                            <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Select" InitialValue="1" ControlToValidate="drpDownType" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-md-2" for="freezone">Details <span class="error">*</span></label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtDetails" runat="server" Rows="5" TextMode="MultiLine" class="form-control txt-box" />
                            <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Details Required" ControlToValidate="txtDetails" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="freezone" class="col-md-2"></label>
                        <div class="col-md-8 text-center">
                            <asp:Button ID="Button1" runat="server" Text="SUBMIT" class="btn btn-genreport" OnClick="Button1_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
