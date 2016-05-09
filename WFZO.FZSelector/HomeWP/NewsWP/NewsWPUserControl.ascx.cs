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
                                         </Eq><Eq>
                                           <FieldRef Name='" + Constants.List.Pages.Fields.ContentType + @"' />
                                           <Value Type='" + Commons.Type.Computed + @"'>News</Value>
                                         </Eq>
                                       </And>  
                                    </Where>
                                    <OrderBy>
                                       <FieldRef Name='" + Constants.List.BaseColumns.Created + @"' Ascending='False' />
                                    </OrderBy>";
                        query.RowLimit = 6;
                        SPListItemCollection col = list.GetItems(query);
                        DataTable NewDT = new DataTable();
                        NewDT.Columns.Add("Title", typeof(string));
                        NewDT.Columns.Add("Url", typeof(string));

                        int Counter = 0;
                        foreach (SPListItem item in col)
                        {
                            if (Counter == 5) { break; }

                            DataRow row = NewDT.NewRow();
                            row["Title"] = Convert.ToString(item[Constants.List.BaseColumns.Title]);
                            row["URL"] = Convert.ToString(item.File.ServerRelativeUrl);
                            NewDT.Rows.Add(row);
                            Counter++;
                        }

                        NewsRP.DataSource = NewDT;
                        NewsRP.DataBind();

                        if (col.Count > 5)
                        {
                            Control FooterTemplate = NewsRP.Controls[NewsRP.Controls.Count - 1].Controls[0];
                            HyperLink hplViewAll = FooterTemplate.FindControl("hplViewAll") as HyperLink;
                            hplViewAll.Visible = true;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "NewsWPUser - BindNewsRP", SPContext.Current.Site);
            }
        }

    }
}
