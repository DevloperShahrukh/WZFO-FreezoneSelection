﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BenchmarkWithWeightWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.BenchmarkWithWeightWP.BenchmarkWithWeightWPUserControl" %>


<script src="/Style Library/WFZO/js/jquery.min.js"></script>
<script src="../_layouts/15/WFZO.FZSelector/js/CustomScript.js"></script>
<script>

    function getAndShowReport() {

        var ErrorVariable = { Error: "" };

        if (validateReportType('<%= rblReportType.ClientID %>', ErrorVariable) && validateCheckedInputs(ErrorVariable) && validateCheckedInputsSum(ErrorVariable)) {

            AddCategoryWithWeights("[name*='chkSelectedCategory']:checked", '<%= hdnCatIdAndWeightageValue.ClientID %>');
            var reportName = $('#<%= rblReportType.ClientID %>').find('input:checked').val();

            var ReportUrl = '<%= SPContext.Current.Web.Url %>/_layouts/15/ReportServer/RSViewerPage.aspx?rv:RelativeReportUrl=/Reports/' + reportName;

            ReportUrl += '&rp:CountryIds=' + $('#<%= hdnCountryIds.ClientID %>').val()
            + '&rp:FreezoneIds=' + $('#<%= hdnFreezoneIds.ClientID %>').val() + '&rp:MacroCategoryIds='
            + $('#<%= hdnCountryCatIds.ClientID %>').val() + '&rp:MicroCategoryIds=' + $('#<%= hdnFreezoneCatIds.ClientID %>').val()
            + '&rp:CategoriesWithWeights=' + $('#<%= hdnCatIdAndWeightageValue.ClientID %>').val();

            window.open(ReportUrl);

            UpdateCategoryAnalyticsOfWeigthted('<%= WFZO.FZSelector.Constants.Modules.Weighted %>');

            //UpdateCategoryAnalytics($('#<%= hdnCountryCatIds.ClientID %>').val() + ',' + $('#<%= hdnFreezoneCatIds.ClientID %>').val(), '<%= WFZO.FZSelector.Constants.Modules.Weighted %>');

            UpdateFreeZoneAnalytics(<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(FreezoneDataList) %>);
        }
        else {
            alert(ErrorVariable.Error);
            ErrorVariable = { Error: "" };
        }


    }
</script>

<asp:HiddenField ID="hdnCountryIds" runat="server" />
<asp:HiddenField ID="hdnFreezoneIds" runat="server" />
<asp:HiddenField ID="hdnCountryCatIds" runat="server" />
<asp:HiddenField ID="hdnFreezoneCatIds" runat="server" />
<asp:HiddenField ID="hdnCatIdAndWeightageValue" runat="server" />

<asp:HiddenField ID="errorMessage" runat="server" />

<div class="inner-container">
    <div class="row">
        <div class="country-form">
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-md-4 text-right" for="inputEmail1">Region <span>*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlRegion" runat="server" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlRegion" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-4 text-right" for="inputEmail1">Country <span>*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlCountry" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlCountry" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="form-group">
                    <label class="col-md-4 text-right" for="inputEmail1">City <span>*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlCity" runat="server" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlCity" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-4 text-right" for="freezone">Free Zone <span>*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlFreeZone" runat="server"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlFreeZone" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label for="freezone" class="col-md-4 text-right"></label>
                    <div class="col-md-8 text-center">
                        <asp:Button ID="Button1" runat="server" Text="ADD TO SELECTION" class="btn btn-collection" OnClick="Button1_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<asp:Panel ID="PlSelectedZone" runat="server" Visible="false">
    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading grid-header">Selected Free Zone</div>
                <div class="panel-body table-responsive">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table country-table" AutoGenerateColumns="False" AllowPaging="True" PageSize="5" OnRowCommand="GridView1_RowCommand" GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField DataField="SR" HeaderText="#" />
                            <asp:BoundField DataField="Region" HeaderText="Region" />
                            <asp:BoundField DataField="Country" HeaderText="Country" />
                            <asp:BoundField DataField="City" HeaderText="City" />
                            <asp:BoundField DataField="FreeZone" HeaderText="FreeZone" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="remove" CommandName="delRow" CommandArgument='<%# Bind("SR") %>'><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>



</asp:Panel>

<asp:Panel ID="pnlCategories" runat="server" Visible="false">
    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading grid-header">Categories <span>*</span></div>
                <div class="panel-body table-responsive">
                    <asp:GridView ID="grdWeightedBenchmarkingCategories" runat="server" CssClass="table country-table" AutoGenerateColumns="False" GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelectedCategory" runat="server" OnCheckedChanged="chkSelectedCategory_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CategoryLevel" HeaderText="Category Level" />
                            <asp:BoundField DataField="Category" HeaderText="Category" />
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWeightage" runat="server" Text="Weightage"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnCatId" runat="server" Value='<%# Eval("Id") %>' />
                                    <asp:HiddenField ID="hdnSubCatIds" runat="server" Value='<%# Eval("SubCategoryIds") %>' />
                                    <asp:TextBox ID="quantity" runat="server" Enabled="false" TextMode="Number" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-md-2"></div>
    </div>
    <asp:Label ID="lblError" runat="server" Text="" Visible="false"></asp:Label>
    <div class="form-group">

        <div class="col-md-8 text-center">
            <asp:RadioButtonList ID="rblReportType" runat="server" Visible="true" RepeatLayout="UnorderedList"></asp:RadioButtonList>
            <asp:Button Enabled="false" ID="btnReport" runat="server" Text="Generate Report" class="btn btn-collection" OnClientClick="getAndShowReport();return false;" />
        </div>
    </div>
</asp:Panel>



