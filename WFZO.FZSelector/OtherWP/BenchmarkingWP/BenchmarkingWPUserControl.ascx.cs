using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WFZO.FZSelector.BenchmarkingWP
{
    public partial class BenchmarkingWPUserControl : UserControl
    {
        ClsDBAccess obj = new ClsDBAccess();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CountryTreeView.Attributes.Add("onclick", "OnTreeClick(event)");
                bindgridview();
                bindRegion();
            }
        }
        public void bindRegion()
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
        public void bindgridview()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SR", typeof(int));
            dt.Columns.Add("Region", typeof(string));
            dt.Columns.Add("Country", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("FreeZone", typeof(string));
            ViewState["TempBenchmarking"] = dt;
        }
        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
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
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
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
        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCity.SelectedItem.Value != "0")
            {
                Hashtable par = new Hashtable();
                par.Add("@CityId", ddlCity.SelectedItem.Value);
                obj.BindList(ddlFreeZone, "FillFreeZone", "FreezoneName", par, "FreezoneName", "FreeZoneID");
                ddlFreeZone.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
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

            if (temprow.Length <= 0)
            {
                dt.Rows.Add(row);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            if (GridView1.Rows.Count >= 2)
            {

                string countries = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    DataSet ds = obj.FillData("SELECT [CountryId] FROM [dbo].[Country] where CountryName = '" + Convert.ToString(dt.Rows[i]["Country"]) + "'");
                    if (countries == "")
                    {
                        countries = Convert.ToString(ds.Tables[0].Rows[0]["CountryId"]);
                    }
                    else
                    {
                        countries = countries + "," + Convert.ToString(ds.Tables[0].Rows[0]["CountryId"]);
                    }


                }
                BindTreeViewControl(countries);

                lblError.Visible=false;
                btnReport.Enabled = true;
                CountryTreeView.Visible = true;
            }
            else
            {
                lblError.Visible=false;
                btnReport.Enabled = false;
                CountryTreeView.Visible = false;
            }
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delRow")
            {
                DataTable dt = (DataTable)ViewState["TempBenchmarking"];
                int rowIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                dt.Rows.Remove(dt.Rows[rowIndex]);
                ViewState["TempBenchmarking"] = dt;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                if (GridView1.Rows.Count > 1)
                {
                    string countries = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        DataSet ds = obj.FillData("SELECT [CountryId] FROM [dbo].[Country] where CountryName = '" + Convert.ToString(dt.Rows[i]["Country"]) + "'");
                        if (countries == "")
                        {
                            countries = Convert.ToString(ds.Tables[0].Rows[0]["CountryId"]);
                        }
                        else
                        {
                            countries = countries + "," + Convert.ToString(ds.Tables[0].Rows[0]["CountryId"]);
                        }


                    }
                    BindTreeViewControl(countries);
                    lblError.Visible = false;
                    btnReport.Visible = true;
                    CountryTreeView.Visible = true;
                }
                else
                {
                    lblError.Visible = false;
                    btnReport.Visible = false;
                    CountryTreeView.Visible = false;
                    //CountryTreeView.
                }

            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["TempBenchmarking"];
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        private void BindTreeViewControl(string CountryIds)
        {
            try
            {
                CountryTreeView.Nodes.Clear();
                Hashtable par = new Hashtable();
                par.Add("@CountryId", CountryIds);
                DataSet ds = obj.SelectDataProc("GetCategoriesandSubCategories", par);                //   DataSet ds = GetDataSet("Select ProductId,ProductName,ParentId from ProductTable");
                DataRow[] Rows = ds.Tables[0].Select("parentId = 0"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["Category"].ToString(), Rows[i]["Id"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    CreateNode(root, ds.Tables[0]);
                    CountryTreeView.Nodes.Add(root);
                }
            }
            catch (Exception Ex) { throw Ex; }
        }


        

        public void CreateNode(TreeNode node, DataTable Dt)
        {
            DataRow[] Rows = Dt.Select("parentId =" + node.Value);
            if (Rows.Length == 0) { return; }
            for (int i = 0; i < Rows.Length; i++)
            {
                TreeNode Childnode = new TreeNode(Rows[i]["Category"].ToString(), Rows[i]["Id"].ToString());
                Childnode.SelectAction = TreeNodeSelectAction.Expand;
                node.ChildNodes.Add(Childnode);
                CreateNode(Childnode, Dt);
            }
        }


        protected void CountryTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            // this.CheckAllChildNodes(e.Node, e.Node.Checked);
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            
            if (CountryTreeView.CheckedNodes.Count <= 0)
            {
                lblError.Visible = true;
                lblError.Text = "Please check atleast one checkbox from treeview";
            }
            else
            {

                lblError.Visible = true;
                lblError.Text = "Report can be generated now";

            }
        }

    }
}