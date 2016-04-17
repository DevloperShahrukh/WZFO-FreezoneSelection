using Microsoft.IdentityModel.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.IdentityModel;
using System;
using System.Data;
using System.IdentityModel.Tokens;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

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

        private void SetUserLabel()
        {
            //if (SPContext.Current.Web.CurrentUser != null)
            //{
            //    LblSignedInUser.Text = SPContext.Current.Web.CurrentUser.LoginName;
            //    if (LblSignedInUser.Text.Contains("|"))
            //    {
            //        LblSignedInUser.Text = LblSignedInUser.Text.Substring(LblSignedInUser.Text.LastIndexOf('|') + 1);
            //        //
            //        #region expiry period
            //        SPList UserLst = SPContext.Current.Site.RootWeb.Lists["Users"];
            //        SPQuery query = new SPQuery();
            //        query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Bool'>true</Value></Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + LblSignedInUser.Text + "</Value></Eq></And></Where>";
            //        SPListItemCollection UserColl = UserLst.GetItems(query);

            //        DataTable dtuserdata = null;

            //        if (UserColl.Count > 0)
            //        {
            //            dtuserdata = UserColl.GetDataTable();
            //            if (dtuserdata.Rows[0]["Expiry_x0020_Date"] != DBNull.Value)
            //            {


            //                string _membershipperiod = "Membership validity till " + Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"])).ToString("dd-MMM-yyyy");
            //                //
            //                string _renew = "<a href='/pages/MembershipRegistration.aspx?code=" + LblSignedInUser.Text + "&rn=1' >Renew</a>";

            //                int _idays = int.Parse(SPContext.Current.Site.RootWeb.Lists["Membership Lenght"].GetItemById(1)["AlertDays"].ToString());
            //                int GraceDays = int.Parse(SPContext.Current.Site.RootWeb.Lists["Membership Lenght"].GetItemById(1)["GraceDays"].ToString());

            //                DateTime dt = Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"])).AddDays(-_idays);
            //                DateTime dtexp = Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"]));

            //                DateTime dtexpGrace = Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"])).AddDays(GraceDays);

            //                if (((DateTime.Now.Date >= dt.Date) && (DateTime.Now.Date <= dtexp.Date)) || ((DateTime.Now.Date >= dtexp.Date) && (DateTime.Now.Date <= dtexpGrace)))
            //                {
            //                    if (dtuserdata.Rows[0]["RequestType"].ToString() != "Renewal")
            //                    {
            //                        _membershipperiod = _membershipperiod + "\n" + _renew;
            //                    }
            //                }
            //                //
            //                ltmembershipperiod.Text = _membershipperiod;
            //                ltmembershipperiod.Visible = true;
            //            }
            //        }
            //        #endregion
            //    }
            //    DivMemberLogin.Visible = false;
            //    DivWelcomUser.Visible = true;
            //}
            //else
            //{
            //    DivMemberLogin.Visible = true;
            //    DivWelcomUser.Visible = false;
            //}

        }
        private void EstablishSessionWithToken(SecurityToken securityToken)
        {
            if (securityToken == null)
            {
                // throw new ArgumentNullException("securityToken");
            }
            SPFederationAuthenticationModule fam = FederatedAuthentication.WSFederationAuthenticationModule as SPFederationAuthenticationModule;
            if (fam == null)
            {
                //ULS.SendTraceTag(0x15d59d, ULSCat.msoulscat_WSS_ClaimsAuthentication, ULSTraceLevel.Verbose, this.LogPrefix + "Federated authentication module is missing for request '{0}'.", new object[] { SPAlternateUrl.ContextUri });
                throw new InvalidOperationException();
            }
            SPSecurity.RunWithElevatedPrivileges(() => fam.SetPrincipalAndWriteSessionToken(securityToken));

        }

        private void SetTextBoxFromCookies()
        {
            if (Request.Cookies["WZFOUserName"] != null)
            {

                TxtUserID.Text = Request.Cookies["WZFOUserName"].Value;
                //TxtPassword.Text = Request.Cookies["WZFOPassword"].Value;
                TxtPassword.Attributes.Add("value", Request.Cookies["WZFOPassword"].Value);
                ChkStaySignedIn.Checked = true;
            }
        }
        private void SetCookieForStaySignedIn()
        {
            if (ChkStaySignedIn.Checked)
            {
                //HttpCookie c =
                if (Request.Cookies["WZFOUserName"] == null)
                {
                    Response.Cookies.Add(new HttpCookie("WZFOUserName", TxtUserID.Text));
                    Response.Cookies.Add(new HttpCookie("WZFOPassword", TxtPassword.Text));

                    Response.Cookies["WZFOUserName"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["WZFOPassword"].Expires = DateTime.Now.AddDays(30);
                }
                else
                {
                    Response.Cookies["WZFOUserName"].Value = TxtUserID.Text;
                    Response.Cookies["WZFOPassword"].Value = TxtPassword.Text;
                }

            }
            else
            {
                if (Request.Cookies["WZFOUserName"] != null)
                {
                    Response.Cookies["WZFOUserName"].Expires = DateTime.Now.AddDays(-1);


                }
                if (Request.Cookies["WZFOPassword"] != null)
                {
                    Response.Cookies["WZFOPassword"].Expires = DateTime.Now.AddDays(-1);

                }
            }
        }
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                int UserLoginNumber = 0;
                SPListItem Item;
                DateTime ActiveDate = DateTime.MaxValue;
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   string reqType = "";
                   using (SPSite site1 = new SPSite(SPContext.Current.Site.ID))
                   {
                       using (SPWeb web1 = site1.OpenWeb())
                       {
                           string CheckuserName = "i:0#.f|fbamembershipprovider|" + TxtUserID.Text;

                           SPUser SU = web1.SiteUsers[CheckuserName];


                           if (SU.IsSiteAdmin == false && WZFOUtility.IsUserMemberOfGroup(SU, "WFZO Visitors") == false)
                           {
                               SPList UserLst = web1.Lists["Users"];
                               SPQuery query = new SPQuery();
                               // query.RowLimit = 1;
                               //   query.Query = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + strUserName + "</Value></Eq></Where>";

                               query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Bool'>true</Value></Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + TxtUserID.Text + "</Value></Eq></And></Where>";

                               SPListItemCollection CurUser = UserLst.GetItems(query);

                               if (CurUser.Count == 0)
                               {

                                   LblInvalidUser.Text = "Invalid User name or Password";

                                   LblInvalidUser.Visible = true;
                                   TxtUserID.Text = "";
                                   TxtPassword.Text = "";
                                   return;
                               }
                               int GraceDays = int.Parse(SPContext.Current.Site.RootWeb.Lists["Membership Lenght"].GetItemById(1)["GraceDays"].ToString());

                               ActiveDate = Convert.ToDateTime(Convert.ToString(CurUser[0]["Expiry_x0020_Date"])).AddDays(GraceDays);
                               SPFieldLookupValue fieldLookupValue = new SPFieldLookupValue(CurUser[0]["RequestType"].ToString());
                               reqType = fieldLookupValue.LookupValue;

                           }
                           //RoleProvider rp = System.Web.Security.Roles.Providers[roleProviderName];

                           // MembershipProvider p =  Membership.Providers[userProviderName];


                           string strUserName = TxtUserID.Text;
                           string strPassword = TxtPassword.Text;


                           if (DateTime.Now.Date >= ActiveDate.Date)
                           {
                               if (reqType == "Renewal")
                               {
                                   LblInvalidUser.Text = "Your application renewal is in process. Please wait for approval.";
                                   LblInvalidUser.Visible = true;
                                   TxtUserID.Text = "";
                                   TxtPassword.Text = "";
                                   return;
                               }

                               string _renew = "<a href='/pages/MembershipRegistration.aspx?code=" + TxtUserID.Text + "&rn=1' >Renew</a>";

                               LblInvalidUser.Text = "Please renew your account to Login. \n " + _renew;
                               LblInvalidUser.Visible = true;
                               TxtUserID.Text = "";
                               TxtPassword.Text = "";
                               return;


                           }
                           // FBA authentication it is. 
                           SPIisSettings iisSettings = SPContext.Current.Site.WebApplication.IisSettings[SPUrlZone.Default];
                           SPFormsAuthenticationProvider formsClaimsAuthenticationProvider = iisSettings.FormsClaimsAuthenticationProvider;

                           //formsClaimsAuthenticationProvider.

                           SecurityToken token = SPSecurityContext.SecurityTokenForFormsAuthentication(new Uri(SPContext.Current.Web.Url), formsClaimsAuthenticationProvider.MembershipProvider, formsClaimsAuthenticationProvider.RoleProvider, strUserName, strPassword, SPFormsAuthenticationOption.PersistentSignInRequest);
                           if (null != token)
                           {

                               EstablishSessionWithToken(token);
                               //base.RedirectToSuccessUrl();


                               SetCookieForStaySignedIn();

                               //LblSignedInUser.Text = TxtUserID.Text;
                               //if (LblSignedInUser.Text.Contains("|"))
                               //{
                               //    LblSignedInUser.Text = LblSignedInUser.Text.Substring(LblSignedInUser.Text.LastIndexOf('|')+1);
                               //}
                               //DivMemberLogin.Visible = false;
                               //DivWelcomUser.Visible = true;

                               PlLogin.Visible = false;
                               Pllogout.Visible = true;

                               //  var rUrl = Request.QueryString.Get("Source");

                               // Response.Redirect(string.IsNullOrEmpty(rUrl) ? "~/SitePages/default.aspx" : rUrl);
                               if (SU.IsSiteAdmin == false && WZFOUtility.IsUserMemberOfGroup(SU, "WFZO Visitors") == false)
                               {

                                   SPList UserLst = web1.Lists["Users"];
                                   SPQuery query = new SPQuery();
                                   // query.RowLimit = 1;
                                   //   query.Query = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + strUserName + "</Value></Eq></Where>";

                                   query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Bool'>true</Value></Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + TxtUserID.Text + "</Value></Eq></And></Where>";

                                   SPListItemCollection CurUser = UserLst.GetItems(query);
                                   if (CurUser != null)
                                   {
                                       Item = CurUser[0];
                                       if (!string.IsNullOrEmpty(Convert.ToString(Item["UserLoginCount"])))
                                       {
                                           UserLoginNumber = Convert.ToInt32(Item["UserLoginCount"].ToString());
                                       }
                                       web1.AllowUnsafeUpdates = true;
                                       Item["UserLoginCount"] = UserLoginNumber + 1;
                                       Item.Update();

                                       // Add into UserLoginLog list for login
                                       SPList list = web1.Lists["UserLoginLog"];

                                       //Add a new item in the List
                                       SPListItem itemToAdd = list.Items.Add();

                                       itemToAdd["User"] = new SPFieldLookupValue(Item.ID, Item.Title);

                                       itemToAdd["LoginDate"] = DateTime.Now;
                                       itemToAdd["LoginTime"] = DateTime.Now;

                                       itemToAdd.Update();

                                       web1.AllowUnsafeUpdates = false;

                                   }

                                   if (UserLoginNumber == 0)
                                       Response.Redirect("/Pages/ChangePassword.aspx");
                                   else
                                       Response.Redirect("/Pages/MemberArea.aspx");

                               }
                               else
                                   Response.Redirect("/Pages/MemberArea.aspx");


                           }
                           else
                           {
                               LblInvalidUser.Text = "Invalid User name or Password";
                               LblInvalidUser.Visible = true;
                               TxtUserID.Text = "";
                               TxtPassword.Text = "";

                               // check if the user is blocked, then send 
                           }

                           MembershipProvider p = Membership.Providers[formsClaimsAuthenticationProvider.MembershipProvider];
                           if (p != null)
                           {
                               MembershipUser U = p.GetUser(strUserName, false);
                               if (U != null)
                               {
                                   if (U.IsLockedOut)
                                   {
                                       InformAdminForBlock(strUserName);
                                       LblInvalidUser.Text = "Account locked, Contact Administrator";
                                       btnInformAdmin.CommandArgument = strUserName;
                                       btnInformAdmin.Visible = true;
                                   }
                               }

                           }




                           // SecurityToken tk = SPSecurityContext.SecurityTokenForFormsAuthentication(

                           //new Uri(SPContext.Current.Web.Url), p.Name, rp.Name,

                           //  TxtUserID.Text, TxtPassword.Text,SPFormsAuthenticationOption.PersistentSignInRequest);



                           //                    if (tk != null)

                           //                    {

                           ////try setting the authentication cookie

                           //                      SPFederationAuthenticationModule fam =   SPFederationAuthenticationModule.Current;

                           //                    fam.SetPrincipalAndWriteSessionToken(tk);



                           //                           //look for the Source query string parameter and use that as the redirection

                           //                           string src = Request.QueryString["Source"];

                           //                           if (!string.IsNullOrEmpty(src))

                           //                           Response.Redirect(src);

                           //                    }

                           //                    else

                           //                    {

                           //                    //StatusLbl.Text = "The credentials weren’t valid or didn’t work or something.";

                           //                    }

                       }
                   }
               });

            }

            catch (Exception ex)
            {

                WZFOUtility.LogException(ex, "Login", SPContext.Current.Site);
                LblInvalidUser.Visible = true;
                TxtUserID.Text = "";
                TxtPassword.Text = "";

            }



        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {


        }

        protected void LnkLogout_Click(object sender, EventArgs e)
        {
            try
            {

                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   FederatedAuthentication.SessionAuthenticationModule.SignOut();
                   FormsAuthentication.SignOut();
                   Response.Redirect("/Pages/Home.aspx");
               });
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "LoginOut", SPContext.Current.Site);

            }
        }

        protected void btnInformAdmin_Click(object sender, EventArgs e)
        {
            InformAdminForBlock(btnInformAdmin.CommandArgument);
        }

        private void InformAdminForBlock(string BlockedEmail)
        {
            string emailBody, emailSub, AdminEmail;
            emailBody = "";
            emailSub = "";
            AdminEmail = "";
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                   {
                       SPSite parentSite = new SPSite(SPContext.Current.Site.ID);

                       SPUserToken systemToken = parentSite.SystemAccount.UserToken;

                       using (SPSite site = new SPSite(SPContext.Current.Site.ID, systemToken))
                       {
                           using (SPWeb web = site.OpenWeb())
                           {
                               SPList eventsList = SPContext.Current.Web.Lists["ConfigList"];
                               SPQuery query = new SPQuery();
                               query.ViewFields = "<FieldRef Name='Key' /><FieldRef Name='Value' />";
                               query.ViewFieldsOnly = true;
                               query.Query = "<Where><Contains><FieldRef Name='Key' /><Value Type='Text'>AdminEmail</Value></Contains></Where>";
                               SPListItemCollection ConfigListItems = eventsList.GetItems(query);
                               if (ConfigListItems != null && ConfigListItems.GetDataTable().Rows.Count > 0)
                               {
                                   AdminEmail = Convert.ToString(ConfigListItems[0]["Value"]);
                               }
                               SPList EmailLst = web.Lists["Email Templates"];
                               query = new SPQuery();
                               query.RowLimit = 1;
                               query.Query = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>UserBlockedEmail</Value></Eq></Where>";

                               SPListItemCollection EmailItem = EmailLst.GetItems(query);
                               if (EmailItem.Count > 0)
                               {
                                   emailBody = EmailItem[0]["Email_x0020_Body"].ToString().Replace("&lt;Email&gt;", BlockedEmail);
                                   emailSub = EmailItem[0]["Email_x0020_Subject"].ToString();
                                   WZFOUtility.SendEmail(AdminEmail, web, emailBody, emailSub, false);
                                   WZFOUtility.LogEmail(AdminEmail, emailSub, emailBody, "User Blocked from Login", site);
                               }
                           }
                       }
                   });

            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "Inform Admin on Blocking Click", SPContext.Current.Site);
            }
        }




    }
}
