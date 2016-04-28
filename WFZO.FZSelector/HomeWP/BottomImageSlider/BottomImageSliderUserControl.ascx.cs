using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

namespace WFZO.FZSelector.HomeWP.BottomImageSlider
{
    public partial class BottomImageSliderUserControl : UserControl
    {
        public int Timeintervalb;
        public int Timeb;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Timeintervalb = BottomImageSlider.DefaultTime;
                Timeb = BottomImageSlider.TimeB;

                getLogoSliderImages();
            }
        }
        protected void getLogoSliderImages()
        {
            try
            {
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {

                        SPList list = web.Lists[Constants.List.BottomSlider.Name];
                        SPQuery query = new SPQuery();
                        query.Query = @"<Where>
                                       
                                            <Eq>
                                           <FieldRef Name='" + Constants.List.BaseColumns.IsActive + @"' />
                                           <Value Type='" + Commons.Type.Boolean + @"'>" + 1 + @"</Value>
                                         </Eq>
                                       
                                    </Where>";

                        SPListItemCollection newVeh = list.GetItems(query);
                        Repeater1.DataSource = newVeh.GetDataTable();
                        Repeater1.DataBind();

                    }
                }
            }

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "getLogoSliderImages", SPContext.Current.Site);
            }
        }
        public string GetSrcFromImgTag(string imgTag)
        {
            string result = "";
            try
            {
                int start = imgTag.IndexOf("src=") + 5;
                int end = imgTag.IndexOf("\"", start);

                if (end > start)
                    result = imgTag.Substring(start, end - start);
                else
                    result = "";
            }
            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "GetSrcFromImgTag", SPContext.Current.Site);
            }
            return result;
        }
    }
}
