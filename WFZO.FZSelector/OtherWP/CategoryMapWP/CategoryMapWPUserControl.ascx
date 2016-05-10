<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryMapWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.OtherWP.CategoryMapWP.CategoryMapWPUserControl" %>

<%--<script src="../../Style%20Library/WFZO/js/jquery-1.7.2.min.js" type="text/javascript"> </script>--%>
<link rel="stylesheet" href="../../Style%20Library/WFZO/css/treeViewStyle.css" />


<script src="/Style%20Library/WFZO/js/jquery.dataTables.min.js" type="text/javascript"> </script>
<link rel="stylesheet" href="../../Style%20Library/WFZO/css/jquery.dataTables.min.css" />

<script type="text/javascript">

    $(document).ready(function () {
        $('.tree li').each(function () {
            responsive: true
            if ($(this).children('ul').length > 0) {
                $(this).addClass('parent');
            }
        });

        $('.tree li.parent > a').click(function () {
            $(this).parent().toggleClass('active');
            $(this).parent().children('ul').slideToggle('fast');
        });

        $('#all').click(function () {

            $('.tree li').each(function () {
                $(this).toggleClass('active');
                $(this).children('ul').slideToggle('fast');
            });
        });

        $('.tree li').each(function () {
            $(this).toggleClass('active');
            $(this).children('ul').slideToggle('fast');
        });

    });

</script>

<script>
    $(document).ready(function () {
        $('.example').DataTable({
            "responsive": true,
            "paging": false,
            "ordering": false,
            "searching": false,
            "info": false
        });
    });
</script>



<asp:HiddenField ID="errorMessage" runat="server" />


<asp:Repeater ID="rptCountyLevel" runat="server" OnItemDataBound="rptCountyLevel_ItemDataBound">
    <HeaderTemplate>
        <div id="wrapper">
            <div class="tree">
                <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><a><%# Eval("CategoryLevel") %></a>
            <%--<li><a><asp:Label ID="CountryLevel" runat="server" Text='<%# Eval("CategoryLevel") %>'></asp:Label></a>--%>

            <asp:Repeater ID="rptCategory" runat="server" OnItemDataBound="rptCategory_ItemDataBound">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><a><%# Eval("CategoryName") %></a>
                        <%--<asp:Label ID="CategoryName" runat="server" Text='<%# Eval("CategoryName") %>'></asp:Label>--%>
                        <asp:Repeater ID="rptSubCategory" runat="server">
                            <HeaderTemplate>
                                <ul>
                                    <li>
                                        <table id="example" class="display example" cellspacing="0" width="100%">
                                            <thead>
                                                <tr>
                                                    <th>SubCategoryName</th>
                                                    <th>Unit</th>
                                                    <th>Methodology</th>
                                                    <th>Direction</th>
                                                    <th>Definition</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>

                                <tr>
                                    <td><%# Eval("SubCategoryName") %></td>
                                    <td><%# Eval("Unit") %></td>
                                    <td><%# Eval("Methodology") %></td>
                                    <td><%# Eval("Direction").ToString() == "-1" ? "-" : "+" %></td>
                                    <td><%# Eval("Definition") %></td>
                                </tr>
                                <%--<li><a><%# Eval("SubCategoryName") %></a>--%>
                                <%--<asp:Label ID="SubCtegoryName" runat="server" Text='<%# Eval("SubCategoryName") %>'></asp:Label>--%>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                                </table>
                                </li>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>

                </ItemTemplate>
                <FooterTemplate>
                    </li>
                </ul>
                </FooterTemplate>
            </asp:Repeater>

    </ItemTemplate>
    <FooterTemplate>
        </li>
        </ul>
        </div>
    </div>
    </FooterTemplate>
</asp:Repeater>


