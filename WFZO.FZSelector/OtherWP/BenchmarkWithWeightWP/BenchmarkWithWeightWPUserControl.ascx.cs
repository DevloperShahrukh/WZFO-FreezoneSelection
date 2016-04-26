using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using WFZO.FZSelector.Classes;
using System.Collections.Generic;
using Microsoft.SharePoint;

namespace WFZO.FZSelector.BenchmarkWithWeightWP
{
    public partial class BenchmarkWithWeightWPUserControl : UserControl
    {
        public List<FreezoneAnalyticData> FreezoneDataList = new List<FreezoneAnalyticData>();
        ClsDBAccess obj = new ClsDBAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindgridview();
                bindRegion();
                PopluateReportTypeList();
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

            dt.Columns.Add("RegionId", typeof(int));
            dt.Columns.Add("CountryId", typeof(int));
            dt.Columns.Add("CityId", typeof(int));
            dt.Columns.Add("FreeZoneId", typeof(int));

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

            row["RegionId"] = ddlRegion.SelectedItem.Value;
            row["CountryId"] = ddlCountry.SelectedItem.Value;
            row["CityId"] = ddlCity.SelectedItem.Value;
            row["FreeZoneId"] = ddlFreeZone.SelectedItem.Value;

            if (string.IsNullOrEmpty(hdnCountryIds.Value))
            {
                hdnCountryIds.Value = ddlCountry.SelectedItem.Value;
            }
            else
            {
                hdnCountryIds.Value += "," + ddlCountry.SelectedItem.Value;
            }


            if (string.IsNullOrEmpty(hdnFreezoneIds.Value))
            {
                hdnFreezoneIds.Value = ddlFreeZone.SelectedItem.Value;
            }
            else
            {
                hdnFreezoneIds.Value += "," + ddlFreeZone.SelectedItem.Value;
            }


