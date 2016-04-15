using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace WFZO.FZSelector.HomeWP.TopImageSlider
{
    [ToolboxItemAttribute(false)]
    public class TopImageSlider : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/WFZO.FZSelector.HomeWP/TopImageSlider/TopImageSliderUserControl.ascx";

        public const int DefaultTime = 3000;
        public static int Time;
        [Category("Slider Time Interval"),
        Personalizable(PersonalizationScope.Shared),
        WebBrowsable(true),
        DefaultValue(DefaultTime),
        WebDisplayName("Time Interval"),
        WebDescription("This Accepts Time Interval Input")]
        public int _TimeInterval
        {
            get { return Time; }
            set { Time = value; }
        }
        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(_ascxPath);
            Controls.Add(control);
        }
    }
}
