using Microsoft.IdentityModel.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.OtherWP.SSOSignOutWP
{
    public partial class SSOSignOutWPUserControl : UserControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (SPWebPartManager.GetCurrentWebPartManager(Page).DisplayMode != WebPartManager.BrowseDisplayMode)
            {
                foreach (object validator in Page.Validators)
                {
                    if (validator is BaseValidator)
                    {
                        ((BaseValidator)validator).Enabled = false;
                    }
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            SSOSignOut();
        }

        protected void SSOSignOut()
        {
            try
            {
                string Source = Convert.ToString(Request.QueryString["source"]);
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    if (SPContext.Current.Web.CurrentUser != null)
                    {
                        FederatedAuthentication.SessionAuthenticationModule.SignOut();
                        FormsAuthentication.SignOut();
                        RemoveCookie();


                        Response.Redirect(Source, false);
                    }
                    else
                    {
                        Response.Redirect(Source, false);
                    }
                });

            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "SSO LogOut FZ Monitor Extension", SPContext.Current.Site);

            }
        }


        protected void RemoveCookie()
        {
            try
            {

                if (Request.Cookies.Get("WZFOUserName") != null)
                {
                    Response.Cookies["WZFOUserName"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["WZFOUserName"].Value = null;

                }
                if (Request.Cookies.Get("WZFOPassword") != null)
                {
                    Response.Cookies["WZFOPassword"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["WZFOPassword"].Value = null;
                }
            }
            catch (Exception ex)
            {
                
                WZFOUtility.LogException(ex, "TopBarUC -  RemoveCookies", SPContext.Current.Site);
            }
        }

    }
}
