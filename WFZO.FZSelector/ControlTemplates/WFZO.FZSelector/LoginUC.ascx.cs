using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WFZO.FZSelector.ControlTemplates.WFZO.FZSelector
{
    public partial class LoginUC : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnUserLogin_Click(object sender, EventArgs e)
        {
            Pllogout.Visible = true;
            PlLogin.Visible = false;
        }

        protected void lblogout_Click(object sender, EventArgs e)
        {
            Pllogout.Visible = false;
            PlLogin.Visible = true;
        }
    }
}
