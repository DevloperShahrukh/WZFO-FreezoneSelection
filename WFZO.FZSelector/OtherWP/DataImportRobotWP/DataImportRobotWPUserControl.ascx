<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataImportRobotWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.OtherWP.DataImportRobotWP.DataImportRobotWPUserControl" %>


<asp:HiddenField ID="errorMessage" runat="server" />


<div class="inner-container">
    <div class="">
        <div class="col-md-8">
            <div role="form" class="form-horizontal">
                <div class="form-group">
                    <label class="col-md-2" for="inputfileupload">Upload File</label>
                    <div class="col-md-10">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="col-md-2" for="inputfileuploadsc"></label>
                    <div class="col-md-10">
                        <div align="left">
                            <asp:Button ID="BtnCountryLevelImportID" runat="server" OnClick="BtnCountryLevelImportID_Click" class="btn btn-collection" Text="Country Level Import" />
                            <asp:Button ID="BtnFreeZoneLevelImportID" runat="server" OnClick="BtnFreeZoneLevelImportID_Click" Text="FreeZone Level Import" class="btn btn-collection" />
                            
                            <asp:Button ID="BtnProfileData" runat="server" Text="Profile Data Import" OnClick="BtnProfileData_Click" class="btn btn-collection" />
                            <asp:Button ID="btnSetupData" runat="server" OnClick="btnSetupData_Click" Text="Setup Data Import" class="btn btn-collection" />
                        </div>
                    </div>
                </div>



                <div class="form-group">
                    <div class="col-md-8 text-center">
                        <asp:Label ID="lblerrorMessage" runat="server" Text="" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
