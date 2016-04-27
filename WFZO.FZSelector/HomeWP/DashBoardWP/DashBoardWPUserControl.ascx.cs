using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WFZO.FZSelector.HomeWP.DashBoardWP
{
    public partial class DashBoardWPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SPContext.Current.Web.CurrentUser != null)
            {
                lblUsername.Text = SPContext.Current.Web.CurrentUser.Name;
            }
        }
    }
}
