using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using WFZO.FZSelector.Classes;

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
            try
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
                                       
                                    </Where>";


                        SPListItemCollection Itemcol = list.GetItems(query);
                        Repeater2.DataSource = Itemcol.GetDataTable();
                        Repeater2.DataBind();

                    }
                }
            }

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "Page_Load", SPContext.Current.Site);
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
                    result= imgTag.Substring(start, end - start);
                else
                    result= "";
            }

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "Page_Load", SPContext.Current.Site);
            }
            return result;
        }
        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
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

            catch (Exception ex)
            {
                errorMessage.Value = "message:'" + ex.Message + "'-stack:'" + ex.StackTrace + "'";
                WZFOUtility.LogException(ex, "Page_Load", SPContext.Current.Site);
            }
        }
    }
}
