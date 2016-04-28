using Microsoft.SharePoint;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.OtherWP.FAQWP
{
    public partial class FAQWPUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rpFAQ.DataSource = GetTable();
                rpFAQ.DataBind();
            }
        }

        public DataTable GetTable()
        {
            DataTable dt = new DataTable();
            try { 
            using (SPSite site = new SPSite(SPContext.Current.Site.RootWeb.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists.TryGetList(Constants.List.FAQ.Name);
                    SPQuery query = new SPQuery();
                    query.Query = @"<Where>
                                         <Eq>
                                           <FieldRef Name='" + Constants.List.BaseColumns.IsActive + @"' />
                                           <Value Type='" + Commons.Type.Boolean + @"'>" + 1 + @"</Value>
                                         </Eq>
                                    </Where>";
                    dt = list.GetItems(query).GetDataTable();
                   
                }
            }
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "GetTable", SPContext.Current.Site);
            }
            return dt;
                    
        }
    }
}
