<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FreeZoneWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.FreeZoneWP.FreeZoneWPUserControl" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:HiddenField ID="errorMessage" runat="server" />

<script src="../_layouts/15/WFZO.FZSelector/js/CustomScript.js"></script>
<script>
    function CollectParameters() {
        AddToHidden('<%= tvFreezoneProfileCategories.ID %>', '<%= hdnFreezoneProfileCatIds.ClientID %>');
        <%--AddToHidden('<%= tvFreezoneCategories.ID %>', '<%= hdnFreezoneCatIds.ClientID %>');
        AddToHidden('<%= tvCountryCategories.ID %>', '<%= hdnCountryCatIds.ClientID %>');--%>
    }

    function getAndShowReport() {
        var ErrorVariable = { Error: "" };
        if (<%--validateTreeviewNodesSelection('<%= tvFreezoneCategories.ID %>', ErrorVariable) || validateTreeviewNodesSelection('<%= tvCountryCategories.ID %>', ErrorVariable) ||--%> validateTreeviewNodesSelection('<%= tvFreezoneProfileCategories.ID %>', ErrorVariable)) {
            CollectParameters();

            var ReportUrl = '<%= SPContext.Current.Web.Url %>/_layouts/15/ReportServer/RSViewerPage.aspx?rv:RelativeReportUrl=/Reports/FreezoneProfile.rdl';

            ReportUrl += '&rp:CountryID=' + $('#<%= ddlCountry.ClientID %>').val() + '&rp:FreezoneId=' + $('#<%= ddlFreeZone.ClientID %>').val() + '&rp:FreezoneProfileFieldsId=' + $('#<%= hdnFreezoneProfileCatIds.ClientID %>').val() + '&rp:CountryLevelSubCategoryIds=0&rp:FreezoneLevelSubCategoryIds=0';

            window.open(ReportUrl);

            <%--UpdateCategoryAnalytics($('#<%= hdnCountryCatIds.ClientID %>').val() + ',' + $('#<%= hdnFreezoneCatIds.ClientID %>').val(), '<%=  WFZO.FZSelector.Constants.Modules.Profile %>');

            UpdateFreeZoneAnalytics(<%= new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(FreezoneDataList) %>);--%>
        }
        else {
            alert(ErrorVariable);
        }
    }
</script>

<div class="inner-container">
    <div class="">
        <div class="col-md-8">
            <div role="form" class="form-horizontal">
                <div class="form-group">
                    <label class="col-md-2" for="inputEmail1">Region <span class="error">*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlRegion" runat="server" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlRegion" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-2" for="inputEmail1">Country <span class="error">*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlCountry" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlCountry" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="form-group">
                    <label class="col-md-2" for="inputEmail1">City <span class="error">*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlCity" runat="server" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlCity" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-2" for="freezone">Free Zone <span class="error">*</span></label>
                    <div class="col-md-8">
                        <div class="select-style">
                            <asp:DropDownList ID="ddlFreeZone" runat="server" OnSelectedIndexChanged="ddlFreeZone_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Select" InitialValue="0" ControlToValidate="ddlFreeZone" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <label for="freezone" class="col-md-4 text-right"></label>
                    <div class="col-md-8 text-center">
                        <%--<asp:Button ID="Button1" runat="server" Text="ADD TO SELECTION" class="btn btn-collection" OnClick="Button1_Click" />--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<asp:Panel ID="pnlTreeviews" runat="server" Visible="false">

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

    </div>

    <div class="inner-container">

        <div>

            <div class="country-form">

                <div class="form-horizontal">

                    <div>

                        <div class="col-md-1"></div>

                        <div class="col-md-8">



                            <h4 class="tree-head">Free Zone Information</h4>

                            <asp:TreeView ID="tvFreezoneProfileCategories" runat="server" ShowExpandCollapse="true" ShowCheckBoxes="All" CssClass="tree-box" CollapseImageUrl="/Style%20Library/WFZO/img/minus-sign.jpg" ExpandImageUrl="/Style%20Library/WFZO/img/plus-sign.jpg" ForeColor="#FF3300">



                                <HoverNodeStyle Font-Underline="False" ForeColor="#5555DD" />

                                <NodeStyle Font-Names="istok web" Font-Size="14px" ForeColor="#FF3300" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />

                                <ParentNodeStyle Font-Bold="False" />

                                <LeafNodeStyle CssClass="tvFreezoneProfileCategoriesChild" />

                                <SelectedNodeStyle Font-Underline="False" ForeColor="#FF3300" HorizontalPadding="0px" VerticalPadding="0px" />



                            </asp:TreeView>

                            <%--<h4 class="tree-head2">Country Level</h4>

                            <asp:TreeView ID="tvCountryCategories" runat="server" ShowCheckBoxes="All" CssClass="tree-box" CollapseImageUrl="/Style%20Library/WFZO/img/minus-sign.jpg" ExpandImageUrl="/Style%20Library/WFZO/img/plus-sign.jpg" ForeColor="#FF3300">

                                <HoverNodeStyle Font-Underline="False" ForeColor="#5555DD" />

                                <NodeStyle Font-Names="istok web" Font-Size="14px" ForeColor="#FF3300" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />

                                <ParentNodeStyle Font-Bold="False" />

                                <SelectedNodeStyle Font-Underline="False" ForeColor="#FF3300" HorizontalPadding="0px" VerticalPadding="0px" />

                                <LeafNodeStyle CssClass="tvCountryCategoriesChild" />



                            </asp:TreeView>

                            <h4 class="tree-head2">Free Zone Level</h4>

                            <asp:TreeView ID="tvFreezoneCategories" runat="server" ShowExpandCollapse="true" ShowCheckBoxes="All" CssClass="tree-box" CollapseImageUrl="/Style%20Library/WFZO/img/minus-sign.jpg" ExpandImageUrl="/Style%20Library/WFZO/img/plus-sign.jpg" ForeColor="#FF3300">



                                <HoverNodeStyle Font-Underline="False" ForeColor="#5555DD" />

                                <NodeStyle Font-Names="istok web" Font-Size="14px" ForeColor="#FF3300" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />

                                <ParentNodeStyle Font-Bold="False" />

                                <SelectedNodeStyle Font-Underline="False" ForeColor="#FF3300" HorizontalPadding="0px" VerticalPadding="0px" />

                                <LeafNodeStyle CssClass="tvFreezoneCategoriesChild" />



                            </asp:TreeView>--%>



                            <asp:HiddenField ID="hdnFreezoneProfileCatIds" runat="server" />

                            <%--<asp:HiddenField ID="hdnFreezoneCatIds" runat="server" />

                            <asp:HiddenField ID="hdnCountryCatIds" runat="server" />--%>

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

                            <input type="button" class="btn btn-collection" onclick="getAndShowReport();" value="GENERATE REPORT" />

                        </div>

                    </div>

                </div>

            </div>

        </div>

    </div>

</asp:Panel>
