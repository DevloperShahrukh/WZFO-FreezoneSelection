using Microsoft.IdentityModel.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.IdentityModel;
using System;
using System.Data;
using System.Data.SqlClient;
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

        string wfzoVisitorGrp = "WFZO FBA Visitors";
        private string wfzoSiteUrl
        {
            get
            {
                if (ViewState["wfzoSiteUrl"] == null)
                    ViewState["wfzoSiteUrl"] = Helper.GetConfigurationValue("WfzoSiteUrl");
                return Convert.ToString(ViewState["wfzoSiteUrl"]);
            }
            set
            {
                ViewState["wfzoSiteUrl"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoginProcess();
            }


            if (SPContext.Current.Web.CurrentUser != null)
            {
                PlLogin.Visible = false;
            }
            else
            {
                PlLogin.Visible = true;
            }
        }


        protected void LoginProcess()
        {
            if (SPContext.Current.Web.CurrentUser == null)
            {
                if (Request.Cookies.Get("WZFOUserName") != null && Request.Cookies.Get("WZFOPassword") != null)
                {
                    if (Request.QueryString["wToken"] == null)
                    {
                        if (IfCookieExists())
                        {

                            Login(Encryption.Decrypt(Convert.ToString(Request.Cookies["WZFOUserName"].Value)), Encryption.Decrypt(Convert.ToString(Request.Cookies["WZFOPassword"].Value)),2);
                        }
                    }
                    else
                    {
                        dualLoginHandler();
                    }
                }
                else
                {
                    SSOMethod();
                }
            }
            else
            {
                dualLoginHandler();
            }
        }


        protected void dualLoginHandler()
        {
            if (Request.QueryString["wToken"] != null)
            {
                if (!SPContext.Current.Web.CurrentUser.LoginName.Equals(Secure.getedecryptedToken(Request.QueryString["wToken"])))
                {
                    Logout();
                    SSOMethod();
                }
            }
        }


        protected void Logout()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                FederatedAuthentication.SessionAuthenticationModule.SignOut();
                FormsAuthentication.SignOut();

                RemoveCookie();
                Response.Redirect("/Pages/default.aspx", false);
            });
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


        protected void btnUserLogin_Click(object sender, EventArgs e)
        {
            Login(txtUserID.Text, txtPassword.Text, 1);
        }

        //protected void LoginWithoutLog(string strUserName, string strPassword)
        //{


        //    SPSecurity.RunWithElevatedPrivileges(delegate()
        //        {
        //            SPIisSettings iisSettings = SPContext.Current.Site.WebApplication.IisSettings[SPUrlZone.Intranet];
        //            SPFormsAuthenticationProvider formsClaimsAuthenticationProvider = iisSettings.FormsClaimsAuthenticationProvider;

        //            //formsClaimsAuthenticationProvider.

        //            SecurityToken token = SPSecurityContext.SecurityTokenForFormsAuthentication(new Uri(SPContext.Current.Web.Url), formsClaimsAuthenticationProvider.MembershipProvider, formsClaimsAuthenticationProvider.RoleProvider, strUserName, strPassword, SPFormsAuthenticationOption.PersistentSignInRequest);
        //            if (null != token)
        //            {

        //                EstablishSessionWithToken(token);
        //            }
        //        });
        //}

        protected void Login(string strUserName, string strPassword, int From)
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


                    //using (SPSite site = new SPSite(SPContext.Current.Site.ID, systemToken))

                    using (SPSite site1 = new SPSite(wfzoSiteUrl))
                    {
                        using (SPWeb web1 = site1.OpenWeb())
                        {

                            web1.AllowUnsafeUpdates = true;
                            string CheckuserName = "i:0#.f|fbamembershipprovider|" + strUserName;

                            SPUser SU = null;
                            try
                            {
                                using (SPSite CurrentSite = new SPSite(SPContext.Current.Web.Url))
                                {
                                    using (SPWeb CurrentWeb = CurrentSite.OpenWeb())
                                    {
                                        CurrentWeb.AllowUnsafeUpdates = true;
                                        SU = CurrentWeb.SiteUsers[CheckuserName];


                                    }
                                }
                            }

                            catch (Exception ex)
                            {
                                WZFOUtility.LogException(ex, "Login getting the user", SPContext.Current.Site);
                                lblInvalidUser.Text = "Invalid User name or Password";
                                lblInvalidUser.Visible = true;
                                txtPassword.Text = "";
                                liLogin.Attributes["class"] += " open";
                                return;
                            }

                            //if (SU.IsSiteAdmin == false && WZFOUtility.IsUserMemberOfGroup(SU, wfzoVisitorGrp) == false)
                            if (SU.IsSiteAdmin == false)
                            {
                                SPList UserLst = web1.Lists["Users"];
                                SPQuery query = new SPQuery();

                                query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Bool'>true</Value></Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + strUserName + "</Value></Eq></And></Where>";

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

                                string _renew = "<a href='" + wfzoSiteUrl + "/pages/MembershipRegistration.aspx?code=" + strUserName + "&rn=1' >Renew</a>";

                                lblInvalidUser.Text = "Please renew your account to Login. \n " + _renew;
                                lblInvalidUser.Visible = true;
                                txtUserID.Text = "";
                                txtPassword.Text = "";
                                liLogin.Attributes["class"] += " open";
                                return;


                            }

                            SPIisSettings iisSettings = Microsoft.SharePoint.SPContext.Current.Site.WebApplication.GetIisSettingsWithFallback(Microsoft.SharePoint.SPContext.Current.Site.Zone);


                            SPFormsAuthenticationProvider formsClaimsAuthenticationProvider = iisSettings.FormsClaimsAuthenticationProvider;



                            SecurityToken token = SPSecurityContext.SecurityTokenForFormsAuthentication(new Uri(SPContext.Current.Web.Url), formsClaimsAuthenticationProvider.MembershipProvider, formsClaimsAuthenticationProvider.RoleProvider, strUserName, strPassword, SPFormsAuthenticationOption.PersistentSignInRequest);


                            if (null != token)
                            {

                                EstablishSessionWithToken(token);

                                InsertEncryptedPassword();

                                SetCookieForStaySignedIn();

                                PlLogin.Visible = false;
                                //Pllogout.Visible = true;

                                if (SU.IsSiteAdmin == false && WZFOUtility.IsUserMemberOfGroup(SU, wfzoVisitorGrp) == false)
                                {

                                    SPList UserLst = web1.Lists["Users"];
                                    SPQuery query = new SPQuery();

                                    query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Bool'>true</Value></Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + strUserName + "</Value></Eq></And></Where>";

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
                                        Response.Redirect(wfzoSiteUrl + "/Pages/ChangePassword.aspx");
                                    else
                                        Response.Redirect("/Pages/Dashboard.aspx", false);
                                }
                                if(From ==1)
                                {
                                    Response.Redirect(HttpContext.Current.Request.Url.ToString(), false);
                                }
                                /*else
                                    Response.Redirect("/Pages/Dashboard.aspx", false);*/
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
                    //Pllogout.Visible = false;
                    PlLogin.Visible = true;

                    Response.Redirect("/Pages/default.aspx", false);
                });
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "LoginOut", SPContext.Current.Site);
            }
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
            if (Request.Cookies.Get("WZFOUserName") != null)
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

                if (Request.Cookies.Get("WZFOUserName") == null)
                {
                    Response.Cookies.Add(new HttpCookie("WZFOUserName", Encryption.Encrypt(txtUserID.Text)));
                    Response.Cookies.Add(new HttpCookie("WZFOPassword", Encryption.Encrypt(txtPassword.Text)));

                    Response.Cookies["WZFOUserName"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["WZFOPassword"].Expires = DateTime.Now.AddDays(30);
                }
                else
                {
                    Response.Cookies["WZFOUserName"].Value = Encryption.Encrypt(txtUserID.Text);
                    Response.Cookies["WZFOPassword"].Value = Encryption.Encrypt(txtPassword.Text);
                }

            }
            else
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
            return ((!string.IsNullOrEmpty(Convert.ToString(Request.Cookies["WZFOUserName"].Value)) && !string.IsNullOrEmpty(Convert.ToString(Request.Cookies["WZFOPassword"].Value))));
        }

        private void SSOMethod()
        {
            try
            {
                string userId = "";
                if (Request.QueryString["wToken"] != null)
                {
                    string wToken = Request.QueryString["wToken"].ToString();

                    userId = Secure.getedecryptedToken(wToken);
                }
                else
                {
                    return;
                }
                string conString = System.Configuration.ConfigurationManager.ConnectionStrings["FBADB2"].ConnectionString;
                SqlConnection con = new SqlConnection(conString);//"Data Source=VM-2;Initial Catalog=aspnetdb; Trusted_Connection=Yes;");//Persist Security Info=True;User ID=KARACHI\\SPFarmUser;Password=abcd@1234");
                SqlCommand cmd1 = new SqlCommand("select Email,EncyPass from aspnet_Membership where Email=@EmailId", con);
                cmd1.Parameters.AddWithValue("@EmailId", userId);
                con.Open();
                string pwd = string.Empty;
                using (SqlDataReader reader = cmd1.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string id = reader["Email"].ToString();
                        pwd = Secure.getedecryptedToken(reader["EncyPass"].ToString());
                    }
                }

                if (pwd == string.Empty)
                {

                    return;
                }
                con.Close();
                Login(userId, pwd,2);

            }
            catch (Exception ex)
            {

                WZFOUtility.LogException(ex, "LoginUC - SSO Method ", SPContext.Current.Site);
            }
        }

        private void InsertEncryptedPassword()
        {
            try
            {
                string conString = System.Configuration.ConfigurationManager.ConnectionStrings["FBADB"].ConnectionString;
                string pwd = Secure.getencryptedToken(txtPassword.Text);

                SqlConnection con = new SqlConnection(conString);
                SqlCommand cmd1 = new SqlCommand("update aspnet_Membership set EncyPass=@pwd,Machine=@MachineName,IsLoggedIn='true' where Email=@EmailId", con);
                cmd1.Parameters.AddWithValue("@EmailId", txtUserID.Text);
                cmd1.Parameters.AddWithValue("@pwd", pwd);
                cmd1.Parameters.AddWithValue("@MachineName", "");
                con.Open();
                cmd1.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                WZFOUtility.LogException(ex, "LoginUC - InsertEncryptedPassword", SPContext.Current.Site);
            }
        }



    }
}
