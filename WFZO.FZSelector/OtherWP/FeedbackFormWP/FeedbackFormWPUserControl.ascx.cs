using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WFZO.FZSelector.OtherWP.FeedbackFormWP
{
    public partial class FeedbackFormWPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getDrpDwnTypeItems();
        
        }
        protected void saveData()
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

                            txtName.Text = string.Empty;
                            txtSubject.Text = string.Empty;
                            txtDetails.Text = string.Empty;
                            drpDownType.SelectedIndex = -1;
                            //CheckBox1.Checked = false;
                        }
                    }
                }
            }
        }
        protected void getDrpDwnTypeItems()
        {
            if (!Page.IsPostBack)
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
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            saveData();
        }
    }
}
