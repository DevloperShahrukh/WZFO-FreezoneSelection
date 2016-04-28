using Microsoft.SharePoint;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.NewsWP
{
    public partial class NewsWPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindNewsRP();
            }
        }
        protected void BindNewsRP()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url + Constants.Subsite.News))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists.TryGetList(Constants.List.Pages.Name);

                        SPQuery query = new SPQuery();
                        query.Query = @"<Where>
                                       <And>
                                            <Eq>
                                           <FieldRef Name='" + Constants.List.BaseColumns.IsActive + @"' />
                                           <Value Type='" + Commons.Type.Boolean + @"'>" + 1 + @"</Value>
                                         </Eq>
<Eq>
                                           <FieldRef Name='" + Constants.List.Pages.Fields.ContentType + @"' />
                                           <Value Type='" + Commons.Type.Computed + @"'>News</Value>
                                         </Eq>
                                       </And>  
                                    </Where>
                                    <OrderBy>
                                       <FieldRef Name='" + Constants.List.BaseColumns.Created + @"' Ascending='False' />
                                    </OrderBy>";
                        query.RowLimit = 5;
                        SPListItemCollection col = list.GetItems(query);
                        DataTable NewDT = new DataTable();
                        NewDT.Columns.Add("Title", typeof(string));
                        NewDT.Columns.Add("Url", typeof(string));
                        foreach (SPListItem item in col)
                        {
                            DataRow row = NewDT.NewRow();
                            row["Title"] = Convert.ToString(item[Constants.List.BaseColumns.Title]);
                            row["URL"] = Convert.ToString(item.File.ServerRelativeUrl);
                            NewDT.Rows.Add(row);
                        }
                        NewsRP.DataSource = NewDT;
                        NewsRP.DataBind();
                    }
                }
            }

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "Page_Load", SPContext.Current.Site);
            }
        }
        protected void NewsRP_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try { 
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lit = (Literal)e.Item.FindControl("ltrUrl");
                Label Title = (Label)e.Item.FindControl("lblTitle");
                Label Url = (Label)e.Item.FindControl("lblUrl");
                lit.Text = "<a href='" + Url.Text + "'>" + Title.Text + "</a>";
            }
            }

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "Page_Load", SPContext.Current.Site);
            }
        }
    }
}
