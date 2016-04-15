using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

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
        public static string GetSrcFromImgTag(string imgTag)
        {
            int start = imgTag.IndexOf("src=") + 5;
            int end = imgTag.IndexOf("\"", start);

            if (end > start)
                return imgTag.Substring(start, end - start);
            else
                return "";
        }
    }
}
