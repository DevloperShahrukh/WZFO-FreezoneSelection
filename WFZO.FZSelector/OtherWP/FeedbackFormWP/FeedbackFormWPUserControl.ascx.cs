using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.OtherWP.FeedbackFormWP
{
    public partial class FeedbackFormWPUserControl : UserControl
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
            if (!IsPostBack)
            {
                getDrpDwnTypeItems();
                CheckEmployeeEmail();
            }

        }

        protected void CheckEmployeeEmail()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        if (!string.IsNullOrWhiteSpace(web.CurrentUser.Email))
                        {
                            txtFrom.Text = web.CurrentUser.Email;
                            txtFrom.Enabled = false;
                            txtFrom.CssClass += "form-control txt-box";

                        }
                        else
                        {
                            txtFrom.Text = string.Empty;
                            txtFrom.Enabled = true;

                        }
                        if (!string.IsNullOrWhiteSpace(web.CurrentUser.Name))
                        {

                            txtName.Text = web.CurrentUser.Name;
                            txtName.Enabled = false;
                            txtFrom.CssClass += "form-control txt-box";
                        }
                        else
                        {

                            txtName.Text = string.Empty;
                            txtName.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "CheckEmployeeEmail", SPContext.Current.Site);
            }
        }
        protected void saveData()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists.TryGetList("FeedbackForm");
                        if (list != null)
                        {
                            SPListItem NewItem = list.Items.Add();
                            {
                                web.AllowUnsafeUpdates = true;
                                NewItem["From"] = txtFrom.Text;
                                NewItem["Name"] = txtName.Text;
                                NewItem["Subject"] = txtSubject.Text;

                                SPFieldLookupValueCollection newType = new SPFieldLookupValueCollection();
                                newType.Add(new SPFieldLookupValue(Convert.ToInt32(drpDownType.SelectedValue), drpDownType.SelectedItem.Text));
                                NewItem["Types"] = newType;

                                NewItem["Details"] = txtDetails.Text;

                                //if (CheckBox1.Checked)
                                //{
                                //    NewItem["SendEmail"] = true;
                                //}
                                //else
                                //{
                                //    NewItem["SendEmail"] = false;
                                //}

                                web.AllowUnsafeUpdates = false;
                                NewItem.Update();

                                //CheckBox1.Checked = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "saveData", SPContext.Current.Site);
            }
        }
        protected void getDrpDwnTypeItems()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPList list = web.Lists["Type"];
                        drpDownType.DataSource = list.GetItems();
                        drpDownType.DataValueField = "ID";
                        drpDownType.DataTextField = "Title";
                        drpDownType.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "getDrpDwnTypeItems", SPContext.Current.Site);
            }
        }
        protected void resetfields()
        {
            //txtFrom.Text = string.Empty;
            txtName.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtDetails.Text = string.Empty;
            drpDownType.SelectedIndex = -1;
            CheckEmployeeEmail();


        }
        protected void SendEmail()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        //WFZO.FZSelector.Classes.Helper.PrepareEmail(1, web, txtName.Text, txtFrom.Text, txtSubject.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "SendEmail", SPContext.Current.Site);
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            saveData();
            SendEmail();
            resetfields();
        }
    }
}