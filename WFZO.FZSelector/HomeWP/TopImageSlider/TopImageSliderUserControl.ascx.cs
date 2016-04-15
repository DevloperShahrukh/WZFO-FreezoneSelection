using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WFZO.FZSelector.HomeWP.TopImageSlider
{
    public partial class TopImageSliderUserControl : UserControl
    {
        public int Timeinterval;
        public int Time;
        protected void Page_Load(object sender, EventArgs e)
        {
            Timeinterval = TopImageSlider.DefaultTime;
            Time = TopImageSlider.Time;

            getLogoSliderImages1();
        }
        protected void getLogoSliderImages1()
        {
            using (SPSite site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb web = site.OpenWeb())
                {

                    SPList list = web.Lists[Constants.List.TopSlider.Name];
                    SPQuery query = new SPQuery();
                    query.Query = @"<Where>
                                       
                                            <Eq>
                                           <FieldRef Name='" + Constants.List.BaseColumns.IsActive + @"' />
                                           <Value Type='" + Commons.Type.Boolean + @"'>" + 1 + @"</Value>
                                         </Eq>
                                       
                                    </Where>" ;


                    SPListItemCollection Itemcol = list.GetItems(query);
                    Repeater2.DataSource = Itemcol.GetDataTable();
                    Repeater2.DataBind();

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
        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HtmlGenericControl divControl = e.Item.FindControl("panelItem") as HtmlGenericControl;

                ListItemType type = e.Item.ItemType;
                if (e.Item.ItemIndex == 0)
                {
                    divControl.Attributes.Add("class", "item active");
                }
                else
                {
                    divControl.Attributes.Add("class", "item");
                }
            }
        }
    }
}
