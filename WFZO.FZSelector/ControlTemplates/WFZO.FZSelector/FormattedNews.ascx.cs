using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.Publishing.Fields;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint;

namespace WFZO.FZSelector.ControlTemplates.WFZO.FZSelector
{
    public partial class FormattedNews : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PublishingPage currentPage = PublishingPage.GetPublishingPage(SPContext.Current.ListItem);
                ltrNewsDate.Text = Convert.ToDateTime(currentPage.ListItem["ArticleStartDate"]).ToString("dd MMMM yyyy");
            }
            catch
            {
            }

        }
    }
}
