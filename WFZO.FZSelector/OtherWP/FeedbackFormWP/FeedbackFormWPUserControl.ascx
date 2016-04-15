<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeedbackFormWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.OtherWP.FeedbackFormWP.FeedbackFormWPUserControl" %>


<div class="row mt35">
    <div class="col-md-3">
        <div class="db-small-tab mb20">
            <div class="db-small-iconbox">
                <div class="fzprofiles"></div>
            </div>
            <div class="db-small-detail">
                <h3 class="fzprofilept"><a href="/Pages/FreeZoneProfile.aspx">Free Zone Profile</a></h3>
            </div>
        </div>
    </div>

    <div class="col-md-3">
        <div class="db-small-tab mb20">
            <div class="db-small-iconbox">
                <div class="benchmarkings"></div>
            </div>
            <div class="db-small-detail">
                <h3><a href="/Pages/Benchmarking.aspx">Benchmarking</a></h3>
            </div>
        </div>
    </div>

    <div class="col-md-3">
        <div class="db-small-tab mb20">
            <div class="db-small-iconbox">
                <div class="weightages"></div>
            </div>
            <div class="db-small-detail">
                <h3 class="weight"><a href="/Pages/BenchmarkwithWeightage.aspx">Benchmark with Weightage</a></h3>
            </div>
        </div>
    </div>

</div>

<div class="inner-container">
    <div class="row">
        <div class="country-form">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-md-4 text-right" for="inputEmail1">Name <span>*</span></label>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtName" runat="server" class="form-control txt-box"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required" ControlToValidate="txtName" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-4 text-right" for="inputEmail1">Subject <span>*</span></label>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtSubject" runat="server" class="form-control txt-box"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Subject Required" ControlToValidate="txtSubject" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-4 text-right" for="inputEmail1">Type <span>*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="drpDownType" runat="server"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Select" InitialValue="1" ControlToValidate="drpDownType" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-4 text-right" for="freezone">Details <span>*</span></label>
                    <div class="col-md-8">
                        <asp:TextBox ID="txtDetails" runat="server" Rows="5" TextMode="MultiLine" class="form-control txt-box"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Details Required" ControlToValidate="txtDetails" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label for="freezone" class="col-md-4 text-right"></label>
                    <div class="col-md-8 text-center">
                        <asp:Button ID="Button1" runat="server" Text="SUBMIT" class="btn btn-genreport" OnClick="Button1_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
