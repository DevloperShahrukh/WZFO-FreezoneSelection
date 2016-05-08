using Microsoft.IdentityModel.Web;
using Microsoft.SharePoint;
using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.ControlTemplates.WFZO.FZSelector
{
    public partial class TopBar : UserControl
    {
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

        string wfzoVisitorGrp = "WFZO FBA Visitors";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (this.Page.Request.Url.AbsolutePath.ToLower() == "/pages/default.aspx")
                    hypHome.Enabled = false;

                this.Page.LoadComplete += new EventHandler(Page_LoadComplete);
            }



        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)

            SetUserLabel();
            hypDashboard.Enabled = hypHome.Enabled = true;
            if (Page.Title == "Dashoard")
                hypDashboard.Enabled = false;
            else if (Page.Title == "Home")
                hypHome.Enabled = false;

        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            try
            {

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    FederatedAuthentication.SessionAuthenticationModule.SignOut();
                    FormsAuthentication.SignOut();

                    RemoveCookie();
                    Response.Redirect("/Pages/default.aspx", false);
                });
            }

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "lnkLogout_Click", SPContext.Current.Site);
            }
        }

        private void SetUserLabel()
        {
            try
            {

                if (SPContext.Current.Web.CurrentUser != null)
                {
                    string userId = SPContext.Current.Web.CurrentUser.Name;

                    if (this.Page.Request.Url.AbsolutePath.ToLower() != "/pages/dashboard.aspx")
                        hypDashboard.Enabled = true;

                    if (userId.Contains("|"))
                    {
                        userId = userId.Substring(userId.LastIndexOf('|') + 1);
                        SetUserLabelProcess(userId);
                    }
                    else
                    {
                        SetUserLabelProcess(userId);
                    }
                }
                else
                {
                    lnkLogout.Visible = false;
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "BindFooterRP", SPContext.Current.Site);
            }
        }


        protected void SetUserLabelProcess(string userId)
        {
            try
            {
                ltrWelcome.Text = "Welcome " + userId;

                #region expiry period
                if (string.IsNullOrEmpty(Convert.ToString(ViewState["MembershipPeriod"])))
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite wSite = new SPSite(wfzoSiteUrl))
                    {
                        using (SPWeb sWeb = wSite.RootWeb)
                        {
                            SPList UserLst = sWeb.Lists["Users"];
                            SPQuery query = new SPQuery();
                            query.Query = "<Where><And><Eq><FieldRef Name='Active' /><Value Type='Bool'>true</Value></Eq><Eq><FieldRef Name='Title' /><Value Type='Text'>" + userId + "</Value></Eq></And></Where>";
                            SPListItemCollection UserColl = UserLst.GetItems(query);

                            DataTable dtuserdata = null;

                            if (UserColl.Count > 0)
                            {
                                dtuserdata = UserColl.GetDataTable();
                                if (dtuserdata.Rows[0]["Expiry_x0020_Date"] != DBNull.Value)
                                {

                                    string _membershipperiod = "Membership validity till " + Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"])).ToString("dd-MMM-yyyy");
                                    //
                                    string _renew = "<a href='/pages/MembershipRegistration.aspx?code=" + userId + "&rn=1' >Renew</a>";

                                    int _idays = int.Parse(sWeb.Lists["Membership Lenght"].GetItemById(1)["AlertDays"].ToString());
                                    int GraceDays = int.Parse(sWeb.Lists["Membership Lenght"].GetItemById(1)["GraceDays"].ToString());

                                    DateTime dt = Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"])).AddDays(-_idays);
                                    DateTime dtexp = Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"]));

                                    DateTime dtexpGrace = Convert.ToDateTime(Convert.ToString(dtuserdata.Rows[0]["Expiry_x0020_Date"])).AddDays(GraceDays);

                                    if (((DateTime.Now.Date >= dt.Date) && (DateTime.Now.Date <= dtexp.Date)) || ((DateTime.Now.Date >= dtexp.Date) && (DateTime.Now.Date <= dtexpGrace)))
                                    {
                                        if (dtuserdata.Rows[0]["RequestType"].ToString() != "Renewal")
                                        {
                                            _membershipperiod = _membershipperiod + "\n" + _renew;
                                        }
                                    }
                                    ViewState["MembershipPeriod"] = _membershipperiod;
                                    ltrMembershipValidity.Text = _membershipperiod;
                                    ltrMembershipValidity.Visible = true;
                                }
                            }
                        }
                    }
                });
                }
                else
                {
                    ltrMembershipValidity.Text = Convert.ToString(ViewState["MembershipPeriod"]);
                    ltrMembershipValidity.Visible = true;
                }

                #endregion

                lnkLogout.Visible = true;
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "TopBarUC - SetUserLabelProcess", SPContext.Current.Site);
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
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "TopBarUC -  RemoveCookies", SPContext.Current.Site);
            }
        }
    }
}