            if (temprow.Length <= 0)
            {
                dt.Rows.Add(row);
                ViewState["TempBenchmarking"] = dt;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            if (GridView1.Rows.Count > 0 /*&& grdWeightedBenchmarkingCategories.Rows.Count > 1*/)
            {
                PlSelectedZone.Visible = true;
                btnReport.Enabled = true;

                if (GridView1.Rows.Count > 1)
                {
                    BindgrdCategories(hdnCountryIds.Value);
                    pnlCategories.Visible = true;
                }

            }
            else
            {
                PlSelectedZone.Visible = false;
                btnReport.Enabled = false;
                pnlCategories.Visible = false;
            }


            if (ViewState["FreezoneDataList"] != null)
            {
                FreezoneDataList = (List<FreezoneAnalyticData>)ViewState["FreezoneDataList"];
            }

            FreezoneAnalyticData FAD = new FreezoneAnalyticData();
            FAD.RegionId = Convert.ToInt32(row["RegionId"]);
            FAD.CountryId = Convert.ToInt32(row["CountryId"]);
            FAD.CityId = Convert.ToInt32(row["CityId"]);
            FAD.FreezoneId = Convert.ToInt32(row["FreeZoneId"]);
            FreezoneDataList.Add(FAD);

            ViewState["FreezoneDataList"] = FreezoneDataList;

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

                hdnCountryIds.Value = "";
                hdnFreezoneIds.Value = "";
                ViewState["FreezoneDataList"] = null;
                FreezoneDataList = new List<FreezoneAnalyticData>();
                foreach (DataRow Freezone in dt.Rows)
                {
                    if (string.IsNullOrEmpty(hdnCountryIds.Value))
                    {
                        hdnCountryIds.Value = Convert.ToString(Freezone["CountryId"]);
                    }
                    else
                    {
                        hdnCountryIds.Value += "," + Convert.ToString(Freezone["CountryId"]);
                    }
                    if (string.IsNullOrEmpty(hdnFreezoneIds.Value))
                    {
                        hdnFreezoneIds.Value = Convert.ToString(Freezone["FreezoneId"]);
                    }
                    else
                    {
                        hdnFreezoneIds.Value += "," + Convert.ToString(Freezone["FreezoneId"]);
                    }

                    FreezoneAnalyticData FAD = new FreezoneAnalyticData();
                    FAD.RegionId = Convert.ToInt32(Freezone["RegionId"]);
                    FAD.CountryId = Convert.ToInt32(Freezone["CountryId"]);
                    FAD.CityId = Convert.ToInt32(Freezone["CityId"]);
                    FAD.FreezoneId = Convert.ToInt32(Freezone["FreeZoneId"]);
                    FreezoneDataList.Add(FAD);

                }
                ViewState["FreezoneDataList"] = FreezoneDataList;



                if (GridView1.Rows.Count > 0 /*&& grdWeightedBenchmarkingCategories.Rows.Count > 0*/)
                {
                    PlSelectedZone.Visible = true;
                    btnReport.Enabled = true;
                    //  GridView2.Visible = true;
                }
                else
                {
                    PlSelectedZone.Visible = false;
                    btnReport.Enabled = false;
                    //GridView2.Visible = false;
                }

                if (dt.Rows.Count > 1)
                {
                    BindgrdCategories(hdnCountryIds.Value);
                    pnlCategories.Visible = true;
                }
                else
                {
                    pnlCategories.Visible = false;

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

        protected void chkSelectedCategory_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow gdrow = (GridViewRow)checkbox.NamingContainer;
            TextBox txt = (TextBox)gdrow.FindControl("quantity");


            int count = 0;
            foreach (GridViewRow row in grdWeightedBenchmarkingCategories.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelectedCategory");
                if (chk.Checked)
                {
                    count++;
                }
            }
            decimal avg = 0;
            if (count != 0)
            {
                avg = 100 / count;
            }


            hdnCountryCatIds.Value = "";
            hdnFreezoneCatIds.Value = "";
            hdnCatIdAndWeightageValue.Value = "";
            foreach (GridViewRow row in grdWeightedBenchmarkingCategories.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelectedCategory");
                if (chk.Checked)
                {
                    TextBox textbox = (TextBox)row.FindControl("quantity");
                    textbox.Text = Convert.ToString(avg);

                    HiddenField hdnSubCatIds = (HiddenField)row.FindControl("hdnSubCatIds");
                    if (row.Cells[1].Text.Equals("Country level"))
                    {
                        if(string.IsNullOrEmpty(hdnCountryCatIds.Value))
                        {
                            hdnCountryCatIds.Value += hdnSubCatIds.Value;
                        }
                        else
                        {
                            hdnCountryCatIds.Value += "," + hdnSubCatIds.Value;
                        }

                    }
                    else if (row.Cells[1].Text.Equals("FreeZone level"))
                    {
                        hdnFreezoneCatIds.Value += hdnSubCatIds.Value;
                    }

                    HiddenField hdnCatId = (HiddenField)row.FindControl("hdnCatId");
                    TextBox txtWeightage = (TextBox)row.FindControl("quantity");

                    if (string.IsNullOrEmpty(hdnCatIdAndWeightageValue.Value))
                    {
                        hdnCatIdAndWeightageValue.Value += hdnCatId.Value + ":" + txtWeightage.Text;
                    }
                    else
                    {
                        hdnCatIdAndWeightageValue.Value += "," + hdnCatId.Value + ":" + txtWeightage.Text;
                    }
                }
            }

            if (checkbox.Checked)
            {
                txt.Enabled = true;
            }
            else
            {
                txt.Enabled = false;
                txt.Text = "";
            }
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            decimal weight = 0;
            foreach (GridViewRow row in grdWeightedBenchmarkingCategories.Rows)
            {

                CheckBox chkbox = (CheckBox)row.Cells[0].FindControl("chkSelectedCategory");

                if (chkbox.Checked)
                {
                    TextBox txtbox = (TextBox)row.Cells[3].FindControl("quantity");
                    weight = weight + Convert.ToInt32(txtbox.Text);
                }
            }
            if (Math.Ceiling(weight) == 100)
            {
                lblError.Visible = true;
                lblError.Text = "Total Wieghtage is approximately 100";
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Total Weightage should be approximately 100";
            }
        }


        private void BindgrdCategories(string CountryIds)
        {
            Hashtable par = new Hashtable();
            par.Add("@CountryId", CountryIds);
            DataSet ds = obj.SelectDataProc("GetCategoriesandSubCategories", par);   //   DataSet ds = GetDataSet("Select ProductId,ProductName,ParentId from ProductTable");
            DataRow[] drCategories = ds.Tables[0].Select("parentId = 0");  // Get all parents nodes

            DataTable dtCountryLevelCategories = ds.Tables[0].Clone();
            dtCountryLevelCategories.Columns.Add("SubCategoryIds", typeof(string));

            foreach (DataRow Row in drCategories)
            {
                DataRow newCategoryRow = dtCountryLevelCategories.NewRow();
                newCategoryRow["Category"] = Row["Category"];
                newCategoryRow["Id"] = Row["Id"];
                newCategoryRow["parentId"] = Row["parentId"];
                newCategoryRow["CategoryLevel"] = Row["CategoryLevel"];

                DataRow[] drSubCategories = ds.Tables[0].Select("parentId = " + Convert.ToString(Row["Id"]));

                string SubCatIds = string.Empty;
                foreach (DataRow SubCategory in drSubCategories)
                {
                    if (string.IsNullOrEmpty(SubCatIds))
                    {
                        SubCatIds = Convert.ToString(SubCategory["Id"]);
                    }
                    else
                    {
                        SubCatIds += "," + Convert.ToString(SubCategory["Id"]);
                    }
                }
                newCategoryRow["SubCategoryIds"] = SubCatIds;

                dtCountryLevelCategories.Rows.Add(newCategoryRow);
            }

            Hashtable FreezoneParameter = new Hashtable();
            FreezoneParameter.Add("@FreezoneIds", hdnFreezoneIds.Value);
            DataSet dsFreezoneCategories = obj.SelectDataProc("GetFreezoneCategoriesAndSubCategoriesByFreezoneIds", FreezoneParameter);   //   DataSet ds = GetDataSet("Select ProductId,ProductName,ParentId from ProductTable");
            DataRow[] drFreezoneCategories = dsFreezoneCategories.Tables[0].Select("parentId = 0");  // Get all parents nodes

            DataTable dtFreezoneLevelCategories = dsFreezoneCategories.Tables[0].Clone();
            dtFreezoneLevelCategories.Columns.Add("SubCategoryIds", typeof(string));


            foreach (DataRow Row in drFreezoneCategories)
            {
                DataRow newCategoryRow = dtFreezoneLevelCategories.NewRow();
                newCategoryRow["Category"] = Row["Category"];
                newCategoryRow["Id"] = Row["Id"];
                newCategoryRow["parentId"] = Row["parentId"];
                newCategoryRow["CategoryLevel"] = Row["CategoryLevel"];

                DataRow[] drSubCategories = dsFreezoneCategories.Tables[0].Select("parentId = " + Convert.ToString(Row["Id"]));

                string SubCatIds = string.Empty;
                foreach (DataRow SubCategory in drSubCategories)
                {
                    if (string.IsNullOrEmpty(SubCatIds))
                    {
                        SubCatIds = Convert.ToString(SubCategory["Id"]);
                    }
                    else
                    {
                        SubCatIds += "," + Convert.ToString(SubCategory["Id"]);
                    }
                }
                newCategoryRow["SubCategoryIds"] = SubCatIds;

                dtFreezoneLevelCategories.Rows.Add(newCategoryRow);
            }
            dtCountryLevelCategories.Merge(dtFreezoneLevelCategories);

            grdWeightedBenchmarkingCategories.DataSource = dtCountryLevelCategories;
            grdWeightedBenchmarkingCategories.DataBind();
        }

        public void PopluateReportTypeList()
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
                                           <Value Type='" + Commons.Type.Text + @"'>" + "Weighted" + @"</Value>
                                         </Eq>
                                    </Where>";
                    DataTable dt = list.GetItems(query).GetDataTable();
                    if (dt.Rows.Count > 0)
                    {
                        rblReportType.DataSource = dt;
                        rblReportType.DataTextField = "Title";
                        rblReportType.DataValueField = "FileLeafRef";
                        rblReportType.DataBind();
                    }
                }
            }
        }

        protected void grdWeightedBenchmarkingCategories_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    //If Salary is less than 10000 than set the row Background Color to Cyan  
            //    if (Convert.ToInt32(e.Row.Cells[3].Text) < 10000)
            //    {
            //        //e.Row.BackColor = Color.Cyan;
            //    }
            //}  

        }
    }

}
