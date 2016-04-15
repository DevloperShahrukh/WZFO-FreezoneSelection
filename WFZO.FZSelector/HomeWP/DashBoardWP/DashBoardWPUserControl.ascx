<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashBoardWPUserControl.ascx.cs" Inherits="WFZO.FZSelector.HomeWP.DashBoardWP.DashBoardWPUserControl" %>


		<div class="row">
        	<div class="welcome">
	        	<h1>Welcome <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label></h1>
            </div>
            
            <div class="col-md-4">
            	<div class="db-main-tab mb20">
                	<div class="db-main-iconbox">
                    	<div class="fzprofileb"></div>
                    </div> 
                    <div class="db-main-detail">
	                    <h3><a href="/Pages/FreeZoneProfile.aspx">Free Zone Profile</a></h3>    
                        <p>Lorem ipsum dolor sit amet, sed dia nonummy nibh euismod tincidunt ut et dolore magna aliquam erat</p>
                        <div class="help">
                        	<a href="#">Need Help?</a>
                        </div>           
                    </div>
                </div>            
            </div>
            
            <div class="col-md-4">
            	<div class="db-main-tab mb20">
                	<div class="db-main-iconbox">
                    	<div class="benchmarkingb"></div>
                    </div> 
                    <div class="db-main-detail">
	                    <h3><a href="/Pages/Benchmarking.aspx">Benchmarking</a></h3>    
                        <p>Lorem ipsum dolor sit amet, sed dia nonummy nibh euismod tincidunt ut et dolore magna aliquam erat</p>
                        <div class="help">
                        	<a href="#">Need Help?</a>
                        </div>           
                    </div>
                </div>            
            </div>
            
            <div class="col-md-4">
            	<div class="db-main-tab mb20">
                	<div class="db-main-iconbox">
                    	<div class="weightageb"></div>
                    </div> 
                    <div class="db-main-detail">
	                    <h3><a href="/Pages/BenchmarkwithWeightage.aspx">Benchmark with Weightage</a></h3>    
                        <p>Lorem ipsum dolor sit amet, sed dia nonummy nibh euismod tincidunt</p>
                        <div class="help">
                        	<a href="#">Need Help?</a>
                        </div>           
                    </div>
                </div>            
            </div>            
            
        </div>
        
        <div class="row mt35">
        	<div class="col-md-2">
            
            </div>
            
            <div class="col-md-8">
            	<div class="notes-box mb20">
                	<h3>Most viewed/queried freezones</h3>
                    This report will show the most top viewed/searched Freezones along with their region, country, and city using monitoring tool.
                </div>
                <div class="notes-box mb20">
                	<h3>Most used Categories and sub-categories</h3>
                    These reports will show the most common categories and sub-categories select by the members in the Benchmarking module.
                </div>
                <div class="notes-box mb20">
                	<h3>Highest weightage Categories</h3>
                    This report will show the most/highest weighted category by the members in weighted analysis.
                </div>
            </div>
            
            <div class="col-md-2">
            
            </div>        
        
        </div>

