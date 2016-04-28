using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using WFZO.FZSelector.Classes;
using Microsoft.SharePoint;

namespace WFZO.FZSelector.OtherWP.CategoryMapWP
{
    public partial class CategoryMapWPUserControl : UserControl
    {
        ClsDBAccess obj = new ClsDBAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                populateCategory();

            }
        }


        protected void populateCategory()
        {

            try
            {
                rptCountyLevel.DataSource = getCategoryLevelList();

                rptCountyLevel.DataBind();
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "populateCategory", SPContext.Current.Site);
            }

        }

        protected DataTable getCategoryLevelList()
        {
            DataSet ds = new DataSet();
            try
            {
                Hashtable par = new Hashtable();
                ds = obj.SelectDataProc("GetCategoryLevelList", par);                //   DataSet ds = GetDataSet("Select ProductId,ProductName,ParentId from ProductTable");
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "getCategoryLevelList", SPContext.Current.Site);
            }

            return ds.Tables[0];
        }

        protected DataTable getCategorylList(string CategoryLevel)
        {
            DataSet ds = new DataSet();
            try
            {
                Hashtable par = new Hashtable();
                par.Add("@CategoryLevel", CategoryLevel);
                ds = obj.SelectDataProc("GetCategoryList", par);                //   DataSet ds = GetDataSet("Select ProductId,ProductName,ParentId from ProductTable");

            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "getCategorylList", SPContext.Current.Site);
            }
            return ds.Tables[0];
        }

        protected DataTable getSubCategoryList(string CategoryId)
        {
            DataSet ds = new DataSet();
            try
            {
                Hashtable par = new Hashtable();
                par.Add("@CategoryId", CategoryId);
                ds = obj.SelectDataProc("GetSubCategoryList", par);                //   DataSet ds = GetDataSet("Select ProductId,ProductName,ParentId from ProductTable");

            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "getSubCategoryList", SPContext.Current.Site);
            }
            return ds.Tables[0];
        }

        protected void rptCountyLevel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView drow = (DataRowView)e.Item.DataItem;
                    string s = drow["CategoryLevel"].ToString();
                    DataTable dt = getCategorylList(s);
                    Repeater rptCategory = (Repeater)e.Item.FindControl("rptCategory");
                    rptCategory.DataSource = dt;
                    rptCategory.DataBind();

                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "rptCountyLevel_ItemDataBound", SPContext.Current.Site);
            }
        }

        protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRowView drow = (DataRowView)e.Item.DataItem;
                    string s = drow["CategoryId"].ToString();
                    DataTable dt = getSubCategoryList(s);
                    Repeater rptSubCategory = (Repeater)e.Item.FindControl("rptSubCategory");
                    rptSubCategory.DataSource = dt;
                    rptSubCategory.DataBind();
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "rptCategory_ItemDataBound", SPContext.Current.Site);
            }
        }
    }
}
