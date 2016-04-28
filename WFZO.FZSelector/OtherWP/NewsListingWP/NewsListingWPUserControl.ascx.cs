using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.OtherWP.NewsListingWP
{
    public partial class NewsListingWPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetPageSize();
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


                            string defaultiamge = "../../Style Library/WFZO/img/news.jpg";
                            if (String.IsNullOrWhiteSpace(Convert.ToString(item["PublishingPageImage"])))
                            {
                                row["Image"] = defaultiamge;
                            }
                            else
                            {

                                row["Image"] = GetSrcFromImgTag(Convert.ToString(item["PublishingPageImage"]));
                            }
                            row["URL"] = Convert.ToString(item.File.ServerRelativeUrl);

                            NewDT.Rows.Add(row);
                        }

                        PagedDataSource pagedData = new PagedDataSource();

                        pagedData.AllowPaging = true;
                        pagedData.PageSize = PageSize;
                        pagedData.DataSource = NewDT.DefaultView;
                        pagedData.CurrentPageIndex = CurrentPage;


                        lblCurrentPage.Text = "<b>Page:</b> " + (CurrentPage + 1).ToString() + " of " + pagedData.PageCount.ToString();

                        PageCount = pagedData.PageCount;

                        // Disable Prev/Next First/Last buttons if necessary
                        cmdPrev.Enabled = !pagedData.IsFirstPage;
                        cmdFirst.Enabled = !pagedData.IsFirstPage;
                        cmdNext.Enabled = !pagedData.IsLastPage;
                        cmdLast.Enabled = !pagedData.IsLastPage;



                        // Wire up the page numbers
                        if (pagedData.PageCount > 1)
                        {
                            rptPages.Visible = true;
                            ArrayList pages = new ArrayList();
                            for (int i = 0; i < pagedData.PageCount; i++)
                                if (i == CurrentPage)
                                {

                                    pages.Add("<b>" + (i + 1).ToString() + "</b>");
                                }
                                else
                                {
                                    pages.Add((i + 1).ToString());
                                }
                            rptPages.DataSource = pages;
                            rptPages.DataBind();
                        }
                        else
                        {
                            rptPages.Visible = false;
                        }

                        NewsRP.DataSource = pagedData;
                        NewsRP.DataBind();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "BindNewsRP", SPContext.Current.Site);
            }
        }
        public string GetSrcFromImgTag(string imgTag)
        {
            string result = "";
            try
            {
                int start = imgTag.IndexOf("src=") + 5;
                int end = imgTag.IndexOf("\"", start);

                if (end > start)
                    result = imgTag.Substring(start, end - start);
                else
                    result = "";
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "GetSrcFromImgTag", SPContext.Current.Site);
            }

            return result;
        }

        protected void NewsRP_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "NewsRP_ItemDataBound", SPContext.Current.Site);
            }
        }

        //Pageing
        protected int CurrentPage
        {
            get
            {
                // look for current page in ViewState
                object o = this.ViewState["_CurrentPage"];
                if (o == null)
                    return 0;   // default to showing the first page
                else
                    return (int)o;
            }

            set
            {
                this.ViewState["_CurrentPage"] = value;
            }
        }

        protected int PageSize
        {
            get
            {
                // look for current page in ViewState
                object o = this.ViewState["PageSize"];
                if (o == null)
                    return 1;   // default to showing the first page
                else
                    return (int)o;
            }

            set
            {
                this.ViewState["PageSize"] = value;
            }
        }
        protected int PageCount
        {
            get
            {
                // look for current page count in ViewState
                object o = this.ViewState["_PageCount"];
                if (o == null)
                    return 1;   // default to just 1 page
                else
                    return (int)o;
            }

            set
            {
                this.ViewState["_PageCount"] = value;
            }
        }


        protected void cmdPrev_Click(object sender, System.EventArgs e)
        {
            // Set viewstate variable to the previous page
            CurrentPage -= 1;

            // Reload control
            BindNewsRP();
        }

        protected void cmdNext_Click(object sender, System.EventArgs e)
        {
            // Set viewstate variable to the next page
            CurrentPage += 1;

            // Reload control
            BindNewsRP();
        }

        protected void cmdFirst_Click(object sender, System.EventArgs e)
        {
            // Set viewstate variable to the first page
            CurrentPage = 0;

            // Reload control
            BindNewsRP();
        }
        protected void cmdLast_Click(object sender, System.EventArgs e)
        {
            // Set viewstate variable to the last page
            CurrentPage = PageCount - 1;

            // Reload control
            BindNewsRP();
        }

        protected void rptPages_ItemCommand(object source,
                                 RepeaterCommandEventArgs e)
        {
            string page = Convert.ToString(e.CommandArgument);
            if (page.Contains("<"))
            {

                page = StripHTML(page);
            }
            CurrentPage = Convert.ToInt32(page) - 1;
            BindNewsRP();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            rptPages.ItemCommand +=
               new RepeaterCommandEventHandler(rptPages_ItemCommand);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BindNewsRP();
        }

        protected bool CurrentPageHighlight(int currPage)
        {
            return currPage == CurrentPage ? true : false;
        }
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        protected void GetPageSize()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.RootWeb.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists.TryGetList(Constants.List.Configuration.Name);

                        SPQuery query = new SPQuery();
                        query.Query = @"<Where>
                                      
                                            <Eq>
                                           <FieldRef Name='" + Constants.List.BaseColumns.Title + @"' />
                                           <Value Type='" + Commons.Type.Text + @"'>PageSize</Value>
                                         </Eq>
                                       
                                    </Where>";
                        SPListItemCollection col = list.GetItems(query);
                        foreach (SPListItem item in col)
                        {
                            PageSize = Convert.ToInt32(item[Constants.List.Configuration.Fields.Value]);
                        }
                        ViewState["PageSize"] = PageSize;
                    }
                }
            }

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "GetPageSize", SPContext.Current.Site);
            }

        }
    }
}
