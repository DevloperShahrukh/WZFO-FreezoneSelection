using Microsoft.SharePoint;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WFZO.FZSelector.ControlTemplates.WFZO.FZSelector
{
    public partial class FooterUC : UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindFooterRP();
            }
        }
        protected void BindFooterRP()
        {
            //string url = "";
            //if (SPContext.Current.Web.IsRootWeb)
            //{
            //    url = SPContext.Current.Web.Url;
            //}
            //else
            //{
            //   url = SPContext.Current.Site.RootWeb.Url;

            //}
            using (SPSite site = new SPSite(SPContext.Current.Site.RootWeb.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists.TryGetList(Constants.List.Footer.Name);


                    SPQuery query = new SPQuery();
                    query.Query = @"<Where>
                                         <Eq>
                                           <FieldRef Name='" + Constants.List.BaseColumns.IsActive + @"' />
                                           <Value Type='" + Commons.Type.Boolean + @"'>" + 1 + @"</Value>
                                         </Eq>
                                    </Where>
                                    <OrderBy>
                                       <FieldRef Name='" + Constants.List.Footer.Fields.Sequence + @"' Ascending='" + Commons.Type.True + @"' />
                                    </OrderBy>";
                    DataTable dt = list.GetItems(query).GetDataTable();
                    FooterRP.DataSource = dt;
                    FooterRP.DataBind();

                }

            }
        }
        protected void FooterRP_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lit = (Literal)e.Item.FindControl("ltrUrl");
                Label lblInWindow = (Label)e.Item.FindControl("lblInWindow");
                Label Title = (Label)e.Item.FindControl("lblTitle");
                Label LinkURL = (Label)e.Item.FindControl("lblUrl");

                string[] getLink = LinkURL.Text.Split(',');

                if (lblInWindow.Text == "1")
                {
                    lit.Text = "<a href='" + getLink[0] + "' target='_blank'>" + Title.Text + "</a>";
                }
                else
                {
                    lit.Text = "<a href='" + getLink[0] + "'>" + Title.Text + "</a>";
                }
            }
        }

    }
}

