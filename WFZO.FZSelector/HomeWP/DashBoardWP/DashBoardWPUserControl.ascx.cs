using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.HomeWP.DashBoardWP
{
    public partial class DashBoardWPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (SPContext.Current.Web.CurrentUser != null)
                {
                    lblUsername.Text = SPContext.Current.Web.CurrentUser.Name;
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
