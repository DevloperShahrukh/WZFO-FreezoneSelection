using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.FreeZoneWP
{

    public partial class FreeZoneWPUserControl : UserControl
    {
        public List<FreezoneAnalyticData> FreezoneDataList = new List<FreezoneAnalyticData>();
        ClsDBAccess obj = new ClsDBAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tvFreezoneProfileCategories.Attributes.Add("onclick", "OnTreeClick(event)");
                tvFreezoneCategories.Attributes.Add("onclick", "OnTreeClick(event)");
                tvCountryCategories.Attributes.Add("onclick", "OnTreeClick(event)");
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
            }
            catch (Exception Ex) { throw Ex; }
        }

        private void BindFreezoneProfileCategoryTreeView(int FreezoneId)
        {
            try
            {
                tvFreezoneProfileCategories.Nodes.Clear();
                Hashtable par = new Hashtable();
                par.Add("@FreezoneId", FreezoneId);
                DataSet ds = obj.SelectDataProc("GetCategoriesAndSubCategoriesOfFreezoneProfileByFreezoneId", par);                //   DataSet ds = GetDataSet("Select ProductId,ProductName,ParentId from ProductTable");
                DataRow[] Rows = ds.Tables[0].Select("parentId = 0"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["Category"].ToString(), Rows[i]["Id"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    
                    CreateNode(root, ds.Tables[0], tvFreezoneProfileCategories);
                    tvFreezoneProfileCategories.Nodes.Add(root);
                }
            }
            catch (Exception Ex) { throw Ex; }
        }

        private void BindCountryCategoryTreeView(string CountryId)
        {
            try
            {
                tvCountryCategories.Nodes.Clear();
                Hashtable par = new Hashtable();
                par.Add("@CountryId", CountryId);
                DataSet ds = obj.SelectDataProc("GetCategoriesandSubCategories", par);                //   DataSet ds = GetDataSet("Select ProductId,ProductName,ParentId from ProductTable");
                DataRow[] Rows = ds.Tables[0].Select("parentId = 0"); // Get all parents nodes
                for (int i = 0; i < Rows.Length; i++)
                {
                    TreeNode root = new TreeNode(Rows[i]["Category"].ToString(), Rows[i]["Id"].ToString());
                    root.SelectAction = TreeNodeSelectAction.Expand;
                    CreateNode(root, ds.Tables[0], tvCountryCategories);
                    tvCountryCategories.Nodes.Add(root);
                }
            }
            catch (Exception Ex) { throw Ex; }
        }
        public void CreateNode(TreeNode node, DataTable Dt, TreeView Control)
        {
            DataRow[] Rows = Dt.Select("parentId =" + node.Value);
            if (Rows.Length == 0) { return; }
            for (int i = 0; i < Rows.Length; i++)
            {
                TreeNode Childnode = new TreeNode(Rows[i]["Category"].ToString(), Rows[i]["Id"].ToString());
                Childnode.SelectAction = TreeNodeSelectAction.Expand;

                Childnode.ToolTip = Convert.ToString(Rows[i]["Definition"]);
                Childnode.Target = "target";
                Childnode.Text = "<span id='" + Childnode.Value + "' class='treeviewnode'>" + Childnode.Text + "</span>";
                node.ChildNodes.Add(Childnode);

                //CreateNode(Childnode, Dt);
            }
        }


        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.ChildNodes)
            {
                node.Checked = nodeChecked;
                if (node.ChildNodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }
        protected void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {

            /* Calls the CheckAllChildNodes method, passing in the current 
            Checked value of the TreeNode whose checked state changed. */
            this.CheckAllChildNodes(e.Node, e.Node.Checked);

        }

        protected void ddlFreeZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            int FreezoneId = Convert.ToInt32(ddlFreeZone.SelectedItem.Value);

            BindFreezoneProfileCategoryTreeView(FreezoneId);
            BindCountryCategoryTreeView(ddlCountry.SelectedItem.Value);
            BindFreezoneTreeView(ddlFreeZone.SelectedItem.Value);


            FreezoneAnalyticData FAD = new FreezoneAnalyticData();
            FAD.CityId = Convert.ToInt32(ddlCity.SelectedItem.Value);
            FAD.CountryId = Convert.ToInt32(ddlCountry.SelectedItem.Value);
            FAD.FreezoneId = Convert.ToInt32(ddlFreeZone.SelectedItem.Value);
            FAD.RegionId = Convert.ToInt32(ddlRegion.SelectedItem.Value);
            FreezoneDataList.Add(FAD);

        }




    }
}
