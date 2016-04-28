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
        string wfzoSiteUrl = "http://sps2013:200";
        string wfzoVisitorGrp = "WFZO FBA Visitors";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IfCookieExists())
                {
                    Login(Convert.ToString(Request.Cookies["WFZOSingleSignOn"]["Username"]), Convert.ToString(Request.Cookies["WFZOSingleSignOn"]["Password"]));


                    //Request.Cookies["WFZOSingleSignOn"]["Username"] = txtUserID.Text;
                    //Request.Cookies["WFZOSingleSignOn"]["Password"] = txtPassword.Text;

                }
            }

            if (SPContext.Current.Web.CurrentUser != null)
            {
                PlLogin.Visible = false;
                //Pllogout.Visible = true;
            }
            else
            {
                PlLogin.Visible = true;
                //Pllogout.Visible = false;
            }
        }

        protected void btnUserLogin_Click(object sender, EventArgs e)
        {
            Login(txtUserID.Text, txtPassword.Text);
        }



        protected void Login(string strUserName,string strPassword  )
        {
            try
            {
                int UserLoginNumber = 0;
                SPListItem Item;
                DateTime ActiveDate = DateTime.MaxValue;

                
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    string reqType = "";
                    SPSite wfzoSite = new SPSite(SPContext.Current.Site.ID);
                    SPUserToken systemToken = wfzoSite.SystemAccount.UserToken;
                    wfzoSite.Dispose();

                    //using (SPSite site = new SPSite(SPContext.Current.Site.ID, systemToken))
                    using (SPSite site1 = new SPSite(wfzoSiteUrl))
                    {
                        using (SPWeb web1 = site1.OpenWeb())
                        {
                            string CheckuserName = "i:0#.f|fbamembershipprovider|" + txtUserID.Text;

                            SPUser SU = null;
                            try
                            {
                                SU = web1.SiteUsers[CheckuserName];
                            }
                            catch (Exception ex)
                            {
                                lblInvalidUser.Text = "Invalid User name or Password";
                                lblInvalidUser.Visible = true;
                                txtPassword.Text = "";
                                liLogin.Attributes["class"] += " open";
                                return;
                            }

                            if (SU.IsSiteAdmin == false && WZFOUtility.IsUserMemberOfGroup(SU, wfzoVisitorGrp) == false)
                            {
                                SPList UserLst = web1.Lists["Users"];
                                SPQuery query = new SPQuery();

                                query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Bool'>true</Value></Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + txtUserID.Text + "</Value></Eq></And></Where>";

                                SPListItemCollection CurUser = UserLst.GetItems(query);

                                if (CurUser.Count == 0)
                                {

                                    lblInvalidUser.Text = "Invalid User name or Password";

                                    lblInvalidUser.Visible = true;
                                    txtUserID.Text = "";
                                    txtPassword.Text = "";
                                    liLogin.Attributes["class"] += " open";
                                    return;
                                }
                                int GraceDays = int.Parse(web1.Lists["Membership Lenght"].GetItemById(1)["GraceDays"].ToString());

                                ActiveDate = Convert.ToDateTime(Convert.ToString(CurUser[0]["Expiry_x0020_Date"])).AddDays(GraceDays);
                                SPFieldLookupValue fieldLookupValue = new SPFieldLookupValue(CurUser[0]["RequestType"].ToString());
                                reqType = fieldLookupValue.LookupValue;

                            }

                            if (DateTime.Now.Date >= ActiveDate.Date)
                            {
                                if (reqType == "Renewal")
                                {
                                    lblInvalidUser.Text = "Your application renewal is in process. Please wait for approval.";
                                    lblInvalidUser.Visible = true;
                                    txtUserID.Text = "";
                                    txtPassword.Text = "";
                                    liLogin.Attributes["class"] += " open";
                                    return;
                                }

                                string _renew = "<a href='http://sps2013:200/pages/MembershipRegistration.aspx?code=" + txtUserID.Text + "&rn=1' >Renew</a>";

                                lblInvalidUser.Text = "Please renew your account to Login. \n " + _renew;
                                lblInvalidUser.Visible = true;
                                txtUserID.Text = "";
                                txtPassword.Text = "";
                                liLogin.Attributes["class"] += " open";
                                return;


                            }
                            // FBA authentication it is. 
                            SPIisSettings iisSettings = SPContext.Current.Site.WebApplication.IisSettings[SPUrlZone.Intranet];
                            SPFormsAuthenticationProvider formsClaimsAuthenticationProvider = iisSettings.FormsClaimsAuthenticationProvider;

                            //formsClaimsAuthenticationProvider.

                            SecurityToken token = SPSecurityContext.SecurityTokenForFormsAuthentication(new Uri(SPContext.Current.Web.Url), formsClaimsAuthenticationProvider.MembershipProvider, formsClaimsAuthenticationProvider.RoleProvider, strUserName, strPassword, SPFormsAuthenticationOption.PersistentSignInRequest);
                            if (null != token)
                            {

                                EstablishSessionWithToken(token);
                                //base.RedirectToSuccessUrl();


                                SetCookieForStaySignedIn();

                                //LblSignedInUser.Text = TxtUserID.Text;
                                // DivMemberLogin.Visible = false;
                                //DivWelcomUser.Visible = true;

                                PlLogin.Visible = false;
                                Pllogout.Visible = true;

                                if (SU.IsSiteAdmin == false && WZFOUtility.IsUserMemberOfGroup(SU, wfzoVisitorGrp) == false)
                                {

                                    SPList UserLst = web1.Lists["Users"];
                                    SPQuery query = new SPQuery();

                                    query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Bool'>true</Value></Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + txtUserID.Text + "</Value></Eq></And></Where>";

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
                                        Response.Redirect("http://sps2013:200/Pages/ChangePassword.aspx");
                                    else
                                        Response.Redirect("/Pages/Dashboard.aspx", false);
                                }
                                else
                                    Response.Redirect("/Pages/Dashboard.aspx", false);
                            }
                            else
                            {
                                lblInvalidUser.Text = "Invalid User name or Password";
                                lblInvalidUser.Visible = true;
                                txtUserID.Text = "";
                                txtPassword.Text = "";
                                liLogin.Attributes["class"] += " open";
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
                                        lblInvalidUser.Text = "Account locked, Contact Administrator";
                                        btnInformAdmin.CommandArgument = strUserName;
                                        btnInformAdmin.Visible = true;
                                        btnUserLogin.Visible = false;
                                        liLogin.Attributes["class"] += " open";
                                    }
                                }
                            }
                        }
                    }
                });

            }

            catch (Exception ex)
            {

                WZFOUtility.LogException(ex, "Login", SPContext.Current.Site);
                lblInvalidUser.Visible = true;
                txtUserID.Text = "";
                txtPassword.Text = "";

            }
        }

        protected void lblogout_Click(object sender, EventArgs e)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    FederatedAuthentication.SessionAuthenticationModule.SignOut();
                    FormsAuthentication.SignOut();
                    Pllogout.Visible = false;
                    PlLogin.Visible = true;
                    
                    Response.Redirect("/Pages/default.aspx", false);
                });
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "LoginOut", SPContext.Current.Site);
            }
        }

        //private void SetUserLabel()
        //{
        //    if (SPContext.Current.Web.CurrentUser != null)
        //    {
        //        string userId = SPContext.Current.Web.CurrentUser.LoginName;

        //        if (userId.Contains("|"))
        //        {
        //            userId = userId.Substring(userId.LastIndexOf('|') + 1);
        //            txtUserID.Text = userId;

        //            #region expiry period
        //            SPSite wSite = new SPSite(wfzoSiteUrl);
        //            SPWeb sWeb = wSite.RootWeb;
        //            SPList UserLst = sWeb.Lists["Users"];
        //            SPQuery query = new SPQuery();
        //            query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Bool'>true</Value></Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + userId + "</Value></Eq></And></Where>";
        //            SPListItemCollection UserColl = UserLst.GetItems(query);

        //            DataTable dtuserdata = null;

        //            if (UserColl.Count > 0)
        //            {
        //                dtuserdata = UserColl.GetDataTable();
        //                if (dtuserdata.Rows[0]["Expiry_x0020_Date"] != DBNull.Value)
        //                {


        //                    string _membershipperiod = "Membership validity till " + Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"])).ToString("dd-MMM-yyyy");
        //                    //
        //                    string _renew = "<a href='/pages/MembershipRegistration.aspx?code=" + userId + "&rn=1' >Renew</a>";

        //                    int _idays = int.Parse(sWeb.Lists["Membership Lenght"].GetItemById(1)["AlertDays"].ToString());
        //                    int GraceDays = int.Parse(sWeb.Lists["Membership Lenght"].GetItemById(1)["GraceDays"].ToString());

        //                    DateTime dt = Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"])).AddDays(-_idays);
        //                    DateTime dtexp = Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"]));

        //                    DateTime dtexpGrace = Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"])).AddDays(GraceDays);

        //                    if (((DateTime.Now.Date >= dt.Date) && (DateTime.Now.Date <= dtexp.Date)) || ((DateTime.Now.Date >= dtexp.Date) && (DateTime.Now.Date <= dtexpGrace)))
        //                    {
        //                        if (dtuserdata.Rows[0]["RequestType"].ToString() != "Renewal")
        //                        {
        //                            _membershipperiod = _membershipperiod + "\n" + _renew;
        //                        }
        //                    }
        //                    //
        //                    //ltmembershipperiod.Text = _membershipperiod;
        //                    //ltmembershipperiod.Visible = true;
        //                }
        //            }
        //            #endregion
        //        }
        //        PlLogin.Visible = false;
        //        Pllogout.Visible = true;
        //    }
        //    else
        //    {
        //        PlLogin.Visible = true;
        //        Pllogout.Visible = false;
        //    }

        //}
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

                txtUserID.Text = Request.Cookies["WZFOUserName"].Value;
                //TxtPassword.Text = Request.Cookies["WZFOPassword"].Value;
                txtPassword.Attributes.Add("value", Request.Cookies["WZFOPassword"].Value);
                chkStaySignedIn.Checked = true;
            }
        }
        private void SetCookieForStaySignedIn()
        {
            if (chkStaySignedIn.Checked)
            {
                //HttpCookie c =
                if (Request.Cookies["WZFOUserName"] == null)
                {
                    //Response.Cookies.Add(new HttpCookie("WZFOUserName", txtUserID.Text));
                    //Response.Cookies.Add(new HttpCookie("WZFOPassword", txtPassword.Text));

                    //Response.Cookies["WZFOUserName"].Expires = DateTime.Now.AddDays(30);
                    //Response.Cookies["WZFOPassword"].Expires = DateTime.Now.AddDays(30);

                    HttpCookie aCookie = new HttpCookie("WFZOSingleSignOn");
                    aCookie.Values["Username"] = txtUserID.Text;
                    aCookie.Values["Password"] = txtPassword.Text;
                    aCookie.Expires = DateTime.Now.AddDays(30);


                    
                }
                else
                {
                    //Response.Cookies["WZFOUserName"].Value = txtUserID.Text;
                    //Response.Cookies["WZFOPassword"].Value = txtPassword.Text;

                    HttpCookie aCookie = new HttpCookie("WFZOSingleSignOn");
                    Request.Cookies["WFZOSingleSignOn"]["Username"] = txtUserID.Text;
                    Request.Cookies["WFZOSingleSignOn"]["Password"] = txtPassword.Text;

                    //Response.Cookies["WZFOUserName"].Domain = "";
                    //Response.Cookies["WZFOPassword"].Domain = "";
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
                       //SPSite parentSite = new SPSite(SPContext.Current.Site.ID);
                       //SPUserToken systemToken = parentSite.SystemAccount.UserToken;

                       //using (SPSite site = new SPSite(SPContext.Current.Site.ID, systemToken))
                       using (SPSite parentSite = new SPSite(wfzoSiteUrl))
                       {
                           using (SPWeb web = parentSite.OpenWeb())
                           {
                               SPList eventsList = web.Lists["ConfigList"];
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
                                   WZFOUtility.LogEmail(AdminEmail, emailSub, emailBody, "User Blocked from Login", parentSite);
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


        protected bool IfCookieExists()
        {

            //return (!string.IsNullOrEmpty(Response.Cookies["WZFOUserName"].Value) && !string.IsNullOrEmpty(Response.Cookies["WZFOPassword"].Value));

            //return (!string.IsNullOrEmpty(Request.Cookies["WFZOSingleSignOn"]["Username"]) && !string.IsNullOrEmpty(Request.Cookies["WFZOSingleSignOn"]["Password"]));


            return (Request.Cookies["WFZOSingleSignOn"] != null);
        }

        
    }
}
