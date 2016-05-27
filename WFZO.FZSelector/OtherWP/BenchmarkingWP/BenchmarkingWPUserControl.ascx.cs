using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.BenchmarkingWP
{
    public partial class BenchmarkingWPUserControl : UserControl
    {
        public List<FreezoneAnalyticData> FreezoneDataList = new List<FreezoneAnalyticData>();
        ClsDBAccess obj = new ClsDBAccess();

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (SPWebPartManager.GetCurrentWebPartManager(Page).DisplayMode != WebPartManager.BrowseDisplayMode)
            {
                foreach (object validator in Page.Validators)
                {
                    if (validator is BaseValidator)
                    {
                        ((BaseValidator)validator).Enabled = false;
                    }
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                tvCountryCategories.Attributes.Add("onclick", "OnTreeClick(event)");
                tvFreezoneCategories.Attributes.Add("onclick", "OnTreeClick(event)");
                bindgridview();
                bindRegion();
                PopluateReportTypeList();
            }
        }
        public void bindRegion()
        {
            try
            {
                obj.BindCombo(ddlRegion, "RegionName", "RegionID", "FillRegion");
                ddlRegion.Items.Insert(0, new ListItem("Select", "0"));

                ddlCountry.Items.Clear();
                ddlCity.Items.Clear();
                ddlFreeZone.Items.Clear();
                ddlCountry.Items.Insert(0, new ListItem("Select", "0"));
                ddlCity.Items.Insert(0, new ListItem("Select", "0"));
                ddlFreeZone.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "bindRegion", SPContext.Current.Site);
            }
        }
        public void bindgridview()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("SR", typeof(int));
                dt.Columns.Add("Region", typeof(string));
                dt.Columns.Add("Country", typeof(string));
                dt.Columns.Add("City", typeof(string));
                dt.Columns.Add("FreeZone", typeof(string));
                dt.Columns.Add("RegionId", typeof(int));
                dt.Columns.Add("CountryId", typeof(int));
                dt.Columns.Add("CityId", typeof(int));
                dt.Columns.Add("FreeZoneId", typeof(int));

                ViewState["TempBenchmarking"] = dt;
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "bindgridview", SPContext.Current.Site);
            }
        }
        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlRegion.SelectedItem.Value != "0")
                {
                    Hashtable par = new Hashtable();
                    par.Add("@RegionID", ddlRegion.SelectedItem.Value);
                    obj.BindList(ddlCountry, "FillCountry", "CountryName", par, "CountryName", "CountryId");
                    ddlCountry.Items.Insert(0, new ListItem("Select", "0"));
                }
                ddlCity.Items.Clear();
                ddlFreeZone.Items.Clear();
                ddlCity.Items.Insert(0, new ListItem("Select", "0"));
                ddlFreeZone.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "ddlRegion_SelectedIndexChanged", SPContext.Current.Site);
            }
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCountry.SelectedItem.Value != "0")
                {
                    Hashtable par = new Hashtable();
                    par.Add("@CountryId", ddlCountry.SelectedItem.Value);
                    obj.BindList(ddlCity, "FillCity", "CityName", par, "CityName", "CityID");
                    ddlCity.Items.Insert(0, new ListItem("Select", "0"));

                }
                ddlFreeZone.Items.Clear();
                ddlFreeZone.Items.Insert(0, new ListItem("Select", "0"));
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "ddlCountry_SelectedIndexChanged", SPContext.Current.Site);
            }

        }
        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCity.SelectedItem.Value != "0")
                {
                    Hashtable par = new Hashtable();
                    par.Add("@CityId", ddlCity.SelectedItem.Value);
                    obj.BindList(ddlFreeZone, "FillFreeZone", "FreezoneName", par, "FreezoneName", "FreeZoneID");
                    ddlFreeZone.Items.Insert(0, new ListItem("Select", "0"));
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "ddlCity_SelectedIndexChanged", SPContext.Current.Site);
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {

                PlSelectedZone.Visible = true;
                DataTable dt = ViewState["TempBenchmarking"] as DataTable;
                DataRow[] temprow = dt.Select("Region ='" + ddlRegion.SelectedItem.Text + "' AND Country='" + ddlCountry.SelectedItem.Text + "' AND City='" + ddlCity.SelectedItem.Text + "' AND FreeZone = '" + ddlFreeZone.SelectedItem.Text + "'");
                DataRow row = dt.NewRow();
                row["SR"] = dt.Rows.Count + 1;
                row["Region"] = ddlRegion.SelectedItem.Text;
                row["Country"] = ddlCountry.SelectedItem.Text;
                row["City"] = ddlCity.SelectedItem.Text;
                row["FreeZone"] = ddlFreeZone.SelectedItem.Text;

                row["RegionId"] = ddlRegion.SelectedItem.Value;
                row["CountryId"] = ddlCountry.SelectedItem.Value;
                row["CityId"] = ddlCity.SelectedItem.Value;
                row["FreeZoneId"] = ddlFreeZone.SelectedItem.Value;

                hdnFreezoneIds.Value = "0";

                if (temprow.Length <= 0)
                {
                    dt.Rows.Add(row);
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }

                hdnFreezoneCount.Value = Convert.ToString(GridView1.Rows.Count);

                if (GridView1.Rows.Count >= 2)
                {
                    PnlTreeViews.Visible = true;
                    FreezoneDataList = new List<FreezoneAnalyticData>();
                    hdnCountryIds.Value = "0";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        DataSet ds = obj.FillData("SELECT [CountryId] FROM [dbo].[Country] where CountryName = '" + Convert.ToString(dt.Rows[i]["Country"]) + "'");
                        if (hdnCountryIds.Value == "0")
                        {
                            hdnCountryIds.Value = Convert.ToString(ds.Tables[0].Rows[0]["CountryId"]);
                        }
                        else
                        {
                            hdnCountryIds.Value += "," + Convert.ToString(ds.Tables[0].Rows[0]["CountryId"]);
                        }

                        if (hdnFreezoneIds.Value == "0")
                        {
                            hdnFreezoneIds.Value = Convert.ToString(dt.Rows[i]["FreezoneId"]);
                        }
                        else
                        {
                            hdnFreezoneIds.Value += "," + Convert.ToString(dt.Rows[i]["FreezoneId"]);
                        }

                        PopulateListOfSelectedFreezones(dt.Rows[i]);
                    }
                    BindTreeViewControl(hdnCountryIds.Value);
                    BindFreezoneTreeView(hdnFreezoneIds.Value);


                    lblError.Visible = false;
                    //btnReport.Enabled = true;
                    //btnReport.Visible = true;
                    tvCountryCategories.Visible = true;
                    tvFreezoneCategories.Visible = true;

                }
                else
                {
                    lblError.Visible = false;
                    //btnReport.Enabled = false;
                    tvCountryCategories.Visible = false;
                    tvFreezoneCategories.Visible = false;
                }

                //else if(rblReportType.SelectedItem.Text.Equals(Constants.ReportTypes.Trend))
                //{
                //    pnlTrend.Visible = true;

                //    DataTable dt = ViewState["TempBenchmarking"] as DataTable;
                //    DataRow[] temprow = dt.Select("Region ='" + ddlRegion.SelectedItem.Text + "' AND Country='" + ddlCountry.SelectedItem.Text + "' AND City='" + ddlCity.SelectedItem.Text + "' AND FreeZone = '" + ddlFreeZone.SelectedItem.Text + "'");
                //    DataRow row = dt.NewRow();
                //    row["SR"] = dt.Rows.Count + 1;
                //    row["Region"] = ddlRegion.SelectedItem.Text;
                //    row["Country"] = ddlCountry.SelectedItem.Text;

                //}

                rblReportType.Visible = true;
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "Button1_Click", SPContext.Current.Site);
            }
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "delRow")
                {
                    DataTable dt = (DataTable)ViewState["TempBenchmarking"];
                    int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    ViewState["TempBenchmarking"] = dt;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    hdnFreezoneIds.Value = "0";

                    if (GridView1.Rows.Count > 1)
                    {
                        FreezoneDataList = new List<FreezoneAnalyticData>();
                        hdnCountryIds.Value = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataSet ds = obj.FillData("SELECT [CountryId] FROM [dbo].[Country] where CountryName = '" + Convert.ToString(dt.Rows[i]["Country"]) + "'");
                            if (hdnCountryIds.Value == "")
                            {
                                hdnCountryIds.Value = Convert.ToString(ds.Tables[0].Rows[0]["CountryId"]);
                            }
                            else
                            {
                                hdnCountryIds.Value += "," + Convert.ToString(ds.Tables[0].Rows[0]["CountryId"]);
                            }

                            if (hdnFreezoneIds.Value == "0")
                            {
                                hdnFreezoneIds.Value = Convert.ToString(dt.Rows[i]["FreezoneId"]);
                            }
                            else
                            {
                                hdnFreezoneIds.Value += "," + Convert.ToString(dt.Rows[i]["FreezoneId"]);
                            }

                            PopulateListOfSelectedFreezones(dt.Rows[i]);
                        }
                        BindTreeViewControl(hdnCountryIds.Value);
                        BindFreezoneTreeView(hdnFreezoneIds.Value);

                        lblError.Visible = false;
                        //btnReport.Visible = true;
                        tvCountryCategories.Visible = true;
                        tvFreezoneCategories.Visible = true;
                    }
                    else
                    {
                        lblError.Visible = false;
                        //btnReport.Visible = false;
                        tvCountryCategories.Visible = false;
                        tvFreezoneCategories.Visible = false;
                    }

                    hdnFreezoneCount.Value = Convert.ToString(GridView1.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "GridView1_RowCommand", SPContext.Current.Site);
            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["TempBenchmarking"];
                GridView1.PageIndex = e.NewPageIndex;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "GetTable", SPContext.Current.Site);
            }
        }

        private void BindFreezoneTreeView(string FreezoneIds)
        {
            try
            {
                tvFreezoneCategories.Nodes.Clear();
                Hashtable par = new Hashtable();
                par.Add("@FreezoneIds", FreezoneIds);
                DataSet ds = obj.SelectDataProc("GetFreezoneCategoriesAndSubCategoriesByFreezoneIds", par);                //   DataSet ds = GetDataSet("Select ProductId,ProductName,ParentId from ProductTable");
                DataRow[] Rows = ds.Tables[0].Select("parentId = 0"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["Category"].ToString(), Rows[i]["Id"].ToString());

                    root.SelectAction = TreeNodeSelectAction.Expand;
                    CreateNode(root, ds.Tables[0], tvFreezoneCategories);
                    tvFreezoneCategories.Nodes.Add(root);
                }
                tvFreezoneCategories.CollapseAll();
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "GridView1_PageIndexChanging", SPContext.Current.Site);
            }
        }
        private void BindTreeViewControl(string CountryIds)
        {
            try
            {
                tvCountryCategories.Nodes.Clear();
                Hashtable par = new Hashtable();
                par.Add("@CountryId", CountryIds);
                DataSet ds = obj.SelectDataProc("GetCategoriesandSubCategories", par);                //   DataSet ds = GetDataSet("Select ProductId,ProductName,ParentId from ProductTable");
                DataRow[] Rows = ds.Tables[0].Select("parentId = 0"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["Category"].ToString(), Rows[i]["Id"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    CreateNode(root, ds.Tables[0], tvCountryCategories);
                    tvCountryCategories.Nodes.Add(root);
                }

                tvCountryCategories.CollapseAll();
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "BindTreeViewControl", SPContext.Current.Site);
            }
        }
        public void CreateNode(TreeNode node, DataTable Dt, TreeView Control)
        {
            try
            {
                DataRow[] Rows = Dt.Select("parentId =" + node.Value);
                if (Rows.Length == 0) { return; }
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode Childnode = new TreeNode(Rows[i]["Category"].ToString(), Rows[i]["Id"].ToString());
                    Childnode.SelectAction = TreeNodeSelectAction.Expand;
                    Childnode.ToolTip = Convert.ToString(Rows[i]["Definition"]);
                    Childnode.Text = "<span id='" + Childnode.Value + "' class='treeviewnode'>" + Childnode.Text + "</span>";

                    node.ChildNodes.Add(Childnode);

                    //CreateNode(Childnode, Dt);
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "CreateNode", SPContext.Current.Site);
            }
        }

        protected void CountryTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            // this.CheckAllChildNodes(e.Node, e.Node.Checked);
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {

            //if (tvCountryCategories.CheckedNodes.Count <= 0)
            //{
            //    lblError.Visible = true;
            //    lblError.Text = "Please check atleast one checkbox from treeview";
            //}
            //else
            //{

            //    lblError.Visible = true;
            //    lblError.Text = "Report can be generated now";

            //}
        }

        protected void PopulateListOfSelectedFreezones(DataRow SelectedFreezoneDetail)
        {
            try
            {
                FreezoneAnalyticData FDA = new FreezoneAnalyticData();
                FDA.RegionId = Convert.ToInt32(SelectedFreezoneDetail["RegionId"]);
                FDA.CountryId = Convert.ToInt32(SelectedFreezoneDetail["CountryId"]);
                FDA.CityId = Convert.ToInt32(SelectedFreezoneDetail["CityId"]);
                FDA.FreezoneId = Convert.ToInt32(SelectedFreezoneDetail["FreezoneId"]);

                FreezoneDataList.Add(FDA);
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "PopulateListOfSelectedFreezones", SPContext.Current.Site);
            }
        }
        public void PopluateReportTypeList()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.RootWeb.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists.TryGetList(Constants.List.Reports.Name);

                        SPQuery query = new SPQuery();
                        query.Query = @"<Where>
                                         <Eq>
                                           <FieldRef Name='" + Constants.List.Reports.Fields.Module + @"' />
                                           <Value Type='" + Commons.Type.Text + @"'>" + Constants.Modules.Benchmarking + @"</Value>
                                         </Eq>
                                    </Where>";

                        DataTable dt = list.GetItems(query).GetDataTable();
                        if (dt.Rows.Count > 0)
                        {
                            rblReportType.DataSource = dt;
                            rblReportType.DataTextField = "Title";
                            rblReportType.DataValueField = "FileLeafRef";
                            rblReportType.DataBind();
                            rblReportType.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "PopluateReportTypeList", SPContext.Current.Site);
            }
        }
    }
}