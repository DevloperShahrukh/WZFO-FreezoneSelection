﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BenchmarkingWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.BenchmarkingWP.BenchmarkingWPUserControl" %>

<script src="../_layouts/15/WFZO.FZSelector/js/CustomScript.js"></script>
<script>
    function CollectParameters() {

        AddToHidden('<%= tvFreezoneCategories.ID %>', '<%= hdnFreezoneCatIds.ClientID %>');
        AddToHidden('<%= tvCountryCategories.ID %>', '<%= hdnCountryCatIds.ClientID %>');
    }

    function getAndShowReport() {

        var ErrorVariable = { Error: "" };

        if ((validateTreeviewNodesSelection('<%= tvFreezoneCategories.ID %>', ErrorVariable) || validateTreeviewNodesSelection('<%= tvCountryCategories.ID %>', ErrorVariable)) && validateReportType('<%= rblReportType.ClientID %>', ErrorVariable)) {

            CollectParameters();

            var reportName = $('#<%= rblReportType.ClientID %>').find('input:checked').val();

            var ReportUrl = '<%= SPContext.Current.Web.Url %>/_layouts/15/ReportServer/RSViewerPage.aspx?rv:RelativeReportUrl=/Reports/' + reportName.split('.')[0] + (isReportLandscape('<%= hdnFreezoneCount.ClientID %>') ? '-landscape.' : '.') + reportName.split('.')[1];

            ReportUrl += '&rp:CountryIds=' + $('#<%= hdnCountryIds.ClientID %>').val()
            + '&rp:FreezoneIds=' + $('#<%= hdnFreezoneIds.ClientID %>').val() + '&rp:MacroCategoryIds='
            + $('#<%= hdnCountryCatIds.ClientID %>').val() + '&rp:MicroCategoryIds=' + $('#<%= hdnFreezoneCatIds.ClientID %>').val();

            window.open(ReportUrl);

            UpdateCategoryAnalytics($('#<%= hdnCountryCatIds.ClientID %>').val() + ',' + $('#<%= hdnFreezoneCatIds.ClientID %>').val(), '<%= WFZO.FZSelector.Constants.Modules.Benchmarking %>');

            UpdateFreeZoneAnalytics(<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(FreezoneDataList) %>);
        }
        else {
            alert(ErrorVariable.Error);
        }
    }
</script>

<asp:HiddenField ID="errorMessage" runat="server" />

<div class="inner-container">
    <div class="">
        <div class="col-md-8">
            <div role="form" class="form-horizontal">
                <div class="form-group">
                    <label class="col-md-2" for="inputEmail1">Region <span class="error">*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlRegion" runat="server" ValidationGroup="form" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator1" ValidationGroup="form" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlRegion" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-2" for="inputEmail1">Country <span class="error">*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlCountry" runat="server" ValidationGroup="form" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator2" ValidationGroup="form" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlCountry" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="form-group">
                    <label class="col-md-2" for="inputEmail1">City <span class="error">*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlCity" runat="server" ValidationGroup="form" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator3" ValidationGroup="form" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlCity" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-2" for="freezone">Free Zone <span class="error">*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlFreeZone" runat="server" ValidationGroup="form"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator4" ValidationGroup="form" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlFreeZone" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label for="freezone" class="col-md-2"></label>
                    <div class="col-md-8 text-center">
                        <asp:Button ID="Button1" runat="server" ValidationGroup="form" Text="ADD TO SELECTION" class="btn btn-collection" OnClick="Button1_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>



    <asp:Panel ID="PlSelectedZone" runat="server" Visible="false">
    <div>
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading grid-header">Selected Free Zone</div>
                <div class="panel-body table-responsive">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table country-table" AutoGenerateColumns="False" AllowPaging="True" PageSize="5" OnRowCommand="GridView1_RowCommand" OnPageIndexChanging="GridView1_PageIndexChanging" GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField DataField="SR" HeaderText="#" />
                            <asp:BoundField DataField="Region" HeaderText="Region" />
                            <asp:BoundField DataField="Country" HeaderText="Country" />
                            <asp:BoundField DataField="City" HeaderText="City" />
                            <asp:BoundField DataField="FreeZone" HeaderText="FreeZone" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="remove" CommandName="delRow" CommandArgument='<%# Bind("SR") %>'><span class="glyphicon glyphicon-trash" ></span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-md-2"></div>
    </div>
        </asp:Panel>
