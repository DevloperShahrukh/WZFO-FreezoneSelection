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
        string wfzoSiteUrl = "http://sps2013:200";
        string wfzoVisitorGrp = "WFZO FBA Visitors";
        protected void Page_Load(object sender, EventArgs e)
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
                    //Pllogout.Visible = false;
                    //PlLogin.Visible = true;
                    //PlLogin.Visible = true;
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
                    string userId = SPContext.Current.Web.CurrentUser.LoginName;

                    if (userId.Contains("|"))
                    {
                        userId = userId.Substring(userId.LastIndexOf('|') + 1);
                        ltrWelcome.Text = "Welcome " + userId;

                        #region expiry period
                        SPSite wSite = new SPSite(wfzoSiteUrl);
                        SPWeb sWeb = wSite.RootWeb;
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
                                //
                                //ltmembershipperiod.Text = _membershipperiod;
                                //ltmembershipperiod.Visible = true;
                            }
                        }
                        #endregion
                    }
                    //PlLogin.Visible = false;
                    lnkLogout.Visible = true;
                }
                else
                {
                    //PlLogin.Visible = true;
                    lnkLogout.Visible = false;
                }
            }

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "BindFooterRP", SPContext.Current.Site);
            }
        }
        protected void RemoveCookie()
        {
            try
            {
                Response.Cookies["WZFOUserName"].Value = "";
                Response.Cookies["WZFOUserName"].Expires = DateTime.Now.AddDays(-1);

                Response.Cookies["WZFOPassword"].Value = "";
                Response.Cookies["WZFOPassword"].Expires = DateTime.Now.AddDays(-1);
            }

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "SetUserLabel", SPContext.Current.Site);
            }
        }
    }
}
