using Microsoft.SharePoint;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WFZO.FZSelector.OtherWP.NewsListingWP
{
    public partial class NewsListingWPUserControl : UserControl
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
                                       <FieldRef Name='" + Constants.List.BaseColumns.Created + @"' Ascending='True' />
                                    </OrderBy>";
                    query.RowLimit = 5;
                    SPListItemCollection col = list.GetItems(query);
                    DataTable NewDT = new DataTable();
                    NewDT.Columns.Add("Title", typeof(string));
                    NewDT.Columns.Add("ArticleStartDate", typeof(string));
                    NewDT.Columns.Add("Comments", typeof(string));
                    NewDT.Columns.Add("Image", typeof(string));
                    NewDT.Columns.Add("Url", typeof(string));
                    foreach (SPListItem item in col)
                    {
                        DataRow row = NewDT.NewRow();
                        row["Title"] = Convert.ToString(item[Constants.List.BaseColumns.Title]);
                        row["ArticleStartDate"] = Convert.ToDateTime(item[Constants.ContentType.News.Date]).ToLongDateString();
                        row["Comments"] = Convert.ToString(item[Constants.ContentType.News.Comments]);



                        row["Image"] = GetSrcFromImgTag(Convert.ToString(item["PublishingPageImage"]));
                        row["URL"] = Convert.ToString(item.File.ServerRelativeUrl);

                        NewDT.Rows.Add(row);
                    }
                    NewsRP.DataSource = NewDT;
                    NewsRP.DataBind();
                }
            }
        }

        public static string GetSrcFromImgTag(string imgTag)
        {
            int start = imgTag.IndexOf("src=") + 5;
            int end = imgTag.IndexOf("\"", start);

            if (end > start)
                return imgTag.Substring(start, end - start);
            else
                return "";

        }

        protected void NewsRP_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lit = (Literal)e.Item.FindControl("ltrUrl");
                Label Title = (Label)e.Item.FindControl("lblTitle");
                Label LinkURL = (Label)e.Item.FindControl("lblUrl");

                string[] getLink = LinkURL.Text.Split(',');
                lit.Text = "<a href='" + getLink[0] + "'>" + Title.Text + "</a>";

            }
        }
    }
}