<asp:Panel ID="PnlTreeViews" runat="server" Visible="false">
    <div class="inner-container">

        <div class="country-form">

            <div class="form-horizontal">

                <div class="col-md-1"></div>

                <div class="col-md-11">

                    <div class="help-link">
                        For more information on the definitions, sources and methodology of the (sub-)categories you can select below, please click 

       <b><a title="manual" href="/pages/categorydefinition.aspx" target="_blank">here</a></b>.

                    </div>

                </div>

            </div>

        </div>

    </div><br />

    <div class="inner-container">
        <div>
            <div class="country-form">
                <div class="form-horizontal">
                    <div>
                        <div class="col-md-1"></div>
                        <div class="col-md-8">
                            <h4 class="tree-head">Country Level</h4>

                            <asp:TreeView ID="tvCountryCategories" runat="server" ShowExpandCollapse="true" ShowCheckBoxes="All" CssClass="tree-box" CollapseImageUrl="/Style%20Library/WFZO/img/minus-sign.jpg" ExpandImageUrl="/Style%20Library/WFZO/img/plus-sign.jpg" ForeColor="#FF3300">

                                <HoverNodeStyle Font-Underline="False" ForeColor="#5555DD" />
                                <NodeStyle Font-Names="istok web" Font-Size="14px" ForeColor="#FF3300" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                <ParentNodeStyle Font-Bold="False" />
                                <SelectedNodeStyle Font-Underline="False" ForeColor="#FF3300" HorizontalPadding="0px" VerticalPadding="0px" />
                                <LeafNodeStyle CssClass="tvCountryCategoriesChild" />

                            </asp:TreeView>

                            <h4 class="tree-head2">Free zone Level</h4>
                            <asp:TreeView ID="tvFreezoneCategories" runat="server" ShowExpandCollapse="true" ShowCheckBoxes="All" CssClass="tree-box" CollapseImageUrl="/Style%20Library/WFZO/img/minus-sign.jpg" ExpandImageUrl="/Style%20Library/WFZO/img/plus-sign.jpg" ForeColor="#FF3300">

                                <HoverNodeStyle Font-Underline="False" ForeColor="#5555DD" />
                                <NodeStyle Font-Names="istok web" Font-Size="14px" ForeColor="#FF3300" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                <ParentNodeStyle Font-Bold="False" />
                                <SelectedNodeStyle Font-Underline="False" ForeColor="#FF3300" HorizontalPadding="0px" VerticalPadding="0px" />
                                <LeafNodeStyle CssClass="tvFreezoneCategoriesChild" />

                            </asp:TreeView>

                            <asp:HiddenField ID="hdnFreezoneCatIds" runat="server" />
                            <asp:HiddenField ID="hdnCountryCatIds" runat="server" />
                            <asp:HiddenField ID="hdnCountryIds" runat="server" />
                            <asp:HiddenField ID="hdnFreezoneIds" runat="server" />
                            <asp:HiddenField ID="hdnFreezoneCount" runat="server" />

                            <h4 class="tree-head2">Report Output</h4>
                            <asp:RadioButtonList CssClass="tree-box" ID="rblReportType" runat="server" Visible="false" RepeatLayout="UnorderedList"></asp:RadioButtonList>

                        </div>
                        <div class="col-md-3"></div>
                    </div>
                </div>
            </div>
            <div class="country-form">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="freezone" class="col-md-10 text-right"></label>
                        <div class="col-md-8 text-center">
                            <input type="button" id="btnShowReport" runat="server" value="GENERATE REPORT" class="btn btn-collection" onclick="getAndShowReport();">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>

<%--<asp:Panel ID="pnlTrend" runat="server" Visible="false">
    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading grid-header">Selected Free Zone</div>
                <div class="panel-body table-responsive">
                    <asp:GridView ID="GridView2" runat="server" CssClass="table country-table" AutoGenerateColumns="False" AllowPaging="True" PageSize="5" OnRowCommand="GridView1_RowCommand" OnPageIndexChanging="GridView1_PageIndexChanging" GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField DataField="SR" HeaderText="#" />
                            <asp:BoundField DataField="Region" HeaderText="Region" />
                            <asp:BoundField DataField="Country" HeaderText="Country" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="remove" CommandName="delRow" CommandArgument='<%# Bind("SR") %>'><span class="glyphicon glyphicon-trash" ></span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

</asp:Panel>--%>


<asp:Label ID="lblError" runat="server" Text="" Visible="false"></asp:Label>



